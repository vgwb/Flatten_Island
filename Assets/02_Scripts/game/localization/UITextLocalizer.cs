using UnityEngine;
using UnityEngine.UI;

public class UITextLocalizer : MonoBehaviour
{
	public string localizationId;

	private Text text;

	private void Awake()
	{
		text = GetComponent<Text>();
	}

	private void OnEnable()
	{
		LocalizationManager.instance.SetLocalizedText(text, localizationId);
	}
}