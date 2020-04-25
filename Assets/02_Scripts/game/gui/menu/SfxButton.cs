public class SfxButton : ToggleButton
{
	private void OnEnable()
	{
		SetState(GameManager.instance.localPlayer.playerSettings.sfxOn);
	}

	public void OnClick()
	{
		bool state = GetState();
		SetState(!state);
		AudioManager.instance.MuteChannel(EAudioChannelType.Sfx, !GetState());

		GameManager.instance.localPlayer.playerSettings.sfxOn = GetState();
		GameManager.instance.SavePlayer();
	}
}