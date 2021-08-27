using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataFlowProcessing.BlockHandlers
{
    public class TransformAsPubSub
    {
        public async Task TransformAsPubSubCall()
        {
            var fred = Enumerable.Range(1, 7);
            var transformer = new TransformBlock<int, string>(i => i.ToString());
            foreach (var i in fred)
            {
                await transformer.SendAsync(i);
            }
            transformer.Complete();
            Console.WriteLine(transformer.OutputCount);
            // Fails if here            // await transformer.Completion;            
            while (await transformer.OutputAvailableAsync())
            {
                var freda = await transformer.ReceiveAsync();
                Console.WriteLine($"Freda: {freda}");
            }
            // Succeeds here            
            await transformer.Completion;
            Console.WriteLine("Finis!");
        }
    }
}