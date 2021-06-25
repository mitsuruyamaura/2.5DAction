using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// ダメージ計算
    /// </summary>
    /// <param name="damage"></param>
    public void CalcDamage(int damage) {
        
        isDamaged = true;

        hp -= damage;

        if (hp <= 0) {
            Destroy(gameObject, 1.0f);
        }

        StartCoroutine(Rotate());
    }

    /// <summary>
    /// 回転演出
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
}
