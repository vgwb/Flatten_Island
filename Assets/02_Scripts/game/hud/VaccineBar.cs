using UnityEngine;

public class VaccineBar : MonoBehaviour
{
	private const float BORDER_MOVEMENT_FACTOR = 100f;

	public Transform barTransform;
	public Canvas barSpriteCanvas;
	public Transform rightBorderPivotTransform;
	public Transform diagonalBorderSpriteTransform;

	private Vector3 rightBorderPivotStartPosition;

	private void Start()
	{
		rightBorderPivotStartPosition = rightBorderPivotTransform.position;
		diagonalBorderSpriteTransform.position = rightBorderPivotStartPosition;
	}

	public void UpdateBar(float percentage)
	{
		barTransform.localScale = new Vector3(percentage, 1.0f);
		diagonalBorderSpriteTransform.position = rightBorderPivotTransform.position;
		barSpriteCanvas.sortingOrder = barSpriteCanvas.sortingOrder; //forcing UnityEngine.UI update
	}
}
