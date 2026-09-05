using System.Collections.Generic;
using System.Linq;
using Octopath_Traveler_Controller.Entities;

namespace Octopath_Traveler.Services;

public class TurnManager
{
    private readonly List<Unit> _allUnits;
    private Queue<Unit> _currentQueue;

    public TurnManager(List<Traveler> travelers, List<Beast> beasts)
    {
        _allUnits = new List<Unit>();
        _allUnits.AddRange(travelers);
        _allUnits.AddRange(beasts);
        _currentQueue = new Queue<Unit>();
    }

    public void PrepareNextRound()
    {
        _currentQueue = new Queue<Unit>(GetAliveUnitsSortedBySpeed());
    }

    public Unit PeekNextUnit() => _currentQueue.Peek();

    public Unit DequeueNextUnit() => _currentQueue.Dequeue();

    public bool HasPendingTurns() => _currentQueue.Count > 0;

    public List<string> GetCurrentRoundAliveNames()
    {
        return _currentQueue.Where(IsAlive).Select(u => u.Name).ToList();
    }

    public List<string> GetNextRoundAliveNames()
    {
        return GetAliveUnitsSortedBySpeed().Select(u => u.Name).ToList();
    }

    private List<Unit> GetAliveUnitsSortedBySpeed()
    {
        return _allUnits
            .Where(IsAlive)
            .Select((unit, index) => new { Unit = unit, Index = index, IsBeast = unit is Beast ? 1 : 0 })
            .OrderByDescending(x => x.Unit.Stats.Speed)
            .ThenBy(x => x.IsBeast)
            .ThenBy(x => x.Index)
            .Select(x => x.Unit)
            .ToList();
    }

    public bool IsAlive(Unit unit) => unit.CurrentHP > 0;
}