using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyatnashki
{
    class Game
    {
        int size;
        int spaceX, spaceY;
        int[,] map;
        static Random rand = new Random();     
        public Game(int size)
        {
            if (size < 2) size = 2;
            if (size > 4) size = 5;
            this.size = size;
            map = new int[size, size];
        }

        public void Start()
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    map[x, y] = CoordsToPosition(x, y) + 1;
            spaceX = size - 1;
            spaceY = size - 1;
            map[spaceX, spaceY] = 0;
        }

        // Перемещение пустой клетки
        public bool Shift(int position)
        {
            int x, y;
            PositionToCoords(position, out x, out y);
            if (Math.Abs(spaceX - x) + Math.Abs(spaceY - y) != 1) return false;
            map[spaceX, spaceY] = map[x, y];
            map[x, y] = 0;
            spaceX = x;
            spaceY = y;
            return true;
        }

        // Случайное перемешивание клеток
        public void RandomShift()
        {
            int r = rand.Next(0, 4);
            int x = spaceX;
            int y = spaceY;
            switch (r)
            {
                case 0: x--; break;
                case 1: y--; break;
                case 2: x++; break;
                case 3: y++; break;
            }
            Shift(CoordsToPosition(x, y));
        }

        public bool EndGame()
        { 
            if(!(spaceX == size - 1 && spaceY == size - 1))
                return false;
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    if (!(x == size - 1 && y == size - 1))
                        if (map[x, y] != CoordsToPosition(x, y) + 1)
                            return false;
            return true;
        }

        // Получение позиции клетки
        public int GetNumber(int position)
        {
            int x, y;
            PositionToCoords(position, out x, out y);
            if (x < 0 || x >= size) return 0;
            if (y < 0 || y >= size) return 0;
            return map[x, y];
        }
        
        private int CoordsToPosition(int x, int y)
        {
            if (x < 0) x = 0;
            if (x > size - 1) x = size - 1;
            if (y < 0) y = 0;
            if (y > size - 1) y = size - 1;
            return y * size + x;
        }

        private void PositionToCoords(int position, out int x, out int y)
        {
            if (position < 0) position = 0;
            if (position > size * size - 1) position = size * size - 1;
            x = position % size;
            y = position / size;
        }
    }
}
