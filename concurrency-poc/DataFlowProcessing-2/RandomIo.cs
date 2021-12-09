using System;
using System.Threading.Tasks;

namespace DataFlowProcessing
{
    public class RandomIo
    {
        private Random random;

        public RandomIo()
        {
            this.random = new Random(10);
        }

        public async Task JitteredIo(int i)
        {
            var rnd = random.Next(1, 1000);
            var delay = (6 - i * rnd % 5) * 250;
            Console.WriteLine($">> Processing {i} with delay {delay}");
            await Task.Delay(delay);
            if (rnd % 5 == 0)
            {
                throw new Exception("random error");
            }
        }
    }
}