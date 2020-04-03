using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProtoTurtle.BitmapDrawing;

public class ChartFactory
{
	public const int WIDTH = 400;
	public const int HEIGHT = 200;
	public const int AXIS_MARGIN = 5;

	public static Sprite CreateChartSprite()
	{
		Texture2D tex = GetTransparentTexture();
		
		int xAxisY = HEIGHT-AXIS_MARGIN;
		int xAxisMaxX = WIDTH-AXIS_MARGIN;
		int yAxisX = AXIS_MARGIN;

		// Axis
		tex.DrawThickLine(AXIS_MARGIN, xAxisY, xAxisMaxX, xAxisY, Color.black, 3);
		tex.DrawThickLine(AXIS_MARGIN, xAxisY, AXIS_MARGIN, AXIS_MARGIN, Color.black, 3);

		// Segment per day. Hardcoded proof of concept
		DrawSegment(tex, 10, 190, 20, 188);
		DrawSegment(tex, 20, 188, 30, 185);
		DrawSegment(tex, 30, 185, 40, 180);
		DrawSegment(tex, 40, 180, 50, 170);
		DrawSegment(tex, 50, 170, 60, 150);
		DrawSegment(tex, 60, 150, 70, 115);
		DrawSegment(tex, 70, 115, 80,  90);
		DrawSegment(tex, 80,  90, 90,  80);
		DrawSegment(tex, 90,  80, 100, 75);
		DrawSegment(tex, 100, 75, 110, 72);
		DrawSegment(tex, 110, 72, 120, 70);
		DrawSegment(tex, 120, 70, 130, 69);
		DrawSegment(tex, 130, 69, 140, 68);
		DrawSegment(tex, 140, 68, 150, 68);
		DrawSegment(tex, 150, 68, 160, 70);
		
		tex.Apply();
		Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
		return mySprite;
	}

	public static Texture2D GetTransparentTexture()
	{
		// TODO ideally this texture could be cached and cloned, but doesn't seem to work
		Texture2D clearTexture = new Texture2D(WIDTH, HEIGHT);
		for (int x = WIDTH-1; x >= 0; x--)
		{
			for (int y = HEIGHT-1; y >= 0; y--)
			{
				clearTexture.SetPixel(x, y, Color.clear);
			}
		}
		return clearTexture;
	}

	public static void DrawSegment(Texture2D tex, int x1, int y1, int x2, int y2)
	{
		tex.DrawThickLine(x1, y1, x2, y2, Color.black, 4);
	}
}
