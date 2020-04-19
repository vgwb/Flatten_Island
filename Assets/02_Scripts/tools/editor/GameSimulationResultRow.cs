using System.Text;

public struct GameSimulationResultRow
{
	public int day;
	public int phase;
	public int growthRate;
	public int capacity;
	public int patients;
	public int money;
	public int publicOpinion;
	public int vaccine;
	public int chosenAdvisorId;
	public int chosenSuggestionId;
	public int chosenOptionId;
	public int startStoryId;
	public int stopStoryId;
	public GameStoryXmlModel[] activeStoriesId;

	public string GetTextRow()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(day + ",");
		stringBuilder.Append(phase + ",");
		stringBuilder.Append(growthRate + ",");
		stringBuilder.Append(capacity + ",");
		stringBuilder.Append(patients + ",");
		stringBuilder.Append(money + ",");
		stringBuilder.Append(publicOpinion + ",");
		stringBuilder.Append(vaccine + ",");
		stringBuilder.Append(chosenAdvisorId + ",");
		stringBuilder.Append(chosenSuggestionId + ",");
		stringBuilder.Append(chosenOptionId + ",");

		if (activeStoriesId.Length > 0)
		{
			stringBuilder.Append("[");
			for (int i = 0; i < activeStoriesId.Length; i++)
			{
				stringBuilder.Append(activeStoriesId[i].id);
				if (i < activeStoriesId.Length - 1)
				{
					stringBuilder.Append(";");
				}
			}
			stringBuilder.Append("],");
		}
		else
		{
			stringBuilder.Append(",");
		}

		stringBuilder.Append(startStoryId + ",");
		stringBuilder.Append(stopStoryId);

		return stringBuilder.ToString();
	}
}
