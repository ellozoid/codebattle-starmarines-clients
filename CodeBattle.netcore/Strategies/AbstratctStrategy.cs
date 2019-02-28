using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarMarines.Models;

namespace StarMarines.Strategies
{
    /// <summary>
    /// Базовый класс для всех стратегий
    /// </summary>
    public abstract class AbstratctStrategy : IStrategy
    {
        public string BotName { get; set; }

        public virtual Command OnReceived(Screen message)
        {
            throw new NotImplementedException();
        }

        public void Statistics()
        {
            Console.WriteLine($"Bot {BotName} alive");
        }
    }
}