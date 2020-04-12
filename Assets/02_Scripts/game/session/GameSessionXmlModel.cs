using System.Xml.Linq;

public class GameSessionXmlModel : XmlModel
{
	public int initialPatients;
	public int initialCapacity;
	public int initialGrowthRate;
	public int initialMoney;
	public int initialPublicOpinion;
	public int initialVaccineDevelopment;
	
	override public void Initialize(XElement element)
	{
		base.Initialize(element);

		initialPatients = ParseIntAttribute(element, "initialPatients");
		initialCapacity = ParseIntAttribute(element, "initialCapacity");
		initialGrowthRate = ParseIntAttribute(element, "initialGrowthRate");
		initialMoney = ParseIntAttribute(element, "initialMoney");
		initialPublicOpinion = ParseIntAttribute(element, "initialPublicOpinion");
		initialVaccineDevelopment = ParseIntAttribute(element, "initialVaccineDevelopment");
	}
}
