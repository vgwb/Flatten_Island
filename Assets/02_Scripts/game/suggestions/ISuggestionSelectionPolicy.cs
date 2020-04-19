public interface ISuggestionSelectionPolicy
{
	void Initialize(LocalPlayer localPlayer);
	void Reset();

	SuggestionXmlModel GetSuggestion(AdvisorXmlModel advisorXmlModel);
}