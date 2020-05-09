using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class TutorialMenu : MonoSingleton
{
	public Image advisorPortrait;
	public string tutorialNextDayTextLocalizationId = "Tutorial_NextDay_Text";

	public GameObject evolutionChart;
	public GameObject chartCapacityPanel;
	public GameObject chartGrowthPanel;
	public GameObject patientsPanelStraight;

	public float blinkingRate = 0.4f;
	public float blinkingDuration = 2.5f;
	public AudioClip blinkingAudioClip;

	private TutorialDialog tutorialDialog;

	public static TutorialMenu instance
	{
		get
		{
			return GetInstance<TutorialMenu>();
		}
	}

	private void OnEnable()
	{
		EventMessageHandler tutorialDialogExitCompletedMessageHandler = new EventMessageHandler(this, OnTutorialDialogExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(TutorialDialogExitCompletedEvent).Name, tutorialDialogExitCompletedMessageHandler);
	}

	private void OnDisable()
	{
		EventMessageManager.instance.RemoveHandler(typeof(TutorialDialogExitCompletedEvent).Name, this);
	}

	public void OnTutorialDialogExitCompleted(EventMessage eventMessage)
	{
		TutorialDialogExitCompletedEvent tutorialDialogExitCompletedEvent = eventMessage.eventObject as TutorialDialogExitCompletedEvent;

		if (tutorialDialogExitCompletedEvent.tutorialDialog.advisorXmlModel != null)
		{
			if (tutorialDialog.advisorXmlModel == tutorialDialogExitCompletedEvent.tutorialDialog.advisorXmlModel)
			{
				AllAdvisorsExitCompletedEvent allAdvisorsExitCompletedEvent = AllAdvisorsExitCompletedEvent.CreateInstance(tutorialDialog.advisorXmlModel);
				EventMessage allAdvisorsExitCompletedEventMessage = new EventMessage(this, allAdvisorsExitCompletedEvent);
				allAdvisorsExitCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
				EventMessageManager.instance.QueueMessage(allAdvisorsExitCompletedEventMessage);

				tutorialDialog.Release();
			}
		}
	}

	public void ShowAdvisorPresentation(AdvisorXmlModel advisorXmlModel)
	{
		ShowAdvisorHud(advisorXmlModel);

		TryShowAdvisorPortrait(advisorXmlModel);

		tutorialDialog = TutorialDialog.ShowAdvisorPresentation(advisorXmlModel, gameObject.transform);
	}

	private void ShowAdvisorHud(AdvisorXmlModel advisorXmlModel)
	{
		switch (advisorXmlModel.id)
		{
			case FlattenIslandGameConstants.PR_ADVISOR_ID:
				ShowPublicOpinionPanel(true);
				break;
			case FlattenIslandGameConstants.TREASURER_ADVISOR_ID:
				ShowMoneyPanel(true);
				break;
			case FlattenIslandGameConstants.LAB_SCIENTIST_ADVISOR_ID:
				ShowVaccineBar(true);
				break;
			case FlattenIslandGameConstants.HOSPITAL_DOCTOR_ADVISOR_ID:
				ShowEvolutionChart(true);
				ShowChartCapacityPanel(true);
				ShowChartGrowthPanel(false);
				ChartManager.instance.UpdateChart(0.1f);
				ShowPatientsStraightPanel(true);
				break;
			case FlattenIslandGameConstants.COMMANDER_ADVISOR_ID:
				ShowChartGrowthPanel(true);
				ChartManager.instance.RestartCurrentDayChartAnimation();
				break;
		}
	}

	public void ShowNextDayTipDialog()
	{
		tutorialDialog = TutorialDialog.ShowTutorialTip(tutorialNextDayTextLocalizationId, gameObject.transform);
	}

	private void TryShowAdvisorPortrait(AdvisorXmlModel advisorXmlModel)
	{
		if (!string.IsNullOrEmpty(advisorXmlModel.portraitFullSprite))
		{
			Sprite advisorSprite = Resources.Load<Sprite>(advisorXmlModel.portraitFullSprite);
			advisorPortrait.overrideSprite = advisorSprite;
			advisorPortrait.gameObject.SetActive(true);
		}
		else
		{
			advisorPortrait.gameObject.SetActive(false);
		}
	}

	public void HideAdvisorPortrait()
	{
		advisorPortrait.gameObject.SetActive(false);
	}

	public void ShowEvolutionChart(bool shown)
	{
		evolutionChart.SetActive(shown);
	}

	public void ShowChartGrowthPanel(bool shown)
	{
		chartGrowthPanel.SetActive(shown);
		EnableBlinkCanvasEffect(chartGrowthPanel, shown);
	}

	public void ShowChartCapacityPanel(bool shown)
	{
		chartCapacityPanel.SetActive(shown);
		EnableBlinkCanvasEffect(chartCapacityPanel, shown);
	}

	public void ShowPatientsStraightPanel(bool shown)
	{
		patientsPanelStraight.SetActive(shown);
		EnableBlinkCanvasEffect(patientsPanelStraight, shown);
	}

	public void ShowVaccineBar(bool shown)
	{
		Hud.instance.vaccineBar.gameObject.SetActive(shown);
		EnableBlinkCanvasEffect(Hud.instance.vaccineBar.gameObject, shown);
	}

	public void ShowDayPanel(bool shown)
	{
		Hud.instance.dayPanel.gameObject.SetActive(shown);
		EnableBlinkCanvasEffect(Hud.instance.dayPanel.gameObject, shown);
	}

	public void ShowMoneyPanel(bool shown)
	{
		Hud.instance.moneyPanel.gameObject.SetActive(shown);
		EnableBlinkCanvasEffect(Hud.instance.moneyPanel.gameObject, shown);
	}

	public void ShowPublicOpinionPanel(bool shown)
	{
		Hud.instance.publicOpinionPanel.gameObject.SetActive(shown);
		EnableBlinkCanvasEffect(Hud.instance.publicOpinionPanel.gameObject, shown);
	}

	private void EnableBlinkCanvasEffect(GameObject gameObject, bool enabled)
	{
		Canvas gameObjectCanvas = gameObject.GetComponent<Canvas>();
		if (gameObjectCanvas != null)
		{
			UIBlinkCanvas uiBlinkCanvas = gameObjectCanvas.GetComponent<UIBlinkCanvas>();
			if (uiBlinkCanvas == null)
			{
				uiBlinkCanvas = gameObject.AddComponent<UIBlinkCanvas>();
				uiBlinkCanvas.blinkingRate = blinkingRate;
				uiBlinkCanvas.blinkingDuration = blinkingDuration;
				uiBlinkCanvas.blinkingAudioClip = blinkingAudioClip;
			}

			uiBlinkCanvas.enabled = enabled;
		}
	}
}
