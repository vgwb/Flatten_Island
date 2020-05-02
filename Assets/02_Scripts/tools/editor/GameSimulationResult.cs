using System.Collections.Generic;
using System.Text;
using System.IO;

public class GameSimulationResult
{
	public int run;
	public string winResult;
	public string loseResult;
	public int days;

	private string error;

	private List<GameSimulationResultRow> gameSimulationResultRows;

	public GameSimulationResult()
	{
		gameSimulationResultRows = new List<GameSimulationResultRow>();
		error = null;
	}

	public void AddRow(GameSimulationResultRow gameSimulationResultRow)
	{
		gameSimulationResultRows.Add(gameSimulationResultRow);
	}

	public void AddError(string error)
	{
		this.error = error;
	}

	public void Write(StreamWriter writer)
	{
		writer.WriteLine("Simulation N., Win Result, Lose Result, Days, Day, Phase, Growth Rate, Capacity, Patients, Money, Public Opinion, Vaccine, Chosen Advisor, Strategy, Chosen Suggestion, Chosen Option, Active Stories, Start Story, Stop Story");
		writer.WriteLine(run + "," + winResult + "," + loseResult + "," + days + ",,,,,,,,,,,,,,,");

		foreach (GameSimulationResultRow gameSimulationResultRow in gameSimulationResultRows)
		{
			writer.WriteLine(",,,," + gameSimulationResultRow.GetTextRow());
		}

		writer.WriteLine("");

		if (error != null)
		{
			writer.WriteLine("-------------------------- ERROR OCCURRED DURING SIMULATION ------------------------");
			writer.WriteLine(error);
			writer.WriteLine("");
			writer.WriteLine("");
		}
	}
}