using UnityEngine;
using UnityEngine.UI;

public class MoneyPanel : MonoBehaviour
{
	public Text moneyText;
	public UIBlinkText uiBlinkText;

    void Update()
    {
		GameSession gameSession = GameManager.instance.localPlayer.gameSession;
		if (gameSession != null)
		{
			if (gameSession.money < gameSession.gameSessionXmlModel.moneyWarningThreshold)
			{
				if (!uiBlinkText.enabled)
				{
					uiBlinkText.ResetBlinkingTimer();
					uiBlinkText.enabled = true;
				}
			}
			else
			{
				uiBlinkText.enabled = false;
				moneyText.enabled = true;
			}
		}
    }
}
