namespace CodeBattleNetLibrary
{
	public class GalaxySnapshot
	{
		public DisasterInfo[] Disasters { get; set; }
		public PlanetInfo[] Planets { get; set; }
		public Portal[] Portals { get; set; }
		public string[] Errors { get; set; }
	}
}
