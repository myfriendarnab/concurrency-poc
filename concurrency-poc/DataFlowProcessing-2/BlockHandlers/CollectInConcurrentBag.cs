using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataFlowProcessing.BlockHandlers
{
    public class CollectInConcurrentBag
    {
        private readonly RandomIo _randomIo;

        public CollectInConcurrentBag()
        {
            _randomIo = new RandomIo();
        }

        public async Task ActionCollectInConcurrentBag()
        {
            var collect = new ConcurrentBag<string>();
            Console.WriteLine("Dataflow hacking");
            var fred = Enumerable.Range(1, 23);

            var action = new ActionBlock<int>(async i => {
                try
                {
                    await _randomIo.JitteredIo(i);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                collect.Add($"Item {i}");
                Console.WriteLine($"<< Processed {i}");
            },
                new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 5 }
            );

            foreach (var i in fred)
            {
                try
                {
                    await action.SendAsync(i);
                }
                catch (AggregateException aex)
                {
                    foreach (var innerExceptions in aex.Flatten().InnerExceptions)
                        Console.WriteLine(innerExceptions.Message);
                }
            }

            Console.WriteLine("*** Complete");
            action.Complete();
            Console.WriteLine("*** Await Completion");
            await action.Completion;
            var itemList = collect.ToList();
            Console.Write($"Collected {itemList.Count} items: ");
            itemList.ForEach(s => Console.Write($"{s}; "));
            Console.WriteLine();
            Console.WriteLine("Finis!");
        }
    }
}