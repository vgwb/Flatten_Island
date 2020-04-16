using UnityEngine;
using Messages;

public class SuggestionMenu : MonoBehaviour
{
	private SuggestionEntry suggestionEntry;
	private SuggestionResultEntry suggestionResultEntry;
	private AdvisorXmlModel advisorXmlModel;

	private void Awake()
	{
		suggestionEntry = null;
		suggestionResultEntry = null;
		advisorXmlModel = null;
	}

	private void OnEnable()
	{
		EventMessageHandler suggestionEntryExitCompletedMessageHandler = new EventMessageHandler(this, OnSuggestionEntryExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(SuggestionEntryExitCompletedEvent).Name, suggestionEntryExitCompletedMessageHandler);

		EventMessageHandler suggestionResultEntryExitCompletedMessageHandler = new EventMessageHandler(this, OnSuggestionResultEntryExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(SuggestionResultEntryExitCompletedEvent).Name, suggestionResultEntryExitCompletedMessageHandler);
	}

	private void OnDisable()
	{
		EventMessageManager.instance.RemoveHandler(typeof(SuggestionEntryExitCompletedEvent).Name, this);
		EventMessageManager.instance.RemoveHandler(typeof(SuggestionResultEntryExitCompletedEvent).Name, this);
	}

	public void ShowSuggestion(SuggestionXmlModel suggestionXmlModel, AdvisorXmlModel advisorXmlModel)
	{
		this.advisorXmlModel = advisorXmlModel;
		suggestionEntry = CreateSuggestionEntry(suggestionXmlModel, advisorXmlModel);
		suggestionEntry.PlayEnterRecipe();
	}

	private SuggestionEntry CreateSuggestionEntry(SuggestionXmlModel suggestionXmlModel, AdvisorXmlModel advisorXmlModel)
	{
		GameObject suggestionEntry = GameObjectFactory.instance.InstantiateGameObject(SuggestionEntry.PREFAB, this.transform, false);
		suggestionEntry.gameObject.transform.SetParent(this.transform, true);
		SuggestionEntry suggestionEntryScript = suggestionEntry.GetComponent<SuggestionEntry>();
		suggestionEntryScript.SetSuggestion(suggestionXmlModel, advisorXmlModel);
		suggestionEntry.gameObject.SetActive(true);
		suggestionEntry.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		return suggestionEntryScript;
	}

	private void OnSuggestionEntryExitCompleted(EventMessage eventMessage)
	{
		Debug.Log("Suggestion Entry has exited");
		suggestionEntry.gameObject.SetActive(false);
		GameObjectFactory.instance.ReleaseGameObject(suggestionEntry.gameObject, SuggestionEntry.PREFAB);
	}

	public void ShowSuggestionResult(SuggestionOptionXmlModel suggestionOptionXmlModel)
	{
		suggestionResultEntry = CreateSuggestionResultEntry(suggestionOptionXmlModel);
		suggestionResultEntry.PlayEnterRecipe();
	}

	private SuggestionResultEntry CreateSuggestionResultEntry(SuggestionOptionXmlModel suggestionOptionXmlModel)
	{
		GameObject suggestionResultEntry = GameObjectFactory.instance.InstantiateGameObject(SuggestionResultEntry.PREFAB, this.transform, false);
		suggestionResultEntry.gameObject.transform.SetParent(this.transform, true);
		SuggestionResultEntry suggestionResultEntryScript = suggestionResultEntry.GetComponent<SuggestionResultEntry>();
		suggestionResultEntryScript.SetSuggestionResult(suggestionOptionXmlModel, advisorXmlModel);
		suggestionResultEntry.gameObject.SetActive(true);
		suggestionResultEntry.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		return suggestionResultEntryScript;
	}

	private void OnSuggestionResultEntryExitCompleted(EventMessage eventMessage)
	{
		SuggestionResultEntryExitCompletedEvent suggestionResultEntryExitCompletedEvent = eventMessage.eventObject as SuggestionResultEntryExitCompletedEvent;
		GameObjectFactory.instance.ReleaseGameObject(suggestionResultEntry.gameObject, SuggestionResultEntry.PREFAB);

		Debug.Log("Suggestion Entry has exited");

		suggestionResultEntry = null;
		suggestionEntry = null;
		advisorXmlModel = null;
	}
}
