using System;
using System.IO;

#pragma warning disable CA1416 //Проверка совместимости платформы

namespace BlackRedTree
{
    public enum Color
    {
        Red = 0,
        Black
    }

    public class Program
    {
        static void Main(string[] args)
        {

            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            RedBlackTree tree = new RedBlackTree();
            using (StreamReader sr = File.OpenText("C:\\Users\\Mak\\source\\repos\\BlackRedTree\\input.txt"))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    tree.Insert(s);
                }
            }

            tree.DisplayTree();

            while (tree.root)
            {
                tree.Delete(tree.root);
                tree.DisplayTree();
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
