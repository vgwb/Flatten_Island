using System;
using System.Collections.Generic;
using UnityEngine;

public class LanguageMenu : DropDownMenu
{
	public List<LocalizationXmlModel> languages;

	protected override void OnAwake()
	{
		base.OnAwake();
		languages = XmlModelManager.instance.FindModels<LocalizationXmlModel>((localizationXmlModel) => localizationXmlModel.supported == true);
	}
	protected override void OnStart()
	{
	}

	private OptionParameterEntry CreateLanguageButton(int index)
	{
		GameObject optionParameterEntry = GameObjectFactory.instance.InstantiateGameObject(OptionParameterEntry.PREFAB, parametersGridLayoutGroup.transform, false);
		optionParameterEntry.gameObject.transform.SetParent(parametersGridLayoutGroup.transform, true);
		OptionParameterEntry optionParameterEntryScript = optionParameterEntry.GetComponent<OptionParameterEntry>();
		optionParameterEntryScript.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		return optionParameterEntryScript;
	}
}