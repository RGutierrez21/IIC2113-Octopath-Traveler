using System.Collections.Generic;
using Octopath_Traveler_Controller.Entities;
using Octopath_Traveler_View;

namespace Octopath_Traveler.Views;

public class CombatView
{
    private readonly View _view;

    public CombatView(View view)
    {
        _view = view;
    }

    public void ShowRoundStart(int roundNumber)
    {
        _view.WriteLine("----------------------------------------");
        _view.WriteLine($"INICIA RONDA {roundNumber}");
        _view.WriteLine("----------------------------------------");
    }

    public void ShowBoardState(List<Traveler> travelers, List<Beast> beasts)
    {
        ShowTravelersState(travelers);
        ShowBeastsState(beasts);
    }

    private void ShowTravelersState(List<Traveler> travelers)
    {
        _view.WriteLine("Equipo del jugador");
        char position = 'A';
        foreach (var t in travelers)
        {
            _view.WriteLine($"{position}-{t.Name} - HP:{t.CurrentHP}/{t.Stats.HP} SP:{t.CurrentSP}/{t.Stats.SP} BP:{t.BP}");
            position++;
        }
    }

    private void ShowBeastsState(List<Beast> beasts)
    {
        _view.WriteLine("Equipo del enemigo");
        char position = 'A';
        foreach (var b in beasts)
        {
            _view.WriteLine($"{position}-{b.Name} - HP:{b.CurrentHP}/{b.Stats.HP} Shields:{b.Shields}");
            position++;
        }
        _view.WriteLine("----------------------------------------");
    }

    public void ShowTurnQueues(List<string> current, List<string> next)
    {
        PrintQueue("Turnos de la ronda", current);
        PrintQueue("Turnos de la siguiente ronda", next);
    }

    private void PrintQueue(string title, List<string> items)
    {
        _view.WriteLine(title);
        for (int i = 0; i < items.Count; i++)
        {
            _view.WriteLine($"{i + 1}.{items[i]}");
        }
        _view.WriteLine("----------------------------------------");
    }

    public void ShowSeparator()
    {
        _view.WriteLine("----------------------------------------");
    }

    public string PromptTravelerAction(string travelerName)
    {
        _view.WriteLine($"Turno de {travelerName}");
        _view.WriteLine("1: Ataque básico\n2: Usar habilidad\n3: Defender\n4: Huir");
        return _view.ReadLine();
    }

    public string PromptSkillSelection(string travelerName, string[] skills)
    {
        _view.WriteLine("----------------------------------------");
        _view.WriteLine($"Seleccione una habilidad para {travelerName}");
        for (int i = 0; i < skills.Length; i++)
        {
            _view.WriteLine($"{i + 1}: {skills[i]}");
        }
        _view.WriteLine($"{skills.Length + 1}: Cancelar");
        return _view.ReadLine();
    }

    public string PromptWeaponSelection(string[] weapons)
    {
        _view.WriteLine("----------------------------------------");
        _view.WriteLine("Seleccione un arma");
        for (int i = 0; i < weapons.Length; i++)
        {
            _view.WriteLine($"{i + 1}: {weapons[i]}");
        }
        _view.WriteLine($"{weapons.Length + 1}: Cancelar");
        return _view.ReadLine();
    }

    public string PromptTargetSelection(string travelerName, List<Beast> aliveBeasts)
    {
        _view.WriteLine("----------------------------------------");
        _view.WriteLine($"Seleccione un objetivo para {travelerName}");
        for (int i = 0; i < aliveBeasts.Count; i++)
        {
            _view.WriteLine($"{i + 1}: {aliveBeasts[i].Name} - HP:{aliveBeasts[i].CurrentHP}/{aliveBeasts[i].Stats.HP} Shields:{aliveBeasts[i].Shields}");
        }
        _view.WriteLine($"{aliveBeasts.Count + 1}: Cancelar");
        return _view.ReadLine();
    }

    public void PromptBoostPoints()
    {
        _view.WriteLine("----------------------------------------");
        _view.WriteLine("Seleccione cuantos BP utilizar");
        _view.ReadLine(); 
    }

    public void ShowTravelerAttack(string attacker, string target, int damage, string type, int targetHp)
    {
        _view.WriteLine("----------------------------------------");
        _view.WriteLine($"{attacker} ataca");
        _view.WriteLine($"{target} recibe {damage} de daño de tipo {type}");
        _view.WriteLine($"{target} termina con HP:{targetHp}");
    }

    public void ShowBeastAttack(string beastName, string beastSkill, string targetName, int damage, int targetHp)
    {
        _view.WriteLine($"{beastName} usa {beastSkill}");
        _view.WriteLine($"{targetName} recibe {damage} de daño físico");
        _view.WriteLine($"{targetName} termina con HP:{targetHp}");
    }

    public void ShowFleeMessage()
    {
        _view.WriteLine("----------------------------------------");
        _view.WriteLine("El equipo de viajeros ha huido!");
    }

    public void ShowWinner(bool playerWon)
    {
        _view.WriteLine("----------------------------------------");
        _view.WriteLine(playerWon ? "Gana equipo del jugador" : "Gana equipo del enemigo");
    }
}