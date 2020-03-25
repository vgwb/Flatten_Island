public class LoadingScene : MonoSingleton
{
	public const string NAME = "Loading";

	public LoadingPanel loadingPanel;

	public static LoadingScene instance
	{
		get
		{
			return GetInstance<LoadingScene>();
		}
	}
}
