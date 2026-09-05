using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Octopath_Traveler_Controller.Entities;

namespace Octopath_Traveler.Services;

public class BeastRepository
{
    private Dictionary<string, Beast> _catalog;

    // 1. DTO privado: Espejo exacto del archivo .json plano.
    // Protege a nuestra clase Beast de tener que adaptarse a la estructura del archivo.
    private class BeastDto
    {
        public string Name { get; set; } = string.Empty;
        public Stats Stats { get; set; } = new Stats();
        public string Skill { get; set; } = string.Empty;
        public int Shields { get; set; }
        public string[] Weaknesses { get; set; } = Array.Empty<string>();
    }

    public BeastRepository(string jsonPath)
    {
        string jsonString = File.ReadAllText(jsonPath);
        
        List<BeastDto> rawBeasts = JsonSerializer.Deserialize<List<BeastDto>>(jsonString) 
                                   ?? new List<BeastDto>();
        
        _catalog = new Dictionary<string, Beast>();
        
        foreach (BeastDto dto in rawBeasts)
        {
            BeastStats bStats = new BeastStats
            {
                Skill = dto.Skill,
                Shields = dto.Shields,
                Weaknesses = dto.Weaknesses
            };

            Beast cleanBeast = new Beast(dto.Name, dto.Stats, bStats);
            
            _catalog.Add(cleanBeast.Name, cleanBeast);
        }
    }

    public Beast GetBeast(string name)
    {
        if (_catalog.TryGetValue(name, out Beast? baseBeast))
        {
            return CloneBeast(baseBeast);
        }

        throw new KeyNotFoundException($"La bestia '{name}' no existe en el catálogo.");
    }

    private Beast CloneBeast(Beast original)
    {
        Stats clonedStats = new Stats
        {
            HP = original.Stats.HP,
            PhysAtk = original.Stats.PhysAtk,
            PhysDef = original.Stats.PhysDef,
            ElemAtk = original.Stats.ElemAtk,
            ElemDef = original.Stats.ElemDef,
            Speed = original.Stats.Speed
        };
        
        BeastStats clonedBeastStats = new BeastStats
        {
            Skill = original.Skill,
            Shields = original.Shields,
            Weaknesses = original.Weaknesses.ToArray() 
        };
        
        return new Beast(original.Name, clonedStats, clonedBeastStats);
    }
}