using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoMini;

namespace Audio_Analysis_and_Head_Tracking
{
    public class I2CDev
    {
        // I2Cdev library collection - Main I2C device class header file
        // Abstracts bit and byte I2C R/W functions into a convenient class
        // 6/9/2012 by Jeff Rowberg <jeff@rowberg.net>
        //
        // Changelog:
        //     2012-06-09 - fix major issue with reading > 32 bytes at a time with Arduino Wire
        //                - add compiler warnings when using outdated or IDE or limited I2Cdev implementation
        //     2011-11-01 - fix write*Bits mask calculation (thanks sasquatch @ Arduino forums)
        //     2011-10-03 - added automatic Arduino version detection for ease of use
        //     2011-10-02 - added Gene Knight's NBWire TwoWire class implementation with small modifications
        //     2011-08-31 - added support for Arduino 1.0 Wire library (methods are different from 0.x)
        //     2011-08-03 - added optional timeout parameter to read* methods to easily change from default
        //     2011-08-02 - added support for 16-bit registers
        //                - fixed incorrect Doxygen comments on some methods
        //                - added timeout value for read operations (thanks mem @ Arduino forums)
        //     2011-07-30 - changed read/write function structures to return success or byte counts
        //                - made all methods static for multi-device memory savings
        //     2011-07-28 - initial release

        /* ============================================
        I2Cdev device library code is placed under the MIT license
        Copyright (c) 2012 Jeff Rowberg

        Permission is hereby granted, free of charge, to any person obtaining a copy
        of this software and associated documentation files (the "Software"), to deal
        in the Software without restriction, including without limitation the rights
        to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
        copies of the Software, and to permit persons to whom the Software is
        furnished to do so, subject to the following conditions:

        The above copyright notice and this permission notice shall be included in
        all copies or substantial portions of the Software.

        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
        IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
        FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
        AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
        LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
        OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
        THE SOFTWARE.
        ===============================================
        */

        // 1000ms default read timeout (modify with "readTimeout = [ms];")
        private static int I2CDEV_DEFAULT_READ_TIMEOUT = 1000;

        /** Default timeout value for read operations.
            * Set this to 0 to disable timeout detection.
            */
        public static int readTimeout = I2CDEV_DEFAULT_READ_TIMEOUT;
        
        private I2CDevice.Configuration config;
        private I2CDevice i2c;

        public void initialize(byte devAddr)
        {
            config = new I2CDevice.Configuration(devAddr, 400);//0x68, 400);
            i2c = new I2CDevice(config);
        }

        /** Read a single bit from an 8-bit device register.
            * @param devAddr I2C slave device address
            * @param regAddr Register regAddr to read from
            * @param bitNum Bit position to read (0-7)
            * @param data Container for single bit value
            * @param timeout Optional read timeout in milliseconds (0 to disable, leave off to use default class value in readTimeout)
            * @return Status of read operation (true = success)
            */
        public byte readBit(byte devAddr, byte regAddr, byte bitNum, byte data, ushort timeout = 1000)
        {
            return readByte(devAddr, regAddr, data, timeout);
        }

        /** Read a single bit from a 16-bit device register.
            * @param devAddr I2C slave device address
            * @param regAddr Register regAddr to read from
            * @param bitNum Bit position to read (0-15)
            * @param data Container for single bit value
            * @param timeout Optional read timeout in milliseconds (0 to disable, leave off to use default class value in readTimeout)
            * @return Status of read operation (true = success)
            */
        byte readBitW(byte devAddr, byte regAddr, byte bitNum, ushort data, ushort timeout)
        {
            return readWord(devAddr, regAddr, data, timeout);
        }

        /** Read multiple bits from an 8-bit device register.
            * @param devAddr I2C slave device address
            * @param regAddr Register regAddr to read from
            * @param bitStart First bit position to read (0-7)
            * @param length Number of bits to read (not more than 8)
            * @param data Container for right-aligned value (i.e. '101' read from any bitStart position will equal 0x05)
            * @param timeout Optional read timeout in milliseconds (0 to disable, leave off to use default class value in readTimeout)
            * @return Status of read operation (true = success)
            */
        public byte readBits(byte devAddr, byte regAddr, byte bitStart, byte length, byte data, ushort timeout = 1000)
        {
            // 01101001 read byte
            // 76543210 bit numbers
            //    xxx   args: bitStart=4, length=3
            //    010   masked
            //   -> 010 shifted
            byte count;
            if ((count = readByte(devAddr, regAddr, data, timeout)) != 0)
            {
                byte mask = (byte)(((1 << length) - 1) << (bitStart - length + 1));
                data &= mask;
                data >>= (bitStart - length + 1);
            }
            return count;
        }

