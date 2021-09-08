using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_Confusion : PlayerConditionBase 
{
    [SerializeField]
    private GameObject confusionEffectPrefab;

    private GameObject confusionEffect;

    /// <summary>
    /// �����ɂ�郉���_���ړ��Ƒ����ݕs�̌��ʂ̂����A�����ݕs�̌���
    /// </summary>
    public override void ApplyEffect() {

        // Stage ���Ő���
        mapMoveController.GetStage().GetInputManager().SwitchActivateSteppingButton(false);

        Debug.Log("Confusion");
    }

    protected override IEnumerator OnEnterCondition() {
        // �G�t�F�N�g����

        Debug.Log("�����p�̃G�t�F�N�g�@����");

        return base.OnEnterCondition();
    }


    protected override IEnumerator OnExitCondition() {

        // �G�t�F�N�g�j��
        Destroy(confusionEffect);

        return base.OnExitCondition();
    }
}
