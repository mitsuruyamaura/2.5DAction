using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コンディションのベースクラス
/// </summary>
public class PlayerConditionBase : MonoBehaviour
{
    protected float conditionDuration;
    protected float conditionValue;

    /// <summary>
    /// コンディションをセットする際に呼び出す
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="value"></param>
    public void AddCondition(float duration, float value) {
        conditionDuration = duration;
        conditionValue = value;

        StartCoroutine(OnEnterCondition());
    }

    protected virtual IEnumerator OnEnterCondition() {
        yield return null;

        Debug.Log("コンディション付与");
    }

    /// <summary>
    /// コンディションが終了するときに呼び出す
    /// </summary>
    public void RemoveCondition() {
        StartCoroutine(OnExitCondition());
    }

    protected virtual IEnumerator OnExitCondition() {
        yield return null;

        Debug.Log("コンディション削除");
    }

    /// <summary>
    /// コンディションの残り時間の更新
    /// </summary>
    public virtual void CalcDuration() {

        // コンディションの残り時間がなくなったら
        if (conditionDuration <= 0) {

            // コンディションを削除して終了する
            RemoveCondition();
        }
    }
}
