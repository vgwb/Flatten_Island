using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Messages;
using ProtoTurtle.BitmapDrawing;

public class ChartManager : MonoSingleton
{
	public const int WIDTH = 500;
	public const int HEIGHT = 300;
	public const int AXIS_MARGIN = 5;
	public const int DAY_X_INCREMENT = 20;
	public const int AXIS_THICKNESS = 2;
	public const int LINE_THICKNESS = 3;
	public const int BIG_CIRCLE_RADIUS = 7;
	public const int GROWTH_PANEL_OFFSET_X = -575;
	public const int GROWTH_PANEL_OFFSET_Y = -900;
	public const float TOTAL_ANIMATION_TIME_SEC = 2.0f;

	GameSession session;
	GameObject growthPanel;
	GameObject evolutionChart;
    private float elapsedTime = 0.0f;
	private bool animating = false;

	public static ChartManager instance
	{
		get
		{
			return GetInstance<ChartManager>();
		}
	}

	protected override void OnMonoSingletonAwake()
	{
		Debug.Log("Chart OnMonoSingletonAwake");
		base.OnMonoSingletonAwake();

		session = GameManager.instance.localPlayer.gameSession;
		growthPanel = GameObject.Find("GrowthPanel");
		evolutionChart = GameObject.Find("EvolutionChart");
		UpdateChart(0.0f);

		EventMessageHandler suggestionResultEntryExitCompletedMessageHandler = new EventMessageHandler(this, OnSuggestionResultEntryExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(SuggestionResultEntryExitCompletedEvent).Name, suggestionResultEntryExitCompletedMessageHandler);
	}

	protected override void OnMonoSingletonUpdate()
	{
		elapsedTime += Time.deltaTime;
		if (animating && elapsedTime > TOTAL_ANIMATION_TIME_SEC)
		{
			// TODO publish event ChartUpdateFinished
			// Do a single final render in complete state, including the Patients indicator
			UpdateChart(TOTAL_ANIMATION_TIME_SEC);
			ShowPatientsIndicator();
			animating = false;
		} else if (animating)
		{
			UpdateChart(elapsedTime);
		}
		EventMessageManager.instance.Update();
	}

	protected override void OnMonoSingletonDestroyed()
	{
		Debug.Log("Chart OnMonoSingletonDestroyed");
		EventMessageManager.instance.RemoveHandler(typeof(SuggestionResultEntryExitCompletedEvent).Name, this);
		base.OnMonoSingletonDestroyed();
	}

	private void OnSuggestionResultEntryExitCompleted(EventMessage eventMessage)
	{
		Debug.Log("Animate the chart");
		animating = true;
		elapsedTime = 0.0f; // restarts the animation
	}

	public void UpdateChart(float elapsedTime)
	{
		Image evolutionChartImage = evolutionChart.gameObject.GetComponent<Image>();
		evolutionChartImage.sprite = CreateChartSprite(elapsedTime);
	}

	public void ShowPatientsIndicator() {
		growthPanel.transform.localPosition = CalculateGrowthPanelPosition();
	}

	/*
	   NOTE: Ideally the growth panel and chart would reuse the same functions to calculate
	   the position of days and patients. However, the drawing library uses a different coordinate
	   model and the chart is scaled to reduce the sprite size, so the functions are separate.
	 */
	public Vector3 CalculateGrowthPanelPosition() {
		int patients = session.patients[session.day-1];
		int day = session.day;
		int ratio = 2; // Charts scales x2 to reduce sprite size
		float patientsNormal = patients / (float)GameSession.MAX_PATIENTS; 
		float x = GROWTH_PANEL_OFFSET_X + day * DAY_X_INCREMENT * ratio;
		float y = GROWTH_PANEL_OFFSET_Y + patientsNormal * HEIGHT * ratio;
		return new Vector3(x, y, 0f);
	}

	public Sprite CreateChartSprite(float elapsedTime)
	{
		Texture2D tex = GetTransparentTexture();
		
		float animationProgress = elapsedTime / TOTAL_ANIMATION_TIME_SEC;
		int[] patients = session.patients;
		int day = session.day;
		int xAxisY = HEIGHT-AXIS_MARGIN;
		int xAxisMaxX = WIDTH-AXIS_MARGIN;

		// Axis
		tex.DrawThickLine(AXIS_MARGIN, xAxisY, xAxisMaxX, xAxisY, Color.black, AXIS_THICKNESS);
		tex.DrawThickLine(AXIS_MARGIN, xAxisY, AXIS_MARGIN, AXIS_MARGIN, Color.black, AXIS_THICKNESS);

		// Line, as concatenation of segments. Animate segments up to some day
		float daysToDraw = day * animationProgress;
		int lastFullDay = (int)Math.Truncate(daysToDraw);
		float lastDayProportion = daysToDraw - lastFullDay;

		for (int d = 1; d < lastFullDay; d++)
		{
			DrawOneDay(tex, d, patients);
		}
		if (lastFullDay > 0 && lastFullDay < day && lastDayProportion > 0.05f) // TUNE, some epsilon worth drawing 
		{
			DrawOnePartialDay(tex, lastFullDay, patients, lastDayProportion);
		}

		// Begin and end of the line. The end only when the animation is about to finish
		DrawBigWhiteCircle(tex, GetXForDay(0), GetYForPatients(patients[0]));
		if (animationProgress > 0.95) // TODO tune
		{
			DrawBigWhiteCircle(tex, GetXForDay(day-1), GetYForPatients(patients[day-1]));
		}

		tex.Apply();
		Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
		return mySprite;
	}

	private static void DrawOneDay(Texture2D tex, int day, int[] patients)
	{
		int x1 = GetXForDay(day-1);
		int x2 = GetXForDay(day);
		int y1 = GetYForPatients(patients[day-1]);
		int y2 = GetYForPatients(patients[day]);
		DrawSegment(tex, x1, y1, x2, y2);
	}

	private static void DrawOnePartialDay(Texture2D tex, int day, int[] patients, float proportion)
	{
		int x1 = GetXForDay(day-1);
		int x2 = GetXForDay(day);
		int y1 = GetYForPatients(patients[day-1]);
		int y2 = GetYForPatients(patients[day]);
		int xp = x1 + (int)Math.Round((x2 - x1) * proportion);
		int yp = y1 + (int)Math.Round((y2 - y1) * proportion);
		DrawSegment(tex, x1, y1, xp, yp);
	}

	private static int GetXForDay(int day)
	{
		return AXIS_MARGIN + BIG_CIRCLE_RADIUS + day * DAY_X_INCREMENT;
	}

	private static int GetYForPatients(int patients)
	{
		int maxY = HEIGHT - (AXIS_MARGIN + BIG_CIRCLE_RADIUS);
		return maxY - (patients * maxY) / GameSession.MAX_PATIENTS;
	}

	public Texture2D GetTransparentTexture()
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
