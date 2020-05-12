using UnityEngine;

public class Donation : MonoBehaviour
{
	public string url;

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
