// I2Cdev library collection - MPU6050 I2C device class
// Based on InvenSense MPU-6050 register map document rev. 2.0, 5/19/2011 (RM-MPU-6000A-00)
// 10/3/2011 by Jeff Rowberg <jeff@rowberg.net>
// Updates should (hopefully) always be available at https://github.com/jrowberg/i2cdevlib
//
// Changelog:
//     ... - ongoing debug release

// NOTE: THIS IS ONLY A PARIAL RELEASE. THIS DEVICE CLASS IS CURRENTLY UNDERGOING ACTIVE
// DEVELOPMENT AND IS STILL MISSING SOME IMPORTANT FEATURES. PLEASE KEEP THIS IN MIND IF
// YOU DECIDE TO USE THIS PARTICULAR CODE FOR ANYTHING.

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

//#include "I2Cdev.h"

// special methods for MotionApps 2.0 implementation
//#define MPU6050_INCLUDE_DMP_MOTIONAPPS20

// special methods for MotionApps 4.1 implementation
//#define MPU6050_INCLUDE_DMP_MOTIONAPPS41

using System;
using Microsoft.SPOT;

namespace Audio_Analysis_and_Head_Tracking
{
    public class MPU6050 {

        private byte devAddr;
        private byte[] buffer;

        #region Registers
        #pragma warning disable 414
        private const byte MPU9150_RA_MAG_ADDRESS = 0x0C;

        private const byte MPU9150_RA_MAG_XOUT_L		= 0x03;
        private const byte MPU9150_RA_MAG_XOUT_H		= 0x04;
        private const byte MPU9150_RA_MAG_YOUT_L		= 0x05;
        private const byte MPU9150_RA_MAG_YOUT_H		= 0x06;
        private const byte MPU9150_RA_MAG_ZOUT_L		= 0x07;
        private const byte MPU9150_RA_MAG_ZOUT_H		= 0x08;

        private const byte MPU6050_ADDRESS_AD0_LOW     = 0x68; // address pin low (GND), default for InvenSense evaluation board
        private const byte MPU6050_ADDRESS_AD0_HIGH    = 0x69; // address pin high (VCC)
        private const byte MPU6050_DEFAULT_ADDRESS     = MPU6050_ADDRESS_AD0_LOW;

        private const byte MPU6050_RA_XG_OFFS_TC       = 0x00; //[7] PWR_MODE, [6:1] XG_OFFS_TC, [0] OTP_BNK_VLD
        private const byte MPU6050_RA_YG_OFFS_TC       = 0x01; //[7] PWR_MODE, [6:1] YG_OFFS_TC, [0] OTP_BNK_VLD
        private const byte MPU6050_RA_ZG_OFFS_TC       = 0x02; //[7] PWR_MODE, [6:1] ZG_OFFS_TC, [0] OTP_BNK_VLD
        private const byte MPU6050_RA_X_FINE_GAIN      = 0x03; //[7:0] X_FINE_GAIN
        private const byte MPU6050_RA_Y_FINE_GAIN      = 0x04; //[7:0] Y_FINE_GAIN
        private const byte MPU6050_RA_Z_FINE_GAIN      = 0x05; //[7:0] Z_FINE_GAIN
        private const byte MPU6050_RA_XA_OFFS_H        = 0x06; //[15:0] XA_OFFS
        private const byte MPU6050_RA_XA_OFFS_L_TC     = 0x07;
        private const byte MPU6050_RA_YA_OFFS_H        = 0x08; //[15:0] YA_OFFS
        private const byte MPU6050_RA_YA_OFFS_L_TC     = 0x09;
        private const byte MPU6050_RA_ZA_OFFS_H        = 0x0A; //[15:0] ZA_OFFS
        private const byte MPU6050_RA_ZA_OFFS_L_TC     = 0x0B;
        private const byte MPU6050_RA_XG_OFFS_USRH     = 0x13; //[15:0] XG_OFFS_USR
        private const byte MPU6050_RA_XG_OFFS_USRL     = 0x14;
        private const byte MPU6050_RA_YG_OFFS_USRH     = 0x15; //[15:0] YG_OFFS_USR
        private const byte MPU6050_RA_YG_OFFS_USRL     = 0x16;
        private const byte MPU6050_RA_ZG_OFFS_USRH     = 0x17; //[15:0] ZG_OFFS_USR
        private const byte MPU6050_RA_ZG_OFFS_USRL     = 0x18;
        private const byte MPU6050_RA_SMPLRT_DIV       = 0x19;
        private const byte MPU6050_RA_CONFIG           = 0x1A;
        private const byte MPU6050_RA_GYRO_CONFIG      = 0x1B;
        private const byte MPU6050_RA_ACCEL_CONFIG     = 0x1C;
        private const byte MPU6050_RA_FF_THR           = 0x1D;
        private const byte MPU6050_RA_FF_DUR           = 0x1E;
        private const byte MPU6050_RA_MOT_THR          = 0x1F;
        private const byte MPU6050_RA_MOT_DUR          = 0x20;
        private const byte MPU6050_RA_ZRMOT_THR        = 0x21;
        private const byte MPU6050_RA_ZRMOT_DUR        = 0x22;
        private const byte MPU6050_RA_FIFO_EN          = 0x23;
        private const byte MPU6050_RA_I2C_MST_CTRL     = 0x24;
        private const byte MPU6050_RA_I2C_SLV0_ADDR    = 0x25;
        private const byte MPU6050_RA_I2C_SLV0_REG     = 0x26;
        private const byte MPU6050_RA_I2C_SLV0_CTRL    = 0x27;
        private const byte MPU6050_RA_I2C_SLV1_ADDR    = 0x28;
        private const byte MPU6050_RA_I2C_SLV1_REG     = 0x29;
        private const byte MPU6050_RA_I2C_SLV1_CTRL    = 0x2A;
        private const byte MPU6050_RA_I2C_SLV2_ADDR    = 0x2B;
        private const byte MPU6050_RA_I2C_SLV2_REG     = 0x2C;
        private const byte MPU6050_RA_I2C_SLV2_CTRL    = 0x2D;
        private const byte MPU6050_RA_I2C_SLV3_ADDR    = 0x2E;
        private const byte MPU6050_RA_I2C_SLV3_REG     = 0x2F;
        private const byte MPU6050_RA_I2C_SLV3_CTRL    = 0x30;
        private const byte MPU6050_RA_I2C_SLV4_ADDR    = 0x31;
        private const byte MPU6050_RA_I2C_SLV4_REG     = 0x32;
        private const byte MPU6050_RA_I2C_SLV4_DO      = 0x33;
        private const byte MPU6050_RA_I2C_SLV4_CTRL    = 0x34;
        private const byte MPU6050_RA_I2C_SLV4_DI      = 0x35;
        private const byte MPU6050_RA_I2C_MST_STATUS   = 0x36;
        private const byte MPU6050_RA_INT_PIN_CFG      = 0x37;
        private const byte MPU6050_RA_INT_ENABLE       = 0x38;
        private const byte MPU6050_RA_DMP_INT_STATUS   = 0x39;
        private const byte MPU6050_RA_INT_STATUS       = 0x3A;
        private const byte MPU6050_RA_ACCEL_XOUT_H     = 0x3B;
        private const byte MPU6050_RA_ACCEL_XOUT_L     = 0x3C;
        private const byte MPU6050_RA_ACCEL_YOUT_H     = 0x3D;
        private const byte MPU6050_RA_ACCEL_YOUT_L     = 0x3E;
        private const byte MPU6050_RA_ACCEL_ZOUT_H     = 0x3F;
        private const byte MPU6050_RA_ACCEL_ZOUT_L     = 0x40;
        private const byte MPU6050_RA_TEMP_OUT_H       = 0x41;
        private const byte MPU6050_RA_TEMP_OUT_L       = 0x42;
        private const byte MPU6050_RA_GYRO_XOUT_H      = 0x43;
        private const byte MPU6050_RA_GYRO_XOUT_L      = 0x44;
        private const byte MPU6050_RA_GYRO_YOUT_H      = 0x45;
        private const byte MPU6050_RA_GYRO_YOUT_L      = 0x46;
        private const byte MPU6050_RA_GYRO_ZOUT_H      = 0x47;
        private const byte MPU6050_RA_GYRO_ZOUT_L      = 0x48;
        private const byte MPU6050_RA_EXT_SENS_DATA_00 = 0x49;
        private const byte MPU6050_RA_EXT_SENS_DATA_01 = 0x4A;
        private const byte MPU6050_RA_EXT_SENS_DATA_02 = 0x4B;
        private const byte MPU6050_RA_EXT_SENS_DATA_03 = 0x4C;
        private const byte MPU6050_RA_EXT_SENS_DATA_04 = 0x4D;
        private const byte MPU6050_RA_EXT_SENS_DATA_05 = 0x4E;
        private const byte MPU6050_RA_EXT_SENS_DATA_06 = 0x4F;
        private const byte MPU6050_RA_EXT_SENS_DATA_07 = 0x50;
        private const byte MPU6050_RA_EXT_SENS_DATA_08 = 0x51;
        private const byte MPU6050_RA_EXT_SENS_DATA_09 = 0x52;
        private const byte MPU6050_RA_EXT_SENS_DATA_10 = 0x53;
        private const byte MPU6050_RA_EXT_SENS_DATA_11 = 0x54;
        private const byte MPU6050_RA_EXT_SENS_DATA_12 = 0x55;
        private const byte MPU6050_RA_EXT_SENS_DATA_13 = 0x56;
        private const byte MPU6050_RA_EXT_SENS_DATA_14 = 0x57;
        private const byte MPU6050_RA_EXT_SENS_DATA_15 = 0x58;
        private const byte MPU6050_RA_EXT_SENS_DATA_16 = 0x59;
        private const byte MPU6050_RA_EXT_SENS_DATA_17 = 0x5A;
        private const byte MPU6050_RA_EXT_SENS_DATA_18 = 0x5B;
        private const byte MPU6050_RA_EXT_SENS_DATA_19 = 0x5C;
        private const byte MPU6050_RA_EXT_SENS_DATA_20 = 0x5D;
        private const byte MPU6050_RA_EXT_SENS_DATA_21 = 0x5E;
        private const byte MPU6050_RA_EXT_SENS_DATA_22 = 0x5F;
        private const byte MPU6050_RA_EXT_SENS_DATA_23 = 0x60;
        private const byte MPU6050_RA_MOT_DETECT_STATUS    = 0x61;
        private const byte MPU6050_RA_I2C_SLV0_DO      = 0x63;
        private const byte MPU6050_RA_I2C_SLV1_DO      = 0x64;
        private const byte MPU6050_RA_I2C_SLV2_DO      = 0x65;
        private const byte MPU6050_RA_I2C_SLV3_DO      = 0x66;
        private const byte MPU6050_RA_I2C_MST_DELAY_CTRL   = 0x67;
        private const byte MPU6050_RA_SIGNAL_PATH_RESET    = 0x68;
        private const byte MPU6050_RA_MOT_DETECT_CTRL      = 0x69;
        private const byte MPU6050_RA_USER_CTRL        = 0x6A;
        private const byte MPU6050_RA_PWR_MGMT_1       = 0x6B;
        private const byte MPU6050_RA_PWR_MGMT_2       = 0x6C;
        private const byte MPU6050_RA_BANK_SEL         = 0x6D;
        private const byte MPU6050_RA_MEM_START_ADDR   = 0x6E;
        private const byte MPU6050_RA_MEM_R_W          = 0x6F;
        private const byte MPU6050_RA_DMP_CFG_1        = 0x70;
        private const byte MPU6050_RA_DMP_CFG_2        = 0x71;
        private const byte MPU6050_RA_FIFO_COUNTH      = 0x72;
        private const byte MPU6050_RA_FIFO_COUNTL      = 0x73;
        private const byte MPU6050_RA_FIFO_R_W         = 0x74;
        private const byte MPU6050_RA_WHO_AM_I         = 0x75;

        private const byte MPU6050_TC_PWR_MODE_BIT     = 7;
        private const byte MPU6050_TC_OFFSET_BIT       = 6;
        private const byte MPU6050_TC_OFFSET_LENGTH    = 6;
        private const byte MPU6050_TC_OTP_BNK_VLD_BIT  = 0;

        private const byte MPU6050_VDDIO_LEVEL_VLOGIC  = 0;
        private const byte MPU6050_VDDIO_LEVEL_VDD     = 1;

        private const byte MPU6050_CFG_EXT_SYNC_SET_BIT    = 5;
        private const byte MPU6050_CFG_EXT_SYNC_SET_LENGTH = 3;
        private const byte MPU6050_CFG_DLPF_CFG_BIT    = 2;
        private const byte MPU6050_CFG_DLPF_CFG_LENGTH = 3;

        private const byte MPU6050_EXT_SYNC_DISABLED       = 0x0;
        private const byte MPU6050_EXT_SYNC_TEMP_OUT_L     = 0x1;
        private const byte MPU6050_EXT_SYNC_GYRO_XOUT_L    = 0x2;
        private const byte MPU6050_EXT_SYNC_GYRO_YOUT_L    = 0x3;
        private const byte MPU6050_EXT_SYNC_GYRO_ZOUT_L    = 0x4;
        private const byte MPU6050_EXT_SYNC_ACCEL_XOUT_L   = 0x5;
        private const byte MPU6050_EXT_SYNC_ACCEL_YOUT_L   = 0x6;
        private const byte MPU6050_EXT_SYNC_ACCEL_ZOUT_L   = 0x7;

        private const byte MPU6050_DLPF_BW_256         = 0x00;
        private const byte MPU6050_DLPF_BW_188         = 0x01;
        private const byte MPU6050_DLPF_BW_98          = 0x02;
        private const byte MPU6050_DLPF_BW_42          = 0x03;
        private const byte MPU6050_DLPF_BW_20          = 0x04;
        private const byte MPU6050_DLPF_BW_10          = 0x05;
        private const byte MPU6050_DLPF_BW_5           = 0x06;

        private const byte MPU6050_GCONFIG_FS_SEL_BIT      = 4;
        private const byte MPU6050_GCONFIG_FS_SEL_LENGTH   = 2;

        private const byte MPU6050_GYRO_FS_250         = 0x00;
        private const byte MPU6050_GYRO_FS_500         = 0x01;
        private const byte MPU6050_GYRO_FS_1000        = 0x02;
        private const byte MPU6050_GYRO_FS_2000        = 0x03;

        private const byte MPU6050_ACONFIG_XA_ST_BIT           = 7;
        private const byte MPU6050_ACONFIG_YA_ST_BIT           = 6;
        private const byte MPU6050_ACONFIG_ZA_ST_BIT           = 5;
        private const byte MPU6050_ACONFIG_AFS_SEL_BIT         = 4;
        private const byte MPU6050_ACONFIG_AFS_SEL_LENGTH      = 2;
        private const byte MPU6050_ACONFIG_ACCEL_HPF_BIT       = 2;
        private const byte MPU6050_ACONFIG_ACCEL_HPF_LENGTH    = 3;

        private const byte MPU6050_ACCEL_FS_2          = 0x00;
        private const byte MPU6050_ACCEL_FS_4          = 0x01;
        private const byte MPU6050_ACCEL_FS_8          = 0x02;
        private const byte MPU6050_ACCEL_FS_16         = 0x03;

        private const byte MPU6050_DHPF_RESET          = 0x00;
        private const byte MPU6050_DHPF_5              = 0x01;
        private const byte MPU6050_DHPF_2P5            = 0x02;
        private const byte MPU6050_DHPF_1P25           = 0x03;
        private const byte MPU6050_DHPF_0P63           = 0x04;
        private const byte MPU6050_DHPF_HOLD           = 0x07;

        private const byte MPU6050_TEMP_FIFO_EN_BIT    = 7;
        private const byte MPU6050_XG_FIFO_EN_BIT      = 6;
        private const byte MPU6050_YG_FIFO_EN_BIT      = 5;
        private const byte MPU6050_ZG_FIFO_EN_BIT      = 4;
        private const byte MPU6050_ACCEL_FIFO_EN_BIT   = 3;
        private const byte MPU6050_SLV2_FIFO_EN_BIT    = 2;
        private const byte MPU6050_SLV1_FIFO_EN_BIT    = 1;
        private const byte MPU6050_SLV0_FIFO_EN_BIT    = 0;

        private const byte MPU6050_MULT_MST_EN_BIT     = 7;
        private const byte MPU6050_WAIT_FOR_ES_BIT     = 6;
        private const byte MPU6050_SLV_3_FIFO_EN_BIT   = 5;
        private const byte MPU6050_I2C_MST_P_NSR_BIT   = 4;
        private const byte MPU6050_I2C_MST_CLK_BIT     = 3;
        private const byte MPU6050_I2C_MST_CLK_LENGTH  = 4;

        private const byte MPU6050_CLOCK_DIV_348       = 0x0;
        private const byte MPU6050_CLOCK_DIV_333       = 0x1;
        private const byte MPU6050_CLOCK_DIV_320       = 0x2;
        private const byte MPU6050_CLOCK_DIV_308       = 0x3;
        private const byte MPU6050_CLOCK_DIV_296       = 0x4;
        private const byte MPU6050_CLOCK_DIV_286       = 0x5;
        private const byte MPU6050_CLOCK_DIV_276       = 0x6;
        private const byte MPU6050_CLOCK_DIV_267       = 0x7;
        private const byte MPU6050_CLOCK_DIV_258       = 0x8;
        private const byte MPU6050_CLOCK_DIV_500       = 0x9;
        private const byte MPU6050_CLOCK_DIV_471       = 0xA;
        private const byte MPU6050_CLOCK_DIV_444       = 0xB;
        private const byte MPU6050_CLOCK_DIV_421       = 0xC;
        private const byte MPU6050_CLOCK_DIV_400       = 0xD;
        private const byte MPU6050_CLOCK_DIV_381       = 0xE;
        private const byte MPU6050_CLOCK_DIV_364       = 0xF;

        private const byte MPU6050_I2C_SLV_RW_BIT      = 7;
        private const byte MPU6050_I2C_SLV_ADDR_BIT    = 6;
        private const byte MPU6050_I2C_SLV_ADDR_LENGTH = 7;
        private const byte MPU6050_I2C_SLV_EN_BIT      = 7;
        private const byte MPU6050_I2C_SLV_BYTE_SW_BIT = 6;
        private const byte MPU6050_I2C_SLV_REG_DIS_BIT = 5;
        private const byte MPU6050_I2C_SLV_GRP_BIT     = 4;
        private const byte MPU6050_I2C_SLV_LEN_BIT     = 3;
        private const byte MPU6050_I2C_SLV_LEN_LENGTH  = 4;

        private const byte MPU6050_I2C_SLV4_RW_BIT         = 7;
        private const byte MPU6050_I2C_SLV4_ADDR_BIT       = 6;
        private const byte MPU6050_I2C_SLV4_ADDR_LENGTH    = 7;
        private const byte MPU6050_I2C_SLV4_EN_BIT         = 7;
        private const byte MPU6050_I2C_SLV4_INT_EN_BIT     = 6;
        private const byte MPU6050_I2C_SLV4_REG_DIS_BIT    = 5;
        private const byte MPU6050_I2C_SLV4_MST_DLY_BIT    = 4;
        private const byte MPU6050_I2C_SLV4_MST_DLY_LENGTH = 5;

        private const byte MPU6050_MST_PASS_THROUGH_BIT    = 7;
        private const byte MPU6050_MST_I2C_SLV4_DONE_BIT   = 6;
        private const byte MPU6050_MST_I2C_LOST_ARB_BIT    = 5;
        private const byte MPU6050_MST_I2C_SLV4_NACK_BIT   = 4;
        private const byte MPU6050_MST_I2C_SLV3_NACK_BIT   = 3;
        private const byte MPU6050_MST_I2C_SLV2_NACK_BIT   = 2;
        private const byte MPU6050_MST_I2C_SLV1_NACK_BIT   = 1;
        private const byte MPU6050_MST_I2C_SLV0_NACK_BIT   = 0;

        private const byte MPU6050_INTCFG_INT_LEVEL_BIT        = 7;
        private const byte MPU6050_INTCFG_INT_OPEN_BIT         = 6;
        private const byte MPU6050_INTCFG_LATCH_INT_EN_BIT     = 5;
        private const byte MPU6050_INTCFG_INT_RD_CLEAR_BIT     = 4;
        private const byte MPU6050_INTCFG_FSYNC_INT_LEVEL_BIT  = 3;
        private const byte MPU6050_INTCFG_FSYNC_INT_EN_BIT     = 2;
        private const byte MPU6050_INTCFG_I2C_BYPASS_EN_BIT    = 1;
        private const byte MPU6050_INTCFG_CLKOUT_EN_BIT        = 0;

        private const byte MPU6050_INTMODE_ACTIVEHIGH  = 0x00;
        private const byte MPU6050_INTMODE_ACTIVELOW   = 0x01;

        private const byte MPU6050_INTDRV_PUSHPULL     = 0x00;
        private const byte MPU6050_INTDRV_OPENDRAIN    = 0x01;

        private const byte MPU6050_INTLATCH_50USPULSE  = 0x00;
        private const byte MPU6050_INTLATCH_WAITCLEAR  = 0x01;

