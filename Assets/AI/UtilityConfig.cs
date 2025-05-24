// UtilityConfig.cs
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Utility Config")]
public class UtilityConfig : ScriptableObject
{
    [Header("Weights")]
    [Range(0, 1)] public float distanceWeight = 0.4f;
    [Range(0, 1)] public float healthWeight = 0.3f;
    [Range(0, 1)] public float threatWeight = 0.2f;
    
    [Header("Type Bonuses")]
    public float preferredTypeBonus = 50f;
    public float sameTypePenalty = -30f;
}