using UnityEngine;
using UnityEngine.UI;

public class GrowthPanel : MonoBehaviour
{
	public Text growthPanelText;
	public UIBlinkText uiBlinkText;

    void Update()
    {
		if (ChartManager.instance != null)
		{
			if (ChartManager.instance.IsGrowthRateInWarningZone())
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
				growthPanelText.enabled = true;
			}
		}
    }
}
