using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Messages;
using ProtoTurtle.BitmapDrawing;

public class ChartManager : MonoSingleton
{

	public Sprite patientsNormalImg;
	public Sprite patientsOverflowImg;
	public GameObject patientsPanel;
	public GameObject growthPanel;
	public GameObject evolutionChart;
	public GameObject initialDot;
	public GameObject finalDot;

	// Public options
	public float totalAnimationTime;
	public int dashLineSpacing;

	// Measures, adjusted to viewport. Act like consts but they are potentially overriden
	private int VIEWPORT_WITDH = 2235;
	private int WIDTH;
	private int CURVE_MIN_WIDTH;
	private int CURVE_MAX_WIDTH;
	private float DAY_WIDTH_INCREMENT;

	private const int CHART_X_MARGIN = 125;
	private const int VIEWPORT_HEIGHT = 1242;
	private const int HEIGHT = 300;
	private const int CAPACITY_LINE_Y = 30; // 10% down from the top, adjusted in the editor
	private const int MAX_Y_RANGE = HEIGHT - CAPACITY_LINE_Y;
	private const int OVERFLOW_MIN_Y = CAPACITY_LINE_Y -15; // Just to see the circle above
	private const int LINE_THICKNESS = 3;
	private const int MAX_DAYS = 100;
	private const float EPSILON = 0.05f;

	// Sprite adjustments
	private const int PATIENTS_PANEL_OFFSET_X = -37; // to adjust the tip of the box
	private const int PATIENTS_PANEL_OFFSET_Y = 13; // to adjust the tip of the box

	// Internal state
	private int dayToDrawTo = -1; // The chart is being drawn up to day and day-1
    private float elapsedTime = 0.0f;
	private bool animating = false;
	private bool firstDrawPending = true;


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

	protected override void OnMonoSingletonUpdate()
	{
		if (firstDrawPending && GameManager.instance.localPlayer.gameSession != null)
		{
			firstDrawPending = false;
			dayToDrawTo = GameManager.instance.localPlayer.gameSession.day;
			UpdateFullChart(); // First draw, without a day increase, from the stored state
		}
		else if (animating)
		{
			// Note: Due to the order, the chart is animated before the day increase, but values are of day+1
			dayToDrawTo = GameManager.instance.localPlayer.gameSession.day + 1;

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
		DAY_WIDTH_INCREMENT = (CURVE_MAX_WIDTH - CURVE_MIN_WIDTH) / (float)MAX_DAYS;
	}

	public void RestartChartAnimation()
	{
		animating = true;
		elapsedTime = 0.0f; // restarts the animation
		HidePatientsIndicator();
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
		patientsPanel.transform.localPosition = new Vector3(10000, 10000, 0f); // Push out
	}

	private void ShowPatientsIndicator()
	{
		int day = GetDayToDrawTo();
		int patients = GetPatients(day-1);
		bool isOverflow = patients > GetCurrentCapacity();

		Image patientsPanelImage = patientsPanel.gameObject.GetComponent<Image>();
		patientsPanelImage.sprite = isOverflow ? patientsOverflowImg : patientsNormalImg;

		patientsPanel.transform.localPosition = CalculatePatientsPanelPosition(day, patients);
	}

	private Vector3 CalculatePatientsPanelPosition(int day, int patients) {
		// Coordinates in the sprite space (like any dot in the chart sprite)
		float x = PATIENTS_PANEL_OFFSET_X + GetXForDay(day-1);
		float y = PATIENTS_PANEL_OFFSET_Y + HEIGHT - GetYForPatients(patients);

		return CoordinatesInViewport(x, y);
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
		
		int day = GetDayToDrawTo() + 1;
		float daysToDraw = MAX_DAYS * animationProgress;
		for (int d = day; d < daysToDraw; d+=dashLineSpacing) 
		{
			DrawPredictionDay(tex, d);
		}
	}

	private float GetSegmentsAnimationTime()
	{
		int day = GetDayToDrawTo();
		return (totalAnimationTime * day) / (float)MAX_DAYS;
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

	private void DrawPredictionDay(Texture2D tex, int day)
	{
		int x = GetXForDay(day);
		int y = GetYForPatients(FutureProjector.instance.GetPredictedPatients(day));

		Color color = Color.grey;
		if (y == OVERFLOW_MIN_Y)
		{
			animating = false; // Confirm behaviour
			color = Color.red;
		}
		tex.DrawFilledCircle(x, y, 2, color);
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
		if (GameManager.instance.localPlayer.gameSession == null) return HEIGHT;
		
		float currentCapacity = (float)GameManager.instance.localPlayer.gameSession.capacity;
		float capacityUsage = patients / currentCapacity;
		int y = HEIGHT - (int)(capacityUsage * MAX_Y_RANGE);
		return (y < CAPACITY_LINE_Y) ? OVERFLOW_MIN_Y : y; // Avoid drawing too out of bounds
	}

	public int GetDayToDrawTo()
	{
		if (GameManager.instance.localPlayer.gameSession == null) return 1;
		
		return (dayToDrawTo > -1) ? dayToDrawTo : GameManager.instance.localPlayer.gameSession.day;
	}

	private int GetPatients(int day)
	{
		if (GameManager.instance.localPlayer.gameSession == null) return 0;
		
		int[] patients = GameManager.instance.localPlayer.gameSession.patients;
		return (day < 2) ? patients[1] : patients[day];
	}

	private int GetCurrentCapacity()
	{
		if (GameManager.instance.localPlayer.gameSession == null) return 1000; // anything big
		
		return GameManager.instance.localPlayer.gameSession.capacity;
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
