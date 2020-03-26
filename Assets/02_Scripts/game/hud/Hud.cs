using UnityEngine;
using UnityEngine.UI;
using Messages;

public class Hud : MonoSingleton
{
	public HudChef hudChef;

	public static Hud instance
	{
		get
		{
			return GetInstance<Hud>();
		}
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	public void Setup()
	{
		//ActivateGroup(healthGroup, true);
	}

	public void Unsetup()
	{
		//ActivateGroup(healthGroup, false);
	}

	private void ActivateGroup(GameObject group, bool active)
	{
		group.SetActive(active);
	}

	public void DestroyHud()
	{
		Destroy(gameObject);
	}
}
