namespace Octopath_Traveler_Controller.Entities;

public class Beast : Unit
{
    private BeastStats _beastStats;
    // Properties read only
    public string Skill => _beastStats.Skill;
    public int Shields => _beastStats.Shields;
    public string[] Weaknesses => _beastStats.Weaknesses;
    public Beast(string name, Stats stats, BeastStats beastStats)
        : base(name, stats)
    {
        _beastStats = beastStats;
    }
}