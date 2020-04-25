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

		LocalizationManager.instance.SetLocalizedText(highScoreText, localizationId);
		highScoreText.text = LocalizationManager.instance.ReplaceIntInText(highScoreText.text, day);
	}
}
