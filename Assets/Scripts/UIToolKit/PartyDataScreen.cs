using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.VFX;
using UnityEngine.UIElements;
using System;

public class PartyDataScreen : MonoBehaviour
{
    [SerializeField] Texture2D avaterImage;
    [SerializeField] string playerName;

    VisualElement rootVisualElement;

    private void Awake()
    {
        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

        var avaterImageElement = rootVisualElement.Query("CharacterDataPanel").AtIndex(1);
        avaterImageElement.Q("Avatar").style.backgroundImage = avaterImage;
        avaterImageElement.Q<Label>("NameLabel").text = playerName;
        //rootVisualElement.Q<Label>("NameLabel").text = "√∞œ’º“";
    }

   

    private void OnSelectMulityPlayer(Label label)
    {
        label.text = "√∞œ’º“";
    }




    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rootVisualElement.style.display = rootVisualElement.style.display == DisplayStyle.Flex ? DisplayStyle.None : DisplayStyle.Flex;
        }
    }
}
