using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Net;

using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using GHIElectronics.NETMF.FEZ;

namespace WiFly
{
    public class Shield
    {
        private static SPI spiInterface;
        private static Version firmwareVersion;

        // Known WiFly Shield crystals
        public enum ClockRate
        {
            Xtal_12Mhz,
            Xtal_14MHz
        };

        // Available baud rates on the SC16C750
        public enum BaudRate
        {
            BaudRate_1200 = '1',
            BaudRate_2400,
            BaudRate_4800,
            BaudRate_9600,
            BaudRate_19200,
            BaudRate_38400,
            BaudRate_57600,
            BaudRate_115200,
            BaudRate_230400,
            BaudRate_460800,
            BaudRate_921600
        };

        // Available authentication types
        public enum AuthenticationType
        {
            Open = '0',
            WEP_128,
            WPA1_TKIP,
            MIXED_PSK,
            WPA2_AES,
            Adhoc = '6',
        }

        #region Register Handlers
        private enum Register
        {
            // General register set
            THR = 0x00 << 3,
            RHR = 0x00 << 3,
            IER = 0x01 << 3,
            FCR = 0x02 << 3,
            IIR = 0x02 << 3,
            LCR = 0x03 << 3,
            MCR = 0x04 << 3,
            LSR = 0x05 << 3,
            MSR = 0x06 << 3,
            SPR = 0x07 << 3,
            TCR = 0x06 << 3,
            TLR = 0x07 << 3,
            TXFIFO = 0x08 << 3,
            RXFIFO = 0x09 << 3,
            IODIR = 0x0A << 3,
            IOSTATE = 0x0B << 3,
            IOINTMSK = 0x0C << 3,
            IOCTRL = 0x0E << 3,
            EFCR = 0x0F << 3,
            // Special register set
            DLL = 0x00 << 3,
            DLH = 0x01 << 3,
            // Enhanced register set
            EFR = 0x02 << 3,
            XON1 = 0x04 << 3,
            XON2 = 0x05 << 3,
            XOFF1 = 0x06 << 3,
            XOFF2 = 0x07 << 3,
        }

        private static byte[] bytex2 = new byte[2];

        private static void WriteRegister(Register reg, byte b)
        {
            bytex2[0] = (byte)reg;  // Register address byte
            bytex2[1] = b;
            spiInterface.Write(bytex2);
        }

        private static byte ReadRegister(Register reg)
        {
            bytex2[0] = (byte)((byte)reg | 0x80);   // Top bit toggles reading
            bytex2[1] = 0;
            spiInterface.WriteRead(bytex2, bytex2);
            return bytex2[1];
        }
        #endregion

        #region Command handlers

        private static void WriteArray(byte[] ba)
        {
            for (int i = 0; i < ba.Length; i++)
                WriteRegister(Register.THR, ba[i]);
        }

        public static void SendString(string str)
        {
            byte[] ba = Encoding.UTF8.GetBytes(str);
            WriteArray(ba);
        }

        public static void SendCommand(string command)
        {
            SendString(command);
            WriteRegister(Register.THR, (byte)'\r');
            WriteRegister(Register.THR, (byte)'\n');
        }

        public static void EnterCommandMode()
        {
            SendCommand("");
            SendCommand("exit");

            Thread.Sleep(150);
            SendString("$$$");
            Thread.Sleep(150);
            
            if (!WaitForResponse("CMD"))
                throw new IOException("Failed to enter command mode!");
        }

        public static string ReadResponse()
        {
            string reply = "";

            // Read out of the RX FIFO
            byte b = 0;
            while (b != '\r')
            {
                if ((ReadRegister(Register.LSR) & 0x01) > 0)
                {
                    while (ReadRegister(Register.RXFIFO) != 0)
                    {
                        b = ReadRegister(Register.RHR);
                        if (b >= 0x20)
                            reply += ((char)b);
                        if (b == '\r')
                            break;
                    }
                }
            }
            // Swallow new line
            ReadRegister(Register.RHR);  // '\n'

            return reply;
        }

        private static string errorResponse = "ERR";

