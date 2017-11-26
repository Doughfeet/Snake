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
            StartGame();
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            //Check for Game Over
            if (Settings.GameOver == true)
            {
                //check if Enter is pressed
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer();
            }
            pbCanvas.Invalidate();
        }

        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Up: Snake[i].Y++;
                            break;
                        case Direction.Down:Snake[i].Y--;
                            break;
                        case Direction.Left: Snake[i].X--;
                            break;
                        case Direction.Right: Snake[i].X++;
                            break;
                        default:
                            break;
                    }

                    //Get maximum X and Y position

                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;

                    //Detect collission with game borders

                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }

                    for (int j = 0; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    //Detect colliison with food piece
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }
                    

                }
                else
                {
                    //Move body
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }

        }

        private void Eat()
        {
            Circle food = new Circle();
            food.X = Snake[Snake.Count - 1].X;
            food.Y = Snake[Snake.Count - 1].Y;


            Snake.Add(food);

            Settings.Score += Settings.Points;
            LabelScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void StartGame()
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

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (Settings.GameOver != false)
            {
                //Set color of snake
                Brush snakeColor;

                //Draw snake
                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                    {
                        snakeColor = Brushes.Black; //Draw head
                    }
                    else
                        snakeColor = Brushes.Green; // rest of body

                    canvas.FillEllipse(snakeColor,
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width, Settings.Height));

                    //Draw food
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width,
                                      food.Y * Settings.Height, Settings.Width, Settings.Height));


                }
            }
            else
            {
                labelGameOver.Visible = true;
                labelGameOver.Text = "Game over, press enter to restart";
            }
        }
    }



}
