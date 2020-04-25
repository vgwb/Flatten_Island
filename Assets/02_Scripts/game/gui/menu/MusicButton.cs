public class MusicButton : ToggleButton
{
	private void OnEnable()
	{
		SetState(GameManager.instance.localPlayer.playerSettings.musicOn);
	}

	public void OnClick()
	{
		bool state = GetState();
		SetState(!state);
		AudioManager.instance.MuteChannel(EAudioChannelType.Music, !GetState());

		GameManager.instance.localPlayer.playerSettings.musicOn = GetState();
		GameManager.instance.SavePlayer();
	}
}