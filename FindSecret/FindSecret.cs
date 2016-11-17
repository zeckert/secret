using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindAdditive
{
    //You are given a function 'secret()' that accepts a single integer parameter and returns an integer. In your favorite programming language, 
    //write a command-line program that takes one command-line argument (a number) and determines if the secret() function is additive 
    //[secret(x+y) = secret(x) + secret(y)], for all combinations x and y, where x and y are all prime numbers less than the number passed via the command-line argument.  
    //Describe how to run your examples. Please generate the list of primes without using built-in functionality.



    class Additive
    {
        static void Main()
        {
            //get input
            int n = getInput(0);
            //create new object additive and get list of primes
            List<int> primeNums = findPrimes(n);
            printPrimes(primeNums, n);
            if (isAdditive(primeNums))
            {
                Console.WriteLine("Secret is additive for all combinations of primes less than " + n);
            }
            else
            {
                Console.WriteLine("Secret is NOT additive for all combinations of primes less than" + n);
            }
            Console.ReadLine();
        }

        private static bool isAdditive(List<int> primes)
        {
            //dictioinary for the results of secret() so secret() is run only once per number
            Dictionary<long, long> secretSolutions = new Dictionary<long, long>();
            long secretXY;
            long secretX;
            long secretY;
            //if there are no primes less than the number originally input, then it is not additive for that number.
            if (primes.Count == 0)
            {
                return false;
            }
            //go through all of the primes
            for (int x = 0; x < primes.Count; x++)
            {
                //start with the current outer loop number so that there aren't duplicate comparisons
                for (int y = x; y < primes.Count; y++)
                {
                    //look for values in dictionary, if they aren't in dictionary, compute them and put them in the dictionary
                    if (!secretSolutions.TryGetValue(primes[x], out secretX))
                    {
                        secretX = secret(primes[x]);
                        secretSolutions.Add(primes[x], secretX);
                    }

                    if (!secretSolutions.TryGetValue(primes[y], out secretY))
                    {
                        secretY = secret(primes[y]);
                        secretSolutions.Add(primes[y], secretY);
                    }

                    if (!secretSolutions.TryGetValue(primes[x] + primes[y], out secretXY))
                    {
                        secretXY = secret(primes[x] + primes[y]);
                        secretSolutions.Add(primes[x] + primes[y], secretXY);
                    }
                    //check if it is additive, return false if not
                    if (secretXY != secretX + secretY)
                    {
                        return false;
                    }
                }
            }
            //since false was never returned, it must be additive
            return true;
        }

        //recursively call to get input with up to 5 attempts
        private static int getInput(int failCount)
        {
            failCount++;

            //get input from console and parse it into an integer
            Console.WriteLine("Please enter an integer greater than 0");
            var input = Console.ReadLine();
            int n;
            //if it doesn't parse to an integer, let the user know what to do again
            if (!int.TryParse(input, out n)|| n < 1)
            {
                //if the user has attempts left, prompt them
                if (failCount < 5)
                {
                    Console.WriteLine("Value must be a positive integer. '"+input+"' is not a positive integer");
                    Console.WriteLine("This was attempt number "+failCount+". "+(5-failCount)+" remaining");
                    Console.WriteLine();
                    return getInput(failCount);
                }
                //if they are out of attempts, choose 100 for theem
                else if (failCount == 5)
                {
                    Console.WriteLine("Value must be a positive integer. '" + input + "' is not a positive integer");
                    Console.WriteLine("Since you have attempted 5 times, 100 is being chosen");
                    Console.WriteLine();
                    return 100;
                }
            }
            Console.WriteLine("Your input is '" + n + "'");
            return n;
        }

        //made up secret function that is additive
        private static int secret(int num)
        {
            return num;
        }

        //print the prime numbers 10 to a line
        private static void printPrimes(List<int> primes, int n)
        {
            int count = 0;
            Console.WriteLine("Primes less than " + n + ":");
            foreach(int i in primes){
                if (count < 10)
                {
                    Console.Write(i + " ");
                    count++;
                }
                else
                {
                    count = 0;
                    Console.WriteLine();
                }
            }

            Console.WriteLine();
        }

        //function using a sieve method to find primes
        //adapted from primes function at http://stackoverflow.com/a/3035188
        private static List<int> findPrimes(int n)
        {
            bool[] sieve = new bool[n];
            List<int> primes = new List<int>();
            //if n is greater than 2, then 2 is a prime less than n
            if (n > 2)
            {
                primes.Add(2);
            }
            
            sieve = new bool[n];

            //start at 3 and count by 2 up to n/2+1
            for (int i = 3; i < (n / 2) + 1; i += 2)
            {
                //if i might be prime
                if (!sieve[i])
                {
                    //if i^2 is less than n and greater than 0 (due to overflow issues)
                    if (i * i < n && i * i > 0)
                    {
                        //start at i^2 and count by 2*i up to n, ruling out every index you hit 
                        for (int j = i * i; j < n; j = j + (i * 2))
                        {
                            sieve[j] = true;
                        }
                    }
                }
            }
            //start at 3 and count only evens, checking for primes
            for (int i = 3; i < n; i = i + 2)
            {
                if (!sieve[i])
                {
                    primes.Add(i);
                }
            }
            return primes;
        }
    }
}