        public static bool WaitForResponse(string response)
        {
            char received = '\0';
            int errOffset = 0, offset = 0;
            string msg = "";

            while (offset < response.Length && errOffset < 3)
            {
                if ((ReadRegister(Register.LSR) & 0x01) > 0)
                {
                    while (ReadRegister(Register.RXFIFO) != 0)
                    {
                        received = Convert.ToChar(ReadRegister(Register.RHR));
                        
                        if (received >= ' ')
                            msg += received;
                        else
                        if (received != 0 && msg != "")
                        {
                            Debug.Print(msg);
                            msg = "";
                        }

                        if (response[offset] == received)
                            offset++;
                        if (errorResponse[errOffset] == received && offset == 0)
                            errOffset++;

                        if (offset >= response.Length || errOffset >= 3)
                            break;
                    }
                }
            }
            // Swallow carriage return and new line
            ReadRegister(Register.RHR);  // '\r'
            ReadRegister(Register.RHR);  // '\n'

            if (errOffset == 3)
                return false;

            return true;
        }

        public static void FlushRX()
        {
            // Read out of the RX FIFO
            byte b = 0;
            while (ReadRegister(Register.RXFIFO) > 0)
            {
                if ((ReadRegister(Register.LSR) & 0x01) > 0)
                {
                    b = ReadRegister(Register.RHR);
                }
            }
        }
        #endregion

        public static void Setup(
                                    Cpu.Pin chipSelect,
                                    SPI.SPI_module spiModule,
                                    ClockRate clockRate,
                                    BaudRate baudRateIndex)
        {
            #region SPI Init
            try
            {
                // Initialise SDI
                SPI.Configuration spiConfig = new SPI.Configuration(
                    chipSelect, // Chip select pin
                    false,      // Active state
                    10,         // Setup time (ms)
                    10,         // Hold time (ms)
                    false,      // Clock idle (low)
                    true,       // Edge (rising)
                    2048,       // Clock rate (KHz)
                    spiModule); // SPI module

                spiInterface = new SPI(spiConfig);
            }
            catch (Exception e)
            {
                throw new Exception("SPI Init Error (" + e.Message + ")", e);
            }
            #endregion

            #region SPI to WiFly UART Init
            // Initialise the WiFly shield SPI<->UART chip

            // Convert the enum to a baud rate
            int baudRate = 1200;
            int depth = Convert.ToInt32(baudRateIndex.ToString()) - (int)BaudRate.BaudRate_1200;
            for (int i = 0; i < depth; i++)
            {
                baudRate += baudRate;
            }

            // Make DLL and DLH accessible
            if (clockRate == ClockRate.Xtal_12Mhz)
            {
                // I doesn't look like a 12MHz clock can do 921600bps
                if (baudRate == 921600)
                    baudRate = 460800;

                int divisor = 12288000 / (baudRate * 16);
                WriteRegister(Register.LCR, 0x80);  // 0x80 to program baudrate
                WriteRegister(Register.DLH, (byte)((divisor & 0x0000ff00) >> 8));
                WriteRegister(Register.DLL, (byte)((divisor & 0x000000ff) >> 0));
            }
            else
            {
                int divisor = 14745600 / (baudRate * 16);
                WriteRegister(Register.LCR, 0x80);  // 0x80 to program baudrate
                WriteRegister(Register.DLH, (byte)((divisor & 0x0000ff00) >> 8));
                WriteRegister(Register.DLL, (byte)((divisor & 0x000000ff) >> 0));
            }

            // Make EFR accessible
            WriteRegister(Register.LCR, 0xBF);  // access EFR register
            
            // Enable CTS, RTS, and Enhanced functions (IER[7:4], FCR[5:4], and MCR[7:5])
            WriteRegister(Register.EFR, (1 << 7) | (1 << 6) | (1 << 4));

            // Finally setup LCR
            WriteRegister(Register.LCR, 0x03);  // no parity, 1 stop bit, 8 data bit
            WriteRegister(Register.FCR, 0x06);  // reset TX and RX FIFO
            WriteRegister(Register.FCR, 0x01);  // enable FIFO mode

            // Perform read/write test of scratchpad register to check if UART is working
            WriteRegister(Register.SPR, 0x55);
            byte data = ReadRegister(Register.SPR);
            if (data != 0x55)
                throw new IOException("Failed to init SPI<->UART chip");

            #endregion

            // Enter command mode
            try
            {
                EnterCommandMode();
            }
            catch (Exception e)
            {
                throw new IOException(e.ToString());
            }

            // Turn off WiFi auto-joining
            SendCommand("set wlan join 0");
            WaitForResponse("AOK");

            SendCommand("save");
            WaitForResponse("Storing in config");

            // "reboot" to a known state
            Debug.Print("Rebooting WiFly-GSX");
            SendCommand("reboot");
            WaitForResponse("*Reboot*");
            WaitForResponse("*READY*");

            // Back to command mode
            try
            {
                EnterCommandMode();
            }
            catch (Exception e)
            {
                throw new IOException(e.ToString());
            }

            FlushRX();

            // Grab the version
            SendCommand("ver");
            WaitForResponse("ver");
            string version = "";
            while ((version = ReadResponse()) == "")
                ;
            string[] tokens = version.Split(' ');
            string[] split = tokens[2].Split('.');

            firmwareVersion = new Version(Convert.ToInt32(split[0]), Convert.ToInt32(split[1].TrimEnd(',')));
            Debug.Print("WiFly firmware version: " + firmwareVersion.Major.ToString() + "." + firmwareVersion.Minor.ToString());

            // Exit command mode
            SendCommand("exit");
            WaitForResponse("EXIT");
        }

