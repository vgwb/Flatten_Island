using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProtoTurtle.BitmapDrawing;

public class MainScene : MonoSingleton
{
	public const string NAME = "Main";

	private MainSceneFsm sceneFsm;
	public GameObject advisorImage;
	public GameObject evolutionChart;
	public Text growthValue;
	public Text moneyValue;
	public Text dayValue;

	// Temporary. Hacky test
	public Sprite advisorSprite1;
	public Sprite advisorSprite2;

	public static MainScene instance
	{
		get
		{
			return GetInstance<MainScene>();
		}
	}

	public void Init()
	{
		sceneFsm = new MainSceneFsm(this);
		sceneFsm.StartFsm();
	}

	protected override void OnMonoSingletonUpdate()
	{
		if (sceneFsm != null)
		{
			sceneFsm.Update();
			RenderCurrentState();
		}
	}

	public void DestroyScene()
	{
		Hud.instance.DestroyHud();
		Destroy(gameObject);
		Resources.UnloadUnusedAssets();
	}

	public void SetupScene()
	{
		InitScene.instance.loadingPanel.Exit(OnLoadingPanelExitCompleted);
	}

	public void UnsetupScene()
	{
		Hud.instance.Unsetup();
	}

	private void OnLoadingPanelExitCompleted()
	{
		Hud.instance.Setup();
	}

	public void RenderCurrentState() // TODO refactor and extract formatting responsibilities
	{
		Debug.Log("StartDayTransition. Recalculating suggestion");
		LocalPlayer lpn = GameManager.instance.localPlayer;
		growthValue.text = lpn.growthRate + "%";
		moneyValue.text = lpn.money + "M";
		dayValue.text = lpn.day + "";
		Image advisorImageImage = advisorImage.gameObject.GetComponent<Image>();
		advisorImageImage.sprite = advisorSprite1; // TODO hardcoded
		Image evolutionChartImage = evolutionChart.gameObject.GetComponent<Image>();
		evolutionChartImage.sprite = ChartFactory.CreateChartSprite(lpn.patients, LocalPlayer.MAX_PATIENTS, lpn.day);
	}

	public void StartDayTransition()
	{
		RenderCurrentState();
	}

	public void AnimateDayTransition()
	{
		// TODO
	}

	public void AcceptSuggestion()
	{
		GameManager.instance.localPlayer.IncreaseDayWithSuggestion();
		Debug.Log("AcceptSuggestion");
		sceneFsm.TriggerState(MainSceneFsm.DayTransitionState);
	}

	public void RejectSuggestion()
	{
		GameManager.instance.localPlayer.IncreaseDayWithoutMeasures();
		Debug.Log("RejectSuggestion");
		sceneFsm.TriggerState(MainSceneFsm.DayTransitionState);
	}

	public void GoToMenu()
	{
		ScenesFlowManager.instance.UnloadingMainScene();
	}
}
