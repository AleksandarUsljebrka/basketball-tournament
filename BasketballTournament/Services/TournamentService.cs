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
					team.Group = group.Key;
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

		public static List<List<BasketballTeam>> DrawTeams(Dictionary<string, List<BasketballTeam>> groupResults)
		{
			var rankedTeams = groupResults.SelectMany(gr => gr.Value)
				.OrderByDescending(t => t.Points)
				.ThenByDescending(t => t.PointDifference)
				.ThenByDescending(t => t.ScoredPoints)
				.ToList();

			//var firstPlaceTeams = rankedTeams.Where((t, i) => i < 3).ToList();
			//var secondPlaceTeams = rankedTeams.Where((t, i) => i >= 3 && i < 6).ToList();
			//var thirdPlaceTeams = rankedTeams.Where((t, i) => i >= 6 && i < 9).ToList();

			var potD = new List<BasketballTeam> { rankedTeams[0], rankedTeams[1] };
			var potE = new List<BasketballTeam> { rankedTeams[2], rankedTeams[3] };
			var potF = new List<BasketballTeam> { rankedTeams[4], rankedTeams[5] };
			var potG = new List<BasketballTeam> { rankedTeams[6], rankedTeams[7] };

			var tempPotD = new List<BasketballTeam>(potD);
			var tempPotE = new List<BasketballTeam>(potE);
			var tempPotF = new List<BasketballTeam>(potF);
			var tempPotG = new List<BasketballTeam>(potG);

			var pots = new List<List<BasketballTeam>> { potD, potE, potF, potG };
			var random = new Random();

			var quarterFinalPairs = new List<List<BasketballTeam>>();

			// Formiranje parova za cetvrtfinale
			while (tempPotD.Any() && tempPotG.Any())
			{
				var team1 = tempPotD[random.Next(tempPotD.Count)];
				var team2 = tempPotG[random.Next(tempPotG.Count)];

				if (!team1.Group.Equals(team2.Group))
				{
					quarterFinalPairs.Add(new List<BasketballTeam> { team1, team2 });
					tempPotD.Remove(team1);
					tempPotG.Remove(team2);
				}
			}

			while (tempPotE.Any() && tempPotF.Any())
			{
				var team1 = tempPotE[random.Next(tempPotE.Count)];
				var team2 = tempPotF[random.Next(tempPotF.Count)];

				if (!team1.Group.Equals(team2.Group))
				{
					quarterFinalPairs.Add(new List<BasketballTeam> { team1, team2 });
					tempPotE.Remove(team1);
					tempPotF.Remove(team2);

				}
			}

			Console.WriteLine("\nŠeširi:");
			var potNames = new[] { "Šešir D", "Šešir E", "Šešir F", "Šešir G" };
			for (int i = 0; i < pots.Count; i++)
			{
				Console.WriteLine($"    {potNames[i]}:");
				foreach (var team in pots[i])
				{
					Console.WriteLine($"        {team.Team}");
				}
			}

			Console.WriteLine("\nEliminaciona faza:");
			foreach (var pair in quarterFinalPairs)
			{
				Console.WriteLine($"    {pair[0].Team} - {pair[1].Team}");
			}

			return quarterFinalPairs;
		}

		public static BasketballTeam SimulateMatch(BasketballTeam teamA, BasketballTeam teamB)
		{
			var random = new Random();
			var scoreA = random.Next(70, 101) + (teamB.FIBARanking - teamA.FIBARanking);
			var scoreB = random.Next(70, 101) + (teamA.FIBARanking - teamB.FIBARanking);

			teamA.ScoredPoints += scoreA;
			teamA.ConcededPoints += scoreB;
			teamB.ScoredPoints += scoreB;
			teamB.ConcededPoints += scoreA;

			Console.WriteLine($"    {teamA.Team} - {teamB.Team} ({scoreA}:{scoreB})");

			return scoreA > scoreB ? teamA : teamB;
		}

		public static void SimulateKnockoutPhase(List<List<BasketballTeam>> quarterFinalPairs)
		{
			var semiFinalParticipants = new List<BasketballTeam>();

			Console.WriteLine("\nČetvrtfinale:");
			foreach (var pair in quarterFinalPairs)
			{
				var winner = SimulateMatch(pair[0], pair[1]);
				semiFinalParticipants.Add(winner);
			}

			Console.WriteLine("\nPolufinale:");

			var finalist1 = SimulateMatch(semiFinalParticipants[0], semiFinalParticipants[1]);
			var finalist2 = SimulateMatch(semiFinalParticipants[2], semiFinalParticipants[3]);

			var thirdPlaceParticipant1 = GetLoser(finalist1, semiFinalParticipants[0], semiFinalParticipants[1]);
			var thirdPlaceParticipant2 = GetLoser(finalist2, semiFinalParticipants[2], semiFinalParticipants[3]);

            Console.WriteLine("\nMeč za treće mesto:");
            var thirdPlaceWinner = SimulateMatch(thirdPlaceParticipant1, thirdPlaceParticipant2);


			Console.WriteLine("\nFinale:");
			var champion = SimulateMatch(finalist1, finalist2);
			var secondPlace = finalist1.Team == champion.Team ? finalist2.Team : finalist1.Team;
			Console.WriteLine("\nMedalje:");
			Console.WriteLine($"    1. {champion.Team}");
			Console.WriteLine($"    2. {secondPlace}");
			Console.WriteLine($"    3. {thirdPlaceWinner.Team}");

		}

		public static BasketballTeam GetLoser(BasketballTeam winner,BasketballTeam participant1, BasketballTeam participant2)
		{
			if(participant1.Team == winner.Team)
			{
				return participant2;
			}
			else
			{
				return participant1;
			}
		}
		

		

	}
}
