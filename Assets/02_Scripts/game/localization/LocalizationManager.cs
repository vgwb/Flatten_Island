using ArabicSupport;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Messages;

public class LocalizationManager : MonoSingleton
{
	public TextAsset localizationFile;
	public Font latinFont;

	public static string DEFAULT_LANGUAGE_ID = "Spanish";
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

		EventMessageHandler languageSelectedMessageHandler = new EventMessageHandler(this, OnLanguageSelectedEvent);
		EventMessageManager.instance.AddHandler(typeof(LanguageSelectedEvent).Name, languageSelectedMessageHandler);
	}

	protected override void OnMonoSingletonDestroyed()
	{
		EventMessageManager.instance.RemoveHandler(typeof(LanguageSelectedEvent).Name, this);

		base.OnMonoSingletonDestroyed();
	}

	private void OnLanguageSelectedEvent(EventMessage eventMessage)
	{
		LanguageSelectedEvent languageSelectedEvent = eventMessage.eventObject as LanguageSelectedEvent;

		LocalizationXmlModel currentLanguageXmlModel = instance.GetCurrentLanguage();

		if (languageSelectedEvent.languageXmlModel.id != currentLanguageXmlModel.id)
		{
			GameManager.instance.localPlayer.SetLanguageId(languageSelectedEvent.languageXmlModel.languageId);
			GameManager.instance.SavePlayer();

			SendLanguageChangedEvent();
		}
	}

	private void SendLanguageChangedEvent()
	{
		LanguageChangedEvent languageChangedEvent = LanguageChangedEvent.CreateInstance(GetCurrentLanguage());
		EventMessage languageChangedEventMessage = new EventMessage(this, languageChangedEvent);
		languageChangedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(languageChangedEventMessage);
	}

	public LocalizationXmlModel GetCurrentLanguage()
	{
		LocalPlayer localPlayer = GameManager.instance.localPlayer;
		return FindLocalizationXmlModelWithLanguageId(localPlayer.GetLanguageId());
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

		localizedText = localizedText.Replace("%n%", Environment.NewLine);
		TryFixRightToLeftText(textField, localizedText, localizationId);
		Canvas.ForceUpdateCanvases();
	}

	public void SetLocalizedTextWithParameter(Text textField, string localizationId, string parameter, string parameterValue)
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

		localizedText = localizedText.Replace("%n%", Environment.NewLine);
		localizedText = localizedText.Replace(parameter, parameterValue.ToString());

		TryFixRightToLeftText(textField, localizedText, localizationId);

		Canvas.ForceUpdateCanvases();
	}


	private void TryFixRightToLeftText(Text textField, string localizedText, string localizationId)
	{
		LocalizationXmlModel currentLanguageXmlModel = instance.GetCurrentLanguage();
		if (currentLanguageXmlModel.isRightToLeft)
		{
			string rtlText = ArabicFixer.Fix(localizedText, false, false);
			rtlText = rtlText.Replace("\r", ""); // the Arabix fixer Return \r\n for everyy \n .. need to be removed

			string finalText = "";
			string[] rtlParagraph = rtlText.Split('\n');

			textField.text = "";
			for (int lineIndex = 0; lineIndex < rtlParagraph.Length; lineIndex++)
			{
				string[] words = rtlParagraph[lineIndex].Split(' ');
				Array.Reverse(words);
				textField.text = string.Join(" ", words);

				Canvas.ForceUpdateCanvases();
				//Debug.LogWarning("text:" + textField.text + " - lines count For Layout:" + textField.cachedTextGeneratorForLayout.lines.Count + " lines count:" + textField.cachedTextGenerator.lines.Count);

				if (textField.cachedTextGenerator.lines.Count > 0)
				{
					for (int i = 0; i < textField.cachedTextGenerator.lines.Count; i++)
					{
						int startIndex = textField.cachedTextGenerator.lines[i].startCharIdx;
						int endIndex = (i == textField.cachedTextGenerator.lines.Count - 1) ? textField.text.Length : textField.cachedTextGenerator.lines[i + 1].startCharIdx;
						int length = endIndex - startIndex;
						//Debug.LogWarning("line " + i + " - startIndex:" + startIndex + " endIndex:" + endIndex + " length:" + length );

						if (length > 0)
						{
							if (length > textField.text.Length)
							{
								Debug.LogWarning("localization Id:" + localizationId + " length:" + length + " > textField length:" + textField.text.Length);
								length = textField.text.Length;
							}

							string[] lineWords = textField.text.Substring(startIndex, length).Split(' ');
							Array.Reverse(lineWords);
							finalText = finalText + string.Join(" ", lineWords).Trim() + Environment.NewLine;
							//Debug.LogWarning("finalText:" + finalText);
						}
						else
						{
							Debug.LogWarning("localizationId:" + localizationId + " length <= 0");
						}
					}
				}
				else
				{
					finalText = rtlText;
				}
			}
			textField.text = finalText.TrimEnd('\n');
		}
		else
		{
			textField.text = localizedText;
		}
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

	public string GetPlainText(string localizationId)
	{
		return GetText(localizationId);
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

	public LocalizationXmlModel FindLocalizationXmlModelWithLanguageId(string languageId)
	{
		return localizationXmlModels.Find((localizationXmlModel) => localizationXmlModel.languageId == languageId);
	}
}
