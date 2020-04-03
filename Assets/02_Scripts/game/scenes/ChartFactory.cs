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
		tex.DrawThickLine(AXIS_MARGIN, xAxisY, xAxisMaxX, xAxisY, Color.black, 2);
		tex.DrawThickLine(AXIS_MARGIN, xAxisY, AXIS_MARGIN, AXIS_MARGIN, Color.black, 2);

		// Segment per day. Hardcoded proof of concept
		DrawSegment(tex, 10, 190, 20, 188);
		DrawSegment(tex, 20, 188, 30, 185);
		DrawSegment(tex, 30, 185, 40, 180);
		DrawSegment(tex, 40, 180, 50, 170);
		DrawSegment(tex, 50, 170, 60, 160);
		DrawSegment(tex, 60, 160, 70, 148);
		DrawSegment(tex, 70, 148, 80, 135);
		DrawSegment(tex, 80, 135, 90, 122);
		DrawSegment(tex, 90, 122, 100, 115);
		DrawSegment(tex, 100, 115, 110, 110);
		DrawSegment(tex, 110, 110, 120, 106);
		DrawSegment(tex, 120, 106, 130, 102);
		DrawSegment(tex, 130, 102, 140, 99);
		DrawSegment(tex, 140, 99, 150, 96);
		DrawSegment(tex, 150, 96, 160, 93);
		DrawSegment(tex, 160, 93, 170, 90);
		DrawSegment(tex, 170, 90, 180, 87);
		DrawSegment(tex, 180, 87, 190, 82);
		DrawSegment(tex, 190, 82, 200, 75);
		DrawSegment(tex, 200, 75, 210, 72);
		DrawSegment(tex, 210, 72, 220, 70);
		DrawSegment(tex, 220, 70, 230, 69);
		DrawSegment(tex, 230, 69, 240, 68);
		DrawSegment(tex, 240, 68, 250, 68);
		DrawSegment(tex, 250, 68, 260, 70);

		// Begin and end of the line
		tex.DrawFilledCircle(10, 190, 7, Color.white);
		tex.DrawCircle(10, 190, 7, Color.black);
		tex.DrawFilledCircle(260, 70, 7, Color.white);
		tex.DrawCircle(260, 70, 7, Color.black);

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
		tex.DrawThickLine(x1, y1, x2, y2, Color.black, 3);
	}
}
