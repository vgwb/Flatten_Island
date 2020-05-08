using UnityEngine;
using UnityEngine.UI;

public class PublicOpinionPanel : MonoBehaviour
{
	public Text publicOpinionText;
	public UIBlinkText uiBlinkText;

    void Update()
    {
		GameSession gameSession = GameManager.instance.localPlayer.gameSession;
		if (gameSession != null)
		{
			if (gameSession.publicOpinion < gameSession.gameSessionXmlModel.publicOpinionWarningThreshold)
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
				publicOpinionText.enabled = true;
			}
		}
    }
}
