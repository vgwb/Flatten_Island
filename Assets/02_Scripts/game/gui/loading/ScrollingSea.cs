using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingSea : MonoBehaviour
{
	public RawImage seaImage;
	public float speed;

	private Rect original_uv_rect;

	private void Awake()
	{
		original_uv_rect = seaImage.uvRect;
	}

	private void OnEnable()
	{
		seaImage.uvRect = original_uv_rect;		
	}

	private void Update()
	{
		Rect rect = seaImage.uvRect;
		rect.x += speed;
		seaImage.uvRect = rect;
	}
}