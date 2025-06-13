using System;
using System.Collections.Generic;
using System.IO;

namespace Красно_черное_дерево
{

    class Program
    {
        static void Main()
        {
            Random rand = new Random();
            List<int> sizes = new List<int>();
            for (int i = 100; i <= 10000; i += 100)
                sizes.Add(i);

            foreach (var size in sizes)
            {
                double insertTime = 0, searchTime = 0, deleteTime = 0;

                for (int trial = 0; trial < 5; trial++)
                {
                    var words = GenerateRandomWords(size, rand);
                    RedBlackTree tree = new RedBlackTree();

                    // Вставка
                    DateTime start = DateTime.Now;
                    foreach (var word in words)
                        tree.Insert(word);
                    insertTime += (DateTime.Now - start).TotalMilliseconds;

                    // Поиск
                    start = DateTime.Now;
                    foreach (var word in words)
                        tree.Search(word);
                    searchTime += (DateTime.Now - start).TotalMilliseconds;

                    // Удаление
                    start = DateTime.Now;
                    foreach (var word in words)
                        tree.Delete(word);
                    deleteTime += (DateTime.Now - start).TotalMilliseconds;
                }

                Console.WriteLine($"{size} | {insertTime / 5:F2} | {searchTime / 5:F2} | {deleteTime / 5:F2}");
            }
        }

        static List<string> GenerateRandomWords(int count, Random rand)
        {
            var words = new List<string>();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < count; i++)
            {
                string word = new string(Enumerable.Repeat(chars, 5).Select(s => s[rand.Next(s.Length)]).ToArray());
                words.Add(word);
            }
            return words;
        }
    }
}