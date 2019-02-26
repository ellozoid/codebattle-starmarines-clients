using Newtonsoft.Json;

namespace CodeBattleNetLibrary
{
	public class DisasterInfo
	{
		[JsonProperty("type")] public string Type { get; set; }
		[JsonProperty("sourcePlanetId")] public int SourcePlanetId { get; set; }
		[JsonProperty("targetPlanetId")] public int TargetPlanetId { get; set; }
		[JsonProperty("planetId")] public int PlanetId { get; set; }
	}
}
