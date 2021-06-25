using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private int hp;

    [SerializeField]
    private float rotateSpeed;


    private bool isDamaged;

    public bool IsDamaged {
        set { isDamaged = value; }
        get { return isDamaged; }  
    }
    
    /// <summary>
    /// ƒ_ƒ[ƒWŒvZ
    /// </summary>
    /// <param name="damage"></param>
    public void CalcDamage(int damage) {
        
        isDamaged = true;

        hp -= damage;
        
        DoRotate();

        GameObject hitEffect = Instantiate(EffectManager.instance.enemyHitEffectPrefab, transform);
        Destroy(hitEffect, 1.0f);

        //StartCoroutine(Rotate());
    }

    /// <summary>
    /// ‰ñ“]‰‰o
    /// </summary>
    /// <returns></returns>
    private IEnumerator Rotate() {
        int timer = 0;
        while (timer <= 150) {
            timer++;
            transform.Rotate(0, rotateSpeed, 0);
            yield return null;
        }
        transform.eulerAngles = Vector3.zero;

        isDamaged = false;
    }

    /// <summary>
    /// ‰ñ“]‰‰o
    /// </summary>
    private void DoRotate() {
        transform.DORotate(new Vector3(0, 720, 0), 1.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutBack)
            .OnComplete(() => 
            { 
                isDamaged = false;
                CheckDestroy();
            });
    }

    /// <summary>
    /// “G‚Ì”j‰óŠm”F
    /// </summary>
    private void CheckDestroy() {
        if (hp <= 0) {
            Destroy(gameObject);

            GameObject destroyEffect = Instantiate(EffectManager.instance.destroyEffectPrefab, transform.position, EffectManager.instance.destroyEffectPrefab.transform.rotation);
            Destroy(destroyEffect, 1.0f);
        } 
    }
}
