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
	public Text growthValue;
	public Text moneyValue;
	public Text dayValue;
	public Text patientsValue;


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
		RenderHudInformation(); // TODO the Hud could have a manager updating this
	}

	public void RenderHudInformation()
	{
		GameSession session = GameManager.instance.localPlayer.gameSession;
		growthValue.text = session.growthRate + "%";
		moneyValue.text = session.money + "M";
		dayValue.text = session.day + "";
		patientsValue.text = session.patients[session.day-1] + "";
	}

	public void GoToMenu()
	{
		//quitting current game session, we should have here a popup dialog	
		GameManager.instance.localPlayer.QuitGameSession();
		GameManager.instance.SavePlayer();

		ScenesFlowManager.instance.UnloadingMainScene();
	}
}
