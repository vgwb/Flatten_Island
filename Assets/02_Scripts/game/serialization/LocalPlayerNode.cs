public class LocalPlayerNode
{
    public const int MAX_DAYS = 110;
    public const int MAX_PATIENTS = 10000;
     
    public int day { get; set; }
    public int[] patients { get; set; }
    public int growthRate { get; set; }
    public int capacity { get; set; }
    public int money { get; set; }
    public float publicOpinion { get; set; }

	public LocalPlayerNode()
	{
		// TODO provisional initial state
		day = 1;
		patients = new int[MAX_DAYS];
		patients[0] = 1000;
		growthRate = 5;
		capacity = 1000;
		money = 10000;
		publicOpinion = 0.5f;
	}

    // TODO apply a suggestion's effect
    public void IncreaseDayWithSuggestion() {
		money-=400;
		growthRate--;
		int patientsIncrease = (patients[day-1] * growthRate) / 100;
		patients[day] = patients[day-1] + patientsIncrease; 
        day++;
    }

    public void IncreaseDayWithoutMeasures() {
		money+=500;
		growthRate++; // TODO, Remove. just to play with the chart
		int patientsIncrease = (patients[day-1] * growthRate) / 100;
		patients[day] = patients[day-1] + patientsIncrease; 
        day++;
    }
}