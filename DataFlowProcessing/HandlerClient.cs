using System;

namespace DataFlowProcessing
{
    public class HandlerClient
    {
        public static void Call(AbstractHandler handler, int choice)
        {
            Console.WriteLine($"choice is {choice}");
            var result = handler.Handle(choice.ToString());
            Console.WriteLine($"result is:{result}");
        }
    }
}