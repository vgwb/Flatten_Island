using System.Xml.Linq;

public class GameSessionXmlModel : XmlModel
{
	public int initialPatients;
	public int initialCapacity;
	public int initialGrowthRate;
	public int initialMoney;
	public int initialPublicOpinion;
	public int initialVaccineDevelopment;

	public int initialTutorialPatients;
	public int initialTutorialCapacity;
	public int initialTutorialGrowthRate;
	public int initialTutorialMoney;
	public int initialTutorialPublicOpinion;
	public int initialTutorialVaccineDevelopment;

	public int nextDayMoneyIncrement;
	public int nextDayVaccineIncrement;
	public int nextDayGrowthRateIncrement;

	public int publicOpinionWarningThreshold;
	public int moneyWarningThreshold;
	public float capacityWarningThreshold;

	override public void Initialize(XElement element)
	{
		base.Initialize(element);

		initialPatients = ParseIntAttribute(element, "initialPatients");
		initialCapacity = ParseIntAttribute(element, "initialCapacity");
		initialGrowthRate = ParseIntAttribute(element, "initialGrowthRate");
		initialMoney = ParseIntAttribute(element, "initialMoney");
		initialPublicOpinion = ParseIntAttribute(element, "initialPublicOpinion");
		initialVaccineDevelopment = ParseIntAttribute(element, "initialVaccineDevelopment");

		initialTutorialPatients = ParseIntAttribute(element, "initialTutorialPatients");
		initialTutorialCapacity = ParseIntAttribute(element, "initialTutorialCapacity");
		initialTutorialGrowthRate = ParseIntAttribute(element, "initialTutorialGrowthRate");
		initialTutorialMoney = ParseIntAttribute(element, "initialTutorialMoney");
		initialTutorialPublicOpinion = ParseIntAttribute(element, "initialTutorialPublicOpinion");
		initialTutorialVaccineDevelopment = ParseIntAttribute(element, "initialTutorialVaccineDevelopment");

		nextDayMoneyIncrement = ParseIntAttribute(element, "nextDayMoneyIncrement");
		nextDayVaccineIncrement = ParseIntAttribute(element, "nextDayVaccineIncrement");
		nextDayGrowthRateIncrement = ParseIntAttribute(element, "nextDayGrowthRateIncrement");

		publicOpinionWarningThreshold = ParseIntAttribute(element, "publicOpinionWarningThreshold");
		moneyWarningThreshold = ParseIntAttribute(element, "moneyWarningThreshold");
		capacityWarningThreshold = ParseFloatAttribute(element, "capacityWarningThreshold");
	}
}
