using System;
using System.Linq;

namespace CodeBattleNetLibrary
{
	//Первая конкретная реализация-стратегия 
	public class BasicStrategy : AbstratctStrategy
	{
		public override Command OnReceived(GalaxySnapshot message)
		{
			var rand = new Random();
			Command command = new Command(); // формируем команду
			if (message.Planets.Count() > 0)
			{  // проверяем наличие планет в ответе
				var my = message.Planets.Where(x =>
				{ // выираем только свои планеты
					if (x.Owner != null)
					{
						return x.Owner.Equals(BotName, StringComparison.InvariantCultureIgnoreCase);
					}
					return false;
				}).ToList(); // берем первую
				if (my.Count() > 0)
				{ // проверяем а остались ли свои планеты?
					my.ForEach(p =>
					{
						var nei = p.Neighbours.ToList();  // находим соседей
						var droid = p.Droids / nei.Count(); // делим на всех соседей поровну
						nei.ForEach(x =>
						{
							command.actions.Add(
								new ClientAction
								{ // формируем действие. Можно сформироват ьпо одному действию с каждой вашей планеты.
									Src = p.Id,
									Dest = x,
									UnitCounts = droid //rand.Next(1, p.droids)
								});
						});
					});
				}
				else
				{
					Console.WriteLine("У вас больше нет планет.");
				}
			}
			if (message.Errors.Count() > 0) // а есть ли ошибки?
			{
				// увы есть.. покажем их.
				foreach (var error in message.Errors)
					Console.WriteLine(error);
			}
			return command;
		}
	}
}
