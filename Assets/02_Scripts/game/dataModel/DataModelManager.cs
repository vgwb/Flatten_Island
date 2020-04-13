using UnityEngine;
using System.Collections.Generic;

public class DataModelManager : MonoSingleton
{
	public TextAsset[] dataModelFiles;

	public static DataModelManager instance
	{
		get
		{
			return GetInstance<DataModelManager>();
		}
	}

	public bool preloadInstancesCompleted { get; private set; }

	protected override void OnMonoSingletonAwake()
	{
		base.OnMonoSingletonAwake();
		InitializeMapping();
		LoadModels();
		PreloadInstances();
		preloadInstancesCompleted = true;

	}

	private void InitializeMapping()
	{
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

	private void LoadModels()
	{
		foreach (TextAsset dataModelFile in dataModelFiles)
		{
			if (dataModelFile != null && !string.IsNullOrEmpty(dataModelFile.text))
			{
				XmlModelManager.instance.LoadXml(dataModelFile.text);
			}
		}
	}

	private void PreloadInstances()
	{
		List<GameObjectXmlModel> gameObjectXmlModels = XmlModelManager.instance.FindModels<GameObjectXmlModel>();

		foreach (GameObjectXmlModel gameObjectModel in gameObjectXmlModels)
		{
			gameObjectModel.PreloadInstances();
		}
	}
}