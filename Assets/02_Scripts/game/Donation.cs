using UnityEngine;

public class Donation : MonoBehaviour
{
	public string url;

    public void CallForDonation()
    {
		if (!string.IsNullOrEmpty(url))
		{
			Application.OpenURL(url);
		}
	}
}