        public static void JoinNetwork(
                                        string SSID,
                                        string passPhrase,
                                        AuthenticationType authType,
                                        ushort listeningPort)
        {
            if (SSID == "")
                throw new ArgumentException("Valid SSID required");
            if (passPhrase == "")
                throw new ArgumentException("Valid passphrase required");

            // Check for spaces
            string[] ssidTokens = SSID.Split(' ');
            if (ssidTokens.Length > 1)
                throw new ArgumentException("Oops! SSID contains space(s), replace them with $");

            // Check for spaces
            string[] phraseTokens = passPhrase.Split(' ');
            if (phraseTokens.Length > 1)
                throw new ArgumentException("Oops! PassPhrase contains space(s), replace them with $");

            try
            {
                EnterCommandMode();
            }
            catch (Exception e)
            {
                throw new IOException(e.ToString());
            }

            SendCommand("set wlan auth " + (char)authType);
            if (!WaitForResponse("AOK"))
                throw new IOException("Failed to set authentication type!");

            SendCommand("set wlan phrase " + passPhrase);
            if (!WaitForResponse("AOK"))
                throw new IOException("Failed to set pass phrase!");

            SendCommand("set ip dchp 1");
            if (!WaitForResponse("AOK"))
                throw new IOException("Failed to set Dynamic Host Configuration Protocol!");

            SendCommand("set ip protocol 3");
            if (!WaitForResponse("AOK"))
                throw new IOException("Failed to set IP Protocol!");

            SendCommand("set ip listen " + listeningPort.ToString());
            if (!WaitForResponse("AOK"))
                throw new IOException("Failed to set listening port!");

            Debug.Print("Attempting to join SSID: " + SSID);
            SendCommand("join " + SSID);

            WaitForResponse("Associated!");
            Debug.Print("Associated!");
            
            //WaitForResponse("Listen on " + listeningPort.ToString());

            // Turn on WiFi auto-joining
            SendCommand("set wlan join 1");
            if (!WaitForResponse("AOK"))
                throw new IOException("Failed to set auto connect!");

            try
            {
                EnterCommandMode();
            }
            catch (Exception e)
            {
                throw new IOException(e.ToString());
            }

            SendCommand("set comm remote 220$Ready<CRLF>");
            SendCommand("set comm open *OPEN*");
            SendCommand("set comm close 0");
            FlushRX();

            // Make sure these configurations are saved
            SendCommand("save");
            WaitForResponse("Storing in config");

            IPAddress ip = GetIPAddress();
            Debug.Print("IP Address " + ip.ToString() + ":" + listeningPort.ToString());

            // Exit command mode
            SendCommand("exit");
            WaitForResponse("EXIT");
        }

        public static IPAddress GetIPAddress()
        {
            WaitForResponse("<" + firmwareVersion.Major.ToString() + "." + firmwareVersion.Minor.ToString() + ">");
            SendCommand("get ip a");
            WaitForResponse("get ip a");
            string ipStr;
            while ((ipStr = ReadResponse()) == "")
                ;
            string[] tokens = ipStr.Split('.');
            byte[] ipAddr = new byte[4] {   Convert.ToByte(tokens[0]), 
                                            Convert.ToByte(tokens[1]), 
                                            Convert.ToByte(tokens[2]), 
                                            Convert.ToByte(tokens[3]) };
            return new IPAddress(ipAddr);
        }

        public static bool ClientRequest()
        {
            if ((ReadRegister(Register.LSR) & 0x01) == 0x01)
            {
                string response = "";
                while ((response = ReadResponse()) != "")
                {
                    Debug.Print(response);
                }

                // Dont care, lets just dump it
                FlushRX();
                FlushRX();
                return true;
            }
            return false;
        }
    }
}
