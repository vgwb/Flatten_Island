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
	public GameObject advisorImage;
	public Text advisorTitle;
	public Text advisorDescription;

	public Sprite advisorSprite1;
	public Sprite advisorSprite2;
	public Sprite advisorSprite3;
	public Sprite advisorSprite4;
	public Sprite advisorSprite5;
	public Dictionary<Advisor, Sprite> advisorImages;

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
		InitialiseAdvisorAvatars();
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

	private void InitialiseAdvisorAvatars()
	{
		advisorImages = new Dictionary<Advisor, Sprite>();
		advisorImages.Add(Advisor.PR, advisorSprite1);
		advisorImages.Add(Advisor.Treasurer, advisorSprite2);
		advisorImages.Add(Advisor.HospitalManager, advisorSprite3);
		advisorImages.Add(Advisor.ExpertDoctor, advisorSprite4);
		advisorImages.Add(Advisor.Scientist, advisorSprite5);
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
		RenderCurrentAdvice();
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

	public void RenderCurrentAdvice() 
	{
		LocalPlayer lpn = GameManager.instance.localPlayer;
		Image advisorImageImage = advisorImage.gameObject.GetComponent<Image>();
		advisorImageImage.sprite = advisorImages[lpn.adviced.advisor];
		advisorTitle.text = lpn.adviced.title;
		advisorDescription.text = lpn.adviced.description;
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
		sceneFsm.TriggerState(MainSceneFsm.DayTransitionState);
	}

	public void RejectSuggestion()
	{
		Debug.Log("RejectSuggestion");
		GameManager.instance.localPlayer.IncreaseDayRejectSuggestion();
		sceneFsm.TriggerState(MainSceneFsm.DayTransitionState);
	}

	public void GoToMenu()
	{
		ScenesFlowManager.instance.UnloadingMainScene();
	}
}
