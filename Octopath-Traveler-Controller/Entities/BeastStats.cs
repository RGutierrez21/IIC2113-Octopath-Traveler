using System;

namespace Octopath_Traveler_Controller.Entities;

public class BeastStats
{
    public string Skill { get; set; } = string.Empty;
    public int Shields { get; set; }
    public string[] Weaknesses { get; set; } = Array.Empty<string>();
}