using UnityEngine;
using Messages;

public class GameManager : MonoSingleton
{
	public Camera mainCamera { get { return Camera.main; } }
	public LocalPlayer localPlayer;

	public GameSerializer gameSerializer
	{
		get; private set;
	}

	public static GameManager instance
	{
		get
		{
			return GetInstance<GameManager>();
		}
	}

	protected override void OnMonoSingletonAwake()
	{
		gameSerializer = new PlayerPrefsGameSerializer();
	}

	protected override void OnMonoSingletonStart()
	{
		base.OnMonoSingletonStart();

		localPlayer = new LocalPlayer();
	}

	protected override void OnMonoSingletonUpdate()
	{
		EventMessageManager.instance.Update();
	}

	protected override void OnMonoSingletonDestroyed()
	{
		localPlayer = null;
		base.OnMonoSingletonDestroyed();
	}

	public void LoadLocalPlayer()
	{
		gameSerializer.ReadSaveGame(out localPlayer);
	}
}