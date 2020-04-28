public interface IGamePhase : ISavable
{
	void Start(GameSession gameSession, int gamePhaseId, int startDay);
	void Stop();
	void Resume(GameSession gameSession);
	void Dispose();

	void StartMusic();
	void StopMusic();

	string GetName();

	IAdvisorSpawnPolicy GetAdvisorSpawnPolicy();

	int GetNextPhaseId();
	int GetPhaseId();

	int GetStartDay();

	bool IsFinished();
	void Advisor_Enter();
	void NextDayConfirmation_Enter();
}