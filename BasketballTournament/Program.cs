using BasketballTournament.Models;
using BasketballTournament.Services;
using System.Text.Json;

internal class Program
{
	static void Main(string[] args)
	{
		var binPath = AppDomain.CurrentDomain.BaseDirectory;
		var projectPath = Directory.GetParent(binPath).Parent.Parent.Parent.FullName;

		string jsonFilePath = Path.Combine(projectPath, "Data", "groups.json");

		string jsonString = File.ReadAllText(jsonFilePath);
		var groups = JsonSerializer.Deserialize<Dictionary<string, List<BasketballTeam>>>(jsonString);

		var groupResults = TournamentService.SimulateGroupPhase(groups);

		TournamentService.DisplayGroupResults(groupResults);
	}
}