using System;
using System.Threading.Tasks;
using DataFlowProcessing.BlockHandlers;

namespace DataFlowProcessing.Handles
{
    public class TransFormActionFirstHandler : AbstractHandler
    {
        public override async Task<object> Handle(object request)
        {
            if (request as string == "3")
            {
                var transformActionFirst = new TransFormActionFirst();
                Console.WriteLine($"Inside {nameof(transformActionFirst)}  processing");
                await transformActionFirst.TransFormAction();
                return Task.FromResult(request);
            }

            return base.Handle(request);
        }
    }
}