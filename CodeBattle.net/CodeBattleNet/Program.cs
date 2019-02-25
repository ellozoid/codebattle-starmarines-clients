using System.Configuration;
using CodeBattleNetLibrary;
using System;

namespace CodeBattleNet
{
	public class Program
	{
		public static void Main(string[] args)
		{
			GameClientStarMarines gcb = new GameClientStarMarines(ConfigurationManager.AppSettings["server"], ConfigurationManager.AppSettings["botname"], ConfigurationManager.AppSettings["token"]);
			gcb.Run(() =>
			{
				if (gcb.GetErrors().Count != 0)
				{
					foreach (var error in gcb.GetErrors())
					{
						Console.WriteLine(error);
					}
				}
				foreach (var planet in gcb.GetMyPlanets())
				{
					foreach (var neighbour in planet.Neighbours)
					{
						if (neighbour != planet.Id)
						{
							gcb.SendDrones(planet.Id, neighbour, planet.Droids / planet.Neighbours.Length);
						}
					}
				}
				gcb.SendMessage();
			});

			Console.ReadKey(true);
		}
	}
}
