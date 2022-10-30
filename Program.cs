using System;

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

        private static void CharacterMove(ref int x, ref int y, ref char[,] map)
        {
            var tempX = x;
            var tempY = y;

            if (map[Console.CursorTop, Console.CursorLeft] != '█')
            {
                CharacterMoveDisplay(Console.CursorLeft, Console.CursorTop, ref map, '☻');
                if (map[y, x] != '‡')
                {
                    x = Console.CursorLeft;
                    y = Console.CursorTop;
                }
                CharacterMoveDisplay(tempX, tempY, ref map, ' ');
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
                        mazeLevel = File.ReadAllLines(@"levels/levelOne.txt");
                        break;
                    case "2":
                        mazeLevel = File.ReadAllLines(@"levels/levelTwo.txt");
                        break;
                    default:
                        mazeLevel = File.ReadAllLines(@"levels/levelThree.txt");
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

                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.UpArrow:
                            Console.CursorTop--;
                            break;
                        case ConsoleKey.DownArrow:
                            Console.CursorTop++;
                            break;
                        case ConsoleKey.LeftArrow:
                            Console.CursorLeft--;
                            break;
                        case ConsoleKey.RightArrow:
                            Console.CursorLeft++;
                            break;
                    }

                    if (map[Console.CursorTop, Console.CursorLeft] == '†')
                    {
                        canGoOut = true;
                    }

                    if (map[Console.CursorTop, Console.CursorLeft] != '‡' || canGoOut)
                    {
                        CharacterMove(ref characterX, ref characterY, ref map);
                    }
                    else
                    {
                        Console.CursorTop = characterY;
                        Console.CursorLeft = characterX;
                    }

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
                Console.Write("Для возвращения к выбору уровня нажмите Enter, для завершения любую клавишу.");

                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    Console.Clear();
                    break;
                };
            }
        }
    }
}
