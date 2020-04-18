using UnityEngine;
using UnityEngine.UI;
using System;

public class ViewportUtils 
{
	public static Vector2 MeasureViewport(GameObject canvas)
	{
		CanvasScaler scaler = canvas.gameObject.GetComponent<CanvasScaler>();
		
		float viewportX = scaler.referenceResolution.x;
		float viewportY = scaler.referenceResolution.y;
		
		// Calculate if it's going to scale in X or Y
		float scaleX = scaler.referenceResolution.x / Screen.width;
		float scaleY = scaler.referenceResolution.y / Screen.height;

		if (scaleX > scaleY) // Will expand on Y (is narrower than the reference)
		{
			viewportY *= scaleX / scaleY; 
		}
		else // Will expand on X (is wider than the reference)
		{
			viewportX *= scaleY / scaleX; 
		}

	 	Debug.Log("viewport: " + viewportX + " , " + viewportY);
		return new Vector2(viewportX, viewportY);
	}
}
