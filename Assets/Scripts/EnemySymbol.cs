using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySymbol : SymbolBase
{

    public override void TriggerAppearEffect() {

        base.TriggerAppearEffect();

        Debug.Log("�ړ���œG�ɐڐG");

        transform.DOShakeScale(0.75f, 1.0f)
            .SetEase(Ease.OutQuart)
            .OnComplete(() => { PreparateBattle(); } );
    }

    /// <summary>
    /// �o�g���̏���
    /// </summary>
    private void PreparateBattle() {



        // TODO �o�g���O�ɍ��W���� GameData �ɕێ�


        // TODO �G�t�F�N�g�� SE


        // TODO �G�̏����擾


        // TODO �V�[���J��

        SceneStateManager.instance.LoadBattleScene();
    }
}
