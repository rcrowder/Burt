#region Namespaces used
// Setup namespaces used
using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.Hardware;
using GHIElectronics.NETMF.IO;
using GHIElectronics.NETMF.System;
using GHIElectronics.NETMF.USBClient;

using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Xml;
using System.Text;
#endregion

// Current FEZ Domino firmware: 4.1.5.0

namespace Burt_MainController
{
    public class Program
    {
        private static PersistentStorage sdCardInterface = null;
        private static string ssid = "BillyWizz";
        private static string passphrase = "6EE745A5AE";

        static OutputPort m_LED = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.LED, true);

        private static SerialPort mMotorController_COM2 = null;
        private static SerialPort mCMUCam3Controller_COM1 = null; 

        public static void Main()
        {
            #region Debug and GC setup
            // Check debug interface
            if (Configuration.DebugInterface.GetCurrent() == Configuration.DebugInterface.Port.USB1)
                throw new InvalidOperationException("Current debug interface is USB. It must be changed to something else before proceeding. Refer to your platform user manual to change the debug interface.");

            // Disable garbage collector messages
            Debug.EnableGCMessages(false);

            // Start MS
            USBC_MassStorage ms = USBClientController.StandardDevices.StartMassStorage();

            // wait to connect to PC
            while (USBClientController.GetState() != USBClientController.State.Running)
            {
                Debug.Print("Waiting to connect to PC...");
                Thread.Sleep(1000);
            }
            #endregion

            #region SD Card Init
            if (PersistentStorage.DetectSDCard())
            {
                // Assume SD card is connected
                try
                {
                    sdCardInterface = new PersistentStorage("SD");
                }
                catch
                {
                    throw new Exception("SD card not detected");
                }
                ms.AttachLun(0, sdCardInterface, " ", " ");

                sdCardInterface.MountFileSystem();

                if (File.Exists("\\SD\\Credentials.xml"))
                {
                    Debug.Print("Found Credentials.xml");
                    ParseCredentialsXML();
                }
            }
            #endregion

            //Setup_RLP();
            Setup_WiFly_SPI1();
            Setup_CMUCam3_COM1();
            Setup_MotorController_COM2();

            //Test_MotorController();
            Test_Nunchuck();

            #region SD Card Cleanup
            if (PersistentStorage.DetectSDCard())
            {
                // Clean up SD card interface
                sdCardInterface.UnmountFileSystem();
                ms.EnableLun(0);
                ms.DisableLun(0);
            }
            #endregion

            // Sleep forever
            Debug.Print("Going to sleep...\n");
            Thread.Sleep(Timeout.Infinite);
        }

        private static void Setup_RLP()
        {
            // Make sure to enable and unlock RLP before use!
            //RLP.Enable();
            //RLP.Unlock("RICCRO@GMAIL.COMB5D03C76221C5B99", new byte[] { 0xE8, 0xB8, 0xDA, 0xE0, 0x84, 0x42, 0x5B, 0x1E, 0x18, 0x61, 0x60, 0xB8, 0x00, 0x1B, 0xB3, 0x64, 0x60, 0x42, 0x99, 0x4B, 0xE9, 0xCA, 0x97, 0xFD, 0x53, 0x58, 0x60, 0xE8, 0x9D, 0x0A, 0xE8, 0x15 });

            //byte[] elf_file = Burt.Resources.GetBytes(Burt.Resources.BinaryResources.RLP_S65Shield);
            //RLP.LoadELF(elf_file);
            //RLP.InitializeBSSRegion(elf_file);

            //ls020_rectangle8 = RLP.GetProcedure(elf_file, "ls020_rectangle8");

            //elf_file = null;
            //Debug.GC(true);
        }

        #region Wifi
        public static void Setup_WiFly_SPI1()
        {
            try
            {
                WiFly.Shield.Setup((Cpu.Pin)FEZ_Pin.Digital.Di10,
                                    SPI.SPI_module.SPI1,
                                    WiFly.Shield.ClockRate.Xtal_12Mhz,
                                    WiFly.Shield.BaudRate.BaudRate_57600);
            }
            catch (Exception e)
            {
                Thread.Sleep(Timeout.Infinite);
            }

            try
            {
                WiFly.Shield.JoinNetwork(   ssid, passphrase,
                                            WiFly.Shield.AuthenticationType.WPA2_AES,
                                            80);
            }
            catch
            {
                Thread.Sleep(Timeout.Infinite);
            }
        }

