using UnityEngine;
using UnityEngine.UI;
using Messages;

public class Hud : MonoSingleton
{
	public HudChef hudChef;

	public VaccineBar vaccineBar;
	public GameObject dayPanel;
	public GameObject moneyPanel;
	public GameObject publicOpinionPanel;

	public Text growthValue;
	public Text moneyValue;
	public Text dayValue;
	public Text patients1Value;
	public Text patients2Value;
	public Text patients3Value;
	public Text patients4Value;
	public Text patients5Value;
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
		UpdateDayValues();
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
		string patientsText = session.patients[session.day - 1].ToString();
		patients1Value.text = patientsText;
		patients2Value.text = patientsText;
		patients3Value.text = patientsText;
		patients4Value.text = patientsText;
		patients5Value.text = patientsText;
		capacityValue.text = session.capacity.ToString();
		publicOpinionValue.text = session.publicOpinion + "%";
		vaccineBar.UpdateBar(session.vaccineDevelopment / 100f);
	}

	public void UpdateSuggestionOptions()
	{
		GameSession session = GameManager.instance.localPlayer.gameSession;
		growthValue.text = session.growthRate + "%";
		moneyValue.text = session.money.ToString();
		string patientsText = session.patients[session.day].ToString();
		patients1Value.text = patientsText;
		patients2Value.text = patientsText;
		patients3Value.text = patientsText;
		patients4Value.text = patientsText;
		patients5Value.text = patientsText;
		capacityValue.text = session.capacity.ToString();
		publicOpinionValue.text = session.publicOpinion + "%";
		vaccineBar.UpdateBar(session.vaccineDevelopment/100f);
	}
}
