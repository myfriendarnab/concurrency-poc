﻿using System.Threading.Tasks;
using DataFlowProcessing.BlockHandlers;

namespace DataFlowProcessing.Handles
{
    public class CollectInConcurrentBagHandler : AbstractHandler
    {
        public override async Task<object> Handle(object request)
        {
            if (request as string == "3")
            {
                var collectInConcurrentBag = new CollectInConcurrentBag();
                await collectInConcurrentBag.ActionCollectInConcurrentBag();
                return Task.FromResult(request);
            }

            return base.Handle(request);
        }
    }
}