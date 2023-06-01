using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider area;
    public TrailRenderer trail;

    public int curAmmo;
    public int maxAmmo;

    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;



    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        if (type == Type.Range && curAmmo > 0)
        {
            curAmmo -= 1;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        area.enabled = true;
        trail.enabled = true;

        yield return new WaitForSeconds(0.3f);
        area.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trail.enabled = false;
    }

    IEnumerator Shot()
    {
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRb = instantBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = bulletPos.forward * 50;

        yield return null;

        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRb = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVector = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRb.AddForce(caseVector, ForceMode.Impulse);
        caseRb.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }
}
