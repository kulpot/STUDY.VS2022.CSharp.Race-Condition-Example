using System;

//ref link:https://www.youtube.com/watch?v=2AYwSAiO670&list=PLRwVmtr-pp06KcX24ycbC-KkmAISAFKV5&index=16
// part 2
// ctrl+m+o -- collapse all line

class MainClass
{
    static Queue<int> numbers = new Queue<int>(); // Queue Structure -- requires knowledge in data structures
    static Random rand = new Random(987);
    const int NumThreads = 3;
    static int[] sums = new int[NumThreads]; // for total added array
    static void ProduceNumbers()    // Producing Method
    {
        for (int i = 0; i < 10; i++)
        {
            int numToEnqueue = rand.Next(10);
            //numbers.Enqueue(rand.Next(10));
            Console.WriteLine("Producing thread adding " + numToEnqueue + " to the queue.");
            numbers.Enqueue(numToEnqueue);
            Thread.Sleep(rand.Next(1000));
        }
    }
    static void SumNumbers(object threadNumber)   // Consuming Method
    {   //---------poorman's method of synchronization technique---------- needs improvements
        DateTime startTime = DateTime.Now;
        int mySum = 0;
        while ((DateTime.Now - startTime).Seconds < 11)
        {
            lock (numbers)
            {
                if (numbers.Count != 0)
                {
                    int numToSum;
                    try
                    {
                        numToSum = numbers.Dequeue();
                    }
                    catch
                    {
                        Console.WriteLine("Thread #" + threadNumber + "having an issue!");
                        throw;
                    }
                    //mySum += numbers.Dequeue();
                    //int numToSum = numbers.Dequeue();
                    mySum += numToSum;
                    Console.WriteLine("Consuming thread #"
                        + threadNumber + " adding "
                        + numToSum + " to its total sum making "
                        + numToSum + " for the thread total.");
                }
            }
        }
        sums[(int)threadNumber] = mySum;
    }
    static void Main()
    {
        var producingThread = new Thread(ProduceNumbers);
        producingThread.Start();
        Thread[] threads = new Thread[NumThreads];
        for (int i = 0; i < NumThreads; i++)
        {
            threads[i] = new Thread(SumNumbers);
            threads[i].Start(i);
        }
        for (int i = 0; i < NumThreads; i++)
            threads[i].Join();
        int totalSum = 0;
        for (int i = 0; i < NumThreads; i++)
            totalSum += sums[i];
        Console.WriteLine("Done adding. Total is " + totalSum);
    }
}