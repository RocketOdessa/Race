using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace PLIIIIIIIIIIZ
{

    public partial class StreetRacing : Form
    {
        // Основные переменные для работы 
        int carSpeed =5;
        int roadSpeed = 8;
        bool carLeft;
        bool carRight;
        int trafficSpeed = 5;
        int Score = 0;
        Random rnd = new Random();

        public StreetRacing()
        {
            InitializeComponent();
            Start.Enabled = true;
            backgroundWorker1.RunWorkerAsync();
            Reset();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        private void Reset() //1
        {
            trophy.Visible = false; // скрываем картинку трофея 

            Start.Enabled = false; // выключаем кнопку когда игра идет 

            explosion.Visible = false; // скрываем GIF-Image взрыва , пока идет игра

            trafficSpeed =10 ; // скорость движения 

            roadSpeed = 10; // скорость дороги

            // Score = 0; // счет
            //сброс координат расположения игрока
            player.Left = 161;
            player.Top = 286;
            // сброс движения игрока
            carLeft = false;
            carRight = false;

            // переместитесь в положение по умолчанию, это будет отключено от экрана
            AI1.Left = 66;
            AI1.Top = -120;

            AI2.Left = 294;
            AI2.Top = -185;
            // сброс дороги к ее начальной позиции
            RoadTrack2.Left = -3;
            RoadTrack2.Top = -222;
            RoadTrack2.Left = -2;
            RoadTrack1.Top = -638;
            // включаем таймер
            Timer1.Start();

        }

        // устанавливем кнопки управления 
        private void MoveCar(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && player.Left > 0)
            {
                carLeft = true;
            }

            if (e.KeyCode == Keys.Right && player.Left + player.Width < GameField.Width)
            {
                carRight = true;
            }
        }

        private void StopCar(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                carLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                carRight = false;
            }
        }
        // изменения картинок AI
        private void ChangeAI1()//3
        {
            int num = rnd.Next(1, 8);

            switch (num)
            {
                case 1:
                    AI1.Image = Properties.Resources.carGreen;
                    break;
                case 2:
                    AI1.Image = Properties.Resources.carYellow;
                    break;
                case 3:
                    AI1.Image = Properties.Resources.carGrey;
                    break;
                case 4:
                    AI1.Image = Properties.Resources.carOrange;
                    break;
                case 5:
                    AI1.Image = Properties.Resources.CarRed;
                    break;
                case 6:
                    AI1.Image = Properties.Resources.carYellow;
                    break;
                case 7:
                    AI1.Image = Properties.Resources.TruckWhite;
                    break;
                case 8:
                    AI1.Image = Properties.Resources.ambulance;
                    break;
                default:
                    break;
            }
        }
        private void ChangeAI2()//3
        {
            int num = rnd.Next(1, 8);

            switch (num)
            {
                case 1:
                    AI2.Image = Properties.Resources.carGreen;
                    break;
                case 2:
                    AI2.Image = Properties.Resources.carYellow;
                    break;
                case 3:
                    AI2.Image = Properties.Resources.carGrey;
                    break;
                case 4:
                    AI2.Image = Properties.Resources.carOrange;
                    break;
                case 5:
                    AI2.Image = Properties.Resources.CarRed;
                    break;
                case 6:
                    AI2.Image = Properties.Resources.TruckBlue;
                    break;
                case 7:
                    AI2.Image = Properties.Resources.TruckWhite;
                    break;
                case 8:
                    AI2.Image = Properties.Resources.ambulance;
                    break;
                default:
                    break;
            }
        }
        private void GameOver()
        {
            trophy.Visible = true;

            Timer1.Stop();

            Start.Enabled = true;

            explosion.Visible = true;
            player.Controls.Add(explosion);
            explosion.Location = new Point(-8, 5);
            explosion.BackColor = Color.Transparent;
            explosion.BringToFront();

            if (Score < 1000)
            {
                trophy.Image = Properties.Resources.bronze;
            }
            if (Score > 2500)
            {
                trophy.Image = Properties.Resources.silver;
            }
            if (Score > 3500)
            {
                trophy.Image = Properties.Resources.gold;
            }
            backgroundWorker2.RunWorkerAsync();
        }

        private void Start_Click(object sender, EventArgs e)
        {
            Score = 0;
            Reset();
            backgroundWorker1.RunWorkerAsync();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Score++; // увелечения счета, когда машинка двигается 
            distance.Text = "" + Score; // отображение счета в форме


            RoadTrack1.Top += roadSpeed; // движение трека вниз +=  скорости дороги
            RoadTrack2.Top += roadSpeed;

            // усли трек будет ниже -630 , возвращает его в начальную позицию (-630)
            // аналогично если будет выше 630
            // это дает нам без пробельную анимацию
            if (RoadTrack1.Top > -630)
            {
                RoadTrack1.Top = -630;
            }
            if (RoadTrack2.Top > 630)
            {
                RoadTrack2.Top = -630;
            }
            // конец анимации трека
            if (carLeft) { player.Left -= carSpeed; }; // переместите автомобиль влево, если автомобиль остался (true) 
            if (carRight) { player.Left += carSpeed; };// переместите автомобиль вправо, если автомобиль остался (true) 
            // конец движения машины

            if (player.Left < 1) // если машина уезжает за поле
            {
                carLeft = false; // остановить ее продвижение за игровую зону
            }
            else if (player.Left + player.Width > 380)
            {
                carRight = false;
            }

            // передвижение AI вниз поля
            AI1.Top += trafficSpeed;
            AI2.Top += trafficSpeed;

            if (AI1.Top > GameField.Height) // респавн AI и изменение их картинок 
            {
                ChangeAI1();
                AI1.Left = rnd.Next(2, 160);
                AI1.Top = rnd.Next(100, 200) * -1;
            }

            if (AI2.Top > GameField.Height)
            {
                ChangeAI2();
                AI2.Left = rnd.Next(185, 327);
                AI2.Top = rnd.Next(100, 200) * -1;
            }
            // проверка ударился ли игрок и AI 
            // если они столкнулись -
            if (player.Bounds.IntersectsWith(AI1.Bounds) || player.Bounds.IntersectsWith(AI2.Bounds))
            {
                GameOver();// - игра прекращается 
                backgroundWorker1.RunWorkerAsync();
            }
            // в этой части кода мы увелечиваем скорость 
            // если счет входит в определенные пределы
            if (Score > 100 && Score < 1000)
            {
                trafficSpeed = 8;
                roadSpeed = 11;
            }
            else if (Score > 1000 && Score < 2500)
            {
                trafficSpeed = 10;
                roadSpeed = 13;
            }
            else if (Score > 2500)
            {
                trafficSpeed = 15;
                roadSpeed = 17;
            }
        }
        public static System.Drawing.Drawing2D.GraphicsPath BuildTransparencyPath(Image im)
        {
            int x;
            int y;
            Bitmap bmp = new Bitmap(im);
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            Color mask = bmp.GetPixel(0, 0);

            for (x = 0; x <= bmp.Width - 1; x++)
            {
                for (y = 0; y <= bmp.Height - 1; y++)
                {
                    if (!bmp.GetPixel(x, y).Equals(mask))
                    {
                        gp.AddRectangle(new Rectangle(x, y, 1, 1));
                    }
                }
            }
            bmp.Dispose();
            return gp;
        }

        private void StreetRacing_Load(object sender, EventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = BuildTransparencyPath(player.Image);
            player.Region = new Region(gp);
            System.Drawing.Drawing2D.GraphicsPath mp = BuildTransparencyPath(AI1.Image);
            AI1.Region = new Region(mp);
            System.Drawing.Drawing2D.GraphicsPath rp = BuildTransparencyPath(AI2.Image);
            AI2.Region = new Region(rp);
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.Media.SoundPlayer sound = new System.Media.SoundPlayer(@"Re-evolution.wav");
            sound.Play();
        }

        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.Media.SoundPlayer sound = new System.Media.SoundPlayer(@"hit.wav");
            sound.Play();            
        }        
    }
}
