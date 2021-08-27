﻿using System.Threading.Tasks;
using DataFlowProcessing.BlockHandlers;

namespace DataFlowProcessing.Handles
{
    public class TransformAsPubSubHandler : AbstractHandler
    {
        public override async Task<object> Handle(object request)
        {
            if (request as string=="1")
            {
                var transformAsPubSub = new TransformAsPubSub();
                await transformAsPubSub.TransformAsPubSubCall();
                return Task.FromResult(request);
            }
            
            return base.Handle(request);
        }
    }
}