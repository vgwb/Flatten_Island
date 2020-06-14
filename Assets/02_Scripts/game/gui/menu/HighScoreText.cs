using UnityEngine.UI;

public class HighScoreText : UILocalizedText
{
    public Text highScoreText;
	public string localizationId;

	private int day;

	public void DisplayDayHighScore(int day)
	{
		this.day = day;
		RefreshText();
	}

	protected override void RefreshText()
	{
		base.RefreshText();
		LocalizationManager.instance.SetLocalizedTextWithParameter(highScoreText, localizationId, "%d%", day.ToString());
	}
}
