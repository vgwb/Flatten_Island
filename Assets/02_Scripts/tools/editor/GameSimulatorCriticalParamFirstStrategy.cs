﻿using System;
using System.Collections.Generic;

public class GameSimulatorCriticalParamFirstStrategy : IGameSimulatorStrategy
{
	private GameSession gameSession;
	private string logDescription;

	public void Initialize(GameSession gameSession)
	{
		this.gameSession = gameSession;
	}

	public AdvisorXmlModel ChoseAdvisor(List<AdvisorXmlModel> advisorsAvailable)
	{
		GameSessionXmlModel gameSessionXmlModel = gameSession.gameSessionXmlModel;

		AdvisorXmlModel commanderAdvisor = advisorsAvailable.Find((advisor) => advisor.id == FlattenIslandGameConstants.COMMANDER_ADVISOR_ID);

		float currentCapacity = gameSession.capacity;
		int patients = gameSession.patients[gameSession.day - 1];
		float capacityUsage = patients / currentCapacity;
		bool isInWarningzone = capacityUsage > gameSession.gameSessionXmlModel.capacityWarningThreshold;
		if (isInWarningzone)
		{
			AdvisorXmlModel hospitalAdvisor = advisorsAvailable.Find((advisor) => advisor.id == FlattenIslandGameConstants.HOSPITAL_DOCTOR_ADVISOR_ID);

			if (hospitalAdvisor != null && commanderAdvisor != null)
			{
				int random = RandomGenerator.GetRandom(0, 1);
				if (random == 0)
				{
					logDescription = "Critical - CAPACITY";
					return hospitalAdvisor;
				}
				else
				{
					logDescription = "Critical - GROWTH";
					return commanderAdvisor;
				}
			}
			else if (hospitalAdvisor != null)
			{
				logDescription = "Critical - CAPACITY";
				return hospitalAdvisor;
			}
			else if (commanderAdvisor!= null)
			{
				logDescription = "Critical - GROWTH";
				return commanderAdvisor;
			}
		}

		if (gameSession.money < gameSessionXmlModel.moneyWarningThreshold)
		{
			AdvisorXmlModel treasurerAdvisor = advisorsAvailable.Find((advisor) => advisor.id == FlattenIslandGameConstants.TREASURER_ADVISOR_ID);
			if (treasurerAdvisor != null)
			{
				logDescription = "Critical - MONEY";
				return treasurerAdvisor;
			}
		}

		if (gameSession.publicOpinion < gameSessionXmlModel.publicOpinionWarningThreshold)
		{
			AdvisorXmlModel prAdvisor = advisorsAvailable.Find((advisor) => advisor.id == FlattenIslandGameConstants.PR_ADVISOR_ID);
			if (prAdvisor != null)
			{
				logDescription = "Critical - PUBLIC OPINION";
				return prAdvisor;
			}
		}

		if (commanderAdvisor != null)
		{
			logDescription = "No Critical - Commander";
			return commanderAdvisor;
		}


		logDescription = "No Critical - Random";
		int randomAdvisorIndex = RandomGenerator.GetRandom(0, advisorsAvailable.Count);
		return advisorsAvailable[randomAdvisorIndex];
	}

	public string GetLogDescription()
	{
		return logDescription;
	}
}