        /** Read multiple bits from a 16-bit device register.
            * @param devAddr I2C slave device address
            * @param regAddr Register regAddr to read from
            * @param bitStart First bit position to read (0-15)
            * @param length Number of bits to read (not more than 16)
            * @param data Container for right-aligned value (i.e. '101' read from any bitStart position will equal 0x05)
            * @param timeout Optional read timeout in milliseconds (0 to disable, leave off to use default class value in readTimeout)
            * @return Status of read operation (1 = success, 0 = failure, -1 = timeout)
            */
        byte readBitsW(byte devAddr, byte regAddr, byte bitStart, byte length, ushort data, ushort timeout)
        {
            // 1101011001101001 read byte
            // fedcba9876543210 bit numbers
            //    xxx           args: bitStart=12, length=3
            //    010           masked
            //           -> 010 shifted
            byte count;
            if ((count = readWord(devAddr, regAddr, data, timeout)) != 0)
            {
                ushort mask = (ushort)(((1 << length) - 1) << (bitStart - length + 1));
                data &= mask;
                data >>= (bitStart - length + 1);
            }
            return count;
        }

        /** Read single byte from an 8-bit device register.
            * @param devAddr I2C slave device address
            * @param regAddr Register regAddr to read from
            * @param data Container for byte value read from device
            * @param timeout Optional read timeout in milliseconds (0 to disable, leave off to use default class value in readTimeout)
            * @return Status of read operation (true = success)
            */
        public byte readByte(byte devAddr, byte regAddr, byte data, ushort timeout = 1000)
        {
            byte[] b = new byte[1];
            byte count = readBytes(devAddr, regAddr, 1, b, timeout);
            data = b[0];
            return count;
        }

        /** Read single word from a 16-bit device register.
            * @param devAddr I2C slave device address
            * @param regAddr Register regAddr to read from
            * @param data Container for word value read from device
            * @param timeout Optional read timeout in milliseconds (0 to disable, leave off to use default class value in readTimeout)
            * @return Status of read operation (true = success)
            */
        public byte readWord(byte devAddr, byte regAddr, ushort data, ushort timeout = 1000)
        {
            byte[] b = new byte[2] { 0, 0 };
            byte count = readWords(devAddr, regAddr, 1, b, timeout);
            data = (byte)((b[0] << 8) | b[1]);
            return count;
        }

        /** Read multiple bytes from an 8-bit device register.
            * @param devAddr I2C slave device address
            * @param regAddr First register regAddr to read from
            * @param length Number of bytes to read
            * @param data Buffer to store read data in
            * @param timeout Optional read timeout in milliseconds (0 to disable, leave off to use default class value in readTimeout)
            * @return Number of bytes read (-1 indicates failure)
            */
        public byte readBytes(byte devAddr, byte regAddr, byte length, byte[] data, ushort timeout = 1000)
        {
            I2CDevice.I2CTransaction[] actions;
            actions = new I2CDevice.I2CTransaction[] { I2CDevice.CreateReadTransaction(data), };
            return (byte)i2c.Execute(actions, 1000);
        }

        /** Read multiple words from a 16-bit device register.
            * @param devAddr I2C slave device address
            * @param regAddr First register regAddr to read from
            * @param length Number of words to read
            * @param data Buffer to store read data in
            * @param timeout Optional read timeout in milliseconds (0 to disable, leave off to use default class value in readTimeout)
            * @return Number of words read (0 indicates failure)
            */
        public byte readWords(byte devAddr, byte regAddr, byte length, byte[] data, ushort timeout)
        {
            I2CDevice.I2CTransaction[] actions;
            actions = new I2CDevice.I2CTransaction[] { I2CDevice.CreateReadTransaction(data), };
            return (byte)i2c.Execute(actions, 1000);
        }

        /** write a single bit in an 8-bit device register.
            * @param devAddr I2C slave device address
            * @param regAddr Register regAddr to write to
            * @param bitNum Bit position to write (0-7)
            * @param value New bit value to write
            * @return Status of operation (true = success)
            */
        public bool writeBit(byte devAddr, byte regAddr, byte bitNum, byte data)
        {
            byte b = data;
            readByte(devAddr, regAddr, b, 0);
            b = (data != 0) ? (byte)((b | (1 << bitNum))) : (byte)((b & ~(1 << bitNum)));
            return writeByte(devAddr, regAddr, b);
        }

        /** write a single bit in a 16-bit device register.
            * @param devAddr I2C slave device address
            * @param regAddr Register regAddr to write to
            * @param bitNum Bit position to write (0-15)
            * @param value New bit value to write
            * @return Status of operation (true = success)
            */
        bool writeBitW(byte devAddr, byte regAddr, byte bitNum, ushort data)
        {
            ushort w = data;
            readWord(devAddr, regAddr, w, 0);
            w = (data != 0) ? (byte)((w | (1 << bitNum))) : (byte)((w & ~(1 << bitNum)));
            return writeWord(devAddr, regAddr, w);
        }

