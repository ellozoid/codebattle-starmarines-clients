using Newtonsoft.Json;

namespace CodeBattleNetLibrary
{
	public class Portal
	{
		[JsonProperty("source")] public int Source { get; set; }
		[JsonProperty("target")] public int Target { get; set; }
	}
}
