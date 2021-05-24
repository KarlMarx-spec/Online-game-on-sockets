using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;

namespace Game
{
    public partial class Form4 : Form
    {
        string message;
        private Client client;
        Socket socket;
        public delegate void deleg();
        static deleg bigdel;
        int[] l0 = new int[3];
        int[] l1 = new int[3];
        int[] l2 = new int[3];
        int over = 0;
        public Form4(deleg del,Socket sock)
        {
            bigdel = del;
            InitializeComponent();
            socket = sock;
            client = new Client(socket);
            
        }
        void Winner(bool fl, int type)
        {
            if (fl)
            {
                if (type == 1)
                {
                    b00.Image = Properties.Resources.MY;
                    b01.Image = Properties.Resources.MY;
                    b02.Image = Properties.Resources.MY;
                    b11.Image = Properties.Resources.MY;
                    b10.Image = Properties.Resources.MY;
                    b12.Image = Properties.Resources.MY;
                    b20.Image = Properties.Resources.MY;
                    b22.Image = Properties.Resources.MY;
                    b21.Image = Properties.Resources.MY;
                    
                }
                else
                {
                    b00.Image = Properties.Resources.HIM;
                    b01.Image = Properties.Resources.HIM;
                    b02.Image = Properties.Resources.HIM;
                    b11.Image = Properties.Resources.HIM;
                    b10.Image = Properties.Resources.HIM;
                    b12.Image = Properties.Resources.HIM;
                    b20.Image = Properties.Resources.HIM;
                    b22.Image = Properties.Resources.HIM;
                    b21.Image = Properties.Resources.HIM;
                }
            }
            else
            {
                b00.Image = Properties.Resources.NN;
                b01.Image = Properties.Resources.NN;
                b02.Image = Properties.Resources.NN;
                b11.Image = Properties.Resources.NN;
                b10.Image = Properties.Resources.NN;
                b12.Image = Properties.Resources.NN;
                b20.Image = Properties.Resources.NN;
                b22.Image = Properties.Resources.NN;
                b21.Image = Properties.Resources.NN;
            }
            Thread.Sleep(1500);
            Close();
        }
        void AreYouWinner(int type)
        {
            if (l0[0] == type && l0[1] == type && l0[2] == type)
            {
                Winner(true, type);
                //Close();//zaglushka
            }
            if (l1[0] == type && l1[1] == type && l1[2] == type)
            {
                Winner(true, type);
                //Close();//zaglushka
            }
            if (l2[0] == type && l2[1] == type && l2[2] == type)
            {
                Winner(true, type);
                //Close();//zaglushka
            }
            if (l0[0] == type && l1[0] == type && l2[0] == type)
            {
                Winner(true, type);
                //Close();//zaglushka
            }
            if (l0[1] == type && l1[1] == type && l2[1] == type)
            {
                Winner(true, type);
                //Close();//zaglushka
            }
            if (l0[2] == type && l1[2] == type && l2[2] == type)
            {
                Winner(true, type);
                //Close();//zaglushka
            }
            if (l0[0] == type && l1[1] == type && l2[2] == type)
            {
                Winner(true, type);
                //Close();//zaglushka
            }
            if (l0[2] == type && l1[1] == type && l2[0] == type)
            {
                Winner(true, type);
                //Close();//zaglushka
            }
            if (over >= 9)
            {
                Winner(false, type);
                //Close();//zaglushka
            }
        }
        public async void Receive()
        {
            message = string.Empty;
            message = await client.Receive();
            if (message == "Connection lost")
            {
                //выход из игры
                Close();
            }
            Priem();
            Receive();
        }
        void Priem()
        {
            over++;
            switch (message)
            {
                case "00":
                    b00.Enabled = false;
                    b00.Image = Properties.Resources.O;
                    l0[0] = 2;
                    
                    AreYouWinner(2);
                    break;
                case "01":
                    b01.Enabled = false;
                    b01.Image = Properties.Resources.O;
                    l0[1] = 2;
                    AreYouWinner(2);
                    break;
                case "02":
                    b02.Enabled = false;
                    b02.Image = Properties.Resources.O;
                    l0[2] = 2;
                    AreYouWinner(2);
                    break;
                case "10":
                    b10.Enabled = false;
                    b10.Image = Properties.Resources.O;
                    l1[0] = 2;
                    AreYouWinner(2);
                    break;
                case "11":
                    b11.Enabled = false;
                    b11.Image = Properties.Resources.O;
                    l1[1] = 2;
                    AreYouWinner(2);
                    break;
                case "12":
                    b12.Enabled = false;
                    b12.Image = Properties.Resources.O;
                    l1[2] = 2;
                    AreYouWinner(2);
                    break;
                case "20":
                    b20.Enabled = false;
                    b20.Image = Properties.Resources.O;
                    l2[0] = 2;
                    AreYouWinner(2);
                    break;
                case "21":
                    b21.Enabled = false;
                    b21.Image = Properties.Resources.O;
                    l2[1] = 2;
                    AreYouWinner(2);
                    break;
                case "22":
                    b22.Enabled = false;
                    b22.Image = Properties.Resources.O;
                    l2[2] = 2;
                    AreYouWinner(2);
                    break;
            }
        }
        private void b00_Click(object sender, EventArgs e)
        {
            over++;
            byte[] buffer = new byte[128];
            l0[0] = 1;
            b00.Image = Properties.Resources.X;
            b00.Enabled = false;
            //client.Send("00");
            socket.Send(Encoding.ASCII.GetBytes("00"));
            AreYouWinner(1);
            socket.Receive(buffer);
            message = Encoding.ASCII.GetString(buffer);
            string res = string.Empty;
            for (int i = 0; i < message.Length && message[i] != '\0'; i++)
                res += message[i];
            message = res;
            //Receive();
            Priem();

        }

