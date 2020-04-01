using UnityEngine;

public class GameCamera : MonoBehaviour
{
	public Camera cameraComponent;
	public Vector3 resetPosition = Vector3.zero;

	public void ResetPosition()
	{
		cameraComponent.transform.position = resetPosition;
	}
}
