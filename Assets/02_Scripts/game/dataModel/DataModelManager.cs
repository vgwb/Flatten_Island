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