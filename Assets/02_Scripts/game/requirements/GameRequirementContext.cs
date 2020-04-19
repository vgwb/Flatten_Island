public class GameRequirementContext : RequirementContext
{
	public LocalPlayer localPlayer;

	public GameRequirementContext(LocalPlayer localPlayer)
	{
		this.localPlayer = localPlayer;
	}
}