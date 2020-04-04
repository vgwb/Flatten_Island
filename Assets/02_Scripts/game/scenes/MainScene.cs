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

	// Temporary. Hacky test
	private bool mustRecalculateSuggestion;
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
		mustRecalculateSuggestion = true;
	}

	protected override void OnMonoSingletonUpdate()
	{
		if (sceneFsm != null)
		{
			sceneFsm.Update();
			UpdateMainScene();
		}
	}

	public void UpdateMainScene() {
		if (mustRecalculateSuggestion) // TODO change this to FSM managed events
		{
			Debug.Log("Recalculating suggestion");
		    Image advisorImageImage = advisorImage.gameObject.GetComponent<Image>();
			advisorImageImage.sprite = advisorSprite1; // TODO hardcoded
		    Image evolutionChartImage = evolutionChart.gameObject.GetComponent<Image>();
			int[] patients = GameManager.instance.localPlayerNode.patients;
			int day = GameManager.instance.localPlayerNode.day;
			int maxPatients = LocalPlayerNode.MAX_PATIENTS;
			evolutionChartImage.sprite = ChartFactory.CreateChartSprite(patients, maxPatients, day); // TODO hardcoded
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

	public void GoToMenu()
	{
		ScenesFlowManager.instance.UnloadingMainScene();
	}
}