        private void b01_Click(object sender, EventArgs e)
        {
            over++;
            byte[] buffer = new byte[128];
            l0[1] = 1;
            b01.Image = Properties.Resources.X;
            b01.Enabled = false;
            //client.Send("01");
            socket.Send(Encoding.ASCII.GetBytes("01"));
            AreYouWinner(1);
            socket.Receive(buffer);
            message = Encoding.ASCII.GetString(buffer);
            string res = string.Empty;
            for (int i = 0; i < message.Length && message[i] != '\0'; i++)
                res += message[i];
            message = res;
            //Receive();
            Priem();
        }

        private void b02_Click(object sender, EventArgs e)
        {
            over++;
            byte[] buffer = new byte[128];
            l0[2] = 1;
            b02.Image = Properties.Resources.X;
            b02.Enabled = false;
            //client.Send("02");
            socket.Send(Encoding.ASCII.GetBytes("02"));
            AreYouWinner(1);
            socket.Receive(buffer);
            message = Encoding.ASCII.GetString(buffer);
            string res = string.Empty;
            for (int i = 0; i < message.Length && message[i] != '\0'; i++)
                res += message[i];
            message = res;
            //Receive();
            Priem();
        }

        private void b10_Click(object sender, EventArgs e)
        {
            over++;
            byte[] buffer = new byte[128];
            l1[0] = 1;
            b10.Image = Properties.Resources.X;
            b10.Enabled = false;
            //client.Send("10");
            socket.Send(Encoding.ASCII.GetBytes("10"));
            AreYouWinner(1);
            socket.Receive(buffer);
            message = Encoding.ASCII.GetString(buffer);
            string res = string.Empty;
            for (int i = 0; i < message.Length && message[i] != '\0'; i++)
                res += message[i];
            message = res;
            //Receive();
            Priem();
        }

        private void b11_Click(object sender, EventArgs e)
        {
            over++;
            l1[1] = 1;
            byte[] buffer = new byte[128];
            b11.Image = Properties.Resources.X;
            b11.Enabled = false;
            //client.Send("11");
            socket.Send(Encoding.ASCII.GetBytes("11"));
            AreYouWinner(1);
            socket.Receive(buffer);
            message = Encoding.ASCII.GetString(buffer);
            string res = string.Empty;
            for (int i = 0; i < message.Length && message[i] != '\0'; i++)
                res += message[i];
            message = res;
            //Receive();
            Priem();
        }

        private void b12_Click(object sender, EventArgs e)
        {
            over++;
            l1[2] = 1;
            byte[] buffer = new byte[128];
            b12.Image = Properties.Resources.X;
            b12.Enabled = false;
            //client.Send("12");
            socket.Send(Encoding.ASCII.GetBytes("12"));
            AreYouWinner(1);
            socket.Receive(buffer);
            message = Encoding.ASCII.GetString(buffer);
            string res = string.Empty;
            for (int i = 0; i < message.Length && message[i] != '\0'; i++)
                res += message[i];
            message = res;
            //Receive();
            Priem();
        }

        private void b20_Click(object sender, EventArgs e)
        {
            over++;
            l2[0] = 1;
            byte[] buffer = new byte[128];
            b20.Image = Properties.Resources.X;
            b20.Enabled = false;
            //client.Send("20");
            socket.Send(Encoding.ASCII.GetBytes("20"));
            AreYouWinner(1);
            socket.Receive(buffer);
            message = Encoding.ASCII.GetString(buffer);
            string res = string.Empty;
            for (int i = 0; i < message.Length && message[i] != '\0'; i++)
                res += message[i];
            message = res;
            //Receive();
            Priem();
        }

        private void b21_Click(object sender, EventArgs e)
        {
            over++;
            l2[1] = 1;
            byte[] buffer = new byte[128];
            b21.Image = Properties.Resources.X;
            b21.Enabled = false;
            //client.Send("21");
            socket.Send(Encoding.ASCII.GetBytes("21"));
            AreYouWinner(1);
            socket.Receive(buffer);
            message = Encoding.ASCII.GetString(buffer);
            string res = string.Empty;
            for (int i = 0; i < message.Length && message[i] != '\0'; i++)
                res += message[i];
            message = res;
            //Receive();
            Priem();
        }

        private void b22_Click(object sender, EventArgs e)
        {
            over++;
            l2[2] = 1;
            byte[] buffer = new byte[128];
            b22.Image = Properties.Resources.X;
            b22.Enabled = false;
            //client.Send("22");
            socket.Send(Encoding.ASCII.GetBytes("22"));
            AreYouWinner(1);
            socket.Receive(buffer);
            message = Encoding.ASCII.GetString(buffer);
            string res = string.Empty;
            for (int i = 0; i < message.Length && message[i] != '\0'; i++)
                res += message[i];
            message = res;
            //Receive();
            Priem();
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            bigdel();
        }

        private void Form4_Shown(object sender, EventArgs e)
        {
            byte[] buffer = new byte[128];
            socket.Receive(buffer);
            message = Encoding.ASCII.GetString(buffer);
            string res = string.Empty;
            for (int i = 0; i < message.Length && message[i] != '\0'; i++)
                res += message[i];
            message = res;
            Priem();
        }
    }
}
