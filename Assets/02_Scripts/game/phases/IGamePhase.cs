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

	void DayStart_Enter();
	void ChangeGamePhase_Enter();
	void Advisor_Enter();
	void UpdateResult_Enter();
	void UpdateResult_Update(GameSessionFsm gameSessionFsm);
	void NextDayConfirmation_Enter();
	void NextDayConfirmation_Exit();
}