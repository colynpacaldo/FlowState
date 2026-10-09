namespace FlowState.Models;

/// <summary>A task. Daily missions are cheap and repeatable; Heavy quests cost more energy for bigger rewards.</summary>
public class Mission
{
    public int Id; public string Name = ""; public Cat Cat; public bool Heavy; public int Cost;
    public bool Claimed;
    public int Exp => (int)(Cost * (Heavy ? 2.5 : 1.5));
    public int Gold => (int)(Cost * (Heavy ? 1.25 : 0.75));
}
