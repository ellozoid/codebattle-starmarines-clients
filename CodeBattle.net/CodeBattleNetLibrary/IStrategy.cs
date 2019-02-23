namespace CodeBattleNetLibrary
{
	//Этот интерфейс, должен наследовать класс, реализующий конкреную стратегию
	//Класс Context использует данный интерфейс для вызова конкретной стратегии
	public interface IStrategy
	{
		string BotName { get; set; }
		Command OnReceived(GalaxySnapshot message);

		void Statistics();
	}
}
