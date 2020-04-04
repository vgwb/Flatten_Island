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
	public const int DAY_X_INCREMENT = 10;
	public const int AXIS_THICKNESS = 2;
	public const int LINE_THICKNESS = 3;
	public const int BIG_CIRCLE_RADIUS = 7;

	public static Sprite CreateChartSprite(int[] patients, int maxPatients, int day)
	{
		Texture2D tex = GetTransparentTexture();
		
		int xAxisY = HEIGHT-AXIS_MARGIN;
		int xAxisMaxX = WIDTH-AXIS_MARGIN;
		int yAxisX = AXIS_MARGIN;

		// Axis
		tex.DrawThickLine(AXIS_MARGIN, xAxisY, xAxisMaxX, xAxisY, Color.black, AXIS_THICKNESS);
		tex.DrawThickLine(AXIS_MARGIN, xAxisY, AXIS_MARGIN, AXIS_MARGIN, Color.black, AXIS_THICKNESS);

		// Line, as concatenation of segments
		for (int i = 1; i < day; i++) {
			int x1 = getXForDay(i-1);
			int x2 = getXForDay(i);
			int y1 = getYForPatients(patients[i-1], maxPatients);
			int y2 = getYForPatients(patients[i], maxPatients);
			DrawSegment(tex, x1, y1, x2, y2);
		}

		// Begin and end of the line
		DrawBigWhiteCircle(tex, getXForDay(0), getYForPatients(patients[0], maxPatients));
		DrawBigWhiteCircle(tex, getXForDay(day-1), getYForPatients(patients[day-1], maxPatients));

		tex.Apply();
		Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
		return mySprite;
	}

	private static int getXForDay(int day)
	{
		return AXIS_MARGIN + BIG_CIRCLE_RADIUS + day * DAY_X_INCREMENT;
	}

	private static int getYForPatients(int patients, int maxPatients)
	{
		int maxY = HEIGHT - (AXIS_MARGIN + BIG_CIRCLE_RADIUS);
		return maxY - (patients * maxY) / maxPatients;
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
		tex.DrawThickLine(x1, y1, x2, y2, Color.black, LINE_THICKNESS);
	}

	public static void DrawBigWhiteCircle(Texture2D tex, int x1, int y1)
	{
		tex.DrawFilledCircle(x1, y1, BIG_CIRCLE_RADIUS, Color.white);
		tex.DrawCircle(x1, y1, BIG_CIRCLE_RADIUS, Color.black);
	}
	

}
