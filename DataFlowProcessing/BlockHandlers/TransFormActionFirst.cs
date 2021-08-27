using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataFlowProcessing.BlockHandlers
{
    class TransFormActionFirst
    {
        public async Task TransFormAction()
        {
            var block1 = new TransformBlock<int, string>(i =>
                {
                    //api call
                    return $"{i}";
                }
                , new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 5,
                    EnsureOrdered = false
                });

            var block2 = new ActionBlock<string>(s =>
            {
                //update filestatus
                Console.WriteLine(s + "***");
            });

            // var block3 = new BufferBlock<string>(new ExecutionDataflowBlockOptions { BoundedCapacity = 5 });
            // block1.LinkTo(
            //     block3
            //     , new DataflowLinkOptions{PropagateCompletion = true}
            //     , s => true
            //     );
            
            //NOTE: NullPropagation blocks are required to handle all messages that fails through Predicate of above block
            //block1.LinkTo(DataflowBlock.NullTarget<string>());

            try
            {
                using (block1.LinkTo(block2))
                {
                    Enumerable.Range(1, 10).ToList().ForEach(i => { block1.SendAsync(i); });

                    block1.Complete();
                    await block2.Completion;
                }
                // block3.Complete();
                // block3.TryReceiveAll(out IList<string> items);

                // Console.WriteLine(items.Count);
            }
            catch (AggregateException aex)
            {
                foreach (var innerExceptions in aex.Flatten().InnerExceptions)
                    Console.WriteLine(innerExceptions.Message);
            }
        }
    }
}