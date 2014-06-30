using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Burt_ControlApp
{
    public partial class Burt : Form
    {
        static System.Net.NetworkInformation.PingReply PR;

        static IPAddress localhost = IPAddress.Parse("127.0.0.1");

        static IPAddress localIPaddr = null;
        static IPAddress burtsIPaddr = null;
        static ushort listeningPort = 80;
        static int burtsAddrValid = 0;

        static Timer heartbeatTimer = new Timer();
        static ulong heartbeatCount = 0;

        static bool weAreInitializing = true;

        public Burt()
        {
            InitializeComponent();

            if (localIPaddr != null || burtsIPaddr != null || weAreInitializing != true)
            {
                // Are we restarting from a soft reboot?
            }
        }

        private IPAddress LocalIP()
        {
            IPAddress localIP = localhost;

            string hostname = Dns.GetHostName();
            
            IPHostEntry host = Dns.GetHostEntry(hostname);
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    // Already found an address in the AddressList?
                    if (localIP != localhost)
                        Console.WriteLine("My IP ? " + localIP.ToString());

                    localIP = ip;
                }
            }
            return localIP;
        }

        private static IPAddress FindBurtsIPAddress(string localIPaddr)
        {
            IPAddress burtsIP = localhost;

            try
            {
                System.Net.NetworkInformation.Ping p1 = new System.Net.NetworkInformation.Ping();

                try
                {
                    PR = p1.Send("burt", 250);
                    if (PR.Status.ToString().Equals("Success"))
                        return PR.Address;
                }
                catch (Exception e)
                {
                };

                IPAddress localIP = IPAddress.Parse(localIPaddr);
                byte[] addrBytes = localIP.GetAddressBytes();

                for (byte n = 0; n < 254; n++)
                {
                    int numBytes = addrBytes.GetLength(0);
                    addrBytes.SetValue(n, numBytes - 1);

                    IPAddress lookupIPAddr = new IPAddress(addrBytes);

                    try
                    {
                        PR = p1.Send(lookupIPAddr, 1000 / 254);
                        if (PR.Status.ToString().Equals("Success"))
                            return PR.Address;
                    }
                    catch (Exception e)
                    {
                    };
                    if (PR.Status.ToString().Equals("Success"))
                    {
                        Console.WriteLine(PR.Status.ToString());
                    }
                }
            }
            catch(System.Net.Sockets.SocketException e) {
                Console.WriteLine("SocketException caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }

            return burtsIP;
        }

        private static void HeartbeatTimerEvent(Object myObject, EventArgs myEventArgs)
        {
            heartbeatTimer.Stop();

            heartbeatCount++;
            Console.WriteLine("<3 BS (" + heartbeatCount.ToString("G") + ")");

            heartbeatTimer.Enabled = true;
        }

        private void Burt_Load(object sender, EventArgs e)
        {
            CenterToParent();

            localIPaddr = LocalIP();
            Console.WriteLine("My IP @ " + localIPaddr.ToString());

            // This will assume that Burt's Wifi module is 
            // on the same router subnet as this PC/Laptop/etc.
            burtsIPaddr = localhost;//FindBurtsIPAddress(localIPaddr);
            burtsAddrValid = 0x0F;
            Console.WriteLine("Found Burt @" + burtsIPaddr.ToString());

            // Assume IPV4, e.g. 192.168.1.26
            IPV4_0_1.Text = burtsIPaddr.GetAddressBytes()[0].ToString() + "." +
                            burtsIPaddr.GetAddressBytes()[1].ToString();
            IPV4_2.Text = burtsIPaddr.GetAddressBytes()[2].ToString();
            IPV4_3.Text = burtsIPaddr.GetAddressBytes()[3].ToString();

            listeningPort = 80;
            IPV4_ListeningPort.Text = "80";

            /* Adds the event and the event handler for the method that will 
            process the timer event to the timer. */
            heartbeatTimer.Tick += new EventHandler(HeartbeatTimerEvent);

            // Sets the timer interval to 1 second(s).
            heartbeatTimer.Interval = 1000;
        }

        private void connectButton_MouseUp(object sender, MouseEventArgs e)
        {
            if ((burtsAddrValid & 0x0f) != 0x0f)
            {
                connectButton.Text = "Invalid IPV4 Address :(";
                return;
            }
            connectButton.BackColor = connectButton.FlatAppearance.MouseDownBackColor;
            connectButton.FlatAppearance.MouseOverBackColor = connectButton.BackColor;

            connectButton.Text = "Connecting to Burt...";

            IPEndPoint localEndPoint = new IPEndPoint(burtsIPaddr, (int)listeningPort);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Listen for Burt
            server.Bind(localEndPoint);
            server.Listen(10);
            Socket client = server.Accept();

            client.Close();

            // Or direct connect
            server.Connect(localEndPoint);

            server.Close();
            
            heartbeatTimer.Start();
        }

        #region LinkClicked handlers
        private void OpenLinkInDefaultBrowser(string url)
        {
            // E.g. 'http://www.microsoft.com', 'ftp://ftp.microsoft.com'
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch(System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void IPV4_0_1_link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLinkInDefaultBrowser("https://www.dropbox.com/sh/wbioryidwgw2zj2/AABv94p-pybCfvbIMjIpScGWa/burt");
        }
        private void IPV4_ListeningPort_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLinkInDefaultBrowser("https://www.dropbox.com/sh/wbioryidwgw2zj2/AABv94p-pybCfvbIMjIpScGWa/burt");
        }
        #endregion

        #region TextChanged handlers
        private void IPV4_0_1_TextChanged(object sender, EventArgs e)
        {
            bool showError = true;

            char[] seperators = new char[] { '.' };
            string[] val = IPV4_0_1.Text.Split(seperators);

            int num1,num2;
            if (val.Length == 2 && 
                int.TryParse(val[0], out num1) && num1 >= 0 && num1 <= 255 &&
                int.TryParse(val[1], out num2) && num2 >= 0 && num2 <= 255)
            {
                if (IPV4_2.Text != "" && IPV4_3.Text != "")
                {
                    showError = false;
                    IPV4_0_1.Text = val[0] + "." + val[1];
                    burtsIPaddr = IPAddress.Parse(IPV4_0_1.Text + "." + IPV4_2.Text + "." + IPV4_3.Text);
                }
            }

            if (showError)
            {
                IPV4_0_1.BackColor = Color.Red;
                burtsAddrValid &= ~0x03;
            }
            else
            {
                IPV4_0_1.BackColor = Color.White;
                burtsAddrValid |= 0x03;
            }
        }
        private void IPV4_2_TextChanged(object sender, EventArgs e)
        {
            bool showError = true;

            int num;
            if (int.TryParse(IPV4_2.Text, out num) && num >= 0 && num <= 255)
            {
                if (IPV4_0_1.Text != "" && IPV4_3.Text != "")
                {
                    showError = false;
                    burtsIPaddr = IPAddress.Parse(IPV4_0_1.Text + "." + IPV4_2.Text + "." + IPV4_3.Text);
                }
            }

            if (showError)
            {
                IPV4_2.BackColor = Color.Red;
                burtsAddrValid &= ~0x04;
            }
            else
            {
                IPV4_2.BackColor = Color.White;
                burtsAddrValid |= 0x04;
            }
        }
        private void IPV4_3_TextChanged(object sender, EventArgs e)
        {
            bool showError = true;

            int num;
            if (int.TryParse(IPV4_3.Text, out num) && num >= 0 && num < 254)
            {
                if (IPV4_0_1.Text != "" && IPV4_2.Text != "")
                {
                    showError = false;
                    burtsIPaddr = IPAddress.Parse(IPV4_0_1.Text + "." + IPV4_2.Text + "." + IPV4_3.Text);
                }
            }

            if (showError)
            {
                IPV4_3.BackColor = Color.Red;
                burtsAddrValid &= ~0x04;
            }
            else
            {
                IPV4_3.BackColor = Color.White;
                burtsAddrValid |= 0x04;
            }
        }
        private void IPV4_ListeningPort_TextChanged(object sender, EventArgs e)
        {
            bool showError = true;

            ushort num;
            if (ushort.TryParse(IPV4_ListeningPort.Text, out num) && num >= 0 && num <= 65535)
            {
                showError = false;
                listeningPort = num;
            }

            if (showError)
            {
                IPV4_3.BackColor = Color.Red;
            }
            else
            {
                IPV4_3.BackColor = Color.White;
            }
        }
        #endregion
    }
}
