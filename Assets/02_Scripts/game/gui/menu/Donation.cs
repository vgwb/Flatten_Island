using UnityEngine;

public class Donation : MonoBehaviour
{
	public string urlLocalizationId;
	private string url;

	private void Start()
	{
		url = LocalizationManager.instance.GetPlainText(urlLocalizationId);
	}

	public void CallForDonation()
    {
		if (!string.IsNullOrEmpty(url))
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
	}
}
