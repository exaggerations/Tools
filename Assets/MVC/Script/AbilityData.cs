using UnityEngine;

namespace Architecture.AbilitySystem {
    [CreateAssetMenu(fileName = "AbilityData", menuName = "ScriptableObjects/Ability Data",order =0)]
    public class AbilityData : ScriptableObject
{
     public AnimationClip animationClip;
     public int animationHash;
     public float duration;
        public Sprite icon;
    

    private void OnValidate()
        {
            if (animationClip != null)
        {
            animationHash = Animator.StringToHash(animationClip.name);
        }
        }
    }
    }
