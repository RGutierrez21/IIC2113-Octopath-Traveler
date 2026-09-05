using System;
using System.Collections.Generic;
using System.Linq;

namespace Octopath_Traveler.Services;

public class TeamParser
{
    private const string PlayerTeamHeader = "Player Team";
    private const string EnemyTeamHeader = "Enemy Team";
    private const int MaxTeamSize = 4;

    public ParsedTeams Parse(string[] rawLines)
    {
        if (rawLines == null || rawLines.Length < 2)
            throw new FormatException("Archivo de equipos vacío o insuficiente.");

        if (rawLines[0].Trim() != PlayerTeamHeader)
            throw new FormatException("Cabecera de jugador inválida.");

        int enemyHeaderIndex = Array.IndexOf(rawLines, EnemyTeamHeader);
        if (enemyHeaderIndex <= 0)
            throw new FormatException("Cabecera de enemigo faltante o mal ubicada.");

        List<string> players = ExtractTeam(rawLines, 1, enemyHeaderIndex);
        List<string> enemies = ExtractTeam(rawLines, enemyHeaderIndex + 1, rawLines.Length);

        // Validación estricta de tamaños de equipo
        if (players.Count == 0 || enemies.Count == 0 || players.Count > MaxTeamSize || enemies.Count > MaxTeamSize)
            throw new FormatException("Tamaño de equipo no válido.");

        return new ParsedTeams(players, enemies);
    }

    private List<string> ExtractTeam(string[] lines, int startIndex, int endIndex)
    {
        List<string> team = new List<string>();
        for (int i = startIndex; i < endIndex; i++)
        {
            string line = lines[i]?.Trim();
            if (!string.IsNullOrEmpty(line))
            {
                team.Add(line);
            }
        }
        return team;
    }
}