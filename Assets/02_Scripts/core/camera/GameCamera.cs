using UnityEngine;

public class GameCamera : MonoSingleton
{
	public Camera cameraComponent;

	public static GameCamera instance
	{
		get
		{
			return GetInstance<GameCamera>();
		}
	}
}
