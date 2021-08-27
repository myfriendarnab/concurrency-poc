using System.Threading.Tasks;

namespace a
{
    public interface IHandler
    {
        IHandler Next(IHandler handler);
        Task<object> Handle(object request);
    }
}