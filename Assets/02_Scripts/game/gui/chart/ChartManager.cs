using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Messages;
using ProtoTurtle.BitmapDrawing;

public class ChartManager : MonoSingleton
{

	public GameObject growthPanel;
	public GameObject evolutionChart;
	public GameObject initialDot;
	public GameObject finalDot;
	public Color normalColor;
	public Color overFlowColor;

	// Public options
	public float totalAnimationTime;
	public float dashLineSpacing;
	public int dashLineDotWidth;

	// Measures, adjusted to viewport. Act like consts but they are potentially overriden
	private int VIEWPORT_WITDH = 2235;
	private int WIDTH;
	private int CURVE_MIN_WIDTH;
	private int CURVE_MAX_WIDTH;
	private float DAY_WIDTH_INCREMENT;

	private const int CHART_X_MARGIN = 125;
	private const int VIEWPORT_HEIGHT = 1442;
	private const int HEIGHT = 300;
	private const int CAPACITY_LINE_Y = 67; // 324 from the top in the editor
	private const int MAX_Y_RANGE = HEIGHT - CAPACITY_LINE_Y;
	private const int LINE_THICKNESS = 3;
	private const float EPSILON = 0.05f;

	// Internal state
	private bool animating = false;
	private int dayToDrawTo = -1; // The chart is being drawn up to day and day-1
	private GameSession sessionCopy = null;
    private float elapsedTime = 0.0f;


	public static ChartManager instance
	{
		get
		{
			return GetInstance<ChartManager>();
		}
	}

	protected override void OnMonoSingletonAwake()
	{
		base.OnMonoSingletonAwake();
		AdjustToViewport();
	}

	public void FirstDraw()
	{
		FreezeDrawingData(GameManager.instance.localPlayer.gameSession.day);
		UpdateFullChart();
	}

