namespace CodeBattleNetLibrary
{
	public class PlanetInfo
	{
		public int Id { get; set; }
		public int Droids { get; set; }
		public string Owner { get; set; }
		public string Type { get; set; }
		public int[] Neighbours { get; set; }
	}
}
