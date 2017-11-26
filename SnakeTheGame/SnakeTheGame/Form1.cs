using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeTheGame
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();

            new Settings();

            GameTimer.Interval = 1000 / Settings.Speed;
            GameTimer.Tick += UpdateScreen;
            GameTimer.Start();

            //Start New Game
            StarGame();
        }

        private void StarGame()
        {
            new Settings();

            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);

            LabelScore.Text = Settings.Score.ToString();
            GenerateFood();
        }

        private void GenerateFood()
        {
            int maxXPosition = pbCanvas.Size.Width / Settings.Width;
            int maxYPosition = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            food = new Circle();

            food.X = random.Next(0, maxXPosition);
            food.Y = random.Next(0, maxYPosition);
        }
    }
}
