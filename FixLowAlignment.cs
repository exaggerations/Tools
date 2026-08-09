using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FixLowAlignment : MonoBehaviour
{
    [SerializeField] private List<char> chars;
    [SerializeField] private List<float> offsets;

    private void Awake()
    {
        var tmp = GetComponent<TMP_Text>();
        tmp.OnPreRenderText += OnPreRenderTextHandler;
    }

    private void OnDestroy()
    {
        var tmp = GetComponent<TMP_Text>();
        if (tmp != null)
        {
            tmp.OnPreRenderText -= OnPreRenderTextHandler;
        }
    }

    private void OnPreRenderTextHandler(TMP_TextInfo textInfo)
    {
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (charInfo.isVisible)
            {
                int index = chars.IndexOf(charInfo.character);
                if (index != -1)
                {
                    float offset = offsets[index];
                    int vertexIndex = charInfo.vertexIndex;
                    Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                    vertices[vertexIndex + 0].y += offset;
                    vertices[vertexIndex + 1].y += offset;
                    vertices[vertexIndex + 2].y += offset;
                    vertices[vertexIndex + 3].y += offset;
                }
            }
        }
    }

}
