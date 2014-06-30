using System;
using System.Threading;
//using Microsoft.SPOT;
//using Microsoft.SPOT.Hardware;
//using SecretLabs.NETMF.Hardware;
//using SecretLabs.NETMF.Hardware.NetduinoMini;

namespace Audio_Analysis_and_Head_Tracking
{
    public class Program
    {
        public static void Main()
        {
            MPU6050 ht = new MPU6050();

            ht.Initialize();
            ht.testConnection();
        }

    }
}
