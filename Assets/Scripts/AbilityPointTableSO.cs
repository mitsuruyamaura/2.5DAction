using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityPointTableSO", menuName = "Create AbilityPointTableSO")]
public class AbilityPointTableSO : ScriptableObject
{
    public AbilityType abilityType;
    public List<AbilityTable> abilityTablesList;

    [System.Serializable]
    public class AbilityTable {
        public int abilityLevel;
        public int abilityCost;
        public float powerUpValue;
        public string abilityName;
        public Sprite abilitySprite;
    }
}
