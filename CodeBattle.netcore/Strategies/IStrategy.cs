using System.Threading.Tasks;
using StarMarines.Models;

namespace StarMarines.Strategies
{
    //Этот интерфейс, должен наследовать класс, реализующий конкреную стратегию
    //Класс Context использует данный интерфейс для вызова конкретной стратегии
    public interface IStrategy
    {
        string BotName { get; set;}

        Command OnReceived(Screen message);

        void Statistics();
    }
}