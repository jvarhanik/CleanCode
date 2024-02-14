using Pixel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Snake
{
    public class SnakeGame
    {
        private const int ScreenWidth = 32;
        private const int ScreenHeight = 16;
        private const int InitialScore = 5;
        private const int GameOver = 1;
        private const int BerryGenerationInterval = 500;

        private readonly Random random = new Random();
        private readonly List<int> xPosSnake = new List<int>();
        private readonly List<int> yPosSnake = new List<int>();

        private int score = InitialScore;
        private int berryX = 0;
        private int berryY = 0;
        private DateTime lastBerryGenerationTime = DateTime.Now;
        private string direction = "RIGHT";
        private bool gameOver = false;

        public void RunGame()
        {
            Console.WindowHeight = ScreenHeight;
            Console.WindowWidth = ScreenWidth;

            InitializeSnakeHead();
            GenerateBerry();

            while (!gameOver)
            {
                UpdateGame();
                Thread.Sleep(1000);
            }

            DisplayGameOver();
        }

        private void InitializeSnakeHead()
        {
            SnakePixel head = new SnakePixel
            {
                XPos = ScreenWidth / 2,
                YPos = ScreenHeight / 2,
                ScreenColor = ConsoleColor.Red
            };
            xPosSnake.Add(head.XPos);
            yPosSnake.Add(head.YPos);
        }

        private void GenerateBerry()
        {
            berryX = random.Next(1, ScreenWidth - 2);
            berryY = random.Next(1, ScreenHeight - 2);
        }

        private void UpdateGame()
        {
            HandleInput();
            MoveSnake();
            CheckCollisions();
            DrawGame();
        }

        private void HandleInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow && direction != "DOWN")
                    direction = "UP";
                if (key.Key == ConsoleKey.DownArrow && direction != "UP")
                    direction = "DOWN";
                if (key.Key == ConsoleKey.LeftArrow && direction != "RIGHT")
                    direction = "LEFT";
                if (key.Key == ConsoleKey.RightArrow && direction != "LEFT")
                    direction = "RIGHT";
            }
        }

        private void MoveSnake()
        {
            int nextX = xPosSnake.Last();
            int nextY = yPosSnake.Last();

            switch (direction)
            {
                case "UP":
                    nextY--;
                    break;
                case "DOWN":
                    nextY++;
                    break;
                case "LEFT":
                    nextX--;
                    break;
                case "RIGHT":
                    nextX++;
                    break;
            }

            xPosSnake.Add(nextX);
            yPosSnake.Add(nextY);

            if (nextX == berryX && nextY == berryY)
            {
                score++;
                GenerateBerry();
            }
            else
            {
                xPosSnake.RemoveAt(0);
                yPosSnake.RemoveAt(0);
            }
        }

        private void CheckCollisions()
        {
            int headX = xPosSnake.Last();
            int headY = yPosSnake.Last();

            if (headX == 0 || headX == ScreenWidth - 1 || headY == 0 || headY == ScreenHeight - 1)
                gameOver = true;

            for (int i = 0; i < xPosSnake.Count - 1; i++)
            {
                if (headX == xPosSnake[i] && headY == yPosSnake[i])
                {
                    gameOver = true;
                    break;
                }
            }
        }

        private void DrawGame()
        {
            Console.Clear();

            DrawBorders();

            for (int i = 0; i < xPosSnake.Count; i++)
            {
                Console.SetCursorPosition(xPosSnake[i], yPosSnake[i]);
                Console.Write("■");
            }

            Console.SetCursorPosition(berryX, berryY);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("■");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Score: " + score);
        }

        private void DrawBorders()
        {
            for (int i = 0; i < ScreenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, ScreenHeight - 1);
                Console.Write("■");
            }

            for (int i = 0; i < ScreenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(ScreenWidth - 1, i);
                Console.Write("■");
            }
        }

        private void DisplayGameOver()
        {
            Console.SetCursorPosition(ScreenWidth / 5, ScreenHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
            Console.SetCursorPosition(ScreenWidth / 5, ScreenHeight / 2 + 1);
        }
    }
}
