using System;

namespace SuperStar
{
    public class Entity
    {
        int x;
        int y;
        int minX, minY, maxX, maxY;
        char viewChar;
        ConsoleColor viewColor;

        public int X { get => x; }
        public int Y { get => y; }

        public Entity(int x, int y, int minX, int minY, int maxX, int maxY, char viewChar = '*', ConsoleColor color = ConsoleColor.White)
        {
            if (x < minX || x > maxX) throw new ArgumentException("X out of game field", "x");
            if (y < minY || y > maxY) throw new ArgumentException("Y out of game field", "y");
                    
            this.x = x;
            this.y = y;
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;
            this.viewChar = viewChar;
            this.viewColor = color;
            
            Program.DrawEntity(x, y, viewChar, color);
        }

        public void Move(int offsetX, int offsetY)
        {
            int newX = x + offsetX;
            int newY = y + offsetY;
            if (newX <= maxX && newX >= minX && newY <= maxY && newY >= minY)
            {
                x = newX;
                y = newY;
                Program.DrawEntity(newX, newY, viewChar, viewColor);
                return;
            }
        }
    }
}
