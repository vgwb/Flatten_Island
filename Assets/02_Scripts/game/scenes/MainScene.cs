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
	public GameObject evolutionChart;
	public Text growthValue;
	public Text moneyValue;
	public Text dayValue;


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
		GameManager.instance.localPlayer.Init(); // initial state, including first suggestion
		RenderCurrentState();
	}

	protected override void OnMonoSingletonUpdate()
	{
		if (sceneFsm != null)
		{
			sceneFsm.Update();
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

	public void RenderCurrentState()
	{
		RenderHudInformation();
		RenderChart();
	}

	public void RenderHudInformation()
	{
		LocalPlayer lpn = GameManager.instance.localPlayer;
		growthValue.text = lpn.growthRate + "%";
		moneyValue.text = lpn.money + "M";
		dayValue.text = lpn.day + "";
	}

	public void RenderChart()
	{
		LocalPlayer lpn = GameManager.instance.localPlayer;
		Image evolutionChartImage = evolutionChart.gameObject.GetComponent<Image>();
		evolutionChartImage.sprite = ChartFactory.CreateChartSprite(lpn.patients, LocalPlayer.MAX_PATIENTS, lpn.day);
	}

	public void StartDayTransition()
	{
		Debug.Log("StartDayTransition");
		RenderCurrentState();
	}

	public void AnimateDayTransition()
	{
		// TODO
	}

	public void AcceptSuggestion()
	{
		Debug.Log("AcceptSuggestion");
		GameManager.instance.localPlayer.IncreaseDayAcceptSuggestion();
	}

	public void RejectSuggestion()
	{
		Debug.Log("RejectSuggestion");
		GameManager.instance.localPlayer.IncreaseDayRejectSuggestion();
	}

	public void GoToMenu()
	{
		ScenesFlowManager.instance.UnloadingMainScene();
	}
}
