using System;
using System.Threading;
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
    public partial class Form2 : Form
    {
        ball Ball;
        player Player1;
        player Player2;
        Socket socket;
        Client client;
        public static int left = 0;
        public static int right = 0;
        bool Connection;
        public delegate void deleg();
        static deleg objdel;
        string message;
        public class player
        {
            int Width;
            int Height;
            public Rectangle rec;
            public player(int w, int h, int type) //конструктор класса игрок
            {
                Width = w;
                Height = h;
                if (type == 1)
                    rec = new Rectangle(100, h / 2 - 40, 10, 160);
                else
                    rec = new Rectangle(w - 100, h / 2 - 40, 10, 160);
            }
            public void Drawplayer(object sender, PaintEventArgs e) //событие перерисовки игрока
            {
                e.Graphics.FillRectangle(Brushes.Black, rec);
            }

        }
        public class ball
        {
            StringFormat drawFormat = new StringFormat();
            Font drawFont = new Font("Century Gothic", 25);
            public int dx;
            public int dy;
            bool flag = true;
            int Width;
            int Height;
            string str;
            public string msg;
            public Rectangle rec;
            public delegate void endgame(); //делегат события
            public event endgame stopgame; //событие конца игры
            Socket socket;
            public ball(int w, int h, Socket soc) //конструктор класса мяч
            {
                socket = soc;
                rec = new Rectangle(w / 2 - 10, h / 2 - 10, 20, 20);
                Random random = new Random();
                Width = w;
                Height = h;
                if (random.Next(2) % 2 == 0)
                    dx = 15;
                else
                    dx = -15;
                if (random.Next(2) % 2 == 0)
                    dy = 15;
                else
                    dy = -15;
            }
            public void Offset(Rectangle Player1, Rectangle Player2)
            {
                Random random = new Random();
                bool flag = true;
                bool check = true;
                Rectangle rect1 = Rectangle.Intersect(rec, Player1);
                Rectangle rect2 = Rectangle.Intersect(rec, Player2);
                if ((rec.X + dx > Width - 40 || rec.X + dx < 0)
                    || (flag && ((rect1.Width != 0 && rect1.Height != 0) || (rect2.Width != 0 && rect2.Height != 0))))
                {
                    if (rec.X + dx > Width - 40)
                    {
                        right += 1;
                        check = false;
                        msg = "G";
                    }
                    else if (rec.X + dx < 0)
                    {
                        left += 1;
                        check = false;
                        msg = "G";
                    }
                    else
                        msg = "OK";
                    dx *= -1;
                    flag = false;
                    if (check == false)
                    {

                        rec.X = Width / 2 - 10;
                        rec.Y = Height / 2 - 10;
                        if (random.Next(2) % 2 == 0)
                            dx = 15;
                        else
                            dx = -15;
                        if (random.Next(2) % 2 == 0)
                            dy = 15;
                        else
                            dy = -15;
                        Thread.Sleep(1500);
                    }
                }
                //rec.X += dx;
                if ((rec.Y + dy > Height - 40 || rec.Y + dy < 0)
                    || (flag && ((rect1.Width != 0 && rect1.Height != 0) || (rect2.Width != 0 && rect2.Height != 0))))
                {
                    dy *= -1;
                    msg = "Y";
                }
                //rec.Y += dy;
            }
            public void Drawball(object sender, PaintEventArgs e) //событие перерисовки игрока
            {
                e.Graphics.FillEllipse(Brushes.DarkBlue, rec);
                e.Graphics.DrawString(str, drawFont, Brushes.Black, Width / 2 - 150, 20, drawFormat);
                if (flag)
                    str = right.ToString() + "               :               " + left.ToString();
                if (left >= 2)
                {
                    str = "YOU  WIN";
                    flag = false;
                    msg = "END";
                    stopgame();
                }
                if (right >= 2)
                {
                    str = "YOU  LOSE";
                    flag = false;
                    msg = "END";
                    stopgame();
                }
            }

        }
        void rrrr() //обработик события
        {
            timer2.Enabled = false;
            Invalidate();
            timer1.Enabled = false;
        }
        public Form2(deleg del, Socket socket1)
        {

            socket = socket1;
            socket.Send(Encoding.ASCII.GetBytes("I am started, bitch"));
            objdel = del;
            InitializeComponent();
            Ball = new ball(Width, Height, socket);
            Ball.stopgame += rrrr;
            Player1 = new player(Width, Height, 1);
            Player2 = new player(Width, Height, 2);
            Paint += Ball.Drawball;
            Paint += Player1.Drawplayer;
            Paint += Player2.Drawplayer;
            left = 0;
            right = 0;
            client = new Client(socket);
            timer2.Enabled = true;
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
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //int p1y = Player1.rec.Y;
            //int p1x = Player1.rec.X;
            //socket.Send(Encoding.ASCII.GetBytes(Player1.rec.X.ToString()));
            //socket.Send(Encoding.ASCII.GetBytes(Player1.rec.Y.ToString()));
            //player2_move();
            Invalidate();
        }

        public void Check_connect()
        {
            if (true)
            {
                Connection = false;
            }
        }

        public void player2_move()
        {
            //socket.Send(Encoding.ASCII.GetBytes(Player1.rec.X.ToString()));
            //socket.Send(Encoding.ASCII.GetBytes(Player1.rec.Y.ToString()));
            //byte[] m = new byte[1024];
            //Receive();
            //Ball.rec.X = int.Parse(message);
            //Receive();
            //Ball.rec.X = int.Parse(message);
            //Receive();
            //Player2.rec.X = int.Parse(message);
            //Receive();
            //Player2.rec.Y = int.Parse(message);
            //if (Player2.rec.Y < Ball.rec.Y)
            //    Player2.rec.Y += 20;
            //else
            //    Player2.rec.Y -= 20;
        }

        private void Form2_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {

                case (char)Keys.Escape:
                    objdel(); //делегат открытия первой формы 
                    Close();
                    break;
            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                Thread.Sleep(50);
                string str = Player1.rec.X.ToString();
                socket.Send(Encoding.ASCII.GetBytes(str));
                Thread.Sleep(50);
                str = Player1.rec.Y.ToString();
                socket.Send(Encoding.ASCII.GetBytes(str));
                Thread.Sleep(50);
                Receive();
                Ball.rec.X = int.Parse(message);
                Receive();
                Ball.rec.Y = int.Parse(message);
                Receive();
                Player2.rec.X = int.Parse(message);
                Receive();
                Player2.rec.Y = int.Parse(message);
                Ball.Offset(Player1.rec, Player2.rec);
                socket.Send(Encoding.ASCII.GetBytes(Ball.msg));
            }
            catch (Exception ex) { Close(); }
        }

        private void Form2_Close(object sender, FormClosingEventArgs e)
        {
            objdel(); //делегат открытия первой формы 
        }

        private void Form2_Mousemove(object sender, MouseEventArgs e)
        {
            if (Player1.rec.Y < Height - 120 && Player1.rec.X < Width / 2 - 30)
            {
                Player1.rec.X = MousePosition.X - 200;
                Player1.rec.Y = MousePosition.Y - 200;
            }
            else if (MousePosition.X > Width / 2 - 30)
                Player1.rec.X = Width / 2 - 35;
            else if (MousePosition.Y > Height - 120)
                Player1.rec.Y = Height - 125;
        }

    }
}

