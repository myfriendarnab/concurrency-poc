using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using DataFlowProcessing.BlockHandlers;
using DataFlowProcessing.Handles;

namespace DataFlowProcessing
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var prg = new Program();

            var transformActionFirstHandler = new TransFormActionFirstHandler();
            var collectInConcurrentBagHandler = new CollectInConcurrentBagHandler();
            var transformAsPubSubHandler = new TransformAsPubSubHandler();

            transformAsPubSubHandler
                .Next(collectInConcurrentBagHandler)
                .Next(transformActionFirstHandler);

            //await prg.TransFormActionFirstHandler();
            //await prg.CollectInConcurrentBagHandler();
            //await prg.TransformAsPubSubHandle();

            HandlerClient.Call(transformAsPubSubHandler, 1);

            Console.ReadLine();
        }

        private async Task TransformAsPubSubHandle()
        {
            var transformAsPubSub = new TransformAsPubSub();
            await transformAsPubSub.TransformAsPubSubCall();
        }


        private async Task TransFormActionFirstHandler()
        {
            var transformActionFirst = new TransFormActionFirst();
            await transformActionFirst.TransFormAction();
        }

        private async Task CollectInConcurrentBagHandler()
        {
            var collectInConcurrentBag = new CollectInConcurrentBag();
            await collectInConcurrentBag.ActionCollectInConcurrentBag();
        }
    }
}
