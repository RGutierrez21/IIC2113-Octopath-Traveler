using System;
using System.Collections.Generic;
using System.Linq;
using Octopath_Traveler_Controller.Entities;

namespace Octopath_Traveler.Services;

public record GameCatalogs(CharacterRepository Characters, BeastRepository Beasts);

public class TeamBuilderService
{
    private const int MinTeamSize = 1;
    private const int MaxTeamSize = 4;

    private readonly TeamsFileReader _fileReader;
    private readonly TeamParser _teamParser;
    private readonly CharacterRepository _charRepo;
    private readonly BeastRepository _beastRepo;
    
    public TeamBuilderService(TeamsFileReader reader, TeamParser parser, GameCatalogs catalogs)
    {
        _fileReader = reader;
        _teamParser = parser;
        _charRepo = catalogs.Characters;
        _beastRepo = catalogs.Beasts;
    }
    
    public (List<Traveler>, List<Beast>) BuildTeamsFromPath(string path)
    {
        string[] rawLines = _fileReader.ReadValidLines(path);
        ParsedTeams parsedTeams = _teamParser.Parse(rawLines);
        
        List<Traveler> travelers = BuildTravelers(parsedTeams.PlayerNames);
        List<Beast> beasts = BuildBeasts(parsedTeams.EnemyNames);
        
        ValidateTeamsIntegrity(travelers, beasts);
        
        return (travelers, beasts);
    }
    
    private List<Traveler> BuildTravelers(List<string> names)
    {
        List<Traveler> travelers = new List<Traveler>();
        foreach (string name in names)
        {
            Traveler traveler = _charRepo.GetTraveler(name);
            if (traveler == null) throw new FormatException($"Personaje no encontrado: {name}");
            travelers.Add(traveler);
        }
        return travelers;
    }
    
    private List<Beast> BuildBeasts(List<string> names)
    {
        List<Beast> beasts = new List<Beast>();
        foreach (string name in names)
        {
            Beast beast = _beastRepo.GetBeast(name);
            if (beast == null) throw new FormatException($"Bestia no encontrada: {name}");
            beasts.Add(beast);
        }
        return beasts;
    }

    private void ValidateTeamsIntegrity(List<Traveler> travelers, List<Beast> beasts)
    {
        if (travelers.Count < MinTeamSize || travelers.Count > MaxTeamSize ||
            beasts.Count < MinTeamSize || beasts.Count > MaxTeamSize)
        {
            throw new FormatException("Tamaño de equipo fuera de rango.");
        }

        var uniqueTravelers = new HashSet<string>();
        foreach (var traveler in travelers)
        {
            if (!uniqueTravelers.Add(traveler.Name))
                throw new FormatException("Viajero duplicado.");
        }
    }
}