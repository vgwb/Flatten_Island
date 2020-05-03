using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameSessionSimulatorWindow : EditorWindow
{
	private const string DATA_FOLDER = "09_Data";

	private string runsString = "10";
	private string capacityWarningString = "2000";
	private string capacityWarningStartString = "1000";

	private List<GameSimulatorStrategyData> strategiesData;
	private GameSimulatorStrategyData randomStrategyData;
	private GameSimulatorStrategyData criticalParamFirstStrategyData;

	private List<GameSimulatorOptionSelectionStrategyData> optionSelectionStrategiesData;
	private GameSimulatorOptionSelectionStrategyData randomOptionSelectionStrategyData;
	private GameSimulatorOptionSelectionStrategyData bestOptionSelectionStrategyData;

	private GameSessionSimulator gameSessionSimulator;

	[MenuItem("Flatten Island/Game Session Simulator")]
	static void CreateWindow()
	{
		if (EditorApplication.isCompiling)
		{
			Debug.Log("Unity is compiling");
			return;
		}
		GameSessionSimulatorWindow window = CreateInstance<GameSessionSimulatorWindow>();
		window.Init();
	}

	private void Init()
	{
		titleContent = new GUIContent("Game Session Simulator");
		InitializeXmlModelMappings();
		LoadXmlModels();

		ShowUtility();

		strategiesData = new List<GameSimulatorStrategyData>();
		criticalParamFirstStrategyData = new GameSimulatorStrategyData(new GameSimulatorCriticalParamFirstStrategy(), "100");
		randomStrategyData = new GameSimulatorStrategyData(new GameSimulatorRandomStrategy(), "0");

		optionSelectionStrategiesData = new List<GameSimulatorOptionSelectionStrategyData>();
		bestOptionSelectionStrategyData = new GameSimulatorOptionSelectionStrategyData(new GameSimulatorOptionSelectionBestStrategy(), "100");
		randomOptionSelectionStrategyData = new GameSimulatorOptionSelectionStrategyData(new GameSimulatorOptionSelectionRandomStrategy(), "0");
	}

	private void LoadXmlModels()
	{
		string path = Application.dataPath + "/" + DATA_FOLDER + "/";

		string[] filePaths = Directory.GetFiles(path);

		if (filePaths != null && filePaths.Length > 0)
		{
			for (int i = 0; i < filePaths.Length; i++)
			{
				string filePath = filePaths[i];
				int indexOfAssets = filePath.IndexOf("Assets");
				string relativePath = filePath.Substring(indexOfAssets);

				TextAsset xmlModel = (TextAsset) AssetDatabase.LoadAssetAtPath(relativePath, typeof(TextAsset));
				if (xmlModel != null)
				{
					XmlModelManager.instance.LoadXml(xmlModel.text);
				}
			}
		}
	}

	void OnGUI()
	{
		EditorGUILayout.LabelField("Insert the number of simulations you want to perform:");
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("ADVISOR SELECTION STRATEGIES", GUILayout.Width(250f));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Critical Parameter First Strategy - Probability (%)", GUILayout.Width(350f));
		criticalParamFirstStrategyData.probabilityText = EditorGUILayout.TextField(criticalParamFirstStrategyData.probabilityText, GUILayout.Width(30f));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Random Strategy - Probability (%)", GUILayout.Width(350f));
		randomStrategyData.probabilityText = EditorGUILayout.TextField(randomStrategyData.probabilityText, GUILayout.Width(30f));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("OPTION SELECTION STRATEGIES", GUILayout.Width(250f));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Best Option Strategy - Probability (%)", GUILayout.Width(350f));
		bestOptionSelectionStrategyData.probabilityText = EditorGUILayout.TextField(bestOptionSelectionStrategyData.probabilityText, GUILayout.Width(30f));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Random Strategy - Probability (%)", GUILayout.Width(350f));
		randomOptionSelectionStrategyData.probabilityText = EditorGUILayout.TextField(randomOptionSelectionStrategyData.probabilityText, GUILayout.Width(30f));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("CAPACITY WARNING RULE", GUILayout.Width(250f));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Activates when (patients > Capacity Rule Start) AND (capacity < patients + capacity Backup)", GUILayout.Width(600f));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Capacity Rule Start (# patients):", GUILayout.Width(280f));
		capacityWarningStartString = EditorGUILayout.TextField(capacityWarningStartString, GUILayout.Width(50f));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Capacity Backup:", GUILayout.Width(280f));
		capacityWarningString = EditorGUILayout.TextField(capacityWarningString, GUILayout.Width(50f));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Simulation Runs:", GUILayout.Width(280f));
		runsString = EditorGUILayout.TextField(runsString, GUILayout.Width(50f));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
		if (GUILayout.Button("Run Simulations", GUILayout.Width(150f)))
		{
			int runs;
			if (int.TryParse(runsString, out runs))
			{
				RunSimulations(runs);
			}
		}

		if (gameSessionSimulator != null)
		{
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("RESULTS", GUILayout.Width(180f));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Win Runs", GUILayout.Width(200f));
			EditorGUILayout.LabelField(gameSessionSimulator.winRuns.ToString(), GUILayout.Width(50f));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Lose Runs", GUILayout.Width(200f));
			EditorGUILayout.LabelField(gameSessionSimulator.loseRuns.ToString(), GUILayout.Width(50f));
			string loseResultInfo = " ( Capacity:" + gameSessionSimulator.capacityLoseCount + ", Money:" + gameSessionSimulator.moneyLoseCount + ", Public:" + gameSessionSimulator.publicOpinionLoseCount + ")";
			EditorGUILayout.LabelField(loseResultInfo, GUILayout.Width(400f));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Win Days Record", GUILayout.Width(200f));
			EditorGUILayout.LabelField(gameSessionSimulator.winDaysRecord.ToString(), GUILayout.Width(50f));
			EditorGUILayout.EndHorizontal();
		}
	}

	private void RunSimulations(int runs)
	{
		
		strategiesData.Clear();
		strategiesData.Add(randomStrategyData);
		strategiesData.Add(criticalParamFirstStrategyData);

		optionSelectionStrategiesData.Clear();
		optionSelectionStrategiesData.Add(randomOptionSelectionStrategyData);
		optionSelectionStrategiesData.Add(bestOptionSelectionStrategyData);


		int capacityWarning;
		if (!int.TryParse(capacityWarningString, out capacityWarning))
		{
			capacityWarning = 0;
		}

		int capacityWarningStart;
		if (!int.TryParse(capacityWarningStartString, out capacityWarningStart))
		{
			capacityWarningStart = 100000;
		}

		GameSessionSimulatorSettings simulatorSettings = new GameSessionSimulatorSettings();
		simulatorSettings.simulationRuns = runs;
		simulatorSettings.strategiesData = strategiesData;
		simulatorSettings.optionStrategiesData = optionSelectionStrategiesData;
		simulatorSettings.capacityWarning = capacityWarning;
		simulatorSettings.capacityWarningStart = capacityWarningStart;

		gameSessionSimulator = new GameSessionSimulator();
		gameSessionSimulator.Initialize(simulatorSettings);
		gameSessionSimulator.Run();
	}

	private void InitializeXmlModelMappings()
	{
		XmlModelTypeMappings.instance.RemoveAllMappings();
		XmlModelManager.instance.UnregisterAllModels();

		XmlModelTypeMappings.instance.InitializeCoreMappings();

		//register here other game specific mappings
		XmlModelTypeMappings.instance.AddMapping("localization", typeof(LocalizationXmlModel));
		XmlModelTypeMappings.instance.AddMapping("advisor", typeof(AdvisorXmlModel));
		XmlModelTypeMappings.instance.AddMapping("gamePhase", typeof(GamePhaseXmlModel));
		XmlModelTypeMappings.instance.AddMapping("gameStory", typeof(GameStoryXmlModel));
		XmlModelTypeMappings.instance.AddMapping("suggestion", typeof(SuggestionXmlModel));
		XmlModelTypeMappings.instance.AddMapping("gameSession", typeof(GameSessionXmlModel));
		XmlModelTypeMappings.instance.AddMapping("genericDialog", typeof(GenericDialogXmlModel));
		XmlModelTypeMappings.instance.AddMapping("gameOver", typeof(GameOverXmlModel));
		XmlModelTypeMappings.instance.AddMapping("gamePhaseRequirement", typeof(GamePhaseRequirementXmlModel));
		XmlModelTypeMappings.instance.AddMapping("gameStoryRequirement", typeof(GameStoryRequirementXmlModel));
		XmlModelTypeMappings.instance.AddMapping("gamePhaseDurationCondition", typeof(GamePhaseDurationConditionXmlModel));
		XmlModelTypeMappings.instance.AddMapping("gamePhaseGrowthCondition", typeof(GamePhaseGrowthRateConditionXmlModel));
	}
}
