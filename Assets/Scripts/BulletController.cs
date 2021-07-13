using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int bulletPower;

    public void Shoot(Vector3 dirction, float bulletSpeed, int bulletPower) {

        this.bulletPower = bulletPower;
        GetComponent<Rigidbody>().AddForce(dirction * bulletSpeed);
        Destroy(gameObject, 3.0f);
    }
}
