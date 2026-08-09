using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architecture.AbilitySystem
{
    public interface IAbilityCommand
    {
        void Execute();
    }

    public class AbilityCommand : IAbilityCommand
    {
        readonly AbilityData abilityData;
        public float duration =>abilityData.duration;

        public AbilityCommand(AbilityData abilityData)
        {
            this.abilityData = abilityData;
        }   
        public void Execute()
        {
            Debug.Log("AbilityCommand executed.");
        }
    }
}

