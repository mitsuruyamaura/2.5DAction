using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_WalkThrough : PlayerConditionBase
{
    private SpriteRenderer spritePlayer;
    private float originAlpha = 1.0f;

    protected override void OnEnterCondition() {

        if(mapMoveController.transform.GetChild(0).TryGetComponent(out spritePlayer)) {

            // �v���C���[�̉摜�𔼓����ɂ���
            spritePlayer.color = new Color(1.0f, 1.0f, 1.0f, conditionValue);
        }

        base.OnEnterCondition();
    }

    protected override void OnExitCondition() {

        // �I�����̉��o


        // ���̓����x�ɖ߂�
        spritePlayer.color = new Color(1.0f, 1.0f, 1.0f, originAlpha);

        base.OnExitCondition();
    }
}
