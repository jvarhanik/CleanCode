﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace SnakeGame
{
    enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    class SnakeGame
    {
        const int ScreenWidth = 32;
        const int ScreenHeight = 16;
        const int InitialScore = 5;
        const int MovementIntervalMilliseconds = 500;

        Random random = new Random();
        int score;
        bool gameOver;
        ConsoleColor snakeHeadColor = ConsoleColor.Red;
        ConsoleColor snakeBodyColor = ConsoleColor.Green;
        List<int> xPositions = new List<int>();
        List<int> yPositions = new List<int>();
        int berryX;
        int berryY;
        Direction direction = Direction.RIGHT;
        int pixelsAdded = 0;

        public void Start()
        {
            InitializeGame();

            while (!gameOver)
            {
                DrawFrame();
                HandleInput();
                MoveSnake();
                CheckCollisions();
                Thread.Sleep(MovementIntervalMilliseconds);
            }

            DisplayGameOver();
        }

        void InitializeGame()
        {
            Console.WindowHeight = ScreenHeight;
            Console.WindowWidth = ScreenWidth;
            score = InitialScore;
            gameOver = false;

            xPositions.Clear();
            yPositions.Clear();

            xPositions.Add(ScreenWidth / 2);
            yPositions.Add(ScreenHeight / 2);

            PlaceBerry();
        }

        void DrawFrame()
        {
            Console.Clear();
            DrawBorders();
            DrawSnake();
            DrawBerry();
        }

        void DrawBorders()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
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

        void DrawSnake()
        {
            Console.ForegroundColor = snakeHeadColor;
            Console.SetCursorPosition(xPositions[0], yPositions[0]);
            Console.Write("■");

            Console.ForegroundColor = snakeBodyColor;
            for (int i = 1; i < xPositions.Count; i++)
            {
                Console.SetCursorPosition(xPositions[i], yPositions[i]);
                Console.Write("■");
            }
        }

        void DrawBerry()
        {
            Console.SetCursorPosition(berryX, berryY);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("■");
        }

        void PlaceBerry()
        {
            berryX = random.Next(1, ScreenWidth - 2);
            berryY = random.Next(1, ScreenHeight - 2);
        }

        void HandleInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (direction != Direction.DOWN)
                            direction = Direction.UP;
                        break;
                    case ConsoleKey.DownArrow:
                        if (direction != Direction.UP)
                            direction = Direction.DOWN;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (direction != Direction.RIGHT)
                            direction = Direction.LEFT;
                        break;
                    case ConsoleKey.RightArrow:
                        if (direction != Direction.LEFT)
                            direction = Direction.RIGHT;
                        break;
                }
            }
        }

        void MoveSnake()
        {
            int newX = xPositions[0];
            int newY = yPositions[0];

            switch (direction)
            {
                case Direction.UP:
                    newY--;
                    break;
                case Direction.DOWN:
                    newY++;
                    break;
                case Direction.LEFT:
                    newX--;
                    break;
                case Direction.RIGHT:
                    newX++;
                    break;
            }

            xPositions.Insert(0, newX);
            yPositions.Insert(0, newY);

            if (pixelsAdded < 4)
            {
                pixelsAdded++;
            }
            else if (pixelsAdded >= 4 && xPositions.Count > score)
            {
                xPositions.RemoveAt(xPositions.Count - 1);
                yPositions.RemoveAt(yPositions.Count - 1);
            }
        }

        void CheckCollisions()
        {
            int headX = xPositions[0];
            int headY = yPositions[0];

            if (headX == 0 || headX == ScreenWidth - 1 || headY == 0 || headY == ScreenHeight - 1)
                gameOver = true;

            for (int i = 1; i < xPositions.Count; i++)
            {
                if (headX == xPositions[i] && headY == yPositions[i])
                {
                    gameOver = true;
                    break;
                }
            }

            if (headX == berryX && headY == berryY)
            {
                score++;
                PlaceBerry();
            }
        }

        void DisplayGameOver()
        {
            Console.SetCursorPosition(ScreenWidth / 5, ScreenHeight / 2);
            Console.WriteLine("Game Over, Score: " + score);
        }
    }
}
