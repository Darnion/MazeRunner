namespace MazeRunner
{
    internal class Program
    {
        private static void CharacterMoveDisplay(int x, int y, ref char[,] map, char c)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(c);
            if (map[y, x] != '‡')
            {
                map[y, x] = c;
            }
            Console.CursorLeft--;
        }

        private static void CharacterMove(ref int x, ref int y, ref char[,] map, ref bool canGoOut)
        {
            var tempX = x;
            var tempY = y;

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.UpArrow:
                    y--;
                    break;
                case ConsoleKey.DownArrow:
                    y++;
                    break;
                case ConsoleKey.LeftArrow:
                    x--;
                    break;
                case ConsoleKey.RightArrow:
                    x++;
                    break;
            }

            if (map[y, x] == '†')
            {
                canGoOut = true;
            }

            if ((map[y, x] != '‡' || canGoOut) && map[y, x] != '█')
            {
                CharacterMoveDisplay(x, y, ref map, '☻');
                if (map[y, x] != '‡')
                {
                    x = Console.CursorLeft;
                    y = Console.CursorTop;
                }
                CharacterMoveDisplay(tempX, tempY, ref map, ' ');
            }
            else
            {
                x = tempX;
                y = tempY;
            }
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Введите номер уровня от 1 до 3.");

                var lvlNum = Console.ReadLine();

                while (!int.TryParse(lvlNum, out var levelNumber) || levelNumber < 1 || levelNumber > 3)
                {
                    Console.WriteLine("Неверное значение! Попробуйте ещё!");
                    lvlNum = Console.ReadLine();
                }

                var canGoOut = true;
                string[] mazeLevel;

                switch (lvlNum)
                {
                    case "1":
                        mazeLevel = File.ReadAllLines(Properties.Resources.levelOne);
                        break;
                    case "2":
                        mazeLevel = File.ReadAllLines(Properties.Resources.levelTwo);
                        break;
                    default:
                        mazeLevel = File.ReadAllLines(Properties.Resources.levelThree);
                        break;
                }

                char[,] map = new char[mazeLevel.Length, mazeLevel[0].Length];

                var characterX = 0;
                var characterY = 0;

                for (var i = 0; i < map.GetLength(0); i++)
                {
                    for (var j = 0; j < map.GetLength(1); j++)
                    {
                        map[i, j] = mazeLevel[i][j] == '1'
                            ? '█'
                            : mazeLevel[i][j] == '2'
                                ? '☻'
                                : mazeLevel[i][j] == '3'
                                    ? '‡'
                                    : mazeLevel[i][j] == '4'
                                        ? '†'
                                        : ' ';
                        if (map[i, j] == '☻')
                        {
                            characterX = j;
                            characterY = i;
                        }

                        if (map[i, j] == '†')
                        {
                            canGoOut = false;
                        }
                    }
                }

                Console.SetWindowSize(map.GetLength(1), map.GetLength(0));
                Console.Clear();
                Console.CursorVisible = false;

                for (var i = 0; i < map.GetLength(0); i++)
                {
                    for (var j = 0; j < map.GetLength(1); j++)
                    {
                        Console.Write(map[i, j]);
                    }

                    if (i != map.GetLength(0) - 1)
                    {
                        Console.WriteLine();
                    }
                }

                while (true)
                {
                    Console.SetCursorPosition(characterX, characterY);

                    CharacterMove(ref characterX, ref characterY, ref map, ref canGoOut);

                    if (map[characterY, characterX] == '‡')
                    {
                        break;
                    }
                }

                Console.Clear();
                Console.SetWindowSize(78, 20);
                Console.SetCursorPosition(27, 9);
                Console.Write("Вы прошли лабиринт!");
                Console.SetCursorPosition(1, 10);
                Console.Write("Для завершения нажмите Enter, для возвращения к выбору уровня любую клавишу.");

                if (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    break;
                };
            }
        }
    }
}
