using UnityEngine;

public class LogoScene : MonoSingleton
{
	public const string NAME = "Logo";

	public LogoSceneChef logoSceneChef;

	public static LogoScene instance
	{
		get
		{
			return GetInstance<LogoScene>();
		}
	}

	public void DestroyScene()
	{
		Destroy(gameObject);
		Resources.UnloadUnusedAssets();
	}
}
