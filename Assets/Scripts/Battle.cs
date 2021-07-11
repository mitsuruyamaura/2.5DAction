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

        // Debug�p
        SceneStateManager.instance.PreparateNextScene(SceneName.Stage);

        UpdateDisplayHp();
    }

    /// <summary>
    /// Hp�\���X�V
    /// </summary>
    public void UpdateDisplayHp() {
        txtHp.text = GameData.instance.hp + "/ " + GameData.instance.maxHp;

        sliderHp.DOValue(GameData.instance.hp / GameData.instance.maxHp, 0.5f).SetEase(Ease.Linear);

        Debug.Log("Battle Hp �\���X�V");
    }
}
