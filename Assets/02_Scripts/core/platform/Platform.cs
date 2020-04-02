using UnityEngine;

public abstract class Platform
{
	public string GetCurrentLanguage()
	{
		SystemLanguage systemLanguage = Application.systemLanguage;
		if (systemLanguage == SystemLanguage.Unknown)
		{
			return null;
		}

		return systemLanguage.ToString();
	}
}
