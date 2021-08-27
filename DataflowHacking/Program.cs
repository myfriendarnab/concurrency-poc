using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataflowHacking
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // This isn't "batch" size, but its close enough
            const int BATCH_SIZE = 5;

            var collect = new ConcurrentBag<string>();
            var processedCount = 0;

            Console.WriteLine("Dataflow hacking");

            var fred = Enumerable.Range(1, 23);

            //            var transformer = new TransformBlock<int, string>(i => i.ToString());
            var action = new ActionBlock<int>(async i =>
                {
                    var delay = (6 - (i % 5)) * 250;
                    Console.WriteLine($">> Processing {i} with delay {delay}");

                    await Task.Delay(delay);
                    collect.Add($"Item {i}");
                    Console.WriteLine($"<< Processed {i}");
                    var total = Interlocked.Increment(ref processedCount);
                    if (total % 5 == 0)
                    {
                        Console.WriteLine($"=== Updating total: {total}");
                        await Task.Delay(333);  // Simulate writing to database and sending notification
                        Console.WriteLine($"=== Total updated: {total}");
                    }
                },
                new ExecutionDataflowBlockOptions() {  MaxDegreeOfParallelism = BATCH_SIZE }
            );

            var sw = Stopwatch.StartNew();

            foreach(var i in fred)
            {
                await action.SendAsync(i);
            }

            Console.WriteLine("*** Complete");
            action.Complete();

            Console.WriteLine("*** Await Completion");
            await action.Completion;

            var itemList = collect.ToList();

            sw.Stop();

            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}ms");

            Console.Write($"Collected {itemList.Count} items: ");
            itemList.ForEach(s => Console.Write($"{s}; "));
            Console.WriteLine();

            Console.WriteLine("=== In real world have to update status here too");

            Console.WriteLine("Finis!");
        }
    }
}
