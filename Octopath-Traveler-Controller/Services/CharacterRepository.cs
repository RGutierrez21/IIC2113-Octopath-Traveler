using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using Octopath_Traveler_Controller.Entities;

namespace Octopath_Traveler.Services;

public class CharacterRepository
{
    private Dictionary<string, Traveler> _catalog;

    public CharacterRepository(string jsonPath)
    {
        string jsonString = File.ReadAllText(jsonPath);
        List<Traveler> rawTravelers = JsonSerializer.Deserialize<List<Traveler>>(jsonString)!;
        _catalog = rawTravelers.ToDictionary(t => t.Name, t => t);
    }

    public Traveler GetTraveler(string name)
    {
        string cleanName = name.Contains('(') ? name.Split('(')[0].Trim() : name.Trim();
        if (_catalog.TryGetValue(cleanName, out Traveler? baseTraveler))
        {
            return CloneTraveler(baseTraveler);
        }

        throw new KeyNotFoundException($"El viajero '{name}' no existe en el catálogo.");
    }

    private Traveler CloneTraveler(Traveler original)
    {
        Stats clonedStats = new Stats
        {
            HP = original.Stats.HP,
            SP = original.Stats.SP,
            PhysAtk = original.Stats.PhysAtk,
            PhysDef = original.Stats.PhysDef,
            ElemAtk = original.Stats.ElemAtk,
            ElemDef = original.Stats.ElemDef,
            Speed = original.Stats.Speed
        };
        string[] clonedWeapons = original.Weapons.ToArray();
        Traveler copy = new Traveler(original.Name, clonedStats, clonedWeapons);
        return copy;
    }
}