using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Messages;
using ProtoTurtle.BitmapDrawing;

public class ChartManager : MonoSingleton
{
	// From the editor
	public const int VIEWPORT_WITDH = 2235;
	public const int VIEWPORT_HEIGHT = 1242;

	// Internal config. Reduced sprite for speed, as we're drawing bitmaps at low level
	public const int WIDTH = (HEIGHT * VIEWPORT_WITDH) / VIEWPORT_HEIGHT;
	public const int HEIGHT = 300;
	public const int CAPACITY_LINE_Y = 30; // 10% down from the top, adjusted in the editor
	public const int MAX_Y_RANGE = HEIGHT - CAPACITY_LINE_Y;
	public const int OVERFLOW_MIN_Y = CAPACITY_LINE_Y -15; // Just to see the circle above
	public const int LINE_THICKNESS = 3;
	public const float TOTAL_ANIMATION_TIME_SEC = 1.5f;

	// Sprite adjustments
	public const int CIRCLE_OFFSET_X = 0; // to adjust the position
	public const int CIRCLE_OFFSET_Y = 0; // to adjust the position
	public const int PATIENTS_PANEL_OFFSET_X = -40; // to adjust the tip of the box
	public const int PATIENTS_PANEL_OFFSET_Y = 20; // to adjust the tip of the box

	// X axis daily expansion and margins
	public const int CURVE_MIN_WIDTH = (int)(WIDTH * 0.33); // tune
	public const int CURVE_MAX_WIDTH = (int)(WIDTH * 0.75); // tune
	public const int DAY_WIDTH_INCREMENT = (CURVE_MAX_WIDTH - CURVE_MIN_WIDTH) / 20; // 5%

	public Sprite patientsNormalImg;
	public Sprite patientsOverflowImg;
	public GameObject patientsPanel;
	public GameObject growthPanel;
	public GameObject evolutionChart;
	public GameObject initialDot;
	public GameObject finalDot;
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
		base.OnMonoSingletonAwake();
		UpdateFullChart(); // Could be done later, but it's safe
	}

	protected override void OnMonoSingletonUpdate()
	{
		elapsedTime += Time.deltaTime;
		if (animating && elapsedTime > TOTAL_ANIMATION_TIME_SEC)
		{
			// TODO publish event ChartUpdateFinished
			UpdateFullChart();
			animating = false;
		} else if (animating)
		{
			UpdateChart(elapsedTime);
		}
	}

	public void RestartChartAnimation()
	{
		animating = true;
		elapsedTime = 0.0f; // restarts the animation
		HidePatientsIndicator();
	}

	public void UpdateFullChart()
	{
		UpdateChart(TOTAL_ANIMATION_TIME_SEC);
		if (CanDrawSomePeriod()) 
		{
			ShowPatientsIndicator();
		}
		else
		{
			HidePatientsIndicator();
		}
	}

	public void UpdateChart(float elapsedTime)
	{
		Image evolutionChartImage = evolutionChart.gameObject.GetComponent<Image>();
		evolutionChartImage.sprite = CreateChartSprite(elapsedTime);
	}

	private void HidePatientsIndicator() {
		patientsPanel.transform.localPosition = new Vector3(10000, 10000, 0f); // Push out
	}

	private void ShowPatientsIndicator() {
		int day = GameManager.instance.localPlayer.gameSession.day;
		int patients = GameManager.instance.localPlayer.gameSession.patients[day-1];
		bool isOverflow = patients > GameManager.instance.localPlayer.gameSession.capacity;

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
		if (CanDrawSomePeriod())
		{
			DrawDaySegments(tex, elapsedTime);
			PositionBeginAndEndDots(elapsedTime);
		}
		else
		{
			HideDot(initialDot);
			HideDot(finalDot);
		}
		
		tex.Apply();
		return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
	}

	private bool CanDrawSomePeriod()
	{
		return GameManager.instance.localPlayer.gameSession != null && 
			GameManager.instance.localPlayer.gameSession.day > 1;
	}

	private void DrawDaySegments(Texture2D tex, float elapsedTime)
	{
		float animationProgress = elapsedTime / TOTAL_ANIMATION_TIME_SEC;
		int[] patients = GameManager.instance.localPlayer.gameSession.patients;
		int day = GameManager.instance.localPlayer.gameSession.day;

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
	}

	private void DrawOneDay(Texture2D tex, int day, int[] patients)
	{
		int x1 = GetXForDay(day-1);
		int x2 = GetXForDay(day);
		int y1 = GetYForPatients(patients[day-1]);
		int y2 = GetYForPatients(patients[day]);
		DrawSegment(tex, x1, y1, x2, y2);
	}

	private void DrawOnePartialDay(Texture2D tex, int day, int[] patients, float proportion)
	{
		int x1 = GetXForDay(day-1);
		int x2 = GetXForDay(day);
		int y1 = GetYForPatients(patients[day-1]);
		int y2 = GetYForPatients(patients[day]);
		int xp = x1 + (int)Math.Round((x2 - x1) * proportion);
		int yp = y1 + (int)Math.Round((y2 - y1) * proportion);
		DrawSegment(tex, x1, y1, xp, yp);
	}

	private void PositionBeginAndEndDots(float elapsedTime)
	{		
		float animationProgress = elapsedTime / TOTAL_ANIMATION_TIME_SEC;
		int[] patients = GameManager.instance.localPlayer.gameSession.patients;
		int day = GameManager.instance.localPlayer.gameSession.day;

		PositionDot(initialDot, GetXForDay(0), GetYForPatients(patients[0]));
		if (animationProgress > 0.95) // TODO tune
		{
		    PositionDot(finalDot, GetXForDay(day-1), GetYForPatients(patients[day-1]));
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
		float x1 = CIRCLE_OFFSET_X + x;
		float y1 = CIRCLE_OFFSET_Y + HEIGHT - y;
		dot.transform.localPosition = CoordinatesInViewport(x1, y1);
	}

	private int GetXForDay(int day)
	{
		return day * GetDayXIncrement();
	}

	private int GetYForPatients(int patients)
	{
		float currentCapacity = (float)GameManager.instance.localPlayer.gameSession.capacity;
		float capacityUsage = patients / currentCapacity;
		int y = HEIGHT - (int)(capacityUsage * MAX_Y_RANGE);
		return (y < CAPACITY_LINE_Y) ? OVERFLOW_MIN_Y : y; // Avoid drawing too out of bounds
	}

	private int GetDayXIncrement()
	{
		int day = GameManager.instance.localPlayer.gameSession.day;
		if (day < 1) return 0;
		int dayTargetWidth = CURVE_MIN_WIDTH + day * DAY_WIDTH_INCREMENT;
		int totalWidth = Math.Min(dayTargetWidth, CURVE_MAX_WIDTH);
		return totalWidth / day; // Eg: 150x1, 85x2, 63x3, etc  
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

	private Vector3 CoordinatesInViewport(float x, float y) {
		float ratio = VIEWPORT_WITDH / (float)WIDTH; 
		float x1 = x * ratio - (VIEWPORT_WITDH /2); // anchor is 0.5 , 0
		float y1 = y * ratio;
		
		return new Vector3(x1, y1, 0);
	}
}