        private const byte MPU6050_INTCLEAR_STATUSREAD = 0x00;
        private const byte MPU6050_INTCLEAR_ANYREAD    = 0x01;

        private const byte MPU6050_INTERRUPT_FF_BIT            = 7;
        private const byte MPU6050_INTERRUPT_MOT_BIT           = 6;
        private const byte MPU6050_INTERRUPT_ZMOT_BIT          = 5;
        private const byte MPU6050_INTERRUPT_FIFO_OFLOW_BIT    = 4;
        private const byte MPU6050_INTERRUPT_I2C_MST_INT_BIT   = 3;
        private const byte MPU6050_INTERRUPT_PLL_RDY_INT_BIT   = 2;
        private const byte MPU6050_INTERRUPT_DMP_INT_BIT       = 1;
        private const byte MPU6050_INTERRUPT_DATA_RDY_BIT      = 0;

        // TODO: figure out what these actually do
        // UMPL source code is not very obivous
        private const byte MPU6050_DMPINT_5_BIT            = 5;
        private const byte MPU6050_DMPINT_4_BIT            = 4;
        private const byte MPU6050_DMPINT_3_BIT            = 3;
        private const byte MPU6050_DMPINT_2_BIT            = 2;
        private const byte MPU6050_DMPINT_1_BIT            = 1;
        private const byte MPU6050_DMPINT_0_BIT            = 0;

        private const byte MPU6050_MOTION_MOT_XNEG_BIT     = 7;
        private const byte MPU6050_MOTION_MOT_XPOS_BIT     = 6;
        private const byte MPU6050_MOTION_MOT_YNEG_BIT     = 5;
        private const byte MPU6050_MOTION_MOT_YPOS_BIT     = 4;
        private const byte MPU6050_MOTION_MOT_ZNEG_BIT     = 3;
        private const byte MPU6050_MOTION_MOT_ZPOS_BIT     = 2;
        private const byte MPU6050_MOTION_MOT_ZRMOT_BIT    = 0;

        private const byte MPU6050_DELAYCTRL_DELAY_ES_SHADOW_BIT   = 7;
        private const byte MPU6050_DELAYCTRL_I2C_SLV4_DLY_EN_BIT   = 4;
        private const byte MPU6050_DELAYCTRL_I2C_SLV3_DLY_EN_BIT   = 3;
        private const byte MPU6050_DELAYCTRL_I2C_SLV2_DLY_EN_BIT   = 2;
        private const byte MPU6050_DELAYCTRL_I2C_SLV1_DLY_EN_BIT   = 1;
        private const byte MPU6050_DELAYCTRL_I2C_SLV0_DLY_EN_BIT   = 0;

        private const byte MPU6050_PATHRESET_GYRO_RESET_BIT    = 2;
        private const byte MPU6050_PATHRESET_ACCEL_RESET_BIT   = 1;
        private const byte MPU6050_PATHRESET_TEMP_RESET_BIT    = 0;

        private const byte MPU6050_DETECT_ACCEL_ON_DELAY_BIT       = 5;
        private const byte MPU6050_DETECT_ACCEL_ON_DELAY_LENGTH    = 2;
        private const byte MPU6050_DETECT_FF_COUNT_BIT             = 3;
        private const byte MPU6050_DETECT_FF_COUNT_LENGTH          = 2;
        private const byte MPU6050_DETECT_MOT_COUNT_BIT            = 1;
        private const byte MPU6050_DETECT_MOT_COUNT_LENGTH         = 2;

        private const byte MPU6050_DETECT_DECREMENT_RESET  = 0x0;
        private const byte MPU6050_DETECT_DECREMENT_1      = 0x1;
        private const byte MPU6050_DETECT_DECREMENT_2      = 0x2;
        private const byte MPU6050_DETECT_DECREMENT_4      = 0x3;

        private const byte MPU6050_USERCTRL_DMP_EN_BIT             = 7;
        private const byte MPU6050_USERCTRL_FIFO_EN_BIT            = 6;
        private const byte MPU6050_USERCTRL_I2C_MST_EN_BIT         = 5;
        private const byte MPU6050_USERCTRL_I2C_IF_DIS_BIT         = 4;
        private const byte MPU6050_USERCTRL_DMP_RESET_BIT          = 3;
        private const byte MPU6050_USERCTRL_FIFO_RESET_BIT         = 2;
        private const byte MPU6050_USERCTRL_I2C_MST_RESET_BIT      = 1;
        private const byte MPU6050_USERCTRL_SIG_COND_RESET_BIT     = 0;

        private const byte MPU6050_PWR1_DEVICE_RESET_BIT   = 7;
        private const byte MPU6050_PWR1_SLEEP_BIT          = 6;
        private const byte MPU6050_PWR1_CYCLE_BIT          = 5;
        private const byte MPU6050_PWR1_TEMP_DIS_BIT       = 3;
        private const byte MPU6050_PWR1_CLKSEL_BIT         = 2;
        private const byte MPU6050_PWR1_CLKSEL_LENGTH      = 3;

        private const byte MPU6050_CLOCK_INTERNAL          = 0x00;
        private const byte MPU6050_CLOCK_PLL_XGYRO         = 0x01;
        private const byte MPU6050_CLOCK_PLL_YGYRO         = 0x02;
        private const byte MPU6050_CLOCK_PLL_ZGYRO         = 0x03;
        private const byte MPU6050_CLOCK_PLL_EXT32K        = 0x04;
        private const byte MPU6050_CLOCK_PLL_EXT19M        = 0x05;
        private const byte MPU6050_CLOCK_KEEP_RESET        = 0x07;

        private const byte MPU6050_PWR2_LP_WAKE_CTRL_BIT       = 7;
        private const byte MPU6050_PWR2_LP_WAKE_CTRL_LENGTH    = 2;
        private const byte MPU6050_PWR2_STBY_XA_BIT            = 5;
        private const byte MPU6050_PWR2_STBY_YA_BIT            = 4;
        private const byte MPU6050_PWR2_STBY_ZA_BIT            = 3;
        private const byte MPU6050_PWR2_STBY_XG_BIT            = 2;
        private const byte MPU6050_PWR2_STBY_YG_BIT            = 1;
        private const byte MPU6050_PWR2_STBY_ZG_BIT            = 0;

        private const byte MPU6050_WAKE_FREQ_1P25      = 0x0;
        private const byte MPU6050_WAKE_FREQ_2P5       = 0x1;
        private const byte MPU6050_WAKE_FREQ_5         = 0x2;
        private const byte MPU6050_WAKE_FREQ_10        = 0x3;

        private const byte MPU6050_BANKSEL_PRFTCH_EN_BIT       = 6;
        private const byte MPU6050_BANKSEL_CFG_USER_BANK_BIT   = 5;
        private const byte MPU6050_BANKSEL_MEM_SEL_BIT         = 4;
        private const byte MPU6050_BANKSEL_MEM_SEL_LENGTH      = 5;

        private const byte MPU6050_WHO_AM_I_BIT        = 6;
        private const byte MPU6050_WHO_AM_I_LENGTH     = 6;

        private const byte MPU6050_DMP_MEMORY_BANKS        = 8;
        //private const byte MPU6050_DMP_MEMORY_BANK_SIZE    = 256;
        private const byte MPU6050_DMP_MEMORY_CHUNK_SIZE   = 16;

        // note: DMP code memory blocks defined at end of header file
        #pragma warning restore 414
        #endregion

        I2CDev _i2c;

        /** Default constructor, uses default I2C address.
         * @see MPU6050_DEFAULT_ADDRESS
         */
        public MPU6050() {
            devAddr = MPU6050_DEFAULT_ADDRESS;
        }

        /** Specific address constructor.
         * @param address I2C address
         * @see MPU6050_DEFAULT_ADDRESS
         * @see MPU6050_ADDRESS_AD0_LOW
         * @see MPU6050_ADDRESS_AD0_HIGH
         */
        public MPU6050(byte address)
        {
            devAddr = address;
        }

        /** Power on and prepare for general usage.
         * This will activate the device and take it out of sleep mode (which must be done
         * after start-up). This function also sets both the accelerometer and the gyroscope
         * to their most sensitive settings, namely +/- 2g and +/- 250 degrees/sec, and sets
         * the clock source to use the X Gyro for reference, which is slightly better than
         * the default internal clock source.
         */
        public void Initialize()
        {
            _i2c = new I2CDev();
            _i2c.initialize(devAddr);

            buffer = new byte[14];
            
         //   setClockSource(MPU6050_CLOCK_PLL_XGYRO);
         //   setFullScaleGyroRange(MPU6050_GYRO_FS_250);
         //   setFullScaleAccelRange(MPU6050_ACCEL_FS_2);
         //   setSleepEnabled(false); // thanks to Jack Elston for pointing this one out!
        }

        /** Verify the I2C connection.
         * Make sure the device is connected and responds as expected.
         * @return True if connection is valid, false otherwise
         */
        public bool testConnection() {
            return getDeviceID() == 0x34;
        }

