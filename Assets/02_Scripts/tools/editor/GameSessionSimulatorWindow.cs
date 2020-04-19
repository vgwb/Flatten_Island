using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameSessionSimulatorWindow : EditorWindow
{
	private const string DATA_FOLDER = "09_Data";

	private string runsString = "10";

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
		EditorGUILayout.LabelField("Simulation Runs:", GUILayout.Width(180f));

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
	}

	private void RunSimulations(int runs)
	{
		GameSessionSimulator gameSessionSimulator = new GameSessionSimulator();
		gameSessionSimulator.Initialize(runs);
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
		XmlModelTypeMappings.instance.AddMapping("gamePhaseRequirement", typeof(GamePhaseRequirementXmlModel));
		XmlModelTypeMappings.instance.AddMapping("gameStoryRequirement", typeof(GameStoryRequirementXmlModel));
		XmlModelTypeMappings.instance.AddMapping("gamePhaseDurationCondition", typeof(GamePhaseDurationConditionXmlModel));
		XmlModelTypeMappings.instance.AddMapping("gamePhaseGrowthCondition", typeof(GamePhaseGrowthRateConditionXmlModel));
	}
}
