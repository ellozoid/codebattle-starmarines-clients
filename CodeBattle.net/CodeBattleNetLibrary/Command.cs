using System.Collections.Generic;

namespace CodeBattleNetLibrary
{
	public class Command
	{
		public string token;
		public IList<ClientAction> actions = new List<ClientAction>();
	}
}
