using UnityEngine;
using UnityEngine.UI;
using Messages;

public class Hud : MonoSingleton
{
	public HudChef hudChef;

	public Text growthValue;
	public Text moneyValue;
	public Text dayValue;
	public Text patientsValue;
	public Text publicOpinionValue;
	public Text capacityValue;

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

	public void UpdateDayValues()
	{
		GameSession session = GameManager.instance.localPlayer.gameSession;
		growthValue.text = session.growthRate + "%";
		moneyValue.text = session.money.ToString();
		dayValue.text = session.day.ToString();
		patientsValue.text = session.patients[session.day - 1].ToString();
		capacityValue.text = session.capacity.ToString();
		publicOpinionValue.text = session.publicOpinion + "%";
	}
}