        private static void ParseCredentialsXML()
        {
            FileStream xmlFileStream = new FileStream("\\SD\\Credentials.xml", FileMode.Open);
            XmlReaderSettings ss = new XmlReaderSettings();
            ss.IgnoreWhitespace = true;
            ss.IgnoreComments = false;

            string currentElement = "";
            bool foundSSID = false;
            bool foundPassPhrase = false;

            XmlReader xmlr = XmlReader.Create(xmlFileStream, ss);
            while (!xmlr.EOF)
            {
                xmlr.Read();
                switch (xmlr.NodeType)
                {
                    case XmlNodeType.Element:
                        //Debug.Print("element: " + xmlr.Name);
                        currentElement = xmlr.Name;
                        break;
                    case XmlNodeType.Text:
                        //Debug.Print("text: " + xmlr.Value);
                        if (currentElement.ToLower() == "ssid")
                        {
                            ssid = xmlr.Value;
                            foundSSID = true;
                        }
                        else if (currentElement.ToLower() == "passphrase")
                        {
                            passphrase = xmlr.Value;
                            foundPassPhrase = true;
                        }
                        break;
                    default:
                        //Debug.Print(xmlr.NodeType.ToString());
                        break;
                }
            }

            if (!foundSSID || !foundPassPhrase)
            {
                Debug.Print("Failed to parse Credentials.xml!\n");
                ssid = "";
                passphrase = "";
            }
        }
        #endregion

        #region CMU-Cam v3
        public static void Setup_CMUCam3_COM1()
        {
            mCMUCam3Controller_COM1 = new SerialPort("COM1", 38400, Parity.None, 8, StopBits.One);
            mCMUCam3Controller_COM1.ReadTimeout = 250;
            mCMUCam3Controller_COM1.WriteTimeout = 250;
            mCMUCam3Controller_COM1.Open();
            
        }
        #endregion

        #region Motor Controller
        private static byte[] rx_byte = new byte[16];
        private static byte[] tx_data = new byte[5];

        public static void Setup_MotorController_COM2()
        {
            mMotorController_COM2 = new SerialPort("COM2", 115200);
            mMotorController_COM2.ReadTimeout = 500;
            mMotorController_COM2.WriteTimeout = 500;
            mMotorController_COM2.Open();
        }

        public static void ConstructMotorCommand(char motor, char direction, short speed, ref byte[] data)
        {
            data[0] = (byte)motor;
            data[1] = (byte)direction;
            data[2] = (byte)(speed >> 8);
            data[3] = (byte)(speed & 0xff);

            if (speed == 0)
                data[3] = (byte)'\r';
            else
                data[4] = (byte)'\r';
        }

        public static void MC_WriteCommand(char motors, char direction, short speed)
        {
            int read_count = 0;
            String rx = "";

            // https://code.google.com/p/burt/source/browse/trunk/Hardware/Motors/Firmware/main.c
            ConstructMotorCommand(motors, direction, speed, ref tx_data);

            //OutputPort reset = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di10, false);
            //reset.Write(true);

            byte rx_prev = 0;
            while (true)
            {
                rx_prev = rx_byte[0];

                read_count = mMotorController_COM2.Read(rx_byte, 0, 1);
                if (read_count > 0)// do we have data?
                {
                    rx += (char)rx_byte[0];
                }
                if ((rx_prev == '>' && rx_byte[0] == ' ') ||
                    read_count == 0)
                {
                    mMotorController_COM2.Write(tx_data, 0, 5);
                    mMotorController_COM2.Read(rx_byte, 0, 16);
                    //mMotorController_COM2.Flush();
                    return;
                }
            }
        }

        public static void Test_MotorController()
        {
            char motors = 'b';
            char direction = 'f';
            short s, speed = 0;

            for (s = speed; s <= 0x3ff; s += (0x3ff >> 8))
                MC_WriteCommand(motors, direction, s);

            //OutputPort reset = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di10, false);
            //reset.Write(true);

            // Sleep for 1 second
            Thread.Sleep(1000);

            for (; s > 0; s -= (0x3ff >> 8))
                MC_WriteCommand(motors, direction, s);

            speed = 0;
            MC_WriteCommand(motors, direction, speed);
        }
        #endregion