        /** Get gyroscope output rate divider.
         * The sensor register output, FIFO output, DMP sampling, Motion detection, Zero
         * Motion detection, and Free Fall detection are all based on the Sample Rate.
         * The Sample Rate is generated by dividing the gyroscope output rate by
         * SMPLRT_DIV:
         *
         * Sample Rate = Gyroscope Output Rate / (1 + SMPLRT_DIV)
         *
         * where Gyroscope Output Rate = 8kHz when the DLPF is disabled (DLPF_CFG = 0 or
         * 7), and 1kHz when the DLPF is enabled (see Register 26).
         *
         * Note: The accelerometer output rate is 1kHz. This means that for a Sample
         * Rate greater than 1kHz, the same accelerometer sample may be output to the
         * FIFO, DMP, and sensor registers more than once.
         *
         * For a diagram of the gyroscope and accelerometer signal paths, see Section 8
         * of the MPU-6000/MPU-6050 Product Specification document.
         *
         * @return Current sample rate
         * @see MPU6050_RA_SMPLRT_DIV
         */
        byte getRate() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_SMPLRT_DIV, b);
            return b;
        } 

        byte checkMagStatus() {
            byte b = 0;
            _i2c.readByte(MPU9150_RA_MAG_ADDRESS, 0x02, b);
            return b;
        }


        /** Set gyroscope sample rate divider.
         * @param rate New sample rate divider
         * @see getRate()
         * @see MPU6050_RA_SMPLRT_DIV
         */
        void setRate(byte rate) {
            _i2c.writeByte(devAddr, MPU6050_RA_SMPLRT_DIV, rate);
        }

        // CONFIG register

        /** Get external FSYNC configuration.
         * Configures the external Frame Synchronization (FSYNC) pin sampling. An
         * external signal connected to the FSYNC pin can be sampled by configuring
         * EXT_SYNC_SET. Signal changes to the FSYNC pin are latched so that short
         * strobes may be captured. The latched FSYNC signal will be sampled at the
         * Sampling Rate, as defined in register 25. After sampling, the latch will
         * reset to the current FSYNC signal state.
         *
         * The sampled value will be reported in place of the least significant bit in
         * a sensor data register determined by the value of EXT_SYNC_SET according to
         * the following table.
         *
         * <pre>
         * EXT_SYNC_SET | FSYNC Bit Location
         * -------------+-------------------
         * 0            | Input disabled
         * 1            | TEMP_OUT_L[0]
         * 2            | GYRO_XOUT_L[0]
         * 3            | GYRO_YOUT_L[0]
         * 4            | GYRO_ZOUT_L[0]
         * 5            | ACCEL_XOUT_L[0]
         * 6            | ACCEL_YOUT_L[0]
         * 7            | ACCEL_ZOUT_L[0]
         * </pre>
         *
         * @return FSYNC configuration value
         */
        byte getExternalFrameSync() {
            byte b = 0;
            _i2c.readBits(devAddr, MPU6050_RA_CONFIG, MPU6050_CFG_EXT_SYNC_SET_BIT, MPU6050_CFG_EXT_SYNC_SET_LENGTH, b);
            return b;
        }
        /** Set external FSYNC configuration.
         * @see getExternalFrameSync()
         * @see MPU6050_RA_CONFIG
         * @param sync New FSYNC configuration value
         */
        void setExternalFrameSync(byte sync) {
            _i2c.writeBits(devAddr, MPU6050_RA_CONFIG, MPU6050_CFG_EXT_SYNC_SET_BIT, MPU6050_CFG_EXT_SYNC_SET_LENGTH, sync);
        }
        /** Get digital low-pass filter configuration.
         * The DLPF_CFG parameter sets the digital low pass filter configuration. It
         * also determines the internal sampling rate used by the device as shown in
         * the table below.
         *
         * Note: The accelerometer output rate is 1kHz. This means that for a Sample
         * Rate greater than 1kHz, the same accelerometer sample may be output to the
         * FIFO, DMP, and sensor registers more than once.
         *
         * <pre>
         *          |   ACCELEROMETER    |           GYROSCOPE
         * DLPF_CFG | Bandwidth | Delay  | Bandwidth | Delay  | Sample Rate
         * ---------+-----------+--------+-----------+--------+-------------
         * 0        | 260Hz     | 0ms    | 256Hz     | 0.98ms | 8kHz
         * 1        | 184Hz     | 2.0ms  | 188Hz     | 1.9ms  | 1kHz
         * 2        | 94Hz      | 3.0ms  | 98Hz      | 2.8ms  | 1kHz
         * 3        | 44Hz      | 4.9ms  | 42Hz      | 4.8ms  | 1kHz
         * 4        | 21Hz      | 8.5ms  | 20Hz      | 8.3ms  | 1kHz
         * 5        | 10Hz      | 13.8ms | 10Hz      | 13.4ms | 1kHz
         * 6        | 5Hz       | 19.0ms | 5Hz       | 18.6ms | 1kHz
         * 7        |   -- Reserved --   |   -- Reserved --   | Reserved
         * </pre>
         *
         * @return DLFP configuration
         * @see MPU6050_RA_CONFIG
         * @see MPU6050_CFG_DLPF_CFG_BIT
         * @see MPU6050_CFG_DLPF_CFG_LENGTH
         */
        byte getDLPFMode() {
            byte b = 0;
            _i2c.readBits(devAddr, MPU6050_RA_CONFIG, MPU6050_CFG_DLPF_CFG_BIT, MPU6050_CFG_DLPF_CFG_LENGTH, b);
            return b;
        }
        /** Set digital low-pass filter configuration.
         * @param mode New DLFP configuration setting
         * @see getDLPFBandwidth()
         * @see MPU6050_DLPF_BW_256
         * @see MPU6050_RA_CONFIG
         * @see MPU6050_CFG_DLPF_CFG_BIT
         * @see MPU6050_CFG_DLPF_CFG_LENGTH
         */
        void setDLPFMode(byte mode) {
            _i2c.writeBits(devAddr, MPU6050_RA_CONFIG, MPU6050_CFG_DLPF_CFG_BIT, MPU6050_CFG_DLPF_CFG_LENGTH, mode);
        }

        // GYRO_CONFIG register

        /** Get full-scale gyroscope range.
         * The FS_SEL parameter allows setting the full-scale range of the gyro sensors,
         * as described in the table below.
         *
         * <pre>
         * 0 = +/- 250 degrees/sec
         * 1 = +/- 500 degrees/sec
         * 2 = +/- 1000 degrees/sec
         * 3 = +/- 2000 degrees/sec
         * </pre>
         *
         * @return Current full-scale gyroscope range setting
         * @see MPU6050_GYRO_FS_250
         * @see MPU6050_RA_GYRO_CONFIG
         * @see MPU6050_GCONFIG_FS_SEL_BIT
         * @see MPU6050_GCONFIG_FS_SEL_LENGTH
         */
        uint getFullScaleGyroRange() {
            byte b = 0;
            _i2c.readBits(devAddr, MPU6050_RA_GYRO_CONFIG, MPU6050_GCONFIG_FS_SEL_BIT, MPU6050_GCONFIG_FS_SEL_LENGTH, b);
            return b;
        }
        /** Set full-scale gyroscope range.
         * @param range New full-scale gyroscope range value
         * @see getFullScaleRange()
         * @see MPU6050_GYRO_FS_250
         * @see MPU6050_RA_GYRO_CONFIG
         * @see MPU6050_GCONFIG_FS_SEL_BIT
         * @see MPU6050_GCONFIG_FS_SEL_LENGTH
         */
        void setFullScaleGyroRange(byte range) {
            _i2c.writeBits(devAddr, MPU6050_RA_GYRO_CONFIG, MPU6050_GCONFIG_FS_SEL_BIT, MPU6050_GCONFIG_FS_SEL_LENGTH, range);
        }

        // ACCEL_CONFIG register

        /** Get self-test enabled setting for accelerometer X axis.
         * @return Self-test enabled value
         * @see MPU6050_RA_ACCEL_CONFIG
         */
        bool getAccelXSelfTest() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_ACCEL_CONFIG, MPU6050_ACONFIG_XA_ST_BIT, b);
            return b == 1;
        }
        /** Get self-test enabled setting for accelerometer X axis.
         * @param enabled Self-test enabled value
         * @see MPU6050_RA_ACCEL_CONFIG
         */
        void setAccelXSelfTest(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_ACCEL_CONFIG, MPU6050_ACONFIG_XA_ST_BIT, (byte)(enabled ? 0 : 1));
        }
        /** Get self-test enabled value for accelerometer Y axis.
         * @return Self-test enabled value
         * @see MPU6050_RA_ACCEL_CONFIG
         */
        bool getAccelYSelfTest() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_ACCEL_CONFIG, MPU6050_ACONFIG_YA_ST_BIT, b);
            return b == 1;
        }
        /** Get self-test enabled value for accelerometer Y axis.
         * @param enabled Self-test enabled value
         * @see MPU6050_RA_ACCEL_CONFIG
         */
        void setAccelYSelfTest(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_ACCEL_CONFIG, MPU6050_ACONFIG_YA_ST_BIT, (byte)(enabled ? 0 : 1));
        }
        /** Get self-test enabled value for accelerometer Z axis.
         * @return Self-test enabled value
         * @see MPU6050_RA_ACCEL_CONFIG
         */
        bool getAccelZSelfTest() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_ACCEL_CONFIG, MPU6050_ACONFIG_ZA_ST_BIT, b);
            return b == 1;
        }
        /** Set self-test enabled value for accelerometer Z axis.
         * @param enabled Self-test enabled value
         * @see MPU6050_RA_ACCEL_CONFIG
         */
        void setAccelZSelfTest(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_ACCEL_CONFIG, MPU6050_ACONFIG_ZA_ST_BIT, (byte)(enabled ? 0 : 1));
        }
        /** Get full-scale accelerometer range.
         * The FS_SEL parameter allows setting the full-scale range of the accelerometer
         * sensors, as described in the table below.
         *
         * <pre>
         * 0 = +/- 2g
         * 1 = +/- 4g
         * 2 = +/- 8g
         * 3 = +/- 16g
         * </pre>
         *
         * @return Current full-scale accelerometer range setting
         * @see MPU6050_ACCEL_FS_2
         * @see MPU6050_RA_ACCEL_CONFIG
         * @see MPU6050_ACONFIG_AFS_SEL_BIT
         * @see MPU6050_ACONFIG_AFS_SEL_LENGTH
         */
        uint getFullScaleAccelRange() {
            byte b = 0;
            _i2c.readBits(devAddr, MPU6050_RA_ACCEL_CONFIG, MPU6050_ACONFIG_AFS_SEL_BIT, MPU6050_ACONFIG_AFS_SEL_LENGTH, b);
            return b;
        }
        /** Set full-scale accelerometer range.
         * @param range New full-scale accelerometer range setting
         * @see getFullScaleAccelRange()
         */
        void setFullScaleAccelRange(byte range) {
            _i2c.writeBits(devAddr, MPU6050_RA_ACCEL_CONFIG, MPU6050_ACONFIG_AFS_SEL_BIT, MPU6050_ACONFIG_AFS_SEL_LENGTH, range);
        }
        /** Get the high-pass filter configuration.
         * The DHPF is a filter module in the path leading to motion detectors (Free
         * Fall, Motion threshold, and Zero Motion). The high pass filter output is not
         * available to the data registers (see Figure in Section 8 of the MPU-6000/
         * MPU-6050 Product Specification document).
         *
         * The high pass filter has three modes:
         *
         * <pre>
         *    Reset: The filter output settles to zero within one sample. This
         *           effectively disables the high pass filter. This mode may be toggled
         *           to quickly settle the filter.
         *
         *    On:    The high pass filter will pass signals above the cut off frequency.
         *
         *    Hold:  When triggered, the filter holds the present sample. The filter
         *           output will be the difference between the input sample and the held
         *           sample.
         * </pre>
         *
         * <pre>
         * ACCEL_HPF | Filter Mode | Cut-off Frequency
         * ----------+-------------+------------------
         * 0         | Reset       | None
         * 1         | On          | 5Hz
         * 2         | On          | 2.5Hz
         * 3         | On          | 1.25Hz
         * 4         | On          | 0.63Hz
         * 7         | Hold        | None
         * </pre>
         *
         * @return Current high-pass filter configuration
         * @see MPU6050_DHPF_RESET
         * @see MPU6050_RA_ACCEL_CONFIG
         */
        byte getDHPFMode() {
            byte b = 0;
            _i2c.readBits(devAddr, MPU6050_RA_ACCEL_CONFIG, MPU6050_ACONFIG_ACCEL_HPF_BIT, MPU6050_ACONFIG_ACCEL_HPF_LENGTH, b);
            return b;
        }
        /** Set the high-pass filter configuration.
         * @param bandwidth New high-pass filter configuration
         * @see setDHPFMode()
         * @see MPU6050_DHPF_RESET
         * @see MPU6050_RA_ACCEL_CONFIG
         */
        void setDHPFMode(byte bandwidth) {
            _i2c.writeBits(devAddr, MPU6050_RA_ACCEL_CONFIG, MPU6050_ACONFIG_ACCEL_HPF_BIT, MPU6050_ACONFIG_ACCEL_HPF_LENGTH, bandwidth);
        }

        // FF_THR register

        /** Get free-fall event acceleration threshold.
         * This register configures the detection threshold for Free Fall event
         * detection. The unit of FF_THR is 1LSB = 2mg. Free Fall is detected when the
         * absolute value of the accelerometer measurements for the three axes are each
         * less than the detection threshold. This condition increments the Free Fall
         * duration counter (Register 30). The Free Fall interrupt is triggered when the
         * Free Fall duration counter reaches the time specified in FF_DUR.
         *
         * For more details on the Free Fall detection interrupt, see Section 8.2 of the
         * MPU-6000/MPU-6050 Product Specification document as well as Registers 56 and
         * 58 of this document.
         *
         * @return Current free-fall acceleration threshold value (LSB = 2mg)
         * @see MPU6050_RA_FF_THR
         */
        byte getFreefallDetectionThreshold() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_FF_THR, b);
            return b;
        }
        /** Get free-fall event acceleration threshold.
         * @param threshold New free-fall acceleration threshold value (LSB = 2mg)
         * @see getFreefallDetectionThreshold()
         * @see MPU6050_RA_FF_THR
         */
        void setFreefallDetectionThreshold(byte threshold) {
            _i2c.writeByte(devAddr, MPU6050_RA_FF_THR, threshold);
        }

        // FF_DUR register

        /** Get free-fall event duration threshold.
         * This register configures the duration counter threshold for Free Fall event
         * detection. The duration counter ticks at 1kHz, therefore FF_DUR has a unit
         * of 1 LSB = 1 ms.
         *
         * The Free Fall duration counter increments while the absolute value of the
         * accelerometer measurements are each less than the detection threshold
         * (Register 29). The Free Fall interrupt is triggered when the Free Fall
         * duration counter reaches the time specified in this register.
         *
         * For more details on the Free Fall detection interrupt, see Section 8.2 of
         * the MPU-6000/MPU-6050 Product Specification document as well as Registers 56
         * and 58 of this document.
         *
         * @return Current free-fall duration threshold value (LSB = 1ms)
         * @see MPU6050_RA_FF_DUR
         */
        byte getFreefallDetectionDuration() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_FF_DUR, b);
            return b;
        }
        /** Get free-fall event duration threshold.
         * @param duration New free-fall duration threshold value (LSB = 1ms)
         * @see getFreefallDetectionDuration()
         * @see MPU6050_RA_FF_DUR
         */
        void setFreefallDetectionDuration(byte duration) {
            _i2c.writeByte(devAddr, MPU6050_RA_FF_DUR, duration);
        }

        // MOT_THR register

        /** Get motion detection event acceleration threshold.
         * This register configures the detection threshold for Motion interrupt
         * generation. The unit of MOT_THR is 1LSB = 2mg. Motion is detected when the
         * absolute value of any of the accelerometer measurements exceeds this Motion
         * detection threshold. This condition increments the Motion detection duration
         * counter (Register 32). The Motion detection interrupt is triggered when the
         * Motion Detection counter reaches the time count specified in MOT_DUR
         * (Register 32).
         *
         * The Motion interrupt will indicate the axis and polarity of detected motion
         * in MOT_DETECT_STATUS (Register 97).
         *
         * For more details on the Motion detection interrupt, see Section 8.3 of the
         * MPU-6000/MPU-6050 Product Specification document as well as Registers 56 and
         * 58 of this document.
         *
         * @return Current motion detection acceleration threshold value (LSB = 2mg)
         * @see MPU6050_RA_MOT_THR
         */
        byte getMotionDetectionThreshold() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_MOT_THR, b);
            return b;
        }
        /** Set free-fall event acceleration threshold.
         * @param threshold New motion detection acceleration threshold value (LSB = 2mg)
         * @see getMotionDetectionThreshold()
         * @see MPU6050_RA_MOT_THR
         */
        void setMotionDetectionThreshold(byte threshold) {
            _i2c.writeByte(devAddr, MPU6050_RA_MOT_THR, threshold);
        }

        // MOT_DUR register

        /** Get motion detection event duration threshold.
         * This register configures the duration counter threshold for Motion interrupt
         * generation. The duration counter ticks at 1 kHz, therefore MOT_DUR has a unit
         * of 1LSB = 1ms. The Motion detection duration counter increments when the
         * absolute value of any of the accelerometer measurements exceeds the Motion
         * detection threshold (Register 31). The Motion detection interrupt is
         * triggered when the Motion detection counter reaches the time count specified
         * in this register.
         *
         * For more details on the Motion detection interrupt, see Section 8.3 of the
         * MPU-6000/MPU-6050 Product Specification document.
         *
         * @return Current motion detection duration threshold value (LSB = 1ms)
         * @see MPU6050_RA_MOT_DUR
         */
        byte getMotionDetectionDuration() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_MOT_DUR, b);
            return b;
        }
        /** Set motion detection event duration threshold.
         * @param duration New motion detection duration threshold value (LSB = 1ms)
         * @see getMotionDetectionDuration()
         * @see MPU6050_RA_MOT_DUR
         */
        void setMotionDetectionDuration(byte duration) {
            _i2c.writeByte(devAddr, MPU6050_RA_MOT_DUR, duration);
        }

        // ZRMOT_THR register

        /** Get zero motion detection event acceleration threshold.
         * This register configures the detection threshold for Zero Motion interrupt
         * generation. The unit of ZRMOT_THR is 1LSB = 2mg. Zero Motion is detected when
         * the absolute value of the accelerometer measurements for the 3 axes are each
         * less than the detection threshold. This condition increments the Zero Motion
         * duration counter (Register 34). The Zero Motion interrupt is triggered when
         * the Zero Motion duration counter reaches the time count specified in
         * ZRMOT_DUR (Register 34).
         *
         * Unlike Free Fall or Motion detection, Zero Motion detection triggers an
         * interrupt both when Zero Motion is first detected and when Zero Motion is no
         * longer detected.
         *
         * When a zero motion event is detected, a Zero Motion Status will be indicated
         * in the MOT_DETECT_STATUS register (Register 97). When a motion-to-zero-motion
         * condition is detected, the status bit is set to 1. When a zero-motion-to-
         * motion condition is detected, the status bit is set to 0.
         *
         * For more details on the Zero Motion detection interrupt, see Section 8.4 of
         * the MPU-6000/MPU-6050 Product Specification document as well as Registers 56
         * and 58 of this document.
         *
         * @return Current zero motion detection acceleration threshold value (LSB = 2mg)
         * @see MPU6050_RA_ZRMOT_THR
         */
        byte getZeroMotionDetectionThreshold() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_ZRMOT_THR, b);
            return b;
        }
        /** Set zero motion detection event acceleration threshold.
         * @param threshold New zero motion detection acceleration threshold value (LSB = 2mg)
         * @see getZeroMotionDetectionThreshold()
         * @see MPU6050_RA_ZRMOT_THR
         */
        void setZeroMotionDetectionThreshold(byte threshold) {
            _i2c.writeByte(devAddr, MPU6050_RA_ZRMOT_THR, threshold);
        }

        // ZRMOT_DUR register

        /** Get zero motion detection event duration threshold.
         * This register configures the duration counter threshold for Zero Motion
         * interrupt generation. The duration counter ticks at 16 Hz, therefore
         * ZRMOT_DUR has a unit of 1 LSB = 64 ms. The Zero Motion duration counter
         * increments while the absolute value of the accelerometer measurements are
         * each less than the detection threshold (Register 33). The Zero Motion
         * interrupt is triggered when the Zero Motion duration counter reaches the time
         * count specified in this register.
         *
         * For more details on the Zero Motion detection interrupt, see Section 8.4 of
         * the MPU-6000/MPU-6050 Product Specification document, as well as Registers 56
         * and 58 of this document.
         *
         * @return Current zero motion detection duration threshold value (LSB = 64ms)
         * @see MPU6050_RA_ZRMOT_DUR
         */
        byte getZeroMotionDetectionDuration() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_ZRMOT_DUR, b);
            return b;
        }
        /** Set zero motion detection event duration threshold.
         * @param duration New zero motion detection duration threshold value (LSB = 1ms)
         * @see getZeroMotionDetectionDuration()
         * @see MPU6050_RA_ZRMOT_DUR
         */
        void setZeroMotionDetectionDuration(byte duration) {
            _i2c.writeByte(devAddr, MPU6050_RA_ZRMOT_DUR, duration);
        }

        // FIFO_EN register

        /** Get temperature FIFO enabled value.
         * When set to 1, this bit enables TEMP_OUT_H and TEMP_OUT_L (Registers 65 and
         * 66) to be written into the FIFO buffer.
         * @return Current temperature FIFO enabled value
         * @see MPU6050_RA_FIFO_EN
         */
        bool getTempFIFOEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_FIFO_EN, MPU6050_TEMP_FIFO_EN_BIT, b);
            return b == 1;
        }
        /** Set temperature FIFO enabled value.
         * @param enabled New temperature FIFO enabled value
         * @see getTempFIFOEnabled()
         * @see MPU6050_RA_FIFO_EN
         */
        void setTempFIFOEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_FIFO_EN, MPU6050_TEMP_FIFO_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get gyroscope X-axis FIFO enabled value.
         * When set to 1, this bit enables GYRO_XOUT_H and GYRO_XOUT_L (Registers 67 and
         * 68) to be written into the FIFO buffer.
         * @return Current gyroscope X-axis FIFO enabled value
         * @see MPU6050_RA_FIFO_EN
         */
        bool getXGyroFIFOEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_FIFO_EN, MPU6050_XG_FIFO_EN_BIT, b);
            return b == 1;
        }
        /** Set gyroscope X-axis FIFO enabled value.
         * @param enabled New gyroscope X-axis FIFO enabled value
         * @see getXGyroFIFOEnabled()
         * @see MPU6050_RA_FIFO_EN
         */
        void setXGyroFIFOEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_FIFO_EN, MPU6050_XG_FIFO_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get gyroscope Y-axis FIFO enabled value.
         * When set to 1, this bit enables GYRO_YOUT_H and GYRO_YOUT_L (Registers 69 and
         * 70) to be written into the FIFO buffer.
         * @return Current gyroscope Y-axis FIFO enabled value
         * @see MPU6050_RA_FIFO_EN
         */
        bool getYGyroFIFOEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_FIFO_EN, MPU6050_YG_FIFO_EN_BIT, b);
            return b == 1;
        }
        /** Set gyroscope Y-axis FIFO enabled value.
         * @param enabled New gyroscope Y-axis FIFO enabled value
         * @see getYGyroFIFOEnabled()
         * @see MPU6050_RA_FIFO_EN
         */
        void setYGyroFIFOEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_FIFO_EN, MPU6050_YG_FIFO_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get gyroscope Z-axis FIFO enabled value.
         * When set to 1, this bit enables GYRO_ZOUT_H and GYRO_ZOUT_L (Registers 71 and
         * 72) to be written into the FIFO buffer.
         * @return Current gyroscope Z-axis FIFO enabled value
         * @see MPU6050_RA_FIFO_EN
         */
        bool getZGyroFIFOEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_FIFO_EN, MPU6050_ZG_FIFO_EN_BIT, b);
            return b == 1;
        }
        /** Set gyroscope Z-axis FIFO enabled value.
         * @param enabled New gyroscope Z-axis FIFO enabled value
         * @see getZGyroFIFOEnabled()
         * @see MPU6050_RA_FIFO_EN
         */
        void setZGyroFIFOEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_FIFO_EN, MPU6050_ZG_FIFO_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get accelerometer FIFO enabled value.
         * When set to 1, this bit enables ACCEL_XOUT_H, ACCEL_XOUT_L, ACCEL_YOUT_H,
         * ACCEL_YOUT_L, ACCEL_ZOUT_H, and ACCEL_ZOUT_L (Registers 59 to 64) to be
         * written into the FIFO buffer.
         * @return Current accelerometer FIFO enabled value
         * @see MPU6050_RA_FIFO_EN
         */
        bool getAccelFIFOEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_FIFO_EN, MPU6050_ACCEL_FIFO_EN_BIT, b);
            return b == 1;
        }
        /** Set accelerometer FIFO enabled value.
         * @param enabled New accelerometer FIFO enabled value
         * @see getAccelFIFOEnabled()
         * @see MPU6050_RA_FIFO_EN
         */
        void setAccelFIFOEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_FIFO_EN, MPU6050_ACCEL_FIFO_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get Slave 2 FIFO enabled value.
         * When set to 1, this bit enables EXT_SENS_DATA registers (Registers 73 to 96)
         * associated with Slave 2 to be written into the FIFO buffer.
         * @return Current Slave 2 FIFO enabled value
         * @see MPU6050_RA_FIFO_EN
         */
        bool getSlave2FIFOEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_FIFO_EN, MPU6050_SLV2_FIFO_EN_BIT, b);
            return b == 1;
        }
        /** Set Slave 2 FIFO enabled value.
         * @param enabled New Slave 2 FIFO enabled value
         * @see getSlave2FIFOEnabled()
         * @see MPU6050_RA_FIFO_EN
         */
        void setSlave2FIFOEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_FIFO_EN, MPU6050_SLV2_FIFO_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get Slave 1 FIFO enabled value.
         * When set to 1, this bit enables EXT_SENS_DATA registers (Registers 73 to 96)
         * associated with Slave 1 to be written into the FIFO buffer.
         * @return Current Slave 1 FIFO enabled value
         * @see MPU6050_RA_FIFO_EN
         */
        bool getSlave1FIFOEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_FIFO_EN, MPU6050_SLV1_FIFO_EN_BIT, b);
            return b == 1;
        }
        /** Set Slave 1 FIFO enabled value.
         * @param enabled New Slave 1 FIFO enabled value
         * @see getSlave1FIFOEnabled()
         * @see MPU6050_RA_FIFO_EN
         */
        void setSlave1FIFOEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_FIFO_EN, MPU6050_SLV1_FIFO_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get Slave 0 FIFO enabled value.
         * When set to 1, this bit enables EXT_SENS_DATA registers (Registers 73 to 96)
         * associated with Slave 0 to be written into the FIFO buffer.
         * @return Current Slave 0 FIFO enabled value
         * @see MPU6050_RA_FIFO_EN
         */
        bool getSlave0FIFOEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_FIFO_EN, MPU6050_SLV0_FIFO_EN_BIT, b);
            return b == 1;
        }
        /** Set Slave 0 FIFO enabled value.
         * @param enabled New Slave 0 FIFO enabled value
         * @see getSlave0FIFOEnabled()
         * @see MPU6050_RA_FIFO_EN
         */
        void setSlave0FIFOEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_FIFO_EN, MPU6050_SLV0_FIFO_EN_BIT, (byte)(enabled ? 1 : 0));
        }

        // I2C_MST_CTRL register

        /** Get multi-master enabled value.
         * Multi-master capability allows multiple I2C masters to operate on the same
         * bus. In circuits where multi-master capability is required, set MULT_MST_EN
         * to 1. This will increase current drawn by approximately 30uA.
         *
         * In circuits where multi-master capability is required, the state of the I2C
         * bus must always be monitored by each separate I2C Master. Before an I2C
         * Master can assume arbitration of the bus, it must first confirm that no other
         * I2C Master has arbitration of the bus. When MULT_MST_EN is set to 1, the
         * MPU-60X0's bus arbitration detection logic is turned on, enabling it to
         * detect when the bus is available.
         *
         * @return Current multi-master enabled value
         * @see MPU6050_RA_I2C_MST_CTRL
         */
        bool getMultiMasterEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_MST_CTRL, MPU6050_MULT_MST_EN_BIT, b);
            return b == 1;
        }
        /** Set multi-master enabled value.
         * @param enabled New multi-master enabled value
         * @see getMultiMasterEnabled()
         * @see MPU6050_RA_I2C_MST_CTRL
         */
        void setMultiMasterEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_I2C_MST_CTRL, MPU6050_MULT_MST_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get wait-for-external-sensor-data enabled value.
         * When the WAIT_FOR_ES bit is set to 1, the Data Ready interrupt will be
         * delayed until External Sensor data from the Slave Devices are loaded into the
         * EXT_SENS_DATA registers. This is used to ensure that both the internal sensor
         * data (i.e. from gyro and accel) and external sensor data have been loaded to
         * their respective data registers (i.e. the data is synced) when the Data Ready
         * interrupt is triggered.
         *
         * @return Current wait-for-external-sensor-data enabled value
         * @see MPU6050_RA_I2C_MST_CTRL
         */
        bool getWaitForExternalSensorEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_MST_CTRL, MPU6050_WAIT_FOR_ES_BIT, b);
            return b == 1;
        }
        /** Set wait-for-external-sensor-data enabled value.
         * @param enabled New wait-for-external-sensor-data enabled value
         * @see getWaitForExternalSensorEnabled()
         * @see MPU6050_RA_I2C_MST_CTRL
         */
        void setWaitForExternalSensorEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_I2C_MST_CTRL, MPU6050_WAIT_FOR_ES_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get Slave 3 FIFO enabled value.
         * When set to 1, this bit enables EXT_SENS_DATA registers (Registers 73 to 96)
         * associated with Slave 3 to be written into the FIFO buffer.
         * @return Current Slave 3 FIFO enabled value
         * @see MPU6050_RA_MST_CTRL
         */
        bool getSlave3FIFOEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_MST_CTRL, MPU6050_SLV_3_FIFO_EN_BIT, b);
            return b == 1;
        }
        /** Set Slave 3 FIFO enabled value.
         * @param enabled New Slave 3 FIFO enabled value
         * @see getSlave3FIFOEnabled()
         * @see MPU6050_RA_MST_CTRL
         */
        void setSlave3FIFOEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_I2C_MST_CTRL, MPU6050_SLV_3_FIFO_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get slave read/write transition enabled value.
         * The I2C_MST_P_NSR bit configures the I2C Master's transition from one slave
         * read to the next slave read. If the bit equals 0, there will be a restart
         * between reads. If the bit equals 1, there will be a stop followed by a start
         * of the following read. When a write transaction follows a read transaction,
         * the stop followed by a start of the successive write will be always used.
         *
         * @return Current slave read/write transition enabled value
         * @see MPU6050_RA_I2C_MST_CTRL
         */
        bool getSlaveReadWriteTransitionEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_MST_CTRL, MPU6050_I2C_MST_P_NSR_BIT, b);
            return b == 1;
        }
        /** Set slave read/write transition enabled value.
         * @param enabled New slave read/write transition enabled value
         * @see getSlaveReadWriteTransitionEnabled()
         * @see MPU6050_RA_I2C_MST_CTRL
         */
        void setSlaveReadWriteTransitionEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_I2C_MST_CTRL, MPU6050_I2C_MST_P_NSR_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get I2C master clock speed.
         * I2C_MST_CLK is a 4 bit unsigned value which configures a divider on the
         * MPU-60X0 internal 8MHz clock. It sets the I2C master clock speed according to
         * the following table:
         *
         * <pre>
         * I2C_MST_CLK | I2C Master Clock Speed | 8MHz Clock Divider
         * ------------+------------------------+-------------------
         * 0           | 348kHz                 | 23
         * 1           | 333kHz                 | 24
         * 2           | 320kHz                 | 25
         * 3           | 308kHz                 | 26
         * 4           | 296kHz                 | 27
         * 5           | 286kHz                 | 28
         * 6           | 276kHz                 | 29
         * 7           | 267kHz                 | 30
         * 8           | 258kHz                 | 31
         * 9           | 500kHz                 | 16
         * 10          | 471kHz                 | 17
         * 11          | 444kHz                 | 18
         * 12          | 421kHz                 | 19
         * 13          | 400kHz                 | 20
         * 14          | 381kHz                 | 21
         * 15          | 364kHz                 | 22
         * </pre>
         *
         * @return Current I2C master clock speed
         * @see MPU6050_RA_I2C_MST_CTRL
         */
        byte getMasterClockSpeed() {
            byte b = 0;
            _i2c.readBits(devAddr, MPU6050_RA_I2C_MST_CTRL, MPU6050_I2C_MST_CLK_BIT, MPU6050_I2C_MST_CLK_LENGTH, b);
            return b;
        }
        /** Set I2C master clock speed.
         * @reparam speed Current I2C master clock speed
         * @see MPU6050_RA_I2C_MST_CTRL
         */
        void setMasterClockSpeed(byte speed) {
            _i2c.writeBits(devAddr, MPU6050_RA_I2C_MST_CTRL, MPU6050_I2C_MST_CLK_BIT, MPU6050_I2C_MST_CLK_LENGTH, speed);
        }

        // I2C_SLV* registers (Slave 0-3)

        /** Get the I2C address of the specified slave (0-3).
         * Note that Bit 7 (MSB) controls read/write mode. If Bit 7 is set, it's a read
         * operation, and if it is cleared, then it's a write operation. The remaining
         * bits (6-0) are the 7-bit device address of the slave device.
         *
         * In read mode, the result of the read is placed in the lowest available 
         * EXT_SENS_DATA register. For further information regarding the allocation of
         * read results, please refer to the EXT_SENS_DATA register description
         * (Registers 73 - 96).
         *
         * The MPU-6050 supports a total of five slaves, but Slave 4 has unique
         * characteristics, and so it has its own functions (getSlave4* and setSlave4*).
         *
         * I2C data transactions are performed at the Sample Rate, as defined in
         * Register 25. The user is responsible for ensuring that I2C data transactions
         * to and from each enabled Slave can be completed within a single period of the
         * Sample Rate.
         *
         * The I2C slave access rate can be reduced relative to the Sample Rate. This
         * reduced access rate is determined by I2C_MST_DLY (Register 52). Whether a
         * slave's access rate is reduced relative to the Sample Rate is determined by
         * I2C_MST_DELAY_CTRL (Register 103).
         *
         * The processing order for the slaves is fixed. The sequence followed for
         * processing the slaves is Slave 0, Slave 1, Slave 2, Slave 3 and Slave 4. If a
         * particular Slave is disabled it will be skipped.
         *
         * Each slave can either be accessed at the sample rate or at a reduced sample
         * rate. In a case where some slaves are accessed at the Sample Rate and some
         * slaves are accessed at the reduced rate, the sequence of accessing the slaves
         * (Slave 0 to Slave 4) is still followed. However, the reduced rate slaves will
         * be skipped if their access rate dictates that they should not be accessed
         * during that particular cycle. For further information regarding the reduced
         * access rate, please refer to Register 52. Whether a slave is accessed at the
         * Sample Rate or at the reduced rate is determined by the Delay Enable bits in
         * Register 103.
         *
         * @param num Slave number (0-3)
         * @return Current address for specified slave
         * @see MPU6050_RA_I2C_SLV0_ADDR
         */
        byte getSlaveAddress(byte num) {
            if (num > 3) return 0;
            byte b = 0;
            _i2c.readByte(devAddr, (byte)(MPU6050_RA_I2C_SLV0_ADDR + num*3), b);
            return b;
        }
        /** Set the I2C address of the specified slave (0-3).
         * @param num Slave number (0-3)
         * @param address New address for specified slave
         * @see getSlaveAddress()
         * @see MPU6050_RA_I2C_SLV0_ADDR
         */
        void setSlaveAddress(byte num, byte address) {
            if (num > 3) return;
            _i2c.writeByte(devAddr, (byte)(MPU6050_RA_I2C_SLV0_ADDR + num * 3), address);
        }
        /** Get the active internal register for the specified slave (0-3).
         * Read/write operations for this slave will be done to whatever internal
         * register address is stored in this MPU register.
         *
         * The MPU-6050 supports a total of five slaves, but Slave 4 has unique
         * characteristics, and so it has its own functions.
         *
         * @param num Slave number (0-3)
         * @return Current active register for specified slave
         * @see MPU6050_RA_I2C_SLV0_REG
         */
        byte getSlaveRegister(byte num) {
            if (num > 3) return 0;
            byte b = 0;
            _i2c.readByte(devAddr, (byte)(MPU6050_RA_I2C_SLV0_REG + num*3), b);
            return b;
        }
        /** Set the active internal register for the specified slave (0-3).
         * @param num Slave number (0-3)
         * @param reg New active register for specified slave
         * @see getSlaveRegister()
         * @see MPU6050_RA_I2C_SLV0_REG
         */
        void setSlaveRegister(byte num, byte reg) {
            if (num > 3) return;
            _i2c.writeByte(devAddr, (byte)(MPU6050_RA_I2C_SLV0_REG + num * 3), reg);
        }
        /** Get the enabled value for the specified slave (0-3).
         * When set to 1, this bit enables Slave 0 for data transfer operations. When
         * cleared to 0, this bit disables Slave 0 from data transfer operations.
         * @param num Slave number (0-3)
         * @return Current enabled value for specified slave
         * @see MPU6050_RA_I2C_SLV0_CTRL
         */
        bool getSlaveEnabled(byte num) {
            if (num > 3) return false;
            byte b = 0;
            _i2c.readBit(devAddr, (byte)(MPU6050_RA_I2C_SLV0_CTRL + num * 3), MPU6050_I2C_SLV_EN_BIT, b);
            return b == 1;
        }
        /** Set the enabled value for the specified slave (0-3).
         * @param num Slave number (0-3)
         * @param enabled New enabled value for specified slave
         * @see getSlaveEnabled()
         * @see MPU6050_RA_I2C_SLV0_CTRL
         */
        void setSlaveEnabled(byte num, bool enabled) {
            if (num > 3) return;
            _i2c.writeBit(devAddr, (byte)(MPU6050_RA_I2C_SLV0_CTRL + num * 3), MPU6050_I2C_SLV_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get word pair byte-swapping enabled for the specified slave (0-3).
         * When set to 1, this bit enables byte swapping. When byte swapping is enabled,
         * the high and low bytes of a word pair are swapped. Please refer to
         * I2C_SLV0_GRP for the pairing convention of the word pairs. When cleared to 0,
         * bytes transferred to and from Slave 0 will be written to EXT_SENS_DATA
         * registers in the order they were transferred.
         *
         * @param num Slave number (0-3)
         * @return Current word pair byte-swapping enabled value for specified slave
         * @see MPU6050_RA_I2C_SLV0_CTRL
         */
        bool getSlaveWordByteSwap(byte num) {
            if (num > 3) return false;
            byte b = 0;
            _i2c.readBit(devAddr, (byte)(MPU6050_RA_I2C_SLV0_CTRL + num*3), MPU6050_I2C_SLV_BYTE_SW_BIT, b);
            return b == 1;
        }
        /** Set word pair byte-swapping enabled for the specified slave (0-3).
         * @param num Slave number (0-3)
         * @param enabled New word pair byte-swapping enabled value for specified slave
         * @see getSlaveWordByteSwap()
         * @see MPU6050_RA_I2C_SLV0_CTRL
         */
        void setSlaveWordByteSwap(byte num, bool enabled) {
            if (num > 3) return;
            _i2c.writeBit(devAddr, (byte)(MPU6050_RA_I2C_SLV0_CTRL + num * 3), MPU6050_I2C_SLV_BYTE_SW_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get write mode for the specified slave (0-3).
         * When set to 1, the transaction will read or write data only. When cleared to
         * 0, the transaction will write a register address prior to reading or writing
         * data. This should equal 0 when specifying the register address within the
         * Slave device to/from which the ensuing data transaction will take place.
         *
         * @param num Slave number (0-3)
         * @return Current write mode for specified slave (0 = register address + data, 1 = data only)
         * @see MPU6050_RA_I2C_SLV0_CTRL
         */
        bool getSlaveWriteMode(byte num) {
            if (num > 3) return false;
            byte b = 0;
            _i2c.readBit(devAddr, (byte)(MPU6050_RA_I2C_SLV0_CTRL + num*3), MPU6050_I2C_SLV_REG_DIS_BIT, b);
            return b == 1;
        }
        /** Set write mode for the specified slave (0-3).
         * @param num Slave number (0-3)
         * @param mode New write mode for specified slave (0 = register address + data, 1 = data only)
         * @see getSlaveWriteMode()
         * @see MPU6050_RA_I2C_SLV0_CTRL
         */
        void setSlaveWriteMode(byte num, byte mode) {
            if (num > 3) return;
            _i2c.writeBit(devAddr, (byte)(MPU6050_RA_I2C_SLV0_CTRL + num * 3), MPU6050_I2C_SLV_REG_DIS_BIT, mode);
        }
        /** Get word pair grouping order offset for the specified slave (0-3).
         * This sets specifies the grouping order of word pairs received from registers.
         * When cleared to 0, bytes from register addresses 0 and 1, 2 and 3, etc (even,
         * then odd register addresses) are paired to form a word. When set to 1, bytes
         * from register addresses are paired 1 and 2, 3 and 4, etc. (odd, then even
         * register addresses) are paired to form a word.
         *
         * @param num Slave number (0-3)
         * @return Current word pair grouping order offset for specified slave
         * @see MPU6050_RA_I2C_SLV0_CTRL
         */
        bool getSlaveWordGroupOffset(byte num) {
            if (num > 3) return false;
            byte b = 0;
            _i2c.readBit(devAddr, (byte)(MPU6050_RA_I2C_SLV0_CTRL + num*3), MPU6050_I2C_SLV_GRP_BIT, b);
            return b == 1;
        }
        /** Set word pair grouping order offset for the specified slave (0-3).
         * @param num Slave number (0-3)
         * @param enabled New word pair grouping order offset for specified slave
         * @see getSlaveWordGroupOffset()
         * @see MPU6050_RA_I2C_SLV0_CTRL
         */
        void setSlaveWordGroupOffset(byte num, bool enabled) {
            if (num > 3) return;
            _i2c.writeBit(devAddr, (byte)(MPU6050_RA_I2C_SLV0_CTRL + num * 3), MPU6050_I2C_SLV_GRP_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get number of bytes to read for the specified slave (0-3).
         * Specifies the number of bytes transferred to and from Slave 0. Clearing this
         * bit to 0 is equivalent to disabling the register by writing 0 to I2C_SLV0_EN.
         * @param num Slave number (0-3)
         * @return Number of bytes to read for specified slave
         * @see MPU6050_RA_I2C_SLV0_CTRL
         */
        byte getSlaveDataLength(byte num) {
            if (num > 3) return 0;
            byte b = 0;
            _i2c.readBits(devAddr, (byte)(MPU6050_RA_I2C_SLV0_CTRL + num * 3), MPU6050_I2C_SLV_LEN_BIT, MPU6050_I2C_SLV_LEN_LENGTH, b);
            return b;
        }
        /** Set number of bytes to read for the specified slave (0-3).
         * @param num Slave number (0-3)
         * @param length Number of bytes to read for specified slave
         * @see getSlaveDataLength()
         * @see MPU6050_RA_I2C_SLV0_CTRL
         */
        void setSlaveDataLength(byte num, byte length) {
            if (num > 3) return;
            _i2c.writeBits(devAddr, (byte)(MPU6050_RA_I2C_SLV0_CTRL + num*3), MPU6050_I2C_SLV_LEN_BIT, MPU6050_I2C_SLV_LEN_LENGTH, length);
        }

        // I2C_SLV* registers (Slave 4)

        /** Get the I2C address of Slave 4.
         * Note that Bit 7 (MSB) controls read/write mode. If Bit 7 is set, it's a read
         * operation, and if it is cleared, then it's a write operation. The remaining
         * bits (6-0) are the 7-bit device address of the slave device.
         *
         * @return Current address for Slave 4
         * @see getSlaveAddress()
         * @see MPU6050_RA_I2C_SLV4_ADDR
         */
        byte getSlave4Address() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_I2C_SLV4_ADDR, b);
            return b;
        }
        /** Set the I2C address of Slave 4.
         * @param address New address for Slave 4
         * @see getSlave4Address()
         * @see MPU6050_RA_I2C_SLV4_ADDR
         */
        void setSlave4Address(byte address) {
            _i2c.writeByte(devAddr, MPU6050_RA_I2C_SLV4_ADDR, address);
        }
        /** Get the active internal register for the Slave 4.
         * Read/write operations for this slave will be done to whatever internal
         * register address is stored in this MPU register.
         *
         * @return Current active register for Slave 4
         * @see MPU6050_RA_I2C_SLV4_REG
         */
        byte getSlave4Register() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_I2C_SLV4_REG, b);
            return b;
        }
        /** Set the active internal register for Slave 4.
         * @param reg New active register for Slave 4
         * @see getSlave4Register()
         * @see MPU6050_RA_I2C_SLV4_REG
         */
        void setSlave4Register(byte reg) {
            _i2c.writeByte(devAddr, MPU6050_RA_I2C_SLV4_REG, reg);
        }
        /** Set new byte to write to Slave 4.
         * This register stores the data to be written into the Slave 4. If I2C_SLV4_RW
         * is set 1 (set to read), this register has no effect.
         * @param data New byte to write to Slave 4
         * @see MPU6050_RA_I2C_SLV4_DO
         */
        void setSlave4OutputByte(byte data) {
            _i2c.writeByte(devAddr, MPU6050_RA_I2C_SLV4_DO, data);
        }
        /** Get the enabled value for the Slave 4.
         * When set to 1, this bit enables Slave 4 for data transfer operations. When
         * cleared to 0, this bit disables Slave 4 from data transfer operations.
         * @return Current enabled value for Slave 4
         * @see MPU6050_RA_I2C_SLV4_CTRL
         */
        bool getSlave4Enabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_SLV4_CTRL, MPU6050_I2C_SLV4_EN_BIT, b);
            return b == 1;
        }
        /** Set the enabled value for Slave 4.
         * @param enabled New enabled value for Slave 4
         * @see getSlave4Enabled()
         * @see MPU6050_RA_I2C_SLV4_CTRL
         */
        void setSlave4Enabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_I2C_SLV4_CTRL, MPU6050_I2C_SLV4_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get the enabled value for Slave 4 transaction interrupts.
         * When set to 1, this bit enables the generation of an interrupt signal upon
         * completion of a Slave 4 transaction. When cleared to 0, this bit disables the
         * generation of an interrupt signal upon completion of a Slave 4 transaction.
         * The interrupt status can be observed in Register 54.
         *
         * @return Current enabled value for Slave 4 transaction interrupts.
         * @see MPU6050_RA_I2C_SLV4_CTRL
         */
        bool getSlave4InterruptEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_SLV4_CTRL, MPU6050_I2C_SLV4_INT_EN_BIT, b);
            return b == 1;
        }
        /** Set the enabled value for Slave 4 transaction interrupts.
         * @param enabled New enabled value for Slave 4 transaction interrupts.
         * @see getSlave4InterruptEnabled()
         * @see MPU6050_RA_I2C_SLV4_CTRL
         */
        void setSlave4InterruptEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_I2C_SLV4_CTRL, MPU6050_I2C_SLV4_INT_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get write mode for Slave 4.
         * When set to 1, the transaction will read or write data only. When cleared to
         * 0, the transaction will write a register address prior to reading or writing
         * data. This should equal 0 when specifying the register address within the
         * Slave device to/from which the ensuing data transaction will take place.
         *
         * @return Current write mode for Slave 4 (0 = register address + data, 1 = data only)
         * @see MPU6050_RA_I2C_SLV4_CTRL
         */
        bool getSlave4WriteMode() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_SLV4_CTRL, MPU6050_I2C_SLV4_REG_DIS_BIT, b);
            return b == 1;
        }
        /** Set write mode for the Slave 4.
         * @param mode New write mode for Slave 4 (0 = register address + data, 1 = data only)
         * @see getSlave4WriteMode()
         * @see MPU6050_RA_I2C_SLV4_CTRL
         */
        void setSlave4WriteMode(byte mode) {
            _i2c.writeBit(devAddr, MPU6050_RA_I2C_SLV4_CTRL, MPU6050_I2C_SLV4_REG_DIS_BIT, mode);
        }
        /** Get Slave 4 master delay value.
         * This configures the reduced access rate of I2C slaves relative to the Sample
         * Rate. When a slave's access rate is decreased relative to the Sample Rate,
         * the slave is accessed every:
         *
         *     1 / (1 + I2C_MST_DLY) samples
         *
         * This base Sample Rate in turn is determined by SMPLRT_DIV (register 25) and
         * DLPF_CFG (register 26). Whether a slave's access rate is reduced relative to
         * the Sample Rate is determined by I2C_MST_DELAY_CTRL (register 103). For
         * further information regarding the Sample Rate, please refer to register 25.
         *
         * @return Current Slave 4 master delay value
         * @see MPU6050_RA_I2C_SLV4_CTRL
         */
        byte getSlave4MasterDelay() {
            byte b = 0;
            _i2c.readBits(devAddr, MPU6050_RA_I2C_SLV4_CTRL, MPU6050_I2C_SLV4_MST_DLY_BIT, MPU6050_I2C_SLV4_MST_DLY_LENGTH, b);
            return b;
        }
        /** Set Slave 4 master delay value.
         * @param delay New Slave 4 master delay value
         * @see getSlave4MasterDelay()
         * @see MPU6050_RA_I2C_SLV4_CTRL
         */
        void setSlave4MasterDelay(byte delay) {
            _i2c.writeBits(devAddr, MPU6050_RA_I2C_SLV4_CTRL, MPU6050_I2C_SLV4_MST_DLY_BIT, MPU6050_I2C_SLV4_MST_DLY_LENGTH, delay);
        }
        /** Get last available byte read from Slave 4.
         * This register stores the data read from Slave 4. This field is populated
         * after a read transaction.
         * @return Last available byte read from to Slave 4
         * @see MPU6050_RA_I2C_SLV4_DI
         */
        byte getSlate4InputByte() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_I2C_SLV4_DI, b);
            return b;
        }

        // I2C_MST_STATUS register

        /** Get FSYNC interrupt status.
         * This bit reflects the status of the FSYNC interrupt from an external device
         * into the MPU-60X0. This is used as a way to pass an external interrupt
         * through the MPU-60X0 to the host application processor. When set to 1, this
         * bit will cause an interrupt if FSYNC_INT_EN is asserted in INT_PIN_CFG
         * (Register 55).
         * @return FSYNC interrupt status
         * @see MPU6050_RA_I2C_MST_STATUS
         */
        bool getPassthroughStatus() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_MST_STATUS, MPU6050_MST_PASS_THROUGH_BIT, b);
            return b == 1;
        }
        /** Get Slave 4 transaction done status.
         * Automatically sets to 1 when a Slave 4 transaction has completed. This
         * triggers an interrupt if the I2C_MST_INT_EN bit in the INT_ENABLE register
         * (Register 56) is asserted and if the SLV_4_DONE_INT bit is asserted in the
         * I2C_SLV4_CTRL register (Register 52).
         * @return Slave 4 transaction done status
         * @see MPU6050_RA_I2C_MST_STATUS
         */
        bool getSlave4IsDone() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_MST_STATUS, MPU6050_MST_I2C_SLV4_DONE_BIT, b);
            return b == 1;
        }
        /** Get master arbitration lost status.
         * This bit automatically sets to 1 when the I2C Master has lost arbitration of
         * the auxiliary I2C bus (an error condition). This triggers an interrupt if the
         * I2C_MST_INT_EN bit in the INT_ENABLE register (Register 56) is asserted.
         * @return Master arbitration lost status
         * @see MPU6050_RA_I2C_MST_STATUS
         */
        bool getLostArbitration() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_MST_STATUS, MPU6050_MST_I2C_LOST_ARB_BIT, b);
            return b == 1;
        }
        /** Get Slave 4 NACK status.
         * This bit automatically sets to 1 when the I2C Master receives a NACK in a
         * transaction with Slave 4. This triggers an interrupt if the I2C_MST_INT_EN
         * bit in the INT_ENABLE register (Register 56) is asserted.
         * @return Slave 4 NACK interrupt status
         * @see MPU6050_RA_I2C_MST_STATUS
         */
        bool getSlave4Nack() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_MST_STATUS, MPU6050_MST_I2C_SLV4_NACK_BIT, b);
            return b == 1;
        }
        /** Get Slave 3 NACK status.
         * This bit automatically sets to 1 when the I2C Master receives a NACK in a
         * transaction with Slave 3. This triggers an interrupt if the I2C_MST_INT_EN
         * bit in the INT_ENABLE register (Register 56) is asserted.
         * @return Slave 3 NACK interrupt status
         * @see MPU6050_RA_I2C_MST_STATUS
         */
        bool getSlave3Nack() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_MST_STATUS, MPU6050_MST_I2C_SLV3_NACK_BIT, b);
            return b == 1;
        }
        /** Get Slave 2 NACK status.
         * This bit automatically sets to 1 when the I2C Master receives a NACK in a
         * transaction with Slave 2. This triggers an interrupt if the I2C_MST_INT_EN
         * bit in the INT_ENABLE register (Register 56) is asserted.
         * @return Slave 2 NACK interrupt status
         * @see MPU6050_RA_I2C_MST_STATUS
         */
        bool getSlave2Nack() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_MST_STATUS, MPU6050_MST_I2C_SLV2_NACK_BIT, b);
            return b == 1;
        }
        /** Get Slave 1 NACK status.
         * This bit automatically sets to 1 when the I2C Master receives a NACK in a
         * transaction with Slave 1. This triggers an interrupt if the I2C_MST_INT_EN
         * bit in the INT_ENABLE register (Register 56) is asserted.
         * @return Slave 1 NACK interrupt status
         * @see MPU6050_RA_I2C_MST_STATUS
         */
        bool getSlave1Nack() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_MST_STATUS, MPU6050_MST_I2C_SLV1_NACK_BIT, b);
            return b == 1;
        }
        /** Get Slave 0 NACK status.
         * This bit automatically sets to 1 when the I2C Master receives a NACK in a
         * transaction with Slave 0. This triggers an interrupt if the I2C_MST_INT_EN
         * bit in the INT_ENABLE register (Register 56) is asserted.
         * @return Slave 0 NACK interrupt status
         * @see MPU6050_RA_I2C_MST_STATUS
         */
        bool getSlave0Nack() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_MST_STATUS, MPU6050_MST_I2C_SLV0_NACK_BIT, b);
            return b == 1;
        }

        // INT_PIN_CFG register

        /** Get interrupt logic level mode.
         * Will be set 0 for active-high, 1 for active-low.
         * @return Current interrupt mode (0=active-high, 1=active-low)
         * @see MPU6050_RA_INT_PIN_CFG
         * @see MPU6050_INTCFG_INT_LEVEL_BIT
         */
        bool getInterruptMode() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_PIN_CFG, MPU6050_INTCFG_INT_LEVEL_BIT, b);
            return b == 1;
        }
        /** Set interrupt logic level mode.
         * @param mode New interrupt mode (0=active-high, 1=active-low)
         * @see getInterruptMode()
         * @see MPU6050_RA_INT_PIN_CFG
         * @see MPU6050_INTCFG_INT_LEVEL_BIT
         */
        void setInterruptMode(byte mode) {
           _i2c.writeBit(devAddr, MPU6050_RA_INT_PIN_CFG, MPU6050_INTCFG_INT_LEVEL_BIT, mode);
        }
        /** Get interrupt drive mode.
         * Will be set 0 for push-pull, 1 for open-drain.
         * @return Current interrupt drive mode (0=push-pull, 1=open-drain)
         * @see MPU6050_RA_INT_PIN_CFG
         * @see MPU6050_INTCFG_INT_OPEN_BIT
         */
        bool getInterruptDrive() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_PIN_CFG, MPU6050_INTCFG_INT_OPEN_BIT, b);
            return b == 1;
        }
        /** Set interrupt drive mode.
         * @param drive New interrupt drive mode (0=push-pull, 1=open-drain)
         * @see getInterruptDrive()
         * @see MPU6050_RA_INT_PIN_CFG
         * @see MPU6050_INTCFG_INT_OPEN_BIT
         */
        void setInterruptDrive(byte drive) {
            _i2c.writeBit(devAddr, MPU6050_RA_INT_PIN_CFG, MPU6050_INTCFG_INT_OPEN_BIT, drive);
        }
        /** Get interrupt latch mode.
         * Will be set 0 for 50us-pulse, 1 for latch-until-int-cleared.
         * @return Current latch mode (0=50us-pulse, 1=latch-until-int-cleared)
         * @see MPU6050_RA_INT_PIN_CFG
         * @see MPU6050_INTCFG_LATCH_INT_EN_BIT
         */
        bool getInterruptLatch() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_PIN_CFG, MPU6050_INTCFG_LATCH_INT_EN_BIT, b);
            return b == 1;
        }
        /** Set interrupt latch mode.
         * @param latch New latch mode (0=50us-pulse, 1=latch-until-int-cleared)
         * @see getInterruptLatch()
         * @see MPU6050_RA_INT_PIN_CFG
         * @see MPU6050_INTCFG_LATCH_INT_EN_BIT
         */
        void setInterruptLatch(byte latch) {
            _i2c.writeBit(devAddr, MPU6050_RA_INT_PIN_CFG, MPU6050_INTCFG_LATCH_INT_EN_BIT, latch);
        }
        /** Get interrupt latch clear mode.
         * Will be set 0 for status-read-only, 1 for any-register-read.
         * @return Current latch clear mode (0=status-read-only, 1=any-register-read)
         * @see MPU6050_RA_INT_PIN_CFG
         * @see MPU6050_INTCFG_INT_RD_CLEAR_BIT
         */
        bool getInterruptLatchClear() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_PIN_CFG, MPU6050_INTCFG_INT_RD_CLEAR_BIT, b);
            return b == 1;
        }
        /** Set interrupt latch clear mode.
         * @param clear New latch clear mode (0=status-read-only, 1=any-register-read)
         * @see getInterruptLatchClear()
         * @see MPU6050_RA_INT_PIN_CFG
         * @see MPU6050_INTCFG_INT_RD_CLEAR_BIT
         */
        void setInterruptLatchClear(byte clear) {
            _i2c.writeBit(devAddr, MPU6050_RA_INT_PIN_CFG, MPU6050_INTCFG_INT_RD_CLEAR_BIT, clear);
        }
        /** Get FSYNC interrupt logic level mode.
         * @return Current FSYNC interrupt mode (0=active-high, 1=active-low)
         * @see getFSyncInterruptMode()
         * @see MPU6050_RA_INT_PIN_CFG
         * @see MPU6050_INTCFG_FSYNC_INT_LEVEL_BIT
         */
        bool getFSyncInterruptLevel() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_PIN_CFG, MPU6050_INTCFG_FSYNC_INT_LEVEL_BIT, b);
            return b == 1;
        }
        /** Set FSYNC interrupt logic level mode.
         * @param mode New FSYNC interrupt mode (0=active-high, 1=active-low)
         * @see getFSyncInterruptMode()
         * @see MPU6050_RA_INT_PIN_CFG
         * @see MPU6050_INTCFG_FSYNC_INT_LEVEL_BIT
         */
        void setFSyncInterruptLevel(byte level) {
            _i2c.writeBit(devAddr, MPU6050_RA_INT_PIN_CFG, MPU6050_INTCFG_FSYNC_INT_LEVEL_BIT, level);
        }
        /** Get FSYNC pin interrupt enabled setting.
         * Will be set 0 for disabled, 1 for enabled.
         * @return Current interrupt enabled setting
         * @see MPU6050_RA_INT_PIN_CFG
         * @see MPU6050_INTCFG_FSYNC_INT_EN_BIT
         */
        bool getFSyncInterruptEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_PIN_CFG, MPU6050_INTCFG_FSYNC_INT_EN_BIT, b);
            return b == 1;
        }
        /** Set FSYNC pin interrupt enabled setting.
         * @param enabled New FSYNC pin interrupt enabled setting
         * @see getFSyncInterruptEnabled()
         * @see MPU6050_RA_INT_PIN_CFG
         * @see MPU6050_INTCFG_FSYNC_INT_EN_BIT
         */
        void setFSyncInterruptEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_INT_PIN_CFG, MPU6050_INTCFG_FSYNC_INT_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get I2C bypass enabled status.
         * When this bit is equal to 1 and I2C_MST_EN (Register 106 bit[5]) is equal to
         * 0, the host application processor will be able to directly access the
         * auxiliary I2C bus of the MPU-60X0. When this bit is equal to 0, the host
         * application processor will not be able to directly access the auxiliary I2C
         * bus of the MPU-60X0 regardless of the state of I2C_MST_EN (Register 106
         * bit[5]).
         * @return Current I2C bypass enabled status
         * @see MPU6050_RA_INT_PIN_CFG
         * @see MPU6050_INTCFG_I2C_BYPASS_EN_BIT
         */
        bool getI2CBypassEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_PIN_CFG, MPU6050_INTCFG_I2C_BYPASS_EN_BIT, b);
            return b == 1;
        }
        /** Set I2C bypass enabled status.
         * When this bit is equal to 1 and I2C_MST_EN (Register 106 bit[5]) is equal to
         * 0, the host application processor will be able to directly access the
         * auxiliary I2C bus of the MPU-60X0. When this bit is equal to 0, the host
         * application processor will not be able to directly access the auxiliary I2C
         * bus of the MPU-60X0 regardless of the state of I2C_MST_EN (Register 106
         * bit[5]).
         * @param enabled New I2C bypass enabled status
         * @see MPU6050_RA_INT_PIN_CFG
         * @see MPU6050_INTCFG_I2C_BYPASS_EN_BIT
         */
        void setI2CBypassEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_INT_PIN_CFG, MPU6050_INTCFG_I2C_BYPASS_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get reference clock output enabled status.
         * When this bit is equal to 1, a reference clock output is provided at the
         * CLKOUT pin. When this bit is equal to 0, the clock output is disabled. For
         * further information regarding CLKOUT, please refer to the MPU-60X0 Product
         * Specification document.
         * @return Current reference clock output enabled status
         * @see MPU6050_RA_INT_PIN_CFG
         * @see MPU6050_INTCFG_CLKOUT_EN_BIT
         */
        bool getClockOutputEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_PIN_CFG, MPU6050_INTCFG_CLKOUT_EN_BIT, b);
            return b == 1;
        }
        /** Set reference clock output enabled status.
         * When this bit is equal to 1, a reference clock output is provided at the
         * CLKOUT pin. When this bit is equal to 0, the clock output is disabled. For
         * further information regarding CLKOUT, please refer to the MPU-60X0 Product
         * Specification document.
         * @param enabled New reference clock output enabled status
         * @see MPU6050_RA_INT_PIN_CFG
         * @see MPU6050_INTCFG_CLKOUT_EN_BIT
         */
        void setClockOutputEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_INT_PIN_CFG, MPU6050_INTCFG_CLKOUT_EN_BIT, (byte)(enabled ? 1 : 0));
        }

        // INT_ENABLE register

        /** Get full interrupt enabled status.
         * Full register byte for all interrupts, for quick reading. Each bit will be
         * set 0 for disabled, 1 for enabled.
         * @return Current interrupt enabled status
         * @see MPU6050_RA_INT_ENABLE
         * @see MPU6050_INTERRUPT_FF_BIT
         **/
        uint getIntEnabled() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_INT_ENABLE, b);
            return b;
        }
        /** Set full interrupt enabled status.
         * Full register byte for all interrupts, for quick reading. Each bit should be
         * set 0 for disabled, 1 for enabled.
         * @param enabled New interrupt enabled status
         * @see getIntFreefallEnabled()
         * @see MPU6050_RA_INT_ENABLE
         * @see MPU6050_INTERRUPT_FF_BIT
         **/
        void setIntEnabled(bool enabled) {
            _i2c.writeByte(devAddr, MPU6050_RA_INT_ENABLE, (byte)(enabled ? 1 : 0));
        }
        /** Get Free Fall interrupt enabled status.
         * Will be set 0 for disabled, 1 for enabled.
         * @return Current interrupt enabled status
         * @see MPU6050_RA_INT_ENABLE
         * @see MPU6050_INTERRUPT_FF_BIT
         **/
        bool getIntFreefallEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_ENABLE, MPU6050_INTERRUPT_FF_BIT, b);
            return b == 1;
        }
        /** Set Free Fall interrupt enabled status.
         * @param enabled New interrupt enabled status
         * @see getIntFreefallEnabled()
         * @see MPU6050_RA_INT_ENABLE
         * @see MPU6050_INTERRUPT_FF_BIT
         **/
        void setIntFreefallEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_INT_ENABLE, MPU6050_INTERRUPT_FF_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get Motion Detection interrupt enabled status.
         * Will be set 0 for disabled, 1 for enabled.
         * @return Current interrupt enabled status
         * @see MPU6050_RA_INT_ENABLE
         * @see MPU6050_INTERRUPT_MOT_BIT
         **/
        bool getIntMotionEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_ENABLE, MPU6050_INTERRUPT_MOT_BIT, b);
            return b == 1;
        }
        /** Set Motion Detection interrupt enabled status.
         * @param enabled New interrupt enabled status
         * @see getIntMotionEnabled()
         * @see MPU6050_RA_INT_ENABLE
         * @see MPU6050_INTERRUPT_MOT_BIT
         **/
        void setIntMotionEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_INT_ENABLE, MPU6050_INTERRUPT_MOT_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get Zero Motion Detection interrupt enabled status.
         * Will be set 0 for disabled, 1 for enabled.
         * @return Current interrupt enabled status
         * @see MPU6050_RA_INT_ENABLE
         * @see MPU6050_INTERRUPT_ZMOT_BIT
         **/
        bool getIntZeroMotionEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_ENABLE, MPU6050_INTERRUPT_ZMOT_BIT, b);
            return b == 1;
        }
        /** Set Zero Motion Detection interrupt enabled status.
         * @param enabled New interrupt enabled status
         * @see getIntZeroMotionEnabled()
         * @see MPU6050_RA_INT_ENABLE
         * @see MPU6050_INTERRUPT_ZMOT_BIT
         **/
        void setIntZeroMotionEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_INT_ENABLE, MPU6050_INTERRUPT_ZMOT_BIT, (byte)(enabled ? 0 : 1));
        }
        /** Get FIFO Buffer Overflow interrupt enabled status.
         * Will be set 0 for disabled, 1 for enabled.
         * @return Current interrupt enabled status
         * @see MPU6050_RA_INT_ENABLE
         * @see MPU6050_INTERRUPT_FIFO_OFLOW_BIT
         **/
        bool getIntFIFOBufferOverflowEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_ENABLE, MPU6050_INTERRUPT_FIFO_OFLOW_BIT, b);
            return b == 1;
        }
        /** Set FIFO Buffer Overflow interrupt enabled status.
         * @param enabled New interrupt enabled status
         * @see getIntFIFOBufferOverflowEnabled()
         * @see MPU6050_RA_INT_ENABLE
         * @see MPU6050_INTERRUPT_FIFO_OFLOW_BIT
         **/
        void setIntFIFOBufferOverflowEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_INT_ENABLE, MPU6050_INTERRUPT_FIFO_OFLOW_BIT, (byte)(enabled ? 0 : 1));
        }
        /** Get I2C Master interrupt enabled status.
         * This enables any of the I2C Master interrupt sources to generate an
         * interrupt. Will be set 0 for disabled, 1 for enabled.
         * @return Current interrupt enabled status
         * @see MPU6050_RA_INT_ENABLE
         * @see MPU6050_INTERRUPT_I2C_MST_INT_BIT
         **/
        bool getIntI2CMasterEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_ENABLE, MPU6050_INTERRUPT_I2C_MST_INT_BIT, b);
            return b == 1;
        }
        /** Set I2C Master interrupt enabled status.
         * @param enabled New interrupt enabled status
         * @see getIntI2CMasterEnabled()
         * @see MPU6050_RA_INT_ENABLE
         * @see MPU6050_INTERRUPT_I2C_MST_INT_BIT
         **/
        void setIntI2CMasterEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_INT_ENABLE, MPU6050_INTERRUPT_I2C_MST_INT_BIT, (byte)(enabled ? 0 : 1));
        }
        /** Get Data Ready interrupt enabled setting.
         * This event occurs each time a write operation to all of the sensor registers
         * has been completed. Will be set 0 for disabled, 1 for enabled.
         * @return Current interrupt enabled status
         * @see MPU6050_RA_INT_ENABLE
         * @see MPU6050_INTERRUPT_DATA_RDY_BIT
         */
        bool getIntDataReadyEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_ENABLE, MPU6050_INTERRUPT_DATA_RDY_BIT, b);
            return b == 1;
        }
        /** Set Data Ready interrupt enabled status.
         * @param enabled New interrupt enabled status
         * @see getIntDataReadyEnabled()
         * @see MPU6050_RA_INT_CFG
         * @see MPU6050_INTERRUPT_DATA_RDY_BIT
         */
        void setIntDataReadyEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_INT_ENABLE, MPU6050_INTERRUPT_DATA_RDY_BIT, (byte)(enabled ? 0 : 1));
        }

        // INT_STATUS register

        /** Get full set of interrupt status bits.
         * These bits clear to 0 after the register has been read. Very useful
         * for getting multiple INT statuses, since each single bit read clears
         * all of them because it has to read the whole byte.
         * @return Current interrupt status
         * @see MPU6050_RA_INT_STATUS
         */
        byte getIntStatus() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_INT_STATUS, b);
            return b;
        }
        /** Get Free Fall interrupt status.
         * This bit automatically sets to 1 when a Free Fall interrupt has been
         * generated. The bit clears to 0 after the register has been read.
         * @return Current interrupt status
         * @see MPU6050_RA_INT_STATUS
         * @see MPU6050_INTERRUPT_FF_BIT
         */
        bool getIntFreefallStatus() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_STATUS, MPU6050_INTERRUPT_FF_BIT, b);
            return b == 1;
        }
        /** Get Motion Detection interrupt status.
         * This bit automatically sets to 1 when a Motion Detection interrupt has been
         * generated. The bit clears to 0 after the register has been read.
         * @return Current interrupt status
         * @see MPU6050_RA_INT_STATUS
         * @see MPU6050_INTERRUPT_MOT_BIT
         */
        bool getIntMotionStatus() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_STATUS, MPU6050_INTERRUPT_MOT_BIT, b);
            return b == 1;
        }
        /** Get Zero Motion Detection interrupt status.
         * This bit automatically sets to 1 when a Zero Motion Detection interrupt has
         * been generated. The bit clears to 0 after the register has been read.
         * @return Current interrupt status
         * @see MPU6050_RA_INT_STATUS
         * @see MPU6050_INTERRUPT_ZMOT_BIT
         */
        bool getIntZeroMotionStatus() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_STATUS, MPU6050_INTERRUPT_ZMOT_BIT, b);
            return b == 1;
        }
        /** Get FIFO Buffer Overflow interrupt status.
         * This bit automatically sets to 1 when a Free Fall interrupt has been
         * generated. The bit clears to 0 after the register has been read.
         * @return Current interrupt status
         * @see MPU6050_RA_INT_STATUS
         * @see MPU6050_INTERRUPT_FIFO_OFLOW_BIT
         */
        bool getIntFIFOBufferOverflowStatus() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_STATUS, MPU6050_INTERRUPT_FIFO_OFLOW_BIT, b);
            return b == 1;
        }
        /** Get I2C Master interrupt status.
         * This bit automatically sets to 1 when an I2C Master interrupt has been
         * generated. For a list of I2C Master interrupts, please refer to Register 54.
         * The bit clears to 0 after the register has been read.
         * @return Current interrupt status
         * @see MPU6050_RA_INT_STATUS
         * @see MPU6050_INTERRUPT_I2C_MST_INT_BIT
         */
        bool getIntI2CMasterStatus() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_STATUS, MPU6050_INTERRUPT_I2C_MST_INT_BIT, b);
            return b == 1;
        }
        /** Get Data Ready interrupt status.
         * This bit automatically sets to 1 when a Data Ready interrupt has been
         * generated. The bit clears to 0 after the register has been read.
         * @return Current interrupt status
         * @see MPU6050_RA_INT_STATUS
         * @see MPU6050_INTERRUPT_DATA_RDY_BIT
         */
        bool getIntDataReadyStatus() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_STATUS, MPU6050_INTERRUPT_DATA_RDY_BIT, b);
            return b == 1;
        }

        // ACCEL_*OUT_* registers

        /** Get raw 9-axis motion sensor readings (accel/gyro/compass).
         * FUNCTION NOT FULLY IMPLEMENTED YET.
         * @param ax 16-bit signed integer container for accelerometer X-axis value
         * @param ay 16-bit signed integer container for accelerometer Y-axis value
         * @param az 16-bit signed integer container for accelerometer Z-axis value
         * @param gx 16-bit signed integer container for gyroscope X-axis value
         * @param gy 16-bit signed integer container for gyroscope Y-axis value
         * @param gz 16-bit signed integer container for gyroscope Z-axis value
         * @param mx 16-bit signed integer container for magnetometer X-axis value
         * @param my 16-bit signed integer container for magnetometer Y-axis value
         * @param mz 16-bit signed integer container for magnetometer Z-axis value
         * @see getMotion6()
         * @see getAcceleration()
         * @see getRotation()
         * @see MPU6050_RA_ACCEL_XOUT_H
         */
        void getMotion9(short ax, short ay, short az, short gx, short gy, short gz, short mx, short my, short mz)
        {
    
	        //get accel and gyro
	        getMotion6(ax, ay, az, gx, gy, gz);
	
	        //read mag
	        _i2c.writeByte(devAddr, MPU6050_RA_INT_PIN_CFG, 0x02); //set i2c bypass enable pin to true to access magnetometer
        //	delay(10);
	        _i2c.writeByte(MPU9150_RA_MAG_ADDRESS, 0x0A, 0x01); //enable the magnetometer
        //	delay(10);
	        _i2c.readBytes(MPU9150_RA_MAG_ADDRESS, MPU9150_RA_MAG_XOUT_L, 6, buffer);
	        mx = (short)((buffer[1] << 8) | buffer[0]);
            my = (short)((buffer[3] << 8) | buffer[2]);
            mz = (short)((buffer[5] << 8) | buffer[4]);		
        }

        void getMag(short mx, short my, short mz)
        {
    
	        //read mag
	        _i2c.writeByte(devAddr, MPU6050_RA_INT_PIN_CFG, 0x02); //set i2c bypass enable pin to true to access magnetometer
        //	delay(10);
	        _i2c.writeByte(MPU9150_RA_MAG_ADDRESS, 0x0A, 0x01); //enable the magnetometer
        //	delay(10);
	        _i2c.readBytes(MPU9150_RA_MAG_ADDRESS, MPU9150_RA_MAG_XOUT_L, 6, buffer);
            mx = (short)((buffer[1] << 8) | buffer[0]);
            my = (short)((buffer[3] << 8) | buffer[2]);
            mz = (short)((buffer[5] << 8) | buffer[4]);		
        }

        /** Get raw 6-axis motion sensor readings (accel/gyro).
         * Retrieves all currently available motion sensor values.
         * @param ax 16-bit signed integer container for accelerometer X-axis value
         * @param ay 16-bit signed integer container for accelerometer Y-axis value
         * @param az 16-bit signed integer container for accelerometer Z-axis value
         * @param gx 16-bit signed integer container for gyroscope X-axis value
         * @param gy 16-bit signed integer container for gyroscope Y-axis value
         * @param gz 16-bit signed integer container for gyroscope Z-axis value
         * @see getAcceleration()
         * @see getRotation()
         * @see MPU6050_RA_ACCEL_XOUT_H
         */
        void getMotion6(short ax, short ay, short az, short gx, short gy, short gz)
        {
            _i2c.readBytes(devAddr, MPU6050_RA_ACCEL_XOUT_H, 14, buffer);
            ax = (short)((buffer[0] << 8) | buffer[1]);
            ay = (short)((buffer[2] << 8) | buffer[3]);
            az = (short)((buffer[4] << 8) | buffer[5]);
            gx = (short)((buffer[8] << 8) | buffer[9]);
            gy = (short)((buffer[10] << 8) | buffer[11]);
            gz = (short)((buffer[12] << 8) | buffer[13]);
        }
        /** Get 3-axis accelerometer readings.
         * These registers store the most recent accelerometer measurements.
         * Accelerometer measurements are written to these registers at the Sample Rate
         * as defined in Register 25.
         *
         * The accelerometer measurement registers, along with the temperature
         * measurement registers, gyroscope measurement registers, and external sensor
         * data registers, are composed of two sets of registers: an internal register
         * set and a user-facing read register set.
         *
         * The data within the accelerometer sensors' internal register set is always
         * updated at the Sample Rate. Meanwhile, the user-facing read register set
         * duplicates the internal register set's data values whenever the serial
         * interface is idle. This guarantees that a burst read of sensor registers will
         * read measurements from the same sampling instant. Note that if burst reads
         * are not used, the user is responsible for ensuring a set of single byte reads
         * correspond to a single sampling instant by checking the Data Ready interrupt.
         *
         * Each 16-bit accelerometer measurement has a full scale defined in ACCEL_FS
         * (Register 28). For each full scale setting, the accelerometers' sensitivity
         * per LSB in ACCEL_xOUT is shown in the table below:
         *
         * <pre>
         * AFS_SEL | Full Scale Range | LSB Sensitivity
         * --------+------------------+----------------
         * 0       | +/- 2g           | 8192 LSB/mg
         * 1       | +/- 4g           | 4096 LSB/mg
         * 2       | +/- 8g           | 2048 LSB/mg
         * 3       | +/- 16g          | 1024 LSB/mg
         * </pre>
         *
         * @param x 16-bit signed integer container for X-axis acceleration
         * @param y 16-bit signed integer container for Y-axis acceleration
         * @param z 16-bit signed integer container for Z-axis acceleration
         * @see MPU6050_RA_GYRO_XOUT_H
         */
        void getAcceleration(short x, short y, short z)
        {
            _i2c.readBytes(devAddr, MPU6050_RA_ACCEL_XOUT_H, 6, buffer);
            x = (short)((buffer[0] << 8) | buffer[1]);
            y = (short)((buffer[2] << 8) | buffer[3]);
            z = (short)((buffer[4] << 8) | buffer[5]);
        }
        /** Get X-axis accelerometer reading.
         * @return X-axis acceleration measurement in 16-bit 2's complement format
         * @see getMotion6()
         * @see MPU6050_RA_ACCEL_XOUT_H
         */
        short getAccelerationX() {
            _i2c.readBytes(devAddr, MPU6050_RA_ACCEL_XOUT_H, 2, buffer);
            return (short)((buffer[0] << 8) | buffer[1]);
        }
        /** Get Y-axis accelerometer reading.
         * @return Y-axis acceleration measurement in 16-bit 2's complement format
         * @see getMotion6()
         * @see MPU6050_RA_ACCEL_YOUT_H
         */
        short getAccelerationY() {
            _i2c.readBytes(devAddr, MPU6050_RA_ACCEL_YOUT_H, 2, buffer);
            return (short)((buffer[0] << 8) | buffer[1]);
        }
        /** Get Z-axis accelerometer reading.
         * @return Z-axis acceleration measurement in 16-bit 2's complement format
         * @see getMotion6()
         * @see MPU6050_RA_ACCEL_ZOUT_H
         */
        short getAccelerationZ() {
            _i2c.readBytes(devAddr, MPU6050_RA_ACCEL_ZOUT_H, 2, buffer);
            return (short)((buffer[0] << 8) | buffer[1]);
        }

        // TEMP_OUT_* registers

        /** Get current internal temperature.
         * @return Temperature reading in 16-bit 2's complement format
         * @see MPU6050_RA_TEMP_OUT_H
         */
        short getTemperature() {
            _i2c.readBytes(devAddr, MPU6050_RA_TEMP_OUT_H, 2, buffer);
            return (short)((buffer[0] << 8) | buffer[1]);
        }

        // GYRO_*OUT_* registers

        /** Get 3-axis gyroscope readings.
         * These gyroscope measurement registers, along with the accelerometer
         * measurement registers, temperature measurement registers, and external sensor
         * data registers, are composed of two sets of registers: an internal register
         * set and a user-facing read register set.
         * The data within the gyroscope sensors' internal register set is always
         * updated at the Sample Rate. Meanwhile, the user-facing read register set
         * duplicates the internal register set's data values whenever the serial
         * interface is idle. This guarantees that a burst read of sensor registers will
         * read measurements from the same sampling instant. Note that if burst reads
         * are not used, the user is responsible for ensuring a set of single byte reads
         * correspond to a single sampling instant by checking the Data Ready interrupt.
         *
         * Each 16-bit gyroscope measurement has a full scale defined in FS_SEL
         * (Register 27). For each full scale setting, the gyroscopes' sensitivity per
         * LSB in GYRO_xOUT is shown in the table below:
         *
         * <pre>
         * FS_SEL | Full Scale Range   | LSB Sensitivity
         * -------+--------------------+----------------
         * 0      | +/- 250 degrees/s  | 131 LSB/deg/s
         * 1      | +/- 500 degrees/s  | 65.5 LSB/deg/s
         * 2      | +/- 1000 degrees/s | 32.8 LSB/deg/s
         * 3      | +/- 2000 degrees/s | 16.4 LSB/deg/s
         * </pre>
         *
         * @param x 16-bit signed integer container for X-axis rotation
         * @param y 16-bit signed integer container for Y-axis rotation
         * @param z 16-bit signed integer container for Z-axis rotation
         * @see getMotion6()
         * @see MPU6050_RA_GYRO_XOUT_H
         */
        void getRotation(short x, short y, short z)
        {
            _i2c.readBytes(devAddr, MPU6050_RA_GYRO_XOUT_H, 6, buffer);
            x = (short)((buffer[0] << 8) | buffer[1]);
            y = (short)((buffer[2] << 8) | buffer[3]);
            z = (short)((buffer[4] << 8) | buffer[5]);
        }
        /** Get X-axis gyroscope reading.
         * @return X-axis rotation measurement in 16-bit 2's complement format
         * @see getMotion6()
         * @see MPU6050_RA_GYRO_XOUT_H
         */
        short getRotationX() {
            _i2c.readBytes(devAddr, MPU6050_RA_GYRO_XOUT_H, 2, buffer);
            return (short)((buffer[0] << 8) | buffer[1]);
        }
        /** Get Y-axis gyroscope reading.
         * @return Y-axis rotation measurement in 16-bit 2's complement format
         * @see getMotion6()
         * @see MPU6050_RA_GYRO_YOUT_H
         */
        short getRotationY() {
            _i2c.readBytes(devAddr, MPU6050_RA_GYRO_YOUT_H, 2, buffer);
            return (short)((buffer[0] << 8) | buffer[1]);
        }
        /** Get Z-axis gyroscope reading.
         * @return Z-axis rotation measurement in 16-bit 2's complement format
         * @see getMotion6()
         * @see MPU6050_RA_GYRO_ZOUT_H
         */
        short getRotationZ() {
            _i2c.readBytes(devAddr, MPU6050_RA_GYRO_ZOUT_H, 2, buffer);
            return (short)((buffer[0] << 8) | buffer[1]);
        }

        // EXT_SENS_DATA_* registers

        /** Read single byte from external sensor data register.
         * These registers store data read from external sensors by the Slave 0, 1, 2,
         * and 3 on the auxiliary I2C interface. Data read by Slave 4 is stored in
         * I2C_SLV4_DI (Register 53).
         *
         * External sensor data is written to these registers at the Sample Rate as
         * defined in Register 25. This access rate can be reduced by using the Slave
         * Delay Enable registers (Register 103).
         *
         * External sensor data registers, along with the gyroscope measurement
         * registers, accelerometer measurement registers, and temperature measurement
         * registers, are composed of two sets of registers: an internal register set
         * and a user-facing read register set.
         *
         * The data within the external sensors' internal register set is always updated
         * at the Sample Rate (or the reduced access rate) whenever the serial interface
         * is idle. This guarantees that a burst read of sensor registers will read
         * measurements from the same sampling instant. Note that if burst reads are not
         * used, the user is responsible for ensuring a set of single byte reads
         * correspond to a single sampling instant by checking the Data Ready interrupt.
         *
         * Data is placed in these external sensor data registers according to
         * I2C_SLV0_CTRL, I2C_SLV1_CTRL, I2C_SLV2_CTRL, and I2C_SLV3_CTRL (Registers 39,
         * 42, 45, and 48). When more than zero bytes are read (I2C_SLVx_LEN > 0) from
         * an enabled slave (I2C_SLVx_EN = 1), the slave is read at the Sample Rate (as
         * defined in Register 25) or delayed rate (if specified in Register 52 and
         * 103). During each Sample cycle, slave reads are performed in order of Slave
         * number. If all slaves are enabled with more than zero bytes to be read, the
         * order will be Slave 0, followed by Slave 1, Slave 2, and Slave 3.
         *
         * Each enabled slave will have EXT_SENS_DATA registers associated with it by
         * number of bytes read (I2C_SLVx_LEN) in order of slave number, starting from
         * EXT_SENS_DATA_00. Note that this means enabling or disabling a slave may
         * change the higher numbered slaves' associated registers. Furthermore, if
         * fewer total bytes are being read from the external sensors as a result of
         * such a change, then the data remaining in the registers which no longer have
         * an associated slave device (i.e. high numbered registers) will remain in
         * these previously allocated registers unless reset.
         *
         * If the sum of the read lengths of all SLVx transactions exceed the number of
         * available EXT_SENS_DATA registers, the excess bytes will be dropped. There
         * are 24 EXT_SENS_DATA registers and hence the total read lengths between all
         * the slaves cannot be greater than 24 or some bytes will be lost.
         *
         * Note: Slave 4's behavior is distinct from that of Slaves 0-3. For further
         * information regarding the characteristics of Slave 4, please refer to
         * Registers 49 to 53.
         *
         * EXAMPLE:
         * Suppose that Slave 0 is enabled with 4 bytes to be read (I2C_SLV0_EN = 1 and
         * I2C_SLV0_LEN = 4) while Slave 1 is enabled with 2 bytes to be read so that
         * I2C_SLV1_EN = 1 and I2C_SLV1_LEN = 2. In such a situation, EXT_SENS_DATA _00
         * through _03 will be associated with Slave 0, while EXT_SENS_DATA _04 and 05
         * will be associated with Slave 1. If Slave 2 is enabled as well, registers
         * starting from EXT_SENS_DATA_06 will be allocated to Slave 2.
         *
         * If Slave 2 is disabled while Slave 3 is enabled in this same situation, then
         * registers starting from EXT_SENS_DATA_06 will be allocated to Slave 3
         * instead.
         *
         * REGISTER ALLOCATION FOR DYNAMIC DISABLE VS. NORMAL DISABLE:
         * If a slave is disabled at any time, the space initially allocated to the
         * slave in the EXT_SENS_DATA register, will remain associated with that slave.
         * This is to avoid dynamic adjustment of the register allocation.
         *
         * The allocation of the EXT_SENS_DATA registers is recomputed only when (1) all
         * slaves are disabled, or (2) the I2C_MST_RST bit is set (Register 106).
         *
         * This above is also true if one of the slaves gets NACKed and stops
         * functioning.
         *
         * @param position Starting position (0-23)
         * @return Byte read from register
         */
        uint getExternalSensorByte(int position) {
            byte b = 0;
            _i2c.readByte(devAddr, (byte)(MPU6050_RA_EXT_SENS_DATA_00 + position), b);
            return b;
        }
        /** Read word (2 bytes) from external sensor data registers.
         * @param position Starting position (0-21)
         * @return Word read from register
         * @see getExternalSensorByte()
         */
        ushort getExternalSensorWord(int position) {
            _i2c.readBytes(devAddr, (byte)(MPU6050_RA_EXT_SENS_DATA_00 + position), 2, buffer);
            return (ushort)((buffer[0] << 8) | buffer[1]);
        }
        /** Read double word (4 bytes) from external sensor data registers.
         * @param position Starting position (0-20)
         * @return Double word read from registers
         * @see getExternalSensorByte()
         */
        uint getExternalSensorDWord(int position) {
            _i2c.readBytes(devAddr, (Byte)(MPU6050_RA_EXT_SENS_DATA_00 + position), 4, buffer);
            return (uint)((buffer[0] << 24) | ((buffer[1] << 16) | ((buffer[2] << 8) | buffer[3])));
        }

        // MOT_DETECT_STATUS register

        /** Get X-axis negative motion detection interrupt status.
         * @return Motion detection status
         * @see MPU6050_RA_MOT_DETECT_STATUS
         * @see MPU6050_MOTION_MOT_XNEG_BIT
         */
        bool getXNegMotionDetected() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_MOT_DETECT_STATUS, MPU6050_MOTION_MOT_XNEG_BIT, b);
            return b == 1;
        }
        /** Get X-axis positive motion detection interrupt status.
         * @return Motion detection status
         * @see MPU6050_RA_MOT_DETECT_STATUS
         * @see MPU6050_MOTION_MOT_XPOS_BIT
         */
        bool getXPosMotionDetected() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_MOT_DETECT_STATUS, MPU6050_MOTION_MOT_XPOS_BIT, b);
            return b == 1;
        }
        /** Get Y-axis negative motion detection interrupt status.
         * @return Motion detection status
         * @see MPU6050_RA_MOT_DETECT_STATUS
         * @see MPU6050_MOTION_MOT_YNEG_BIT
         */
        bool getYNegMotionDetected() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_MOT_DETECT_STATUS, MPU6050_MOTION_MOT_YNEG_BIT, b);
            return b == 1;
        }
        /** Get Y-axis positive motion detection interrupt status.
         * @return Motion detection status
         * @see MPU6050_RA_MOT_DETECT_STATUS
         * @see MPU6050_MOTION_MOT_YPOS_BIT
         */
        bool getYPosMotionDetected() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_MOT_DETECT_STATUS, MPU6050_MOTION_MOT_YPOS_BIT, b);
            return b == 1;
        }
        /** Get Z-axis negative motion detection interrupt status.
         * @return Motion detection status
         * @see MPU6050_RA_MOT_DETECT_STATUS
         * @see MPU6050_MOTION_MOT_ZNEG_BIT
         */
        bool getZNegMotionDetected() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_MOT_DETECT_STATUS, MPU6050_MOTION_MOT_ZNEG_BIT, b);
            return b == 1;
        }
        /** Get Z-axis positive motion detection interrupt status.
         * @return Motion detection status
         * @see MPU6050_RA_MOT_DETECT_STATUS
         * @see MPU6050_MOTION_MOT_ZPOS_BIT
         */
        bool getZPosMotionDetected() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_MOT_DETECT_STATUS, MPU6050_MOTION_MOT_ZPOS_BIT, b);
            return b == 1;
        }
        /** Get zero motion detection interrupt status.
         * @return Motion detection status
         * @see MPU6050_RA_MOT_DETECT_STATUS
         * @see MPU6050_MOTION_MOT_ZRMOT_BIT
         */
        bool getZeroMotionDetected() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_MOT_DETECT_STATUS, MPU6050_MOTION_MOT_ZRMOT_BIT, b);
            return b == 1;
        }

        // I2C_SLV*_DO register

        /** Write byte to Data Output container for specified slave.
         * This register holds the output data written into Slave when Slave is set to
         * write mode. For further information regarding Slave control, please
         * refer to Registers 37 to 39 and immediately following.
         * @param num Slave number (0-3)
         * @param data Byte to write
         * @see MPU6050_RA_I2C_SLV0_DO
         */
        void setSlaveOutputByte(uint num, byte data) {
            if (num > 3) return;
            _i2c.writeByte(devAddr, (byte)(MPU6050_RA_I2C_SLV0_DO + num), data);
        }

        // I2C_MST_DELAY_CTRL register

        /** Get external data shadow delay enabled status.
         * This register is used to specify the timing of external sensor data
         * shadowing. When DELAY_ES_SHADOW is set to 1, shadowing of external
         * sensor data is delayed until all data has been received.
         * @return Current external data shadow delay enabled status.
         * @see MPU6050_RA_I2C_MST_DELAY_CTRL
         * @see MPU6050_DELAYCTRL_DELAY_ES_SHADOW_BIT
         */
        bool getExternalShadowDelayEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_MST_DELAY_CTRL, MPU6050_DELAYCTRL_DELAY_ES_SHADOW_BIT, b);
            return b == 1;
        }
        /** Set external data shadow delay enabled status.
         * @param enabled New external data shadow delay enabled status.
         * @see getExternalShadowDelayEnabled()
         * @see MPU6050_RA_I2C_MST_DELAY_CTRL
         * @see MPU6050_DELAYCTRL_DELAY_ES_SHADOW_BIT
         */
        void setExternalShadowDelayEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_I2C_MST_DELAY_CTRL, MPU6050_DELAYCTRL_DELAY_ES_SHADOW_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get slave delay enabled status.
         * When a particular slave delay is enabled, the rate of access for the that
         * slave device is reduced. When a slave's access rate is decreased relative to
         * the Sample Rate, the slave is accessed every:
         *
         *     1 / (1 + I2C_MST_DLY) Samples
         *
         * This base Sample Rate in turn is determined by SMPLRT_DIV (register  * 25)
         * and DLPF_CFG (register 26).
         *
         * For further information regarding I2C_MST_DLY, please refer to register 52.
         * For further information regarding the Sample Rate, please refer to register 25.
         *
         * @param num Slave number (0-4)
         * @return Current slave delay enabled status.
         * @see MPU6050_RA_I2C_MST_DELAY_CTRL
         * @see MPU6050_DELAYCTRL_I2C_SLV0_DLY_EN_BIT
         */
        bool getSlaveDelayEnabled(byte num) {
            // MPU6050_DELAYCTRL_I2C_SLV4_DLY_EN_BIT is 4, SLV3 is 3, etc.
            if (num > 4) return false;
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_I2C_MST_DELAY_CTRL, num, b);
            return b == 1;
        }
        /** Set slave delay enabled status.
         * @param num Slave number (0-4)
         * @param enabled New slave delay enabled status.
         * @see MPU6050_RA_I2C_MST_DELAY_CTRL
         * @see MPU6050_DELAYCTRL_I2C_SLV0_DLY_EN_BIT
         */
        void setSlaveDelayEnabled(byte num, bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_I2C_MST_DELAY_CTRL, num, (byte)(enabled ? 1 : 0));
        }

        // SIGNAL_PATH_RESET register

        /** Reset gyroscope signal path.
         * The reset will revert the signal path analog to digital converters and
         * filters to their power up configurations.
         * @see MPU6050_RA_SIGNAL_PATH_RESET
         * @see MPU6050_PATHRESET_GYRO_RESET_BIT
         */
        void resetGyroscopePath() {
            byte b = 1;
            _i2c.writeBit(devAddr, MPU6050_RA_SIGNAL_PATH_RESET, MPU6050_PATHRESET_GYRO_RESET_BIT, b);
        }
        /** Reset accelerometer signal path.
         * The reset will revert the signal path analog to digital converters and
         * filters to their power up configurations.
         * @see MPU6050_RA_SIGNAL_PATH_RESET
         * @see MPU6050_PATHRESET_ACCEL_RESET_BIT
         */
        void resetAccelerometerPath() {
            byte b = 1;
            _i2c.writeBit(devAddr, MPU6050_RA_SIGNAL_PATH_RESET, MPU6050_PATHRESET_ACCEL_RESET_BIT, b);
        }
        /** Reset temperature sensor signal path.
         * The reset will revert the signal path analog to digital converters and
         * filters to their power up configurations.
         * @see MPU6050_RA_SIGNAL_PATH_RESET
         * @see MPU6050_PATHRESET_TEMP_RESET_BIT
         */
        void resetTemperaturePath() {
            byte b = 1;
            _i2c.writeBit(devAddr, MPU6050_RA_SIGNAL_PATH_RESET, MPU6050_PATHRESET_TEMP_RESET_BIT, b);
        }

        // MOT_DETECT_CTRL register

        /** Get accelerometer power-on delay.
         * The accelerometer data path provides samples to the sensor registers, Motion
         * detection, Zero Motion detection, and Free Fall detection modules. The
         * signal path contains filters which must be flushed on wake-up with new
         * samples before the detection modules begin operations. The default wake-up
         * delay, of 4ms can be lengthened by up to 3ms. This additional delay is
         * specified in ACCEL_ON_DELAY in units of 1 LSB = 1 ms. The user may select
         * any value above zero unless instructed otherwise by InvenSense. Please refer
         * to Section 8 of the MPU-6000/MPU-6050 Product Specification document for
         * further information regarding the detection modules.
         * @return Current accelerometer power-on delay
         * @see MPU6050_RA_MOT_DETECT_CTRL
         * @see MPU6050_DETECT_ACCEL_ON_DELAY_BIT
         */
        byte getAccelerometerPowerOnDelay() {
            byte b = 0;
            _i2c.readBits(devAddr, MPU6050_RA_MOT_DETECT_CTRL, MPU6050_DETECT_ACCEL_ON_DELAY_BIT, MPU6050_DETECT_ACCEL_ON_DELAY_LENGTH, b);
            return b;
        }
        /** Set accelerometer power-on delay.
         * @param delay New accelerometer power-on delay (0-3)
         * @see getAccelerometerPowerOnDelay()
         * @see MPU6050_RA_MOT_DETECT_CTRL
         * @see MPU6050_DETECT_ACCEL_ON_DELAY_BIT
         */
        void setAccelerometerPowerOnDelay(byte delay) {
            _i2c.writeBits(devAddr, MPU6050_RA_MOT_DETECT_CTRL, MPU6050_DETECT_ACCEL_ON_DELAY_BIT, MPU6050_DETECT_ACCEL_ON_DELAY_LENGTH, delay);
        }
        /** Get Free Fall detection counter decrement configuration.
         * Detection is registered by the Free Fall detection module after accelerometer
         * measurements meet their respective threshold conditions over a specified
         * number of samples. When the threshold conditions are met, the corresponding
         * detection counter increments by 1. The user may control the rate at which the
         * detection counter decrements when the threshold condition is not met by
         * configuring FF_COUNT. The decrement rate can be set according to the
         * following table:
         *
         * <pre>
         * FF_COUNT | Counter Decrement
         * ---------+------------------
         * 0        | Reset
         * 1        | 1
         * 2        | 2
         * 3        | 4
         * </pre>
         *
         * When FF_COUNT is configured to 0 (reset), any non-qualifying sample will
         * reset the counter to 0. For further information on Free Fall detection,
         * please refer to Registers 29 to 32.
         *
         * @return Current decrement configuration
         * @see MPU6050_RA_MOT_DETECT_CTRL
         * @see MPU6050_DETECT_FF_COUNT_BIT
         */
        byte getFreefallDetectionCounterDecrement() {
            byte b = 0;
            _i2c.readBits(devAddr, MPU6050_RA_MOT_DETECT_CTRL, MPU6050_DETECT_FF_COUNT_BIT, MPU6050_DETECT_FF_COUNT_LENGTH, b);
            return b;
        }
        /** Set Free Fall detection counter decrement configuration.
         * @param decrement New decrement configuration value
         * @see getFreefallDetectionCounterDecrement()
         * @see MPU6050_RA_MOT_DETECT_CTRL
         * @see MPU6050_DETECT_FF_COUNT_BIT
         */
        void setFreefallDetectionCounterDecrement(byte decrement) {
            _i2c.writeBits(devAddr, MPU6050_RA_MOT_DETECT_CTRL, MPU6050_DETECT_FF_COUNT_BIT, MPU6050_DETECT_FF_COUNT_LENGTH, decrement);
        }
        /** Get Motion detection counter decrement configuration.
         * Detection is registered by the Motion detection module after accelerometer
         * measurements meet their respective threshold conditions over a specified
         * number of samples. When the threshold conditions are met, the corresponding
         * detection counter increments by 1. The user may control the rate at which the
         * detection counter decrements when the threshold condition is not met by
         * configuring MOT_COUNT. The decrement rate can be set according to the
         * following table:
         *
         * <pre>
         * MOT_COUNT | Counter Decrement
         * ----------+------------------
         * 0         | Reset
         * 1         | 1
         * 2         | 2
         * 3         | 4
         * </pre>
         *
         * When MOT_COUNT is configured to 0 (reset), any non-qualifying sample will
         * reset the counter to 0. For further information on Motion detection,
         * please refer to Registers 29 to 32.
         *
         */
        byte getMotionDetectionCounterDecrement() {
            byte b = 0;
            _i2c.readBits(devAddr, MPU6050_RA_MOT_DETECT_CTRL, MPU6050_DETECT_MOT_COUNT_BIT, MPU6050_DETECT_MOT_COUNT_LENGTH, b);
            return b;
        }
        /** Set Motion detection counter decrement configuration.
         * @param decrement New decrement configuration value
         * @see getMotionDetectionCounterDecrement()
         * @see MPU6050_RA_MOT_DETECT_CTRL
         * @see MPU6050_DETECT_MOT_COUNT_BIT
         */
        void setMotionDetectionCounterDecrement(byte decrement) {
            _i2c.writeBits(devAddr, MPU6050_RA_MOT_DETECT_CTRL, MPU6050_DETECT_MOT_COUNT_BIT, MPU6050_DETECT_MOT_COUNT_LENGTH, decrement);
        }

        // USER_CTRL register

        /** Get FIFO enabled status.
         * When this bit is set to 0, the FIFO buffer is disabled. The FIFO buffer
         * cannot be written to or read from while disabled. The FIFO buffer's state
         * does not change unless the MPU-60X0 is power cycled.
         * @return Current FIFO enabled status
         * @see MPU6050_RA_USER_CTRL
         * @see MPU6050_USERCTRL_FIFO_EN_BIT
         */
        bool getFIFOEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_USER_CTRL, MPU6050_USERCTRL_FIFO_EN_BIT, b);
            return b == 1;
        }
        /** Set FIFO enabled status.
         * @param enabled New FIFO enabled status
         * @see getFIFOEnabled()
         * @see MPU6050_RA_USER_CTRL
         * @see MPU6050_USERCTRL_FIFO_EN_BIT
         */
        void setFIFOEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_USER_CTRL, MPU6050_USERCTRL_FIFO_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get I2C Master Mode enabled status.
         * When this mode is enabled, the MPU-60X0 acts as the I2C Master to the
         * external sensor slave devices on the auxiliary I2C bus. When this bit is
         * cleared to 0, the auxiliary I2C bus lines (AUX_DA and AUX_CL) are logically
         * driven by the primary I2C bus (SDA and SCL). This is a precondition to
         * enabling Bypass Mode. For further information regarding Bypass Mode, please
         * refer to Register 55.
         * @return Current I2C Master Mode enabled status
         * @see MPU6050_RA_USER_CTRL
         * @see MPU6050_USERCTRL_I2C_MST_EN_BIT
         */
        bool getI2CMasterModeEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_USER_CTRL, MPU6050_USERCTRL_I2C_MST_EN_BIT, b);
            return b == 1;
        }
        /** Set I2C Master Mode enabled status.
         * @param enabled New I2C Master Mode enabled status
         * @see getI2CMasterModeEnabled()
         * @see MPU6050_RA_USER_CTRL
         * @see MPU6050_USERCTRL_I2C_MST_EN_BIT
         */
        void setI2CMasterModeEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_USER_CTRL, MPU6050_USERCTRL_I2C_MST_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Switch from I2C to SPI mode (MPU-6000 only)
         * If this is set, the primary SPI interface will be enabled in place of the
         * disabled primary I2C interface.
         */
        void switchSPIEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_USER_CTRL, MPU6050_USERCTRL_I2C_IF_DIS_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Reset the FIFO.
         * This bit resets the FIFO buffer when set to 1 while FIFO_EN equals 0. This
         * bit automatically clears to 0 after the reset has been triggered.
         * @see MPU6050_RA_USER_CTRL
         * @see MPU6050_USERCTRL_FIFO_RESET_BIT
         */
        void resetFIFO() {
            byte b = 1;
            _i2c.writeBit(devAddr, MPU6050_RA_USER_CTRL, MPU6050_USERCTRL_FIFO_RESET_BIT, b);
        }
        /** Reset the I2C Master.
         * This bit resets the I2C Master when set to 1 while I2C_MST_EN equals 0.
         * This bit automatically clears to 0 after the reset has been triggered.
         * @see MPU6050_RA_USER_CTRL
         * @see MPU6050_USERCTRL_I2C_MST_RESET_BIT
         */
        void resetI2CMaster() {
            byte b = 1;
            _i2c.writeBit(devAddr, MPU6050_RA_USER_CTRL, MPU6050_USERCTRL_I2C_MST_RESET_BIT, b);
        }
        /** Reset all sensor registers and signal paths.
         * When set to 1, this bit resets the signal paths for all sensors (gyroscopes,
         * accelerometers, and temperature sensor). This operation will also clear the
         * sensor registers. This bit automatically clears to 0 after the reset has been
         * triggered.
         *
         * When resetting only the signal path (and not the sensor registers), please
         * use Register 104, SIGNAL_PATH_RESET.
         *
         * @see MPU6050_RA_USER_CTRL
         * @see MPU6050_USERCTRL_SIG_COND_RESET_BIT
         */
        void resetSensors() {
            byte b = 1;
            _i2c.writeBit(devAddr, MPU6050_RA_USER_CTRL, MPU6050_USERCTRL_SIG_COND_RESET_BIT, b);
        }

        // PWR_MGMT_1 register

        /** Trigger a full device reset.
         * A small delay of ~50ms may be desirable after triggering a reset.
         * @see MPU6050_RA_PWR_MGMT_1
         * @see MPU6050_PWR1_DEVICE_RESET_BIT
         */
        void reset() {
            byte b = 1;
            _i2c.writeBit(devAddr, MPU6050_RA_PWR_MGMT_1, MPU6050_PWR1_DEVICE_RESET_BIT, b);
        }
        /** Get sleep mode status.
         * Setting the SLEEP bit in the register puts the device into very low power
         * sleep mode. In this mode, only the serial interface and internal registers
         * remain active, allowing for a very low standby current. Clearing this bit
         * puts the device back into normal mode. To save power, the individual standby
         * selections for each of the gyros should be used if any gyro axis is not used
         * by the application.
         * @return Current sleep mode enabled status
         * @see MPU6050_RA_PWR_MGMT_1
         * @see MPU6050_PWR1_SLEEP_BIT
         */
        bool getSleepEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_PWR_MGMT_1, MPU6050_PWR1_SLEEP_BIT, b);
            return b == 1;
        }
        /** Set sleep mode status.
         * @param enabled New sleep mode enabled status
         * @see getSleepEnabled()
         * @see MPU6050_RA_PWR_MGMT_1
         * @see MPU6050_PWR1_SLEEP_BIT
         */
        void setSleepEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_PWR_MGMT_1, MPU6050_PWR1_SLEEP_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get wake cycle enabled status.
         * When this bit is set to 1 and SLEEP is disabled, the MPU-60X0 will cycle
         * between sleep mode and waking up to take a single sample of data from active
         * sensors at a rate determined by LP_WAKE_CTRL (register 108).
         * @return Current sleep mode enabled status
         * @see MPU6050_RA_PWR_MGMT_1
         * @see MPU6050_PWR1_CYCLE_BIT
         */
        bool getWakeCycleEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_PWR_MGMT_1, MPU6050_PWR1_CYCLE_BIT, b);
            return b == 1;
        }
        /** Set wake cycle enabled status.
         * @param enabled New sleep mode enabled status
         * @see getWakeCycleEnabled()
         * @see MPU6050_RA_PWR_MGMT_1
         * @see MPU6050_PWR1_CYCLE_BIT
         */
        void setWakeCycleEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_PWR_MGMT_1, MPU6050_PWR1_CYCLE_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get temperature sensor enabled status.
         * Control the usage of the internal temperature sensor.
         *
         * Note: this register stores the *disabled* value, but for consistency with the
         * rest of the code, the function is named and used with standard true/false
         * values to indicate whether the sensor is enabled or disabled, respectively.
         *
         * @return Current temperature sensor enabled status
         * @see MPU6050_RA_PWR_MGMT_1
         * @see MPU6050_PWR1_TEMP_DIS_BIT
         */
        bool getTempSensorEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_PWR_MGMT_1, MPU6050_PWR1_TEMP_DIS_BIT, b);
            return b == 0; // 1 is actually disabled here
        }
        /** Set temperature sensor enabled status.
         * Note: this register stores the *disabled* value, but for consistency with the
         * rest of the code, the function is named and used with standard true/false
         * values to indicate whether the sensor is enabled or disabled, respectively.
         *
         * @param enabled New temperature sensor enabled status
         * @see getTempSensorEnabled()
         * @see MPU6050_RA_PWR_MGMT_1
         * @see MPU6050_PWR1_TEMP_DIS_BIT
         */
        void setTempSensorEnabled(bool enabled) {
            // 1 is actually disabled here
            _i2c.writeBit(devAddr, MPU6050_RA_PWR_MGMT_1, MPU6050_PWR1_TEMP_DIS_BIT, (byte)(enabled ? 0 : 1));
        }
        /** Get clock source setting.
         * @return Current clock source setting
         * @see MPU6050_RA_PWR_MGMT_1
         * @see MPU6050_PWR1_CLKSEL_BIT
         * @see MPU6050_PWR1_CLKSEL_LENGTH
         */
        byte getClockSource() {
            byte b = 0;
            _i2c.readBits(devAddr, MPU6050_RA_PWR_MGMT_1, MPU6050_PWR1_CLKSEL_BIT, MPU6050_PWR1_CLKSEL_LENGTH, b);
            return b;
        }
        /** Set clock source setting.
         * An internal 8MHz oscillator, gyroscope based clock, or external sources can
         * be selected as the MPU-60X0 clock source. When the internal 8 MHz oscillator
         * or an external source is chosen as the clock source, the MPU-60X0 can operate
         * in low power modes with the gyroscopes disabled.
         *
         * Upon power up, the MPU-60X0 clock source defaults to the internal oscillator.
         * However, it is highly recommended that the device be configured to use one of
         * the gyroscopes (or an external clock source) as the clock reference for
         * improved stability. The clock source can be selected according to the following table:
         *
         * <pre>
         * CLK_SEL | Clock Source
         * --------+--------------------------------------
         * 0       | Internal oscillator
         * 1       | PLL with X Gyro reference
         * 2       | PLL with Y Gyro reference
         * 3       | PLL with Z Gyro reference
         * 4       | PLL with external 32.768kHz reference
         * 5       | PLL with external 19.2MHz reference
         * 6       | Reserved
         * 7       | Stops the clock and keeps the timing generator in reset
         * </pre>
         *
         * @param source New clock source setting
         * @see getClockSource()
         * @see MPU6050_RA_PWR_MGMT_1
         * @see MPU6050_PWR1_CLKSEL_BIT
         * @see MPU6050_PWR1_CLKSEL_LENGTH
         */
        void setClockSource(byte source) {
            _i2c.writeBits(devAddr, MPU6050_RA_PWR_MGMT_1, MPU6050_PWR1_CLKSEL_BIT, MPU6050_PWR1_CLKSEL_LENGTH, source);
        }

        // PWR_MGMT_2 register

        /** Get wake frequency in Accel-Only Low Power Mode.
         * The MPU-60X0 can be put into Accerlerometer Only Low Power Mode by setting
         * PWRSEL to 1 in the Power Management 1 register (Register 107). In this mode,
         * the device will power off all devices except for the primary I2C interface,
         * waking only the accelerometer at fixed intervals to take a single
         * measurement. The frequency of wake-ups can be configured with LP_WAKE_CTRL
         * as shown below:
         *
         * <pre>
         * LP_WAKE_CTRL | Wake-up Frequency
         * -------------+------------------
         * 0            | 1.25 Hz
         * 1            | 2.5 Hz
         * 2            | 5 Hz
         * 3            | 10 Hz
         * <pre>
         *
         * For further information regarding the MPU-60X0's power modes, please refer to
         * Register 107.
         *
         * @return Current wake frequency
         * @see MPU6050_RA_PWR_MGMT_2
         */
        byte getWakeFrequency() {
            byte b = 0;
            _i2c.readBits(devAddr, MPU6050_RA_PWR_MGMT_2, MPU6050_PWR2_LP_WAKE_CTRL_BIT, MPU6050_PWR2_LP_WAKE_CTRL_LENGTH, b);
            return b;
        }
        /** Set wake frequency in Accel-Only Low Power Mode.
         * @param frequency New wake frequency
         * @see MPU6050_RA_PWR_MGMT_2
         */
        void setWakeFrequency(byte frequency) {
            _i2c.writeBits(devAddr, MPU6050_RA_PWR_MGMT_2, MPU6050_PWR2_LP_WAKE_CTRL_BIT, MPU6050_PWR2_LP_WAKE_CTRL_LENGTH, frequency);
        }

        /** Get X-axis accelerometer standby enabled status.
         * If enabled, the X-axis will not gather or report data (or use power).
         * @return Current X-axis standby enabled status
         * @see MPU6050_RA_PWR_MGMT_2
         * @see MPU6050_PWR2_STBY_XA_BIT
         */
        bool getStandbyXAccelEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_PWR_MGMT_2, MPU6050_PWR2_STBY_XA_BIT, b);
            return b == 1;
        }
        /** Set X-axis accelerometer standby enabled status.
         * @param New X-axis standby enabled status
         * @see getStandbyXAccelEnabled()
         * @see MPU6050_RA_PWR_MGMT_2
         * @see MPU6050_PWR2_STBY_XA_BIT
         */
        void setStandbyXAccelEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_PWR_MGMT_2, MPU6050_PWR2_STBY_XA_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get Y-axis accelerometer standby enabled status.
         * If enabled, the Y-axis will not gather or report data (or use power).
         * @return Current Y-axis standby enabled status
         * @see MPU6050_RA_PWR_MGMT_2
         * @see MPU6050_PWR2_STBY_YA_BIT
         */
        bool getStandbyYAccelEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_PWR_MGMT_2, MPU6050_PWR2_STBY_YA_BIT, b);
            return b == 1;
        }
        /** Set Y-axis accelerometer standby enabled status.
         * @param New Y-axis standby enabled status
         * @see getStandbyYAccelEnabled()
         * @see MPU6050_RA_PWR_MGMT_2
         * @see MPU6050_PWR2_STBY_YA_BIT
         */
        void setStandbyYAccelEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_PWR_MGMT_2, MPU6050_PWR2_STBY_YA_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get Z-axis accelerometer standby enabled status.
         * If enabled, the Z-axis will not gather or report data (or use power).
         * @return Current Z-axis standby enabled status
         * @see MPU6050_RA_PWR_MGMT_2
         * @see MPU6050_PWR2_STBY_ZA_BIT
         */
        bool getStandbyZAccelEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_PWR_MGMT_2, MPU6050_PWR2_STBY_ZA_BIT, b);
            return b == 1;
        }
        /** Set Z-axis accelerometer standby enabled status.
         * @param New Z-axis standby enabled status
         * @see getStandbyZAccelEnabled()
         * @see MPU6050_RA_PWR_MGMT_2
         * @see MPU6050_PWR2_STBY_ZA_BIT
         */
        void setStandbyZAccelEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_PWR_MGMT_2, MPU6050_PWR2_STBY_ZA_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get X-axis gyroscope standby enabled status.
         * If enabled, the X-axis will not gather or report data (or use power).
         * @return Current X-axis standby enabled status
         * @see MPU6050_RA_PWR_MGMT_2
         * @see MPU6050_PWR2_STBY_XG_BIT
         */
        bool getStandbyXGyroEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_PWR_MGMT_2, MPU6050_PWR2_STBY_XG_BIT, b);
            return b == 1;
        }
        /** Set X-axis gyroscope standby enabled status.
         * @param New X-axis standby enabled status
         * @see getStandbyXGyroEnabled()
         * @see MPU6050_RA_PWR_MGMT_2
         * @see MPU6050_PWR2_STBY_XG_BIT
         */
        void setStandbyXGyroEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_PWR_MGMT_2, MPU6050_PWR2_STBY_XG_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get Y-axis gyroscope standby enabled status.
         * If enabled, the Y-axis will not gather or report data (or use power).
         * @return Current Y-axis standby enabled status
         * @see MPU6050_RA_PWR_MGMT_2
         * @see MPU6050_PWR2_STBY_YG_BIT
         */
        bool getStandbyYGyroEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_PWR_MGMT_2, MPU6050_PWR2_STBY_YG_BIT, b);
            return b == 1;
        }
        /** Set Y-axis gyroscope standby enabled status.
         * @param New Y-axis standby enabled status
         * @see getStandbyYGyroEnabled()
         * @see MPU6050_RA_PWR_MGMT_2
         * @see MPU6050_PWR2_STBY_YG_BIT
         */
        void setStandbyYGyroEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_PWR_MGMT_2, MPU6050_PWR2_STBY_YG_BIT, (byte)(enabled ? 1 : 0));
        }
        /** Get Z-axis gyroscope standby enabled status.
         * If enabled, the Z-axis will not gather or report data (or use power).
         * @return Current Z-axis standby enabled status
         * @see MPU6050_RA_PWR_MGMT_2
         * @see MPU6050_PWR2_STBY_ZG_BIT
         */
        bool getStandbyZGyroEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_PWR_MGMT_2, MPU6050_PWR2_STBY_ZG_BIT, b);
            return b == 1;
        }
        /** Set Z-axis gyroscope standby enabled status.
         * @param New Z-axis standby enabled status
         * @see getStandbyZGyroEnabled()
         * @see MPU6050_RA_PWR_MGMT_2
         * @see MPU6050_PWR2_STBY_ZG_BIT
         */
        void setStandbyZGyroEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_PWR_MGMT_2, MPU6050_PWR2_STBY_ZG_BIT, (byte)(enabled ? 1 : 0));
        }

        /** Get current FIFO buffer size.
         * This value indicates the number of bytes stored in the FIFO buffer. This
         * number is in turn the number of bytes that can be read from the FIFO buffer
         * and it is directly proportional to the number of samples available given the
         * set of sensor data bound to be stored in the FIFO (register 35 and 36).
         * @return Current FIFO buffer size
         */
        ushort getFIFOCount() {
            _i2c.readBytes(devAddr, MPU6050_RA_FIFO_COUNTH, 2, buffer, 1000);
            return (ushort)((buffer[0] << 8) | buffer[1]);
        }

        // FIFO_R_W register

        /** Get byte from FIFO buffer.
         * This register is used to read and write data from the FIFO buffer. Data is
         * written to the FIFO in order of register number (from lowest to highest). If
         * all the FIFO enable flags (see below) are enabled and all External Sensor
         * Data registers (Registers 73 to 96) are associated with a Slave device, the
         * contents of registers 59 through 96 will be written in order at the Sample
         * Rate.
         *
         * The contents of the sensor data registers (Registers 59 to 96) are written
         * into the FIFO buffer when their corresponding FIFO enable flags are set to 1
         * in FIFO_EN (Register 35). An additional flag for the sensor data registers
         * associated with I2C Slave 3 can be found in I2C_MST_CTRL (Register 36).
         *
         * If the FIFO buffer has overflowed, the status bit FIFO_OFLOW_INT is
         * automatically set to 1. This bit is located in INT_STATUS (Register 58).
         * When the FIFO buffer has overflowed, the oldest data will be lost and new
         * data will be written to the FIFO.
         *
         * If the FIFO buffer is empty, reading this register will return the last byte
         * that was previously read from the FIFO until new data is available. The user
         * should check FIFO_COUNT to ensure that the FIFO buffer is not read when
         * empty.
         *
         * @return Byte from FIFO buffer
         */
        byte getFIFOByte() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_FIFO_R_W, b);
            return b;
        }
        void getFIFOBytes(byte[] data, byte length)
        {
            _i2c.readBytes(devAddr, MPU6050_RA_FIFO_R_W, length, data, 1000);
        }
        /** Write byte to FIFO buffer.
         * @see getFIFOByte()
         * @see MPU6050_RA_FIFO_R_W
         */
        void setFIFOByte(byte data) {
            _i2c.writeByte(devAddr, MPU6050_RA_FIFO_R_W, data);
        }

        // WHO_AM_I register

        /** Get Device ID.
         * This register is used to verify the identity of the device (0b110100, 0x34).
         * @return Device ID (6 bits only! should be 0x34)
         * @see MPU6050_RA_WHO_AM_I
         * @see MPU6050_WHO_AM_I_BIT
         * @see MPU6050_WHO_AM_I_LENGTH
         */
        byte getDeviceID() {
            byte b = 0;
            _i2c.readBits(devAddr, MPU6050_RA_WHO_AM_I, MPU6050_WHO_AM_I_BIT, MPU6050_WHO_AM_I_LENGTH, b);
            return b;
        }
        /** Set Device ID.
         * Write a new ID into the WHO_AM_I register (no idea why this should ever be
         * necessary though).
         * @param id New device ID to set.
         * @see getDeviceID()
         * @see MPU6050_RA_WHO_AM_I
         * @see MPU6050_WHO_AM_I_BIT
         * @see MPU6050_WHO_AM_I_LENGTH
         */
        void setDeviceID(byte id) {
            _i2c.writeBits(devAddr, MPU6050_RA_WHO_AM_I, MPU6050_WHO_AM_I_BIT, MPU6050_WHO_AM_I_LENGTH, id);
        }

