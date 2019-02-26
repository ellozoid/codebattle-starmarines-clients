using Newtonsoft.Json;

namespace CodeBattleNetLibrary
{
	public class ClientAction
	{
		[JsonProperty("from")] public int Src { get; set; }
		[JsonProperty("to")] public int Dest { get; set; }
		[JsonProperty("unitsCount")] public int UnitCounts { get; set; }
	}
}
