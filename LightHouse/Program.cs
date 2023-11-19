using NUnit.Framework;
using NUnitLite;
using System.Collections.Generic;
using System;

namespace LightHouse
{
    interface IWordManipulator
    {
        string ReverseWords(string input);
    }


    class WordManipulator : IWordManipulator
    {
        public string ReverseWords(string input)
        {
            string[] words = input.Split(' ');
            Array.Reverse(words);
            return string.Join(" ", words);
        }
    }

    class WordProcessor
    {
        private readonly IWordManipulator wordManipulator;

        public WordProcessor(IWordManipulator wordManipulator)
        {
            this.wordManipulator = wordManipulator;
        }

        public List<string> ProcessLines(List<string> lines)
        {
            List<string> results = new List<string>();

            for (int i = 0; i < lines.Count; i++)
            {
                string input = lines[i];

                // Check input limit
                if (input.Length < WordReverser.MinInputLimit || input.Length > WordReverser.MaxInputLimit)
                {
                    results.Add($"Case {i + 1}: Input length should be between {WordReverser.MinInputLimit} and {WordReverser.MaxInputLimit} characters.");
                }
                else
                {
                    string reversedWords = wordManipulator.ReverseWords(input);
                    results.Add($"Case {i + 1}: {reversedWords}");
                }
            }

            return results;
        }
    }

    class WordReverser
    {
        private readonly WordProcessor wordProcessor;
        public const int MinInputLimit = 1;
        public const int MaxInputLimit = 25;

        public WordReverser(WordProcessor wordProcessor)
        {
            this.wordProcessor = wordProcessor;
        }

        public void ProcessCases()
        {
            int N;

            // Input for the number of cases (N) with error handling
            while (true)
            {
                Console.Write("Enter the number of cases (N): ");

                if (int.TryParse(Console.ReadLine(), out N) && N > 0)
                {
                    break;
                }

                Console.WriteLine("Invalid input. Please enter a positive integer.");
            }

            List<string> lines = new List<string>();

            for (int i = 0; i < N; i++)
            {
                Console.Write($"Enter line {i + 1}: ");
                lines.Add(Console.ReadLine());
            }

            List<string> results = wordProcessor.ProcessLines(lines);

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
    }

    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestReverseWords()
        {
            IWordManipulator wordManipulator = new WordManipulator();
            WordProcessor wordProcessor = new WordProcessor(wordManipulator);

            List<string> lines = new List<string>
            {
                "this is a test",
                "foobar",
                "all your base"
            };

            List<string> results = wordProcessor.ProcessLines(lines);

            Assert.AreEqual("Case 1: test a is this", results[0]);
            Assert.AreEqual("Case 2: foobar", results[1]);
            Assert.AreEqual("Case 3: base your all", results[2]);
        }
    }

    class Program
    {
        static void Main()
        {
            // Injecting the dependency
            IWordManipulator wordManipulator = new WordManipulator();
            WordProcessor wordProcessor = new WordProcessor(wordManipulator);
            var wordReverser = new WordReverser(wordProcessor);

            // Run NUnit tests
            RunTests();

            wordReverser.ProcessCases();
            Console.ReadLine();
        }

        static void RunTests()
        {
            var testAssembly = typeof(Tests).Assembly;
            var runner = new AutoRun(testAssembly);
            runner.Execute(new string[0]);
        }
    }
}
