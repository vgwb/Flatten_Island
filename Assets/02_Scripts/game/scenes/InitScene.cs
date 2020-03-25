using UnityEngine;

public class InitScene : MonoSingleton
{
	public GameObject initSceneRoot;

	public const string NAME = "Init";

	public LoadingPanel loadingPanel;

	public static InitScene instance
	{
		get
		{
			return GetInstance<InitScene>();
		}
	}

	protected override void OnMonoSingletonStart()
	{
		GameObjectFactory.instance.CreatePoolRootFolderObject(initSceneRoot.transform);
	}

}
