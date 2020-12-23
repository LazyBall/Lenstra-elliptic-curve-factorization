using Lenstra_elliptic_curve_factorization;
using System;
using System.Linq;
using System.Numerics;

namespace LenstraConsoleApp
{
    class Utility
    {
        private LenstraECFactorization _lenstra;
        private bool _firstRun;

        public Utility()
        {
            this._lenstra = new LenstraECFactorization();
            this._firstRun = true;
        }

        public void ShowMain()
        {
            if (this._firstRun)
            {
                Help();
                this._firstRun = false;
            }

            while (true)
            {
                Console.Write("Enter command:> ");

                switch (Console.ReadLine().Replace(" ", "").ToLower())
                {
                    case "chca":
                        ChangeCountOfAttempts();
                        break;

                    case "chsb":
                        ChangeSizeOfBase();
                        break;

                    case "clear":
                        Console.Clear();
                        break;

                    case "exit":
                        return;

                    case "help":
                        Help();
                        break;

                    case "run":
                        ShowFactorizationMenu();
                        break;

                    case "shconf":
                        ShowInfo();
                        break;

                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
        }

        private void ShowFactorizationMenu()
        {
            Console.Write("Enter number for factorization (min 4, default random): ");
            string inputN = Console.ReadLine().Replace(" ", "");
            BigInteger number;
            if (string.IsNullOrEmpty(inputN))
            {
                number = new Random(DateTime.Now.Millisecond).Next();
                if (number < 4) number += 4;
            }
            else if (BigInteger.TryParse(inputN, out number))
            {
                if (number < 4)
                {
                    Console.WriteLine("Invalid input. The number must be greater than three.\n");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Invalid input.\n");
                return;
            }
            Console.WriteLine("Selected number = {0}\n", number);

            ShowResults(number, RunAlgorithm(number));
        }

        private BigInteger RunAlgorithm(BigInteger number)
        {
            Console.WriteLine("Computing...");
            using (ProgressBar progressBar = new ProgressBar())
            {
                number = this._lenstra.Factorize(number, progressBar);
            }
            Console.WriteLine("Done.");

            return number;
        }

        private void ShowResults(BigInteger number, BigInteger divisor)
        {
            Console.WriteLine();
            Console.WriteLine("\t\t\t\t\t R E S U L T \n");
            Console.WriteLine(Enumerable.Repeat('#', 90).ToArray());
            Console.WriteLine("{0} = {1} * {2}", number, divisor, number / divisor);
            Console.WriteLine(Enumerable.Repeat('#', 90).ToArray());
            Console.WriteLine("\n");
        }

        private void ShowInfo()
        {
            Console.WriteLine();
            Console.WriteLine("\t C U R R E N T  C O N F I G");
            Console.WriteLine(Enumerable.Repeat('#', 50).ToArray());
            Console.WriteLine("Count of attempts: {0}", this._lenstra.Attempts);
            Console.WriteLine("Size of base: {0}", this._lenstra.SizeOfBase);
            Console.WriteLine(Enumerable.Repeat('#', 50).ToArray());
            Console.WriteLine("\n");
        }

        private void ChangeCountOfAttempts()
        {
            Console.Write("Enter new count of attempts (min 1): ");

            if (int.TryParse(Console.ReadLine(), out int count) && count >= 1)
            {
                this._lenstra = new LenstraECFactorization(count, this._lenstra.SizeOfBase);
                Console.WriteLine("Change successful.\n");
            }
            else
            {
                Console.WriteLine("Invalid input. Change canceled.\n");
            }
        }

        private void ChangeSizeOfBase()
        {
            Console.Write("Enter new size of base (count of primes) (min 2): ");

            if (int.TryParse(Console.ReadLine(), out int size) && size >= 2)
            {
                this._lenstra = new LenstraECFactorization(this._lenstra.Attempts, size);
                Console.WriteLine("Change successful.\n");
            }
            else
            {
                Console.WriteLine("Invalid input. Change canceled.\n");
            }
        }

        private static void Help()
        {
            Console.WriteLine();
            Console.WriteLine("\t\t C O M M A N D S \n");
            Console.WriteLine(Enumerable.Repeat('#', 50).ToArray());
            Console.WriteLine("[chca] - change count of attempts");
            Console.WriteLine("[chsb] - change size of base (count of primes)");
            Console.WriteLine("[clear] - clear console output");
            Console.WriteLine("[exit] - close programm");
            Console.WriteLine("[help] - show help");
            Console.WriteLine("[run] - run factorization");
            Console.WriteLine("[shconf] - show current config of algorithm");
            Console.WriteLine(Enumerable.Repeat('#', 50).ToArray());
            Console.WriteLine();
        }
    }
}
