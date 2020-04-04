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
		day = 10;
		patients = new int[MAX_DAYS];
		patients[0] = 1000;
		patients[1] = 1300;
		patients[2] = 1500;
		patients[3] = 1800;
		patients[4] = 2000;
		patients[5] = 2500;
		patients[6] = 2800;
		patients[7] = 3000;
		patients[8] = 3100;
		patients[9] = 3000;
		growthRate = 1;
		capacity = 1000;
		money = 10000;
		publicOpinion = 0.5f;
	}

    public void IncreaseDayWithoutMeasures() {
		int patientsIncrease = (patients[day-1] * growthRate) / 100;
		patients[day] = patients[day-1] + patientsIncrease; 
        day++;
    }
}