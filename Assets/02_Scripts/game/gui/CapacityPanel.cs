using UnityEngine;
using UnityEngine.UI;

public class CapacityPanel : MonoBehaviour
{
	public Text capacityPanel;
	public UIBlinkText uiBlinkText;

    void Update()
    {
		if (ChartManager.instance != null)
		{
			if (ChartManager.instance.IsCapacityInWarningZone())
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
				capacityPanel.enabled = true;
			}
		}
    }
}
