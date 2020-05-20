using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using Messages;

public class PrivacyDialog : MonoBehaviour
{
	public static string TITLE_ID = "PrivacyTitle";
	public static string MESSAGE_ID = "PrivacyText";
	public static string BUTTON_PRIVACY_POLICY_ID = "ButtonPrivacyPolicy";
	public static string BUTTON_ANALYTICS_OPTOUT_ID = "ButtonAnalyticsOpOut";

	public static string PREFAB = "GUI/PrivacyDialog";

	public PrivacyDialogChef privacyDialogChef;

	public Text titleText;
	public Text messageText;
	public Button analyticsOptOutButton;
	public Text analyticsOptOutButtonText;
	public Button privacyPolicyButton;
	public Text privacyPolicyButtonText;
	public string privacyPolicyUrl;

	private EPrivacyDialogResult result;

	private void OnEnable()
	{
		result = EPrivacyDialogResult.NOT_SET;
	}

	public void OnExitDialogSelected()
	{
		result = EPrivacyDialogResult.NOT_SET;
		privacyDialogChef.Cook(privacyDialogChef.onExitRecipe, OnExitRecipeCompleted);
	}

	public void OnAnalyticsOptPutButtonSelected()
	{
		result = EPrivacyDialogResult.ANALYTICS_OPT_OUT;
		privacyDialogChef.Cook(privacyDialogChef.onExitRecipe, OnExitRecipeCompleted);
	}

	public void OnPrivacyPolicyButtonSelected()
	{
		result = EPrivacyDialogResult.SHOW_PRIVACY_POLICY;
		privacyDialogChef.Cook(privacyDialogChef.onExitRecipe, OnExitRecipeCompleted);
	}

	private void OnExitRecipeCompleted()
	{
		Debug.Log("Privacy Dialog Exit Recipe completed");

		ExecutePlayerAction();

		SendExitCompletedEvent();

		gameObject.SetActive(false);
		GameObjectFactory.instance.ReleaseGameObject(gameObject, PREFAB);
	}

	private void ExecutePlayerAction()
	{
		switch (result)
		{
			case EPrivacyDialogResult.SHOW_PRIVACY_POLICY:
				if (!string.IsNullOrEmpty(privacyPolicyUrl))
				{
					if (Application.platform == RuntimePlatform.WebGLPlayer)
					{
						Application.ExternalEval("window.open('" + privacyPolicyUrl + "');");
					}
					else
					{ 
						Application.OpenURL(privacyPolicyUrl);

					}
				}
				break;

			case EPrivacyDialogResult.ANALYTICS_OPT_OUT:
				DataPrivacy.FetchPrivacyUrl(OnAnalyticsOptOutURLReceived);
				break;
		}
	}

	void OnAnalyticsOptOutURLReceived(string url)
	{
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			Application.ExternalEval("window.open('" + url + "');");
		}
		else
		{
			Application.OpenURL(url);

		}
	}

	private void SendExitCompletedEvent()
	{
		PrivacyDialogExitCompletedEvent exitCompletedEvent = PrivacyDialogExitCompletedEvent.CreateInstance(result);
		EventMessage exitCompletedEventMessage = new EventMessage(this, exitCompletedEvent);
		exitCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(exitCompletedEventMessage);
	}

	private void SendEnterCompletedEvent()
	{
		PrivacyDialogEnterCompletedEvent enterCompletedEvent = PrivacyDialogEnterCompletedEvent.CreateInstance(result);
		EventMessage enterCompletedEventMessage = new EventMessage(this, enterCompletedEvent);
		enterCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(enterCompletedEventMessage);
	}

	private void PlayEnterRecipe()
	{
		privacyDialogChef.Cook(privacyDialogChef.onEnterRecipe, OnEnterRecipeCompleted);
	}

	private void OnEnterRecipeCompleted()
	{
		Debug.Log("Privacy Dialog Enter Recipe completed");
		SendEnterCompletedEvent();
	}

	private static PrivacyDialog CreatePrivacyDialog(Transform parent)
	{
		GameObject privacyDialogObject = GameObjectFactory.instance.InstantiateGameObject(PREFAB, parent, false);
		privacyDialogObject.transform.SetParent(parent, true);
		PrivacyDialog privacyDialog = privacyDialogObject.GetComponent<PrivacyDialog>();
		LocalizationManager.instance.SetLocalizedText(privacyDialog.titleText, TITLE_ID);
		LocalizationManager.instance.SetLocalizedText(privacyDialog.messageText, MESSAGE_ID);
		LocalizationManager.instance.SetLocalizedText(privacyDialog.analyticsOptOutButtonText, BUTTON_ANALYTICS_OPTOUT_ID);
		LocalizationManager.instance.SetLocalizedText(privacyDialog.privacyPolicyButtonText, BUTTON_PRIVACY_POLICY_ID);
		privacyDialogObject.gameObject.SetActive(true);
		privacyDialogObject.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		return privacyDialog;
	}
	public static PrivacyDialog Show(Transform parent)
	{
		PrivacyDialog privacyDialog = CreatePrivacyDialog(parent);
		privacyDialog.PlayEnterRecipe();
		return privacyDialog;
	}
}
