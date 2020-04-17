using UnityEngine;
using UnityEngine.UI;
using Messages;

public class Hud : MonoSingleton
{
	public HudChef hudChef;

	public VaccineBar vaccineBar;

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
		vaccineBar.Initialize();
	}

	public void Unsetup()
	{
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

	public void UpdateSuggestionOptions()
	{
		GameSession session = GameManager.instance.localPlayer.gameSession;
		growthValue.text = session.growthRate + "%";
		moneyValue.text = session.money.ToString();
		patientsValue.text = session.patients[session.day].ToString();
		capacityValue.text = session.capacity.ToString();
		publicOpinionValue.text = session.publicOpinion + "%";
		vaccineBar.UpdateBar(session.vaccineDevelopment/100f);
	}
}
