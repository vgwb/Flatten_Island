using UnityEngine;
using System.Collections;

public class PlatformCreator
{
	public Platform CreatePlatform()
	{
		Platform platform;
#if UNITY_ANDROID
		platform = new AndroidPlatform();
#elif UNITY_STANDALONE
		platform = new StandalonePlatform();
#else
		platform = new NullPlatform();
#endif
		return platform;
	}
}
