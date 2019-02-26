using Newtonsoft.Json;

namespace CodeBattleNetLibrary
{
	public class GalaxySnapshot
	{
		[JsonProperty("disasters")] public DisasterInfo[] Disasters { get; set; }
		[JsonProperty("planets")] public PlanetInfo[] Planets { get; set; }
		[JsonProperty("portals")] public Portal[] Portals { get; set; }
		[JsonProperty("errors")] public string[] Errors { get; set; }
	}
}
