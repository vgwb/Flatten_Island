using UnityEngine;
using Messages;

public class GameManager : MonoSingleton
{
	public Camera mainCamera { get { return Camera.main; } }
	public LocalPlayerNode localPlayerNode { get; private set; }

	public static GameManager instance
	{
		get
		{
			return GetInstance<GameManager>();
		}
	}

	protected override void OnMonoSingletonStart()
	{
		base.OnMonoSingletonStart();

		localPlayerNode = new LocalPlayerNode();
	}

	protected override void OnMonoSingletonUpdate()
	{
		EventMessageManager.instance.Update();
	}

	protected override void OnMonoSingletonDestroyed()
	{
		localPlayerNode = null;
		base.OnMonoSingletonDestroyed();
	}
}