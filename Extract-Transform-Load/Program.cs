using System;
using System.Collections.Generic;
using System.Linq;

namespace EtlScrabbleImplementation
{
    public static class EtlScrabble
    {
        public static Dictionary<string, int> Transform(Dictionary<int, string[]> old)
        {
            var transformed = new Dictionary<string, int>();

            foreach (var kvp in old)
            {
                var score = kvp.Key;
                var letters = kvp.Value;

                foreach (var letter in letters)
                {
                    var lowercaseLetter = letter.ToLower();

                    if (!transformed.ContainsKey(lowercaseLetter))
                    {
                        transformed.Add(lowercaseLetter, score);
                    }
                }
            }

            return transformed.OrderBy(kv => kv.Key).ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }

    public class EtlScrabbleTests
    {
        public static void Main(string[] args)
        {
            TestTransform_SingleLetter();
            TestTransform_MultipleLetters();
            TestTransform_MultipleScores();
            TestTransform_AllScores();

            Console.ReadLine();
        }

        public static void TestTransform_SingleLetter()
        {
            // Arrange
            var old = new Dictionary<int, string[]>
            {
                { 1, new string[] { "A" } }
            };

            // Act
            var result = EtlScrabble.Transform(old);

            // Assert
            var expected = new Dictionary<string, int>
            {
                { "a", 1 }
            };

            AssertEqual(expected, result);
        }

        public static void TestTransform_MultipleLetters()
        {
            // Arrange
            var old = new Dictionary<int, string[]>
            {
                { 1, new string[] { "A", "E", "I", "O", "U" } }
            };

            // Act
            var result = EtlScrabble.Transform(old);

            // Assert
            var expected = new Dictionary<string, int>
            {
                { "a", 1 },
                { "e", 1 },
                { "i", 1 },
                { "o", 1 },
                { "u", 1 }
            };

            AssertEqual(expected, result);
        }

        public static void TestTransform_MultipleScores()
        {
            // Arrange
            var old = new Dictionary<int, string[]>
            {
                { 1, new string[] { "A", "E" } },
                { 2, new string[] { "D", "G" } }
            };

            // Act
            var result = EtlScrabble.Transform(old);

            // Assert
            var expected = new Dictionary<string, int>
            {
                { "a", 1 },
                { "d", 2 },
                { "e", 1 },
                { "g", 2 }
            };

            AssertEqual(expected, result);
        }

        public static void TestTransform_AllScores()
        {
            // Arrange
            var old = new Dictionary<int, string[]>
            {
                { 1, new string[] { "A", "E", "I", "O", "U", "L", "N", "R", "S", "T" } },
                { 2, new string[] { "D", "G" } },
                { 3, new string[] { "B", "C", "M", "P" } },
                { 4, new string[] { "F", "H", "V", "W", "Y" } },
                { 5, new string[] { "K" } },
                { 8, new string[] { "J", "X" } },
                { 10, new string[] { "Q", "Z" } }
            };

            // Act
            var result = EtlScrabble.Transform(old);

            // Assert
            var expected = new Dictionary<string, int>
            {
                { "a", 1 }, { "b", 3 }, { "c", 3 }, { "d", 2 }, { "e", 1 },
                { "f", 4 }, { "g", 2 }, { "h", 4 }, { "i", 1 }, { "j", 8 },
                { "k", 5 }, { "l", 1 }, { "m", 3 }, { "n", 1 }, { "o", 1 },
                { "p", 3 }, { "q", 10 }, { "r", 1 }, { "s", 1 }, { "t", 1 },
                { "u", 1 }, { "v", 4 }, { "w", 4 }, { "x", 8 }, { "y", 4 },
                { "z", 10 }
            };

            AssertEqual(expected, result);
        }

        public static void AssertEqual<TKey, TValue>(Dictionary<TKey, TValue> expected, Dictionary<TKey, TValue> actual)
        {
            if (expected.SequenceEqual(actual))
            {
                Console.WriteLine("Test Passed");
            }
            else
            {
                Console.WriteLine("Test Failed");
            }

            Console.WriteLine("Expected:");
            PrintInBatches(expected.Select(kv => $"{{ {kv.Key}, {kv.Value} }}"), 5);

            Console.WriteLine("Actual:");
            PrintInBatches(actual.Select(kv => $"{{ {kv.Key}, {kv.Value} }}"), 5);

            Console.WriteLine();
        }

        public static void PrintInBatches<T>(IEnumerable<T> items, int batchSize)
        {
            var count = 0;
            var batch = new List<T>();

            foreach (var item in items)
            {
                batch.Add(item);
                count++;

                if (count % batchSize == 0)
                {
                    Console.WriteLine(string.Join(", ", batch));
                    batch.Clear();
                }
            }

            if (batch.Count > 0)
            {
                Console.WriteLine(string.Join(", ", batch));
            }
        }
    }
}
