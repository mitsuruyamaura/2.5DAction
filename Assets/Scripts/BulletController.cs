using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int bulletPower;

    public void Shoot(Vector3 dirction, float bulletSpeed, int bulletPower, float arrowDir) {

        this.bulletPower = bulletPower;
        GetComponent<Rigidbody>().AddForce(dirction * bulletSpeed);

        // –î‚¶‚è‚ÌŒü‚«‚ğ“G‚ÌŒü‚¢‚Ä‚¢‚é•ûŒü‚É‡‚í‚¹‚é
        Vector3 temp = transform.localScale;
        temp.x = -arrowDir;
        transform.localScale = temp;

        Destroy(gameObject, 3.0f);
    }
}
