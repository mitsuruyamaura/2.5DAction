using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_Poison : PlayerConditionBase
{
    /// <summary>
    /// 毒によるダメージ効果
    /// </summary>
    public override void ApplyEffect() {
        GameData.instance.hp = Mathf.Clamp(GameData.instance.hp += (int)-conditionValue, 0, GameData.instance.maxHp);

        StartCoroutine(mapMoveController.GetStage().UpdateDisplayHp(1.0f));
    }
}
