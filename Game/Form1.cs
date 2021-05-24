using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace Game
{
    public partial class Form1 : Form
    {
        bool Connection;
        public void Method()
        {
            Show();
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        public void Check_connect()
        {
            if (true)
            {
                Connection = false;
            }
        }


        private void exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chat_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(Method);
            form3.Show();
            Hide();
        }

        private void start_Click(object sender, EventArgs e)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect("DESKTOP-0CCBF4B", 21000);
            Form4 form4 = new Form4(Method, socket);
            form4.Show();
            Hide();
        }
    }
}
