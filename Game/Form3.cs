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
using System.Net;

namespace Game
{
    public partial class Form3 : Form
    {
        public delegate void deleg();
        static deleg anodel;

        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Client client;
        private string message;
        private string text;
        bool flag = false;
        bool game = false;
        string players_name = "";
        public Form3(deleg del)
        {
            anodel = del;
            InitializeComponent();
        }
        private void Form3_Load(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (socket.Connected)
            {
                text = textBox1.Text;
                listBox1.Items.Add(Environment.NewLine + "You: " + text);
                client.Send(text);
                textBox1.Clear();
            }
            else
            {
                listBox1.Items.Add(Environment.NewLine + "No connection");
            }
        }
        void StartGame()
        {

            Form4 form4 = new Form4(Method, socket);
            form4.Show();
            Hide();
        }
        public async void Receive()
        {
            try
            {
                message = await client.Receive();
                if (message != "Connection lost")
                {
                    if (message == "START\0")
                    {
                        StartGame();
                        message = string.Empty;
                        game = true;
                    }
                    else if (!game && message[0] != '\0')
                        listBox1.Items.Add(Environment.NewLine + message);
                    Receive();
                }
                else
                    listBox1.Items.Add(Environment.NewLine + "Server offline now");
                message = string.Empty;
            }
            catch { game = false; }
        }
        string ft_generator()
        {
            int n;
            string str = "Guest";
            Random random = new Random();
            n = random.Next();
            str += n.ToString();
            return (str);
        }
        private int Ft_atoi(string str)
        {
            int i;
            int is_neg;
            int res;

            if (str.Length <= 0)
                return (0);
            i = 0;
            while (str[i] == '\t' || str[i] == '\n' || str[i] == '\v' ||
                    str[i] == '\f' || str[i] == '\r' || str[i] == ' ')
                i++;
            is_neg = (str[i] == '-') ? -1 : 1;
            if (is_neg == -1 || str[i] == '+')
                i++;
            res = 0;
            while (str[i] >= '0' && str[i] <= '9')
                res = (res * 10) + (str[i++] - '0');
            return (res * is_neg);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add("Connection...");
            if (!socket.Connected)
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect("DESKTOP-BFSM69E", 904);
                flag = true;
                listBox1.Items.Add("Connection established");
                if (players_name == "")
                    players_name = ft_generator();
                    //players_name = "IM_FIRST";
                listBox1.Items.Add("Welcome " + players_name);
                socket.Send(Encoding.ASCII.GetBytes(players_name));
                byte[] bytes = new byte[1024];
                socket.Receive(bytes);
                string str = Encoding.ASCII.GetString(bytes);
                int new_port = int.Parse(str);
                socket.Shutdown(SocketShutdown.Both);
                Socket sc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sc.Connect("DESKTOP-BFSM69E", new_port);
                client = new Client(sc);
                socket = sc;
                Receive();
            }
            catch
            {
                listBox1.Items.Add("Server does not respond");
            }
        }
        public void Method()
        {
            Show();
            flag = false;
        }
        private void start_Click(object sender, EventArgs e)
        {
            if (socket.Connected)
                socket.Send(Encoding.ASCII.GetBytes("GAME"));
            //Form2 form2 = new Form2(Method, socket);
            //form2.Show();
            //Hide();
        }
        private void Form3_Keypress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Escape:
                    anodel(); //делегат открытия первой формы 
                    Close();
                    break;
            }
        }
        private void Form3_Close(object sender, FormClosingEventArgs e)
        {
            if (socket.Connected)
            {
                socket.Send(Encoding.ASCII.GetBytes(players_name + " logout"));
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            anodel(); //делегат открытия первой формы 
        }

    }
    public class Client
    {
        private Socket _socket;
        private string message;
        public Client(Socket socket)
        {
            _socket = socket;
        }
        public Task<string> Receive()
        {
            return Task.Run(() =>
            {
                try
                {
                    byte[] buffer1 = new byte[128];
                    int i;
                    _socket.Receive(buffer1);
                    message = Encoding.ASCII.GetString(buffer1);
                    string res = string.Empty;
                    for (i = 0; i < message.Length && message[i] != '\0'; i++)
                        res += message[i];
                    res += message[i];
                    return res;
                }
                catch (Exception e)
                {
                    return "Connection lost";
                }
            });
        }
        public void Send(string text)
        {
            byte[] buffer2 = new byte[128];
            buffer2 = Encoding.ASCII.GetBytes(text);
            _socket.Send(buffer2);
        }
    }
}