#if DMP_METHODS
        // ======== UNDOCUMENTED/DMP REGISTERS/METHODS ========

        // XG_OFFS_TC register

        byte getOTPBankValid() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_XG_OFFS_TC, MPU6050_TC_OTP_BNK_VLD_BIT, b);
            return b;
        }
        void setOTPBankValid(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_XG_OFFS_TC, MPU6050_TC_OTP_BNK_VLD_BIT, (byte)(enabled ? 1 : 0));
        }

        byte getXGyroOffset() {
            byte b = 0;
            _i2c.readBits(devAddr, MPU6050_RA_XG_OFFS_TC, MPU6050_TC_OFFSET_BIT, MPU6050_TC_OFFSET_LENGTH, b);
            return b;
        }
        void setXGyroOffset(byte offset) {
            _i2c.writeBits(devAddr, MPU6050_RA_XG_OFFS_TC, MPU6050_TC_OFFSET_BIT, MPU6050_TC_OFFSET_LENGTH, offset);
        }

        // YG_OFFS_TC register

        byte getYGyroOffset() {
            byte b = 0;
            _i2c.readBits(devAddr, MPU6050_RA_YG_OFFS_TC, MPU6050_TC_OFFSET_BIT, MPU6050_TC_OFFSET_LENGTH, b);
            return b;
        }
        void setYGyroOffset(byte offset) {
            _i2c.writeBits(devAddr, MPU6050_RA_YG_OFFS_TC, MPU6050_TC_OFFSET_BIT, MPU6050_TC_OFFSET_LENGTH, offset);
        }

        // ZG_OFFS_TC register

        byte getZGyroOffset() {
            byte b = 0;
            _i2c.readBits(devAddr, MPU6050_RA_ZG_OFFS_TC, MPU6050_TC_OFFSET_BIT, MPU6050_TC_OFFSET_LENGTH, b);
            return b;
        }
        void setZGyroOffset(byte offset) {
            _i2c.writeBits(devAddr, MPU6050_RA_ZG_OFFS_TC, MPU6050_TC_OFFSET_BIT, MPU6050_TC_OFFSET_LENGTH, offset);
        }

        // X_FINE_GAIN register

        byte getXFineGain() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_X_FINE_GAIN, b);
            return b;
        }
        void setXFineGain(byte gain) {
            _i2c.writeByte(devAddr, MPU6050_RA_X_FINE_GAIN, gain);
        }

        // Y_FINE_GAIN register

        byte getYFineGain() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_Y_FINE_GAIN, b);
            return b;
        }
        void setYFineGain(byte gain) {
            _i2c.writeByte(devAddr, MPU6050_RA_Y_FINE_GAIN, gain);
        }

        // Z_FINE_GAIN register

        byte getZFineGain() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_Z_FINE_GAIN, b);
            return b;
        }
        void setZFineGain(byte gain) {
            _i2c.writeByte(devAddr, MPU6050_RA_Z_FINE_GAIN, gain);
        }

        // XA_OFFS_* registers

        short getXAccelOffset() {
            _i2c.readBytes(devAddr, MPU6050_RA_XA_OFFS_H, 2, buffer, 1000);
            return (short)((buffer[0] << 8) | buffer[1]);
        }
        void setXAccelOffset(ushort offset) {
            _i2c.writeWord(devAddr, MPU6050_RA_XA_OFFS_H, offset);
        }

        // YA_OFFS_* register

        short getYAccelOffset() {
            _i2c.readBytes(devAddr, MPU6050_RA_YA_OFFS_H, 2, buffer, 1000);
            return (short)((buffer[0] << 8) | buffer[1]);
        }
        void setYAccelOffset(ushort offset) {
            _i2c.writeWord(devAddr, MPU6050_RA_YA_OFFS_H, offset);
        }

        // ZA_OFFS_* register

        short getZAccelOffset() {
            _i2c.readBytes(devAddr, MPU6050_RA_ZA_OFFS_H, 2, buffer, 1000);
            return (short)((buffer[0] << 8) | buffer[1]);
        }
        void setZAccelOffset(ushort offset) {
            _i2c.writeWord(devAddr, MPU6050_RA_ZA_OFFS_H, offset);
        }

        // XG_OFFS_USR* registers

        short getXGyroOffsetUser() {
            _i2c.readBytes(devAddr, MPU6050_RA_XG_OFFS_USRH, 2, buffer, 1000);
            return (short)((buffer[0] << 8) | buffer[1]);
        }
        void setXGyroOffsetUser(ushort offset) {
            _i2c.writeWord(devAddr, MPU6050_RA_XG_OFFS_USRH, offset);
        }

        // YG_OFFS_USR* register

        short getYGyroOffsetUser() {
            _i2c.readBytes(devAddr, MPU6050_RA_YG_OFFS_USRH, 2, buffer, 1000);
            return (short)((buffer[0] << 8) | buffer[1]);
        }
        void setYGyroOffsetUser(ushort offset) {
            _i2c.writeWord(devAddr, MPU6050_RA_YG_OFFS_USRH, offset);
        }

        // ZG_OFFS_USR* register

        short getZGyroOffsetUser() {
            _i2c.readBytes(devAddr, MPU6050_RA_ZG_OFFS_USRH, 2, buffer, 1000);
            return (short)((buffer[0] << 8) | buffer[1]);
        }
        void setZGyroOffsetUser(ushort offset) {
            _i2c.writeWord(devAddr, MPU6050_RA_ZG_OFFS_USRH, offset);
        }

        // INT_ENABLE register (DMP functions)

        bool getIntPLLReadyEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_ENABLE, MPU6050_INTERRUPT_PLL_RDY_INT_BIT, b);
            return b == 1;
        }
        void setIntPLLReadyEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_INT_ENABLE, MPU6050_INTERRUPT_PLL_RDY_INT_BIT, (byte)(enabled ? 1 : 0));
        }
        bool getIntDMPEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_ENABLE, MPU6050_INTERRUPT_DMP_INT_BIT, b);
            return b == 1;
        }
        void setIntDMPEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_INT_ENABLE, MPU6050_INTERRUPT_DMP_INT_BIT, (byte)(enabled ? 1 : 0));
        }

        // DMP_INT_STATUS

        bool getDMPInt5Status() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_DMP_INT_STATUS, MPU6050_DMPINT_5_BIT, b);
            return b == 1;
        }
        bool getDMPInt4Status() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_DMP_INT_STATUS, MPU6050_DMPINT_4_BIT, b);
            return b == 1;
        }
        bool getDMPInt3Status() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_DMP_INT_STATUS, MPU6050_DMPINT_3_BIT, b);
            return b == 1;
        }
        bool getDMPInt2Status() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_DMP_INT_STATUS, MPU6050_DMPINT_2_BIT, b);
            return b == 1;
        }
        bool getDMPInt1Status() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_DMP_INT_STATUS, MPU6050_DMPINT_1_BIT, b);
            return b == 1;
        }
        bool getDMPInt0Status() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_DMP_INT_STATUS, MPU6050_DMPINT_0_BIT, b);
            return b == 1;
        }

        // INT_STATUS register (DMP functions)

        bool getIntPLLReadyStatus() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_STATUS, MPU6050_INTERRUPT_PLL_RDY_INT_BIT, b);
            return b == 1;
        }
        bool getIntDMPStatus() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_INT_STATUS, MPU6050_INTERRUPT_DMP_INT_BIT, b);
            return b == 1;
        }

        // USER_CTRL register (DMP functions)

        bool getDMPEnabled() {
            byte b = 0;
            _i2c.readBit(devAddr, MPU6050_RA_USER_CTRL, MPU6050_USERCTRL_DMP_EN_BIT, b);
            return b == 1;
        }
        void setDMPEnabled(bool enabled) {
            _i2c.writeBit(devAddr, MPU6050_RA_USER_CTRL, MPU6050_USERCTRL_DMP_EN_BIT, (byte)(enabled ? 1 : 0));
        }
        void resetDMP() {
            byte b = 1;
            _i2c.writeBit(devAddr, MPU6050_RA_USER_CTRL, MPU6050_USERCTRL_DMP_RESET_BIT, b);
        }

        // BANK_SEL register

        void setMemoryBank(byte bank, bool prefetchEnabled, bool userBank) {
            bank &= 0x1F;
            if (userBank) bank |= 0x20;
            if (prefetchEnabled) bank |= 0x40;
            _i2c.writeByte(devAddr, MPU6050_RA_BANK_SEL, bank);
        }

        // MEM_START_ADDR register

        void setMemoryStartAddress(byte address) {
            _i2c.writeByte(devAddr, MPU6050_RA_MEM_START_ADDR, address);
        }

        // MEM_R_W register

        byte readMemoryByte() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_MEM_R_W, b);
            return b;
        }
        void writeMemoryByte(byte data) {
            _i2c.writeByte(devAddr, MPU6050_RA_MEM_R_W, data);
        }
        void readMemoryBlock(byte[] data, byte dataSize, byte bank, byte address)
        {
            setMemoryBank(bank, false, false);
            setMemoryStartAddress(address);

            byte chunkSize;
            for (byte i = 0; i < dataSize;) {
                // determine correct chunk size according to bank position and data size
                chunkSize = MPU6050_DMP_MEMORY_CHUNK_SIZE;

                // make sure we don't go past the data size
                if (i + chunkSize > dataSize) chunkSize = (byte)(dataSize - i);

                // make sure this chunk doesn't go past the bank boundary (256 bytes)
                if (chunkSize > 256 - address) chunkSize = (byte)(256 - address);

                // read the chunk of data as specified
        //        _i2c.readBytes(devAddr, MPU6050_RA_MEM_R_W, chunkSize, data + i, 1000);
        
                // increase byte index by [chunkSize]
                i += chunkSize;

                // uint automatically wraps to 0 at 256
                address += chunkSize;

                // if we aren't done, update bank (if necessary) and address
                if (i < dataSize) {
                    if (address == 0) bank++;
                    setMemoryBank(bank, false, false);
                    setMemoryStartAddress(address);
                }
            }
        }
        bool writeMemoryBlock(uint[] data, ushort dataSize, byte bank, byte address, bool verify, bool useProgMem)
        {
            setMemoryBank(bank, false, false);
            setMemoryStartAddress(address);
    
            byte chunkSize;
            byte[] verifyBuffer;
            byte[] progBuffer;
            ushort i;
            if (verify) verifyBuffer = new byte[MPU6050_DMP_MEMORY_CHUNK_SIZE];
            if (useProgMem) progBuffer = new byte[MPU6050_DMP_MEMORY_CHUNK_SIZE];

            for (i = 0; i < dataSize;) {
                // determine correct chunk size according to bank position and data size
                chunkSize = MPU6050_DMP_MEMORY_CHUNK_SIZE;

                // make sure we don't go past the data size
                if (i + chunkSize > dataSize) chunkSize = (byte)(dataSize - i);

                // make sure this chunk doesn't go past the bank boundary (256 bytes)
                if (chunkSize > 256 - address) chunkSize = (byte)(256 - address);
        
        //        if (useProgMem) {
        //            // write the chunk of data as specified
        //            for (j = 0; j < chunkSize; j++) progBuffer[j] = pgm_read_byte(data + i + j);
        //        } else {
        //            // write the chunk of data as specified
        //            progBuffer = (uint *)data + i;
        //        }

        //        _i2c.writeBytes(devAddr, MPU6050_RA_MEM_R_W, chunkSize, progBuffer);

                // verify data if needed
                if (verify) {
                    setMemoryBank(bank, false, false);
                    setMemoryStartAddress(address);

        //            _i2c.readBytes(devAddr, MPU6050_RA_MEM_R_W, chunkSize, verifyBuffer, 1000);
            
        /*            if (memcmp(progBuffer, verifyBuffer, chunkSize) != 0) {
                        Serial.print("Block write verification error, bank ");
                        Serial.print(bank, DEC);
                        Serial.print(", address ");
                        Serial.print(address, DEC);
                        Serial.print("!\nExpected:");
                        for (j = 0; j < chunkSize; j++) {
                            Serial.print(" 0x");
                            if (progBuffer[j] < 16) Serial.print("0");
                            Serial.print(progBuffer[j], HEX);
                        }
                        Serial.print("\nReceived:");
                        for (uint j = 0; j < chunkSize; j++) {
                            Serial.print(" 0x");
                            if (verifyBuffer[i + j] < 16) Serial.print("0");
                            Serial.print(verifyBuffer[i + j], HEX);
                        }
                        Serial.print("\n");
                        free(verifyBuffer);
                        if (useProgMem) free(progBuffer);
                        return false; // uh oh.
                    }
        */        }

                // increase byte index by [chunkSize]
                i += chunkSize;

                // uint automatically wraps to 0 at 256
                address += chunkSize;

                // if we aren't done, update bank (if necessary) and address
                if (i < dataSize) {
                    if (address == 0) bank++;
                    setMemoryBank(bank, false, false);
                    setMemoryStartAddress(address);
                }
            }
        //    if (verify) free(verifyBuffer);
        //    if (useProgMem) free(progBuffer);
            return true;
        }
        bool writeProgMemoryBlock(uint[] data, ushort dataSize, byte bank, byte address, bool verify)
        {
            return writeMemoryBlock(data, dataSize, bank, address, verify, true);
        }
        bool writeDMPConfigurationSet(byte[] data, ushort dataSize, bool useProgMem)
        {
        /*    uint *progBuffer, success, special;
            ushort i, j;
            if (useProgMem) {
                progBuffer = (uint *)malloc(8); // assume 8-byte blocks, realloc later if necessary
            }

            // config set data is a long string of blocks with the following structure:
            // [bank] [offset] [length] [byte[0], byte[1], ..., byte[length]]
            uint bank, offset, length;
            for (i = 0; i < dataSize;) {
                if (useProgMem) {
                    bank = pgm_read_byte(data + i++);
                    offset = pgm_read_byte(data + i++);
                    length = pgm_read_byte(data + i++);
                } else {
                    bank = data[i++];
                    offset = data[i++];
                    length = data[i++];
                }

                // write data or perform special action
                if (length > 0) {
                    // regular block of data to write
                    Serial.print("Writing config block to bank ");
                    Serial.print(bank);
                    Serial.print(", offset ");
                    Serial.print(offset);
                    Serial.print(", length=");
                    Serial.println(length);
                    if (useProgMem) {
                        if (sizeof(progBuffer) < length) progBuffer = (uint *)realloc(progBuffer, length);
                        for (j = 0; j < length; j++) progBuffer[j] = pgm_read_byte(data + i + j);
                    } else {
                        progBuffer = (uint *)data + i;
                    }
                    success = writeMemoryBlock(progBuffer, length, bank, offset, true);
                    i += length;
                } else {
                    // special instruction
                    // NOTE: this kind of behavior (what and when to do certain things)
                    // is totally undocumented. This code is in here based on observed
                    // behavior only, and exactly why (or even whether) it has to be here
                    // is anybody's guess for now.
                    if (useProgMem) {
                        special = pgm_read_byte(data + i++);
                    } else {
                        special = data[i++];
                    }
                    Serial.print("Special command code ");
                    Serial.print(special, HEX);
                    Serial.println(" found...");
                    if (special == 0x01) {
                        // enable DMP-related interrupts
                
                        //setIntZeroMotionEnabled(true);
                        //setIntFIFOBufferOverflowEnabled(true);
                        //setIntDMPEnabled(true);
                        _i2c.writeByte(devAddr, MPU6050_RA_INT_ENABLE, 0x32);  // single operation

                        success = true;
                    } else {
                        // unknown special command
                        success = false;
                    }
                }
        
                if (!success) {
                    if (useProgMem) free(progBuffer);
                    return false; // uh oh
                }
            }
            if (useProgMem) free(progBuffer);
        */    return true;
        }
            bool writeProgDMPConfigurationSet(byte[] data, ushort dataSize)
        {
            return writeDMPConfigurationSet(data, dataSize, true);
        }

        // DMP_CFG_1 register

        byte getDMPConfig1() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_DMP_CFG_1, b);
            return b;
        }
        void setDMPConfig1(byte config) {
            _i2c.writeByte(devAddr, MPU6050_RA_DMP_CFG_1, config);
        }

        // DMP_CFG_2 register

        byte getDMPConfig2() {
            byte b = 0;
            _i2c.readByte(devAddr, MPU6050_RA_DMP_CFG_2, b);
            return b;
        }
        void setDMPConfig2(byte config) {
            _i2c.writeByte(devAddr, MPU6050_RA_DMP_CFG_2, config);
        }
#endif
    }
}
