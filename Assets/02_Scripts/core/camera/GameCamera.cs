using UnityEngine;

public class GameCamera : MonoSingleton
{
	public Camera cameraComponent;
	public Vector3 menuScenePosition;

	public static GameCamera instance
	{
		get
		{
			return GetInstance<GameCamera>();
		}
	}

	public void SetMenuScenePosition()
	{
		transform.position = menuScenePosition;
	}
}
