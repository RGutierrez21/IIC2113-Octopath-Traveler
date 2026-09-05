namespace Octopath_Traveler_Controller.Entities;

public class Traveler : Unit
{
    public string[] Weapons { get; private set; }
    public int CurrentSP { get; set; }
    public int BP { get; set; }

    public Traveler(string name, Stats stats, string[] weapons)
        : base(name, stats)
    {
        Weapons = weapons;
        CurrentSP = stats.SP;
        BP = 1;
    }
}