        /** Write multiple bits in an 8-bit device register.
            * @param devAddr I2C slave device address
            * @param regAddr Register regAddr to write to
            * @param bitStart First bit position to write (0-7)
            * @param length Number of bits to write (not more than 8)
            * @param data Right-aligned value to write
            * @return Status of operation (true = success)
            */
        public bool writeBits(byte devAddr, byte regAddr, byte bitStart, byte length, byte data)
        {
            //      010 value to write
            // 76543210 bit numbers
            //    xxx   args: bitStart=4, length=3
            // 00011100 mask byte
            // 10101111 original value (sample)
            // 10100011 original & ~mask
            // 10101011 masked | value
            byte b = data;
            if (readByte(devAddr, regAddr, b, 0) != 0)
            {
                byte mask = (byte)(((1 << length) - 1) << (bitStart - length + 1));
                data <<= (bitStart - length + 1); // shift data into correct position
                data &= mask; // zero all non-important bits in data
                b &= (byte)(~mask); // zero all important bits in existing byte
                b |= data; // combine data with existing byte
                return writeByte(devAddr, regAddr, b);
            }
            else
            {
                return false;
            }
        }

        /** Write multiple bits in a 16-bit device register.
            * @param devAddr I2C slave device address
            * @param regAddr Register regAddr to write to
            * @param bitStart First bit position to write (0-15)
            * @param length Number of bits to write (not more than 16)
            * @param data Right-aligned value to write
            * @return Status of operation (true = success)
            */
        bool writeBitsW(byte devAddr, byte regAddr, byte bitStart, byte length, ushort data)
        {
            //              010 value to write
            // fedcba9876543210 bit numbers
            //    xxx           args: bitStart=12, length=3
            // 0001110000000000 mask byte
            // 1010111110010110 original value (sample)
            // 1010001110010110 original & ~mask
            // 1010101110010110 masked | value
            ushort w = data;
            if (readWord(devAddr, regAddr, w, 9) != 0)
            {
                byte mask = (byte)(((1 << length) - 1) << (bitStart - length + 1));
                data <<= (bitStart - length + 1); // shift data into correct position
                data &= mask; // zero all non-important bits in data
                w &= (ushort)(~mask); // zero all important bits in existing word
                w |= data; // combine data with existing word
                return writeWord(devAddr, regAddr, w);
            }
            else
            {
                return false;
            }
        }

        /** Write single byte to an 8-bit device register.
            * @param devAddr I2C slave device address
            * @param regAddr Register address to write to
            * @param data New byte value to write
            * @return Status of operation (true = success)
            */
        public bool writeByte(byte devAddr, byte regAddr, byte data)
        {
            byte[] b = new byte[1] { data };
            return writeBytes(devAddr, regAddr, 1, b);
        }

        /** Write single word to a 16-bit device register.
            * @param devAddr I2C slave device address
            * @param regAddr Register address to write to
            * @param data New word value to write
            * @return Status of operation (true = success)
            */
        public bool writeWord(byte devAddr, byte regAddr, ushort data)
        {
            byte[] b = new byte[2] { (byte)(data >> 8), (byte)(data & 0xFF) };
            //byte[] b = new byte[2] { (byte)(data & 0xFF), (byte)(data >> 8) };
            return writeWords(devAddr, regAddr, 1, b);
        }

        /** Write multiple bytes to an 8-bit device register.
            * @param devAddr I2C slave device address
            * @param regAddr First register address to write to
            * @param length Number of bytes to write
            * @param data Buffer to copy new data from
            * @return Status of operation (true = success)
            */
        public bool writeBytes(byte devAddr, byte regAddr, byte length, byte[] data)
        {
            I2CDevice.I2CTransaction[] actions;
            actions = new I2CDevice.I2CTransaction[] { I2CDevice.CreateWriteTransaction(data), };
            int nSent = i2c.Execute(actions, 1000);
            return nSent == actions.Length;
        }

        /** Write multiple words to a 16-bit device register.
            * @param devAddr I2C slave device address
            * @param regAddr First register address to write to
            * @param length Number of words to write
            * @param data Buffer to copy new data from
            * @return Status of operation (true = success)
            */
        public bool writeWords(byte devAddr, byte regAddr, byte length, byte[] data)
        {
            I2CDevice.I2CTransaction[] actions;
            actions = new I2CDevice.I2CTransaction[] { I2CDevice.CreateWriteTransaction(data), };
            int nSent = i2c.Execute(actions, 1000);
            return nSent == actions.Length;
        }
    }
}
