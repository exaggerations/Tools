using Architecture.AbilitySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityView : MonoBehaviour
{
    [SerializeField] public AbilityButton [] abilityButtons;

    public Key[] keys = {Key.Digit0, Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4};


    void Awake()
    {
        for (int i = 0; i < abilityButtons.Length; i++)
        {
            if(i >= keys.Length)
            {
                Debug.LogWarning($"Not enough keys defined for ability buttons. Button index {i} will not be assigned a key.");
                break;
            }
            abilityButtons[i].Initialize(i, keys[i]);
        }
    }

    public void UpdateRadial(float progress)
    {
        if(float.IsNaN(progress))
        {
            progress=0f;
        }

        Array.ForEach(abilityButtons, button => button.UpdateRadialFill(progress));
    }
    //float progress = 0f;
    //private void Update()
    //{
    //    progress += Time.deltaTime * 0.1f; // Adjust the speed of the fill as needed
    //    if(progress > 1f) progress = 0f; // Reset progress after it reaches 1
    //    UpdateRadial(progress);
    //}

    public void UpdateButtonSprites(IList<Ability> abilities)
    {
        for (int i = 0; i < abilityButtons.Length; i++)
        {
            if (i < abilities.Count)
            {
                abilityButtons[i].UpdateButtonSprite(abilities[i].data.icon);
            }
            else
            {
                abilityButtons[i].gameObject.SetActive(false); // Clear the sprite if no ability is assigned
            }
        }
    }

}

