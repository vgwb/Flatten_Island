using UnityEngine;
using System.Collections.Generic;

public class LocalizationManager : Singleton
{
	public static string LANGUAGE_KEY = "Language";
	public static string DEFAULT_LANGUAGE_ID = "English";

	public static LocalizationManager instance
	{
		get
		{
			return GetInstance<LocalizationManager>();
		}
	}

	private List<LocalizationXmlModel> localizationXmlModels;

	public LocalizationManager()
	{
		localizationXmlModels = XmlModelManager.instance.FindModels<LocalizationXmlModel>();
	}

	public string GetLocalizedText(string localizationKey)
	{
		return localizationKey;
	}

	public string GetCurrentLanguage()
	{
		if (HasCurrentLanguage())
		{
			return PlayerPrefs.GetString(LANGUAGE_KEY);
		}

		return null;
	}

	public bool HasCurrentLanguage()
	{
		return PlayerPrefs.HasKey(LANGUAGE_KEY);
	}

	public void SetLanguage(string languageId)
	{
		PlayerPrefs.SetString(LANGUAGE_KEY, languageId);
	}

	public void SetDefaultLanguage()
	{
		PlayerPrefs.SetString(LANGUAGE_KEY, DEFAULT_LANGUAGE_ID);
	}

	public LocalizationXmlModel FindLocalizationXmlModel(string languageCode)
	{
		LocalizationXmlModel localizationXmlModelFound = null;
		languageCode = languageCode.ToLower();
		localizationXmlModelFound = FindLocalizationXmlModelWithExactMatch(languageCode);
		if (localizationXmlModelFound == null)
		{
			localizationXmlModelFound = FindLocalizationXmlModelWithPartialMatch(languageCode);
		}

		return localizationXmlModelFound;
	}

	private LocalizationXmlModel FindLocalizationXmlModelWithExactMatch(string languageCode)
	{
		foreach (LocalizationXmlModel localizationXmlModel in localizationXmlModels)
		{
			foreach (string tag in localizationXmlModel.tags)
			{
				if (languageCode.Equals(tag.ToLower()))
				{
					return localizationXmlModel;
				}
			}
		}

		return null;
	}

	private LocalizationXmlModel FindLocalizationXmlModelWithPartialMatch(string languageCode)
	{
		foreach (LocalizationXmlModel localizationXmlModel in localizationXmlModels)
		{
			foreach (string tag in localizationXmlModel.tags)
			{
				if (languageCode.StartsWith(tag.ToLower()))
				{
					return localizationXmlModel;
				}
			}
		}

		return null;
	}
}
