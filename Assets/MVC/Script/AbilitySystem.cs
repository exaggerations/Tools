using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architecture.AbilitySystem
{
    public class AbilitySystem : MonoBehaviour
    {
        [SerializeField]AbilityView abilityView;
        [SerializeField] AbilityData[] startingAbilities;
        AbilityController controller;

        private void Awake()
        {
            controller = new AbilityController.Builder().WithAbilities(startingAbilities).Build(abilityView);

        }

        void Update()
        {
            controller.Update();
        }

    }
}