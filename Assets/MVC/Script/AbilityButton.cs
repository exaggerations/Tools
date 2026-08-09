using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

namespace Architecture.AbilitySystem
{
    public class AbilityButton : MonoBehaviour
    {
        public Image radialImage;
        public Image abilityIcon;
        public int index;
        public Key key;

        public void Initialize(int index,Key key)
        {
            this.index = index;
            this.key = key;
        }
        public event Action<int> OnButtonPressed = delegate { };
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => OnButtonPressed(index));
        }

        // Update is called once per frame
        void Update()
        {
            if (Keyboard.current[key].wasPressedThisFrame)
            {
                OnButtonPressed(index);
            }
        }

        public void RegisterListenser(Action<int> listener)
        {
            OnButtonPressed += listener;
        }

        public void UpdateButtonSprite(Sprite sprite)
        {
            abilityIcon.sprite = sprite;
        }

        public void UpdateRadialFill(float fillAmount)
        {
            if (radialImage == null)
            {
                Debug.LogError("Radial Image is not assigned in AbilityButton.");
                return;
            }
            radialImage.fillAmount = fillAmount;
        }
    }
}
