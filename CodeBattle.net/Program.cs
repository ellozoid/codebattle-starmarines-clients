using System;
using System.Threading.Tasks;
using StarMarines;
using StarMarines.Strategies;

public class Program
{
    private static Context _context;
    private static IStrategy _strategy;

    public static void Main(string[] args)
    {
        /// указываем название бота, игровой сервер и секретный токен с сайта
        _context = new Context("<botName>", "<game server>", "<token>");
        _strategy = new BasicStrategy();  //создается стратегия игры для вашего бота . Вы можете создать свою стратегию на базе базовой 
        _context.SetStrategy(_strategy);
        StartConnectionAsync().Wait();
        StopConnectionAsync(); 
    }

    public static async Task StartConnectionAsync()
    {
        await _context.StartConnectionAsync(async (message) => {
            var command = _strategy.OnReceived(message); // передаем сообщение с сервера в стратегию
            await _context.SendAsync(command); // посылаем сформированный ответ на сервер
        });
    }

    public static async Task StopConnectionAsync()
    {
        await _context.StopConnectionAsync();
    }
}
