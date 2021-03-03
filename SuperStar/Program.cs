using System;
using System.Linq;
using System.Threading;

namespace SuperStar
{
    class Program
    {

        static Entity player;
        static Entity badGuy;
        static void Main(string[] args)
        {
            int minX = Console.WindowWidth / 4,
                minY = Console.WindowHeight / 4,
                maxX = 3 * Console.WindowWidth / 4,
                maxY = 3 * Console.WindowHeight / 4;
            Random rnd = new Random();

            DrawDesk(minX-1, minY-1, maxX+1, maxY+1);
            player = new Entity(minX, minY, minX, minY, maxX, maxY);

            int enemyStartX;
            do
            {
                enemyStartX = rnd.Next(minX, maxX + 1);
            } while (enemyStartX == player.X);
            int enemyStartY;
            do
            {
                enemyStartY = rnd.Next(minY, maxY + 1);
            } while (enemyStartY == player.Y);

            Console.SetCursorPosition(enemyStartX, enemyStartY);
            badGuy = new Entity(enemyStartX, enemyStartY, minX, minY, maxX, maxY, '@', ConsoleColor.Red);
            Console.SetCursorPosition(player.X, player.Y);
            new Thread(() => EnemyLoop()).Start();
            GameLoop();
        }

        static void GameLoop()
        {
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                MovePlayer(keyInfo);
                if (player.X == badGuy.X && player.Y == badGuy.Y)
                {
                    EndGame();
                    return;
                }
            }
            while (keyInfo.Key != ConsoleKey.Escape);
        }

        private static void EndGame()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                for (int j = 0; j < Console.WindowWidth; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(" ");
                }
            }
            
            string[] gameOverText = new string[] {
                    "████──████──█───██─███\t████──█─█──███──████",
                    "█─────█──█──██─███─█  \t█──█──█─█──█────█──█",
                    "█─██──████──█─█─██─███\t█──█──█─█──███──████" ,
                    "█──█──█──█──█───██─█  \t█──█──███──█────█─█" ,
                    "████──█──█──█───██─███\t████───█───███──█─█" ,
                    "",
                    "",
                    "Press any key to quit. "
                    };
            foreach (var line in gameOverText.Select((value, index) => (value, index)))
            {
                Console.SetCursorPosition(2, 2+line.index);
                Console.Write(line.value);
            }
            Console.ReadKey();
        }

        static void EnemyLoop()
        {
            while (true)
            {
                int vectorX = player.X - badGuy.X;
                int vectorY = player.Y - badGuy.Y;
                float vectorDistance = MathF.Sqrt(vectorX + vectorY);
                float normalizedX = (int)(vectorX / vectorDistance);
                float normalizedY = (int)(vectorY / vectorDistance);
                int offsetX = MathF.Abs(normalizedX) > 0.25 ? MathF.Sign(normalizedX) : 0;
                int offsetY = MathF.Abs(normalizedY) > 0.25 ? MathF.Sign(normalizedY) : 0;

                badGuy.Move(offsetX, offsetY);
                Thread.Sleep(1000);
            }
        }

        private static void MovePlayer(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.W:
                    player.Move(0, -1);
                    break;
                case ConsoleKey.S:
                    player.Move(0, 1);
                    break;
                case ConsoleKey.A:
                    player.Move(-1, 0);
                    break;
                case ConsoleKey.D:
                    player.Move(1, 0);
                    break;
            }
        }

        static void DrawDesk(int minX, int minY, int maxX, int maxY)
        {
            for (int i = minX + 1; i < maxX; i++)
            {
                Draw('-', i, minY);
                Draw('-', i, maxY);
            }

            for (int i = minY + 1; i < maxY; i++)
            {
                Draw('|', minX, i);
                Draw('|', maxX, i);
            }
            Draw('+', minX, minY);
            Draw('+', maxX, minY);
            Draw('+', minX, maxY);
            Draw('+', maxX, maxY);
        }
        
        public static void DrawEntity(int x, int y, char viewChar, ConsoleColor color)
        {
            //Console.ForegroundColor = Console.BackgroundColor;
            Console.Write(" ");
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(viewChar);
            Console.SetCursorPosition(x, y);
        }

        static void Draw(char symb, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(symb);
        }
    }
}