	protected override void OnMonoSingletonUpdate()
	{
		if (animating)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > totalAnimationTime)
			{
				UpdateFullChart();
				animating = false;
			} else
			{
				UpdateChart(elapsedTime);
			}
		}
	}

	private void AdjustToViewport()
	{
		GameObject uiMainCanvas = GameObject.Find("UIMainCanvas");
		Vector2 vp = ViewportUtils.MeasureViewport(uiMainCanvas);

		if (vp.x > (VIEWPORT_WITDH + CHART_X_MARGIN*2))
		{
			VIEWPORT_WITDH = (int)(vp.x - CHART_X_MARGIN*2);
		}
		WIDTH = (HEIGHT * VIEWPORT_WITDH) / VIEWPORT_HEIGHT;
		CURVE_MIN_WIDTH = (int)(WIDTH * 0.11); // tune, so show a minimum line and separate indicators
		CURVE_MAX_WIDTH = (int)(WIDTH * 0.85); // tune, to leave space for the patients indicator
		DAY_WIDTH_INCREMENT = (CURVE_MAX_WIDTH - CURVE_MIN_WIDTH) / (float)GameSession.MAX_DAYS;
	}

	public void RestartChartAnimation()
	{
		animating = true;
		elapsedTime = 0.0f; // restarts the animation
		FreezeDrawingData(GameManager.instance.localPlayer.gameSession.day + 1);
		HidePatientsIndicator();
	}

	private void FreezeDrawingData(int dayToDrawTo)
	{
		this.dayToDrawTo = dayToDrawTo;
		this.sessionCopy = new GameSession {
			day = GameManager.instance.localPlayer.gameSession.day,
			patients = (int[])GameManager.instance.localPlayer.gameSession.patients.Clone(),
			growthRate = GameManager.instance.localPlayer.gameSession.growthRate,
			capacity = GameManager.instance.localPlayer.gameSession.capacity
		};
	}

	public void UpdateFullChart()
	{
		UpdateChart(totalAnimationTime);
	}

	public void UpdateChart(float elapsedTime)
	{
		Image evolutionChartImage = evolutionChart.gameObject.GetComponent<Image>();
		evolutionChartImage.sprite = CreateChartSprite(elapsedTime);
	}

	private void HidePatientsIndicator()
	{
 		PatientsPanelSelector.instance.HidePanels();
	}

	private void ShowPatientsIndicator()
	{
		int day = GetDayToDrawTo();
		float x = GetXForDay(day-1);
		float y = HEIGHT - GetYForPatients(GetPatients(day-1));

 		PatientsPanelSelector.instance.HidePanels();
		GameObject patientsPanel = PatientsPanelSelector.instance.SelectPanel();
		patientsPanel.transform.localPosition = CoordinatesInViewport(x, y);
	}

	private Sprite CreateChartSprite(float elapsedTime)
	{
		Texture2D tex = GetTransparentTexture();
		DrawDaySegments(tex, elapsedTime);
		PositionBeginAndEndDots(elapsedTime);
		if (elapsedTime > GetSegmentsAnimationTime())
		{
			ShowPatientsIndicator();
		}
		DrawPredictionCurve(tex, elapsedTime);
		
		tex.Apply();
		return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
	}

	private void DrawDaySegments(Texture2D tex, float elapsedTime)
	{
		float animationProgress = Math.Min(elapsedTime / GetSegmentsAnimationTime(), 1.0f);
		
		// Line, as concatenation of segments. Animate segments up to some day
		int day = GetDayToDrawTo();
		float daysToDraw = day * animationProgress;
		int lastFullDay = (int)Math.Truncate(daysToDraw);
		float lastDayProportion = daysToDraw - lastFullDay;

		for (int d = -1; d < lastFullDay; d++)
		{
			DrawOneDay(tex, d);
		}
		if (lastFullDay > 0 && lastFullDay < day && lastDayProportion > EPSILON) // TUNE, some epsilon worth drawing 
		{
			DrawOnePartialDay(tex, lastFullDay, lastDayProportion);
		}
	}

	private void DrawPredictionCurve(Texture2D tex, float elapsedTime)
	{
		float animationProgress = elapsedTime / totalAnimationTime;
		
		int day = GetDayToDrawTo();
		float daysToDraw = GameSession.MAX_DAYS * animationProgress;
		if (day >= daysToDraw) return;

		/* Draw first point always, from there start moving a vector where a dot is drawn
		   every given distance and some magnitude might be left for the next vector, so we
		   avoid drawing dots per segment and cause a weird effect */
		Vector2 prevPoint = GetPredictionPoint(day);
		Vector2 nextPoint = new Vector2(); 
		float magnitudeRemaining = 0f; // part of the last vector that hasn't been drawn
		DrawPredictionPoint(tex, prevPoint); 
		for (int d = day+1; d < daysToDraw; d++) 
		{
			nextPoint = GetPredictionPoint(d);
			magnitudeRemaining = ContinuePredictionCurve(tex, d, prevPoint, nextPoint, magnitudeRemaining);
			// Debug.Log("DrawPredictionCurve d: " + d + " nextPoint: " + nextPoint + " prevPoint: " + prevPoint + " magnitudeRemaining: " + magnitudeRemaining);
			prevPoint = nextPoint;
		}
	}

	private float GetSegmentsAnimationTime()
	{
		int day = GetDayToDrawTo();
		return (totalAnimationTime * day) / (float)GameSession.MAX_DAYS;
	}

	private void DrawOneDay(Texture2D tex, int day)
	{
		int x1 = GetXForDay(day-1);
		int x2 = GetXForDay(day);
		int y1 = GetYForPatients(GetPatients(day-1));
		int y2 = GetYForPatients(GetPatients(day));
		DrawSegment(tex, x1, y1, x2, y2);
	}

	private void DrawOnePartialDay(Texture2D tex, int day, float proportion)
	{
		int x1 = GetXForDay(day-1);
		int x2 = GetXForDay(day);
		int y1 = GetYForPatients(GetPatients(day-1));
		int y2 = GetYForPatients(GetPatients(day));
		int xp = x1 + (int)Math.Round((x2 - x1) * proportion);
		int yp = y1 + (int)Math.Round((y2 - y1) * proportion);
		DrawSegment(tex, x1, y1, xp, yp);
	}

	private float ContinuePredictionCurve(Texture2D tex, int day, Vector2 prevPoint, Vector2 nextPoint, float magnitudeRemaining)
	{
		// We go from prevPoint to next point carrying magnitudeRemaining.
		// We might draw from 0 to N points depending on the distance, and return another magnitudeRemaining
		float distance = Vector2.Distance(prevPoint, nextPoint);
		float totalMagnitude = magnitudeRemaining + distance;
		float magnitudeToSpend = totalMagnitude;
		float pointInterpolation = dashLineSpacing - magnitudeRemaining;

		while(magnitudeToSpend > dashLineSpacing)
		{
			float t = pointInterpolation / distance;
			magnitudeToSpend -= dashLineSpacing;
			pointInterpolation += dashLineSpacing;

			Vector2 toDraw = Vector2.Lerp(prevPoint, nextPoint, t);
			// Debug.Log("day: " + day + " magnitudeToSpend: " + magnitudeToSpend + " t: " + t + " toDraw: " + toDraw);
			DrawPredictionPoint(tex, toDraw);
		}

		return magnitudeToSpend;
	}

	private void DrawPredictionPoint(Texture2D tex, Vector2 position)
	{
		Color color = (position.y < CAPACITY_LINE_Y) ? overFlowColor : normalColor;
		tex.DrawFilledCircle((int)position.x, (int)position.y, dashLineDotWidth, color);
	}

	private Vector2 GetPredictionPoint(int day)
	{
		int x = GetXForDay(day);
		int y = GetYForPatients(FutureProjector.instance.GetPredictedPatients(day, sessionCopy));
		return new Vector2(x, y);
	}

	private void PositionBeginAndEndDots(float elapsedTime)
	{		
		float animationProgress = elapsedTime / totalAnimationTime;
		bool hasActualCurveFinished = elapsedTime > GetSegmentsAnimationTime();
		int day = GetDayToDrawTo();
		
		PositionDot(initialDot, GetXForDay(-1), GetYForPatients(GetPatients(-1)));
		if (hasActualCurveFinished)
		{
		    PositionDot(finalDot, GetXForDay(day-1), GetYForPatients(GetPatients(day-1)));
		}
		else
		{
			HideDot(finalDot);
		}
	}

	private void HideDot(GameObject dot)
	{		
		PositionDot(dot, -10000, -10000);
	}

	private void PositionDot(GameObject dot, int x, int y) {
		// Coordinates in the sprite space (like any dot in the chart sprite)
		float x1 = x;
		float y1 = HEIGHT - y;
		dot.transform.localPosition = CoordinatesInViewport(x1, y1);
	}

	private int GetXForDay(int day)
	{
		if (day < 0) return 0;
		if (day == 0) return CURVE_MIN_WIDTH;
		return CURVE_MIN_WIDTH + (int)(day * DAY_WIDTH_INCREMENT);
	}

	private int GetYForPatients(int patients)
	{
		if (sessionCopy == null) return HEIGHT;
		
		float currentCapacity = (float)sessionCopy.capacity;
		float capacityUsage = patients / currentCapacity;
		return HEIGHT - (int)(capacityUsage * MAX_Y_RANGE);
	}

	public int GetDayToDrawTo()
	{
		if (sessionCopy == null) return 1;
		
		return (dayToDrawTo > -1) ? dayToDrawTo : sessionCopy.day;
	}

	private int GetPatients(int day)
	{
		if (sessionCopy == null) return 0;
		
		int[] patients = sessionCopy.patients;
		return (day < 2) ? patients[1] : patients[day];
	}

	private int GetCurrentCapacity()
	{
		if (sessionCopy == null) return 1000; // anything big
		
		return sessionCopy.capacity;
	}

	private Vector3 CoordinatesInViewport(float x, float y) {
		float ratio = VIEWPORT_WITDH / (float)WIDTH; 
		return new Vector3(x * ratio, y * ratio, 0);
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
}