        #region Nunchuck
        public static void Test_Nunchuck()
        {
            // http://www.robotshop.com/media/files/PDF/inex-zx-nunchuck-datasheet.pdf
            //
            // To communicate with the Nunchuk, we must send a handshake signal. If you are 
            // using a black Wii Nunchuk, send 2 bytes 0xF0, 0x55 to initialize the first register 
            // and 0xFB, 0x00 to initialize the second register of the Nunchuk.  On a white Wii 
            // Nunchuk, send 0x40, 0x00 followed by 0x00. The I2C address of both Wii Nunchuks 
            // is 0x52.  The frequency used to communicate with the Wii Nunchuk is 100KHz.

            I2CDevice.Configuration con = new I2CDevice.Configuration((0xA4 >> 1), 100);
            I2CDevice MyI2C = new I2CDevice(con);

            I2CDevice.I2CTransaction[] xRestartActions = new I2CDevice.I2CTransaction[1];
            I2CDevice.I2CTransaction[] xConversionActions = new I2CDevice.I2CTransaction[1];
            I2CDevice.I2CTransaction[] xDataReadActions = new I2CDevice.I2CTransaction[1];

            // black send 2 bytes 0xF0, 0x55 and 0xFB, 0x00
            // white send 0x40, 0x00
            //byte[] RestartNunchuck = new byte[4] { 0xF0, 0x55, 0xFB, 0x00 }; // Not tested
            byte[] RestartNunchuck = new byte[2] { 0x40, 0x00 };
            xRestartActions[0] = I2CDevice.CreateWriteTransaction(RestartNunchuck);

            //byte[] ConversionCommand = new byte[2] { 0xFB, 0x00 }; // maybe?
            byte[] ConversionCommand = new byte[1] { 0x00 };
            xConversionActions[0] = I2CDevice.CreateWriteTransaction(ConversionCommand);

            byte[] DataStream = new byte[6];
            xDataReadActions[0] = I2CDevice.CreateReadTransaction(DataStream);

            MyI2C.Execute(xRestartActions, 1000);
            Thread.Sleep(100);

            int deadzoneX = 8;
            int deadzoneY = 8;

            while (true)
            {
                Debug.Print("<3 MC");

                MyI2C.Execute(xConversionActions, 1000);
                Thread.Sleep(100);

                MyI2C.Execute(xDataReadActions, 1000);

                DataStream[0] = nunchuk_decode_byte(DataStream[0]);
                DataStream[1] = nunchuk_decode_byte(DataStream[1]);
                DataStream[2] = nunchuk_decode_byte(DataStream[2]);
                DataStream[3] = nunchuk_decode_byte(DataStream[3]);
                DataStream[4] = nunchuk_decode_byte(DataStream[4]);
                DataStream[5] = nunchuk_decode_byte(DataStream[5]);


                // Taken from http://home.kendra.com/mauser/Joystick.html
                //
                // 1. Get X and Y from the Joystick, do whatever scaling and calibrating you need to do based on your hardware.
                // 2. Invert X
                // 3. Calculate R+L (Call it V): V =(100-ABS(X)) * (Y/100) + Y
                // 4. Calculate R-L (Call it W): W= (100-ABS(Y)) * (X/100) + X
                // 5. Calculate R: R = (V+W) /2
                // 6. Calculate L: L= (V-W)/2
                // 7. Do any scaling on R and L your hardware may require.
                // 8. Send those values to your Robot.
                // 9. Go back to 1.
                //

                int joystickX = (DataStream[0] > 127 - (deadzoneX / 2) &&
                    DataStream[0] < 127 + (deadzoneX / 2)) ? 0 : DataStream[0] - 127;

                int joystickY = (DataStream[1] > 127 - (deadzoneY / 2) &&
                    DataStream[1] < 127 + (deadzoneY / 2)) ? 0 : DataStream[1] - 127;

                float X = (float)joystickX;
                float Y = (float)joystickY;

                X *= -1;

                float V = (100.0f - (float)System.Math.Abs((int)X)) * (Y / 100.0f) + Y;
                float W = (100.0f - (float)System.Math.Abs((int)Y)) * (X / 100.0f) + X;

                float L = (V - W) / 2.0f;
                float R = (V + W) / 2.0f;

                if (L >= 0)
                    MC_WriteCommand('1', 'f', (short)(10.0f * L));
                else
                    MC_WriteCommand('1', 'r', (short)(10.0f * -L));

                if (R >= 0)
                    MC_WriteCommand('2', 'f', (short)(10.0f * R));
                else
                    MC_WriteCommand('2', 'r', (short)(10.0f * -R));

                if ((DataStream[5] & 0x01) == 0)
                {
                    int accelX = (DataStream[2] << 2) | ((DataStream[5] >> 2) & 0x03);
                    int accelY = (DataStream[3] << 2) | ((DataStream[5] >> 4) & 0x03);
                    int accelZ = (DataStream[4] << 2) | ((DataStream[5] >> 6) & 0x03);

                    Debug.Print(
                        "A " + accelX.ToString() + "," + accelY.ToString() + "," + accelZ.ToString());

                    //Debug.Print("Z button");
                }

                if ((DataStream[5] & 0x02) == 0)
                {
                    Debug.Print(
                        "J " + joystickX.ToString() + "," + joystickY.ToString() +
                        " -> " +
                        "M " + L.ToString() + "," + R.ToString());

                    //Debug.Print("C button");
                }

                //Thread.Sleep(150);
            }
        }

        // Encode data to format that most wiimote drivers except
        // only needed if you use one of the regular wiimote drivers
        public static byte nunchuk_decode_byte(byte x)
        {
            x = (byte)((x ^ 0x17) + 0x17);
            return x;
        }
        #endregion

    }
}
