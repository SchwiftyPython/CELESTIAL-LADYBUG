using System.Collections.Generic;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public enum CombatState
    {
        Start,
        PlayerTurn,
        AiTurn,
        EndTurn,
        EndCombat
    }

    public class CombatManager : MonoBehaviour
    {
        private CombatState _currentCombatState;
        private Queue<Entity> _turnOrder;
        private Entity _activeEntity;
        private CombatMap _map;

        private void Awake()
        {
        
        }

        private Queue<Entity> DetermineTurnOrder(List<Entity> combatants)
        {

        }

        private Entity GetNextInTurnOrder()
        {

        }

        private bool ActiveEntityPlayerControlled()
        {

        }
    }
}
