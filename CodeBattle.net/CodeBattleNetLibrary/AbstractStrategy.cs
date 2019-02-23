using System;

namespace CodeBattleNetLibrary
{
	/// <summary>
	/// Базовый класс для всех стратегий
	/// </summary>
	public abstract class AbstratctStrategy : IStrategy
	{
		public string BotName { get; set; }

		public virtual Command OnReceived(GalaxySnapshot message)
		{
			throw new NotImplementedException();
		}

		public void Statistics()
		{
			Console.WriteLine($"Bot {BotName} alive");
		}
	}
}
