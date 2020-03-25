using UnityEngine;
using Messages;

public class ScenesFlowManager : MonoSingleton
{
	public AsyncOperation loadingAsyncOperation = null;
	public AsyncOperation unloadingAsyncOperation = null;
	private ScenesFlowFsm scenesFlowFsm = null;

	private void Start()
	{
		Application.backgroundLoadingPriority = ThreadPriority.Low;

		scenesFlowFsm = new ScenesFlowFsm(this);
		scenesFlowFsm.StartFsm();
	}

	public static ScenesFlowManager instance
	{
		get
		{
			return GetInstance<ScenesFlowManager>();
		}
	}

	protected override void OnMonoSingletonUpdate()
	{
		scenesFlowFsm.Update();
	}

	protected override void OnMonoSingletonAwake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	public void UnloadingMenuScene()
	{
		scenesFlowFsm.TriggerState(ScenesFlowFsm.UnloadingMenuSceneState);
	}

	public void UnloadingMainScene()
	{
		scenesFlowFsm.TriggerState(ScenesFlowFsm.UnloadingMainSceneState);
	}
}
