public interface IGamePhase : ISavable
{
	void Start(int gamePhaseId, int startDay);
	void Stop();
	void Resume();
	void Dispose();

	void StartMusic();
	void StopMusic();

	string GetName();

	IAdvisorSpawnPolicy GetAdvisorSpawnPolicy();

	int GetNextPhaseId();
	int GetPhaseId();

	int GetStartDay();

	bool IsFinished(GameSession gameSession);
	void Advisor_Enter(GameSession gameSession);
}