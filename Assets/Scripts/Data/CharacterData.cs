using UnityEngine;

[CreateAssetMenu(menuName =("CharacterData/CharacterData"),fileName =("CharacterData"),order =1)]
public class CharacterData : ScriptableObject 
{
    [SerializeField] Texture2D characterAvatarImage;
    [SerializeField] string characterName;
    [SerializeField] int characterStartLevel;
    [SerializeField] CharacterState characterState;
    public Texture2D CharacterAvaterImage => characterAvatarImage;

    public string CharacterName => characterName;

    public int CharacterStartLevel => characterStartLevel;

    public CharacterState CharacterState => characterState;
}
