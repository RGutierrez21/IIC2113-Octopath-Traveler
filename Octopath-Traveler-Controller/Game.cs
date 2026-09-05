using System;
using System.Collections.Generic;
using System.IO;
using Octopath_Traveler_View;
using Octopath_Traveler.Services;
using Octopath_Traveler_Controller.Entities;
using Octopath_Traveler.Views;       
using Octopath_Traveler.Controllers; 

namespace Octopath_Traveler;

public class Game
{
    private const string MsgChooseFile = "Elige un archivo para cargar los equipos";
    private const string MsgInvalidTeam = "Archivo de equipos no válido";
    
    private readonly View _view;
    private readonly string _teamsFolder;
    private readonly TeamsFileReader _fileReader;
    private readonly TeamBuilderService _teamBuilder;

    public Game(View view, string teamsFolder)
    {
        _view = view;
        _teamsFolder = teamsFolder;
        _fileReader = new TeamsFileReader();
        
        CharacterRepository charRepo = new CharacterRepository("data/characters.json");
        BeastRepository beastRepo = new BeastRepository("data/enemies.json");
        GameCatalogs catalogs = new GameCatalogs(charRepo, beastRepo);
        TeamParser parser = new TeamParser();
        
        _teamBuilder = new TeamBuilderService(_fileReader, parser, catalogs);
    }

    public Game(View view, string teamsFolder, TeamBuilderService teamBuilder)
    {
        _view = view;
        _teamsFolder = teamsFolder;
        _fileReader = new TeamsFileReader();
        _teamBuilder = teamBuilder;
    }

    public void Play()
    {
        string[] files = _fileReader.GetAvailableFiles(_teamsFolder);
        string selectedFile = PromptUserForFile(files);
        
        if (selectedFile == null)
        {
            _view.WriteLine(MsgInvalidTeam);
            return;
        }

        TryStartBattle(selectedFile);
    }

    private string PromptUserForFile(string[] files)
    {
        _view.WriteLine(MsgChooseFile);
        PrintTeamsFiles(files);
        
        string userInput = _view.ReadLine();
        
        if (IsValidTeamsOption(userInput, files.Length))
        {
            return files[int.Parse(userInput)];
        }
        
        return null;
    }

    private void PrintTeamsFiles(string[] files)
    {
        for (int i = 0; i < files.Length; i++)
        {
            _view.WriteLine($"{i}: {Path.GetFileName(files[i])}");
        }
    }

    private bool IsValidTeamsOption(string userInput, int maxFiles)
    {
        return int.TryParse(userInput, out int index) && index >= 0 && index < maxFiles;
    }

    private void TryStartBattle(string path)
    {
        try
        {
            var (travelers, beasts) = _teamBuilder.BuildTeamsFromPath(path);
            InitializeCombat(travelers, beasts);
        }
        catch (Exception)
        {
            _view.WriteLine(MsgInvalidTeam);
        }
    }

    private void InitializeCombat(List<Traveler> travelers, List<Beast> beasts)
    {
        CombatView combatView = new CombatView(_view);
        BattleController battleController = new BattleController(combatView, travelers, beasts);
        battleController.StartBattle();
    }
}