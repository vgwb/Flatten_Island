using System;
using System.Collections.Generic;

public class GameSimulatorOptionSelectionBestStrategy : IGameSimulatorOptionSelectionStrategy
{
	private GameSession gameSession;
	private GameSessionSimulatorSettings simulatorSettings;
	private string logDescription;

	public void Initialize(GameSession gameSession, GameSessionSimulatorSettings simulatorSettings)
	{
		this.gameSession = gameSession;
		this.simulatorSettings = simulatorSettings;
	}

	public SuggestionOptionXmlModel ChoseOption(AdvisorXmlModel advisorXmlModel, List<SuggestionOptionXmlModel> optionsAvailable)
	{
		if (optionsAvailable.Count == 2)
		{
			SuggestionOptionXmlModel option1 = optionsAvailable[0];
			SuggestionOptionXmlModel option2 = optionsAvailable[1];

			return GetBestOption(advisorXmlModel, option1, option2);
		}
		else
		{
			UnityEngine.Debug.LogWarning("GameSimulatorBestOptionStrategy - Suggestions with a number of options != 2 are not supported! - Returning random one");
			int randomIndex = RandomGenerator.GetRandom(0, optionsAvailable.Count);
			return optionsAvailable[randomIndex];
		}
	}

	public string GetLogDescription()
	{
		return logDescription;
	}

	private SuggestionOptionXmlModel GetBestOption(AdvisorXmlModel advisorXmlModel, SuggestionOptionXmlModel option1, SuggestionOptionXmlModel option2)
	{
		logDescription = "Best Option";
		switch (advisorXmlModel.id)
		{
			case FlattenIslandGameConstants.TREASURER_ADVISOR_ID:
				if (option1.moneyModifier > option2.moneyModifier)
				{
					return option1;
				}
				else
				{
					return option2;
				}

			case FlattenIslandGameConstants.COMMANDER_ADVISOR_ID:
				if (option1.growthRateModifier < option2.growthRateModifier)
				{
					return option1;
				}
				else
				{
					return option2;
				}

			case FlattenIslandGameConstants.HOSPITAL_DOCTOR_ADVISOR_ID:
				if (option1.capacityModifier > option2.capacityModifier)
				{
					return option1;
				}
				else
				{
					return option2;
				}

			case FlattenIslandGameConstants.PR_ADVISOR_ID:
				if (option1.publicOpinionModifier > option2.publicOpinionModifier)
				{
					return option1;
				}
				else
				{
					return option2;
				}

			case FlattenIslandGameConstants.LAB_SCIENTIST_ADVISOR_ID:
				if (option1.vaccineModifier > option2.vaccineModifier)
				{
					return option1;
				}
				else
				{
					return option2;
				}
		}

		logDescription = "Best Option - Random";
		int randomIndex = RandomGenerator.GetRandom(0, 1);
		if (randomIndex == 0)
		{
			return option1;
		}
		else
		{
			return option2;
		}
	}
}