using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LocalizationManager : MonoSingleton
{
	public TextAsset localizationFile;
	public Font latinFont;

	public static string LANGUAGE_KEY = "Language";
	public static string DEFAULT_LANGUAGE_ID = "English";
	public static int LOCALIZATION_FILE_FIRST_LANGUAGE_COLUMN = 2;

	public static LocalizationManager instance
	{
		get
		{
			return GetInstance<LocalizationManager>();
		}
	}

	private List<LocalizationXmlModel> localizationXmlModels;
	private Dictionary<string, Dictionary<string,string>> localizedTexts;
	private List<string> localizedLanguages;

	private Font nonLatinFont;

	protected override void OnMonoSingletonAwake()
	{
		base.OnMonoSingletonAwake();
		localizationXmlModels = null;
		localizedTexts = null;
		localizedLanguages = null;
	}

	public void Init()
	{
		Debug.Log("LocaliationManager - Init()");

		localizationXmlModels = XmlModelManager.instance.FindModels<LocalizationXmlModel>();
		nonLatinFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
	}

	public void SetLocalizedText(Text textField, string localizationId)
	{
		Debug.Log("LocalizationManager - SetLocalizedText - localizationId:" + localizationId);


		string localizedText = GetText(localizationId);
		LocalizationXmlModel currentLocalizationXmlModel = FindLocalizationXmlModelWithLanguageId(GetCurrentLanguage());
		if (currentLocalizationXmlModel.useLatinFont)
		{
			textField.font = latinFont;
		}
		else
		{
			textField.font = nonLatinFont;
		}

		textField.text = localizedText;
	}

	private string GetText(string localizationId)
	{
		Debug.Log("LocalizationManager - GetText - localizationId:" + localizationId);

		if (localizationId == null)
		{
			return null;
		}

		if (localizedTexts == null)
		{
			LoadLocalizedTexts();
		}

		string localizedText = GetLocalizedText(localizationId);
		if (localizedText != null)
		{
			return localizedText;
		}

		return localizationId;
	}

	private string GetLocalizedText(string localizationId)
	{
		Debug.Log("LocalizationManager - GetLocalizedText - localizationId:" + localizationId);
		string currentLanguage = GetCurrentLanguage();
		if (localizedTexts.ContainsKey(localizationId))
		{
			Dictionary<string, string> translations = localizedTexts[localizationId];
			if (translations.ContainsKey(currentLanguage))
			{
				Debug.Log("LocalizationManager - return translations[currentLanguage]:" + translations[currentLanguage]);
				return translations[currentLanguage];
			}

			Debug.Log("LocalizationManager - return translations[DEFAULT_LANGUAGE_ID]:" + translations[DEFAULT_LANGUAGE_ID]);
			return translations[DEFAULT_LANGUAGE_ID];
		}

		return null;
	}

	private void LoadLocalizedTexts()
	{
		Debug.Log("LocalizationManager - LoadLocalizedTexts()");

		localizedTexts = new Dictionary<string, Dictionary<string, string>>();

		Debug.Log("LocalizationManager - LoadLocalizedTexts - localizationFile.text:" + localizationFile.text);

		string[] separator = { "\r\n" };
		string[] fileLines = localizationFile.text.Split(separator, System.StringSplitOptions.None);
		if (fileLines.Length > 0)
		{
			string localizationFileHeader = fileLines[0];

			localizedLanguages = GetLocalizedLanguages(localizationFileHeader);

			for (int i = 1; i < fileLines.Length; i++)
			{
				string line = fileLines[i];
				List<string> lineColumns = GetLocalizationFileLineColumns(line);

				string localizationId = lineColumns[0];
				Dictionary<string, string> translations = GetTranslations(lineColumns);
				localizedTexts.Add(localizationId, translations);
			}
		}
	}

	private Dictionary<string, string> GetTranslations(List<string> lineColumns)
	{
		lineColumns.RemoveAt(0); //removing localizationId
		lineColumns.RemoveAt(0); //removing description column

		Dictionary<string, string> translations = new Dictionary<string, string>();
		for (int i = 0; i < lineColumns.Count; i++)
		{
			translations.Add(localizedLanguages[i], lineColumns[i]);
			Debug.Log("LocalizationManager - GetTranslations() - adding " + localizedLanguages[i] + " ->" + lineColumns[i]);
		}

		return translations;
	}

	private List<string> GetLocalizationFileLineColumns(string line)
	{
		Debug.Log("LocalizationManager - GetLocalizationFileLineColumns() - line:" + line);

		List<string> columns = new List<string>();

		int cursor = 0;
		for (int i = 0; i < line.Length; i++)
		{
			if (line[i] == '\t')
			{
				columns.Add(line.Substring(cursor, (i-cursor)));
				cursor = i + 1;
			}
		}

		columns.Add(line.Substring(cursor));

		return columns;
	}

	private List<string> GetLocalizedLanguages(string localizationFileHeader)
	{
		Debug.Log("LocalizationManager - GetLocalizedLanguages() - localizationFileHeader:" + localizationFileHeader);

		List<string> languages = new List<string>();

		int cursor = 0;
		int columnIndex = 0;
		for (int i = 0; i < localizationFileHeader.Length; i++)
		{
			if (localizationFileHeader[i] == '\t')
			{
				if (columnIndex >= LOCALIZATION_FILE_FIRST_LANGUAGE_COLUMN)
				{
					Debug.Log("LocalizationManager - GetLocalizedLanguages() - Adding language:" + localizationFileHeader.Substring(cursor, (i - cursor)));
					languages.Add(localizationFileHeader.Substring(cursor, (i-cursor)));
				}

				columnIndex++;
				cursor = i + 1;
			}
		}

		languages.Add(localizationFileHeader.Substring(cursor));

		return languages;
	}

	public string GetCurrentLanguage()
	{
		Debug.Log("LocalizationManager - GetCurrentLanguage():" + PlayerPrefs.GetString(LANGUAGE_KEY));

		if (HasCurrentLanguage())
		{
			return PlayerPrefs.GetString(LANGUAGE_KEY);
		}

		Debug.Log("LocalizationManager - GetCurrentLanguage() returns null");
		return null;
	}

	public bool HasCurrentLanguage()
	{
		return PlayerPrefs.HasKey(LANGUAGE_KEY);
	}

	public void SetLanguage(string languageId)
	{
		Debug.Log("LocalizationManager - SetLanguage() - languageId:" + languageId);
		PlayerPrefs.SetString(LANGUAGE_KEY, languageId);
	}

	public void SetDefaultLanguage()
	{
		Debug.Log("LocalizationManager - SetDefaultLanguage() - language:" + DEFAULT_LANGUAGE_ID);
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

		Debug.Log("LocalizationManager - FindLocalizationXmlModel() - localizationXmlModelFound:" + localizationXmlModelFound.name);
		return localizationXmlModelFound;
	}

	private LocalizationXmlModel FindLocalizationXmlModelWithExactMatch(string languageCode)
	{
		Debug.Log("LocalizationManager - FindLocalizationXmlModelWithExactMatch() - languageCode:" + languageCode);

		foreach (LocalizationXmlModel localizationXmlModel in localizationXmlModels)
		{
			foreach (string tag in localizationXmlModel.tags)
			{
				if (languageCode.Equals(tag.ToLower()))
				{
					Debug.Log("LocalizationManager - FindLocalizationXmlModelWithExactMatch() -returns :" + localizationXmlModel.name);
					return localizationXmlModel;
				}
			}
		}

		Debug.Log("LocalizationManager - FindLocalizationXmlModelWithExactMatch() -returns null");
		return null;
	}

	private LocalizationXmlModel FindLocalizationXmlModelWithPartialMatch(string languageCode)
	{
		Debug.Log("LocalizationManager - FindLocalizationXmlModelWithPartialMatch() - languageCode:" + languageCode);
		foreach (LocalizationXmlModel localizationXmlModel in localizationXmlModels)
		{
			foreach (string tag in localizationXmlModel.tags)
			{
				if (languageCode.StartsWith(tag.ToLower()))
				{
					Debug.Log("LocalizationManager - FindLocalizationXmlModelWithPartialMatch() -returns :" + localizationXmlModel.name);
					return localizationXmlModel;
				}
			}
		}

		return null;
	}

	private LocalizationXmlModel FindLocalizationXmlModelWithLanguageId(string languageId)
	{
		Debug.Log("LocalizationManager - FindLocalizationXmlModelWithLanguageId() - languageId:" + languageId);
		return localizationXmlModels.Find((localizationXmlModel) => localizationXmlModel.languageId == languageId);

	}
}
