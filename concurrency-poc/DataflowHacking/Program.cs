using System;
using System.Collections.Generic;
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
            const int BATCH_SIZE = 15;
            const int NOTIFY_EVERY = BATCH_SIZE; // Because of the timings, you can get the notify blocking if this is too small relative to batch size

            var processedCount = 0;

            Console.WriteLine("Dataflow hacking");

            var fred = Enumerable.Range(1, 123);

            var transformer = new TransformBlock<int, string>(async i =>
            {
                var delay = (6 - (i % 5)) * 250;
                Console.WriteLine($">> Processing {i} with delay {delay}");

                await Task.Delay(delay);

                // Result should come from the called task
                var result = $"Item {i}";

                Console.WriteLine($"<< Processed {i}");

                return result;
            },
                new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = BATCH_SIZE }
            );

            var notifyTransformer = new TransformBlock<string, string>(async s =>
            {
                Console.WriteLine("Notify");
                var total = Interlocked.Increment(ref processedCount);

                if (total % NOTIFY_EVERY == 0)
                {
                    Console.WriteLine($"=== Updating total: {total}");
                    await Task.Delay(333);  // Simulate writing to database and sending notification
                    Console.WriteLine($"=== Total updated: {total}");
                }

                return s;
            }
            );

            transformer.LinkTo(notifyTransformer, new DataflowLinkOptions() { PropagateCompletion = true });
            var sw = Stopwatch.StartNew();

            foreach (var i in fred)
            {
                await transformer.SendAsync(i);
            }

            Console.WriteLine("*** Complete");
            transformer.Complete();

            var itemList = new List<string>();
            while (await notifyTransformer.OutputAvailableAsync())
            {
                itemList.Add(await notifyTransformer.ReceiveAsync());
            }

            Console.WriteLine("*** Await Completion");
            await notifyTransformer.Completion;

            // var itemList = collect.ToList();

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
