using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LocalizationManager : MonoSingleton
{
	public TextAsset localizationFile;
	public Font latinFont;

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
		localizationXmlModels = XmlModelManager.instance.FindModels<LocalizationXmlModel>();
		nonLatinFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
	}

	public void SetLocalizedText(Text textField, string localizationId)
	{
		LocalPlayer localPlayer = GameManager.instance.localPlayer;

		string localizedText = GetText(localizationId);
		LocalizationXmlModel currentLocalizationXmlModel = FindLocalizationXmlModelWithLanguageId(localPlayer.GetLanguageId());
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

	public string ReplaceIntInText(string text, int value)
	{
		return text.Replace("%d", value.ToString());
	}

	private string GetText(string localizationId)
	{
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
		LocalPlayer localPlayer = GameManager.instance.localPlayer;

		string languageId = localPlayer.GetLanguageId();
		if (localizedTexts.ContainsKey(localizationId))
		{
			Dictionary<string, string> translations = localizedTexts[localizationId];
			if (translations.ContainsKey(languageId))
			{
				return translations[languageId];
			}

			return translations[DEFAULT_LANGUAGE_ID];
		}

		return null;
	}

	private void LoadLocalizedTexts()
	{
		localizedTexts = new Dictionary<string, Dictionary<string, string>>();

		string[] separator = { "\r\n", "\r", "\n", };
		string[] fileLines = localizationFile.text.Split(separator, System.StringSplitOptions.None);
		if (fileLines.Length > 0)
		{
			string localizationFileHeader = fileLines[0];

			localizedLanguages = GetLocalizedLanguages(localizationFileHeader);

			for (int i = 1; i < fileLines.Length; i++)
			{
				string line = RemoveLineBreaks(fileLines[i]);

				List<string> lineColumns = GetLocalizationFileLineColumns(line);

				string localizationId = lineColumns[0];
				Dictionary<string, string> translations = GetTranslations(lineColumns);
				localizedTexts.Add(localizationId, translations);
			}
		}
	}

	private string RemoveLineBreaks(string inputLine)
	{
		string line = inputLine;
		if (line.StartsWith("\n") || line.StartsWith("\r"))
		{
			line = line.Substring(1);
		}

		if (line.EndsWith("\r") || line.EndsWith("\n"))
		{
			line = line.Substring(0, line.Length - 1);
		}

		return line;
	}

	private Dictionary<string, string> GetTranslations(List<string> lineColumns)
	{
		lineColumns.RemoveAt(0); //removing localizationId
		lineColumns.RemoveAt(0); //removing description column

		Dictionary<string, string> translations = new Dictionary<string, string>();
		for (int i = 0; i < lineColumns.Count; i++)
		{
			translations.Add(localizedLanguages[i], lineColumns[i]);
		}

		return translations;
	}

	private List<string> GetLocalizationFileLineColumns(string line)
	{
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
		List<string> languages = new List<string>();

		int cursor = 0;
		int columnIndex = 0;
		for (int i = 0; i < localizationFileHeader.Length; i++)
		{
			if (localizationFileHeader[i] == '\t')
			{
				if (columnIndex >= LOCALIZATION_FILE_FIRST_LANGUAGE_COLUMN)
				{
					languages.Add(localizationFileHeader.Substring(cursor, (i-cursor)));
				}

				columnIndex++;
				cursor = i + 1;
			}
		}

		languages.Add(localizationFileHeader.Substring(cursor));

		return languages;
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

	private LocalizationXmlModel FindLocalizationXmlModelWithLanguageId(string languageId)
	{
		return localizationXmlModels.Find((localizationXmlModel) => localizationXmlModel.languageId == languageId);
	}
}
