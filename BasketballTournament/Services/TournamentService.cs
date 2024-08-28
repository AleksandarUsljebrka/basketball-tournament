using BasketballTournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballTournament.Services
{
	public class TournamentService
	{

		public static Dictionary<string, List<BasketballTeam>> SimulateGroupPhase(Dictionary<string, List<BasketballTeam>> groups)
		{
			var random = new Random();
			var groupResults = new Dictionary<string, List<BasketballTeam>>();

			foreach (var group in groups)
			{
				Console.WriteLine($"\nGrupa {group.Key}:");

				foreach (var team in group.Value)
				{
					team.Points = 0;
					team.Wins = 0;
					team.Losses = 0;
					team.ScoredPoints = 0;
					team.ConcededPoints = 0;
				}

				// Simulacija utakmica izmedju svih parova timova u grupi
				for (int i = 0; i < group.Value.Count; i++)
				{
					for (int j = i + 1; j < group.Value.Count; j++)
					{
						var teamA = group.Value[i];
						var teamB = group.Value[j];

						// random odluka da li neki tim predaje
						bool teamASurrendered = random.Next(0, 100) < 5; // 5% sanse za predaju
						bool teamBSurrendered = random.Next(0, 100) < 5; 

						if (teamASurrendered || teamBSurrendered)
						{
							if (teamASurrendered && !teamBSurrendered)
							{
								Console.WriteLine($"    {teamA.Team} - {teamB.Team} (predaja)");
								teamB.Wins++;
								teamB.Points += 2;
								teamA.Losses++;
							}
							else if (!teamASurrendered && teamBSurrendered)
							{
								Console.WriteLine($"    {teamA.Team} - {teamB.Team} (predaja)");
								teamA.Wins++;
								teamA.Points += 2;
								teamB.Losses++;
							}
							else
							{
								Console.WriteLine($"    {teamA.Team}  -  {teamB.Team} (nerealizovano)");
								// Ako obojica predaju, nijedan tim ne dobija bodove
							}
						}
						else
						{
							// Generisanje rezultata utakmice ako nijedan tim ne predaje
							var scoreA = random.Next(60, 101) + (teamB.FIBARanking - teamA.FIBARanking);
							var scoreB = random.Next(60, 101) + (teamA.FIBARanking - teamB.FIBARanking);

							teamA.ScoredPoints += scoreA;
							teamA.ConcededPoints += scoreB;
							teamB.ScoredPoints += scoreB;
							teamB.ConcededPoints += scoreA;

							if (scoreA > scoreB)
							{
								teamA.Wins++;
								teamA.Points += 2;
								teamB.Points += 1;
							}
							else
							{
								teamB.Wins++;
								teamB.Points += 2;
								teamA.Points += 1;
							}

							Console.WriteLine($"    {teamA.Team} - {teamB.Team} ({scoreA}:{scoreB})");
						}
					}
				}

				groupResults[group.Key] = group.Value;
			}

			return groupResults;
		}
		public static void DisplayGroupResults(Dictionary<string, List<BasketballTeam>> groupResults)
		{
			Console.WriteLine("\nKonačan plasman u grupama:");

			foreach (var group in groupResults)
			{
				Console.WriteLine($"    Grupa {group.Key}:");

				var rankedTeams = group.Value
					.OrderByDescending(t => t.Points)
					.ThenByDescending(t => t.PointDifference)
					.ThenByDescending(t => t.ScoredPoints)
					.ToList();

				int rank = 1;
				foreach (var team in rankedTeams)
				{
					Console.WriteLine($"        {rank}. {team.Team} - {team.Wins}/{team.Losses} - Bodovi: {team.Points} - Koš razlika: {team.PointDifference}");
					rank++;
				}
			}
		}

	}
}
