public class TutorialButton : ToggleButton
{
	private void OnEnable()
	{
		SetState(GameManager.instance.localPlayer.playerSettings.showTutorial);
	}

	public void OnClick()
	{
		bool state = GetState();
		SetState(!state);

		GameManager.instance.localPlayer.playerSettings.showTutorial = GetState();
		GameManager.instance.SavePlayer();
	}
}