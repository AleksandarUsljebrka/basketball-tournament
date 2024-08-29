
namespace BasketballTournament.Models
{
	public class BasketballTeam
	{
		public string Team { get; set; }
		public string ISOCode { get; set; }
		public int FIBARanking { get; set; }
		public int Points { get; set; } = 0;
		public int Wins { get; set; } = 0;
		public int Losses { get; set; } = 0;
		public int ScoredPoints { get; set; } = 0;
		public int ConcededPoints { get; set; } = 0;
		public int PointDifference => ScoredPoints - ConcededPoints;
		public string Group { get; set; }
	}

}
