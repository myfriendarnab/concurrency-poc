using System;
using System.Threading.Tasks;
using a;

namespace DataFlowProcessing
{
    public abstract class AbstractHandler : IHandler
    {
        private IHandler _nexHandler;
        public IHandler Next(IHandler handler)
        {
            _nexHandler = handler;
            return _nexHandler;
        }

        public virtual Task<object> Handle(object request) => _nexHandler?.Handle(request);
    }
}