using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Battle : MonoBehaviour
{
    [SerializeField]
    private Slider sliderHp;

    [SerializeField]
    private Text txtHp;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);

        // Debug用
        SceneStateManager.instance.PreparateNextScene(SceneName.Stage);

        UpdateDisplayHp();
    }

    /// <summary>
    /// Hp表示更新
    /// </summary>
    public void UpdateDisplayHp() {
        txtHp.text = GameData.instance.hp + "/ " + GameData.instance.maxHp;

        sliderHp.DOValue(GameData.instance.hp / GameData.instance.maxHp, 0.5f).SetEase(Ease.Linear);

        Debug.Log("Battle Hp 表示更新");
    }
}
