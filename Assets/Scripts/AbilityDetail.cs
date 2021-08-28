using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class AbilityDetail : MonoBehaviour
{
    [SerializeField]
    private Button btnAbility;

    [SerializeField]
    private Image imgAbility;

    [SerializeField]
    private Text txtAbilityCost;

    public AbilityData abilityData;

    private SelectAbilityPopUp selectAbilityPopUp;


    /// <summary>
    /// AbilityButtonDetail の設定
    /// </summary>
    /// <param name="level"></param>
    /// <param name="abilityType"></param>
    /// <param name="selectAbilityPopUp"></param>
    public void SetUpAbilityDetail(int level, AbilityType abilityType, SelectAbilityPopUp selectAbilityPopUp) {
        this.selectAbilityPopUp = selectAbilityPopUp;
        abilityData.abilityType = abilityType;

        // AbilityLevel と AbilityType から AbilityTable を取得
        abilityData.abilityTable = DataBaseManager.instance.GetAbilityPointTable(level, abilityType);

        // 取得した情報を使って設定
        imgAbility.sprite = abilityData.abilityTable.abilitySprite;
        txtAbilityCost.text = abilityData.abilityTable.abilityCost.ToString();

        btnAbility.onClick.AddListener(OnClickAbilityDetail);
    }

    /// <summary>
    /// ボタンをクリックした際の処理
    /// </summary>
    public void OnClickAbilityDetail() {
        transform.DOShakeScale(0.5f).SetEase(Ease.OutQuart);

        selectAbilityPopUp.SetAbilityDetail(this);
    }

    /// <summary>
    /// コストの支払い有無を確認してボタンの活性化/非活性化の切り替え
    /// </summary>
    public void JudgeAbilityCost() {
        btnAbility.interactable = false;

        if (GameData.instance.abilityPoint >= abilityData.abilityTable.abilityCost) {
            btnAbility.interactable = true;
        }
    }
}
