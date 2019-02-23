using System.Threading.Tasks;
using System.Configuration;
using CodeBattleNetLibrary;

namespace CodeBattleNet
{
	public class Program
	{
		private static GameClientStarMarines _context;
		private static IStrategy _strategy;

		public static void Main(string[] args)
		{
			/// указываем название бота, игровой сервер и секретный токен с сайта
			_context = new GameClientStarMarines(ConfigurationManager.AppSettings["botname"], ConfigurationManager.AppSettings["server"], ConfigurationManager.AppSettings["token"], bool.Parse(ConfigurationManager.AppSettings["debug"]));
			_strategy = new BasicStrategy();  //создается стратегия игры для вашего бота . Вы можете создать свою стратегию на базе базовой 
			_context.SetStrategy(_strategy);
			StartConnectionAsync().Wait();
			StopConnectionAsync();

			/////////////////////////////////////////////////////////////////////////////////////////////////////////////
			/*
			srand(time(0));
	GameClientStarMarines *gcb = new GameClientStarMarines("localhost:8080", "<login>", "<token>");
	gcb->Run([&]()
	{
		if (gcb->getErrors().size() != 0) {
			for (auto& error : gcb->getErrors()) {
				std::cout << error << '\n';
			}
		}
		for (auto& myPlanet : gcb->getMyPlanets()) {
			for (auto& neighbour : myPlanet.getNeighbours()) {
				if (neighbour != myPlanet.getId()) {
					gcb->sendDrones(myPlanet.getId(), neighbour, myPlanet.getDroids() / myPlanet.getNeighbours().size());
				}
			}
		}
		gcb->sendMessage();
	});

	getchar();   
			*/
			/////////////////////////////////////////////////////////////////////////////////////////////////////////////
		}

		public static async Task StartConnectionAsync()
		{
			await _context.StartConnectionAsync(async (message) =>
			{
				var command = _strategy.OnReceived(message); // передаем сообщение с сервера в стратегию
				await _context.SendAsync(command); // посылаем сформированный ответ на сервер
			});
		}

		public static async Task StopConnectionAsync()
		{
			await _context.StopConnectionAsync();
		}
	}
}
