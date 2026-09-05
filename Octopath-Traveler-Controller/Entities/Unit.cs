namespace Octopath_Traveler_Controller.Entities;

public abstract class Unit
{
    public string Name { get; protected set; }
    public Stats Stats { get; protected set; }
    public int CurrentHP { get; protected set; }

    protected Unit(string name, Stats stats)
    {
        Name = name;
        Stats = stats;
        CurrentHP = stats.HP;
    }

    public void TakeDamage(int damage)
    {
        CurrentHP = Math.Max(0, CurrentHP - damage);
    }
}