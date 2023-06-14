using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHP;
    public int curHP;
    
    Rigidbody rb;
    BoxCollider bCol;

    MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bCol = GetComponent<BoxCollider>();
        mr = GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitByBomb(Vector3 explosionPos)
    {
        curHP -= 100;
        Vector3 reactVector = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVector, true));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHP -= weapon.damage;
            Vector3 reactVector = transform.position - other.transform.position;

            StartCoroutine(OnDamage(reactVector, false));
        }
        else if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHP -= bullet.damage;
            Vector3 reactVector = transform.position - other.transform.position;

            StartCoroutine(OnDamage(reactVector, false));
        }
    }

    IEnumerator OnDamage(Vector3 reactVector, bool isBomb)
    {
        mr.material.color = new Color(255/255f, 20/255f, 147/255f);
        yield return new WaitForSeconds(0.1f);

        if(curHP > 0)
        {
            mr.material.color = Color.white;
        }
        else
        {
            mr.material.color = Color.grey;
            gameObject.layer = 11;

            if(isBomb == true)
            {
                reactVector = reactVector.normalized;
                reactVector += Vector3.up * 3;

                rb.freezeRotation = false;
                rb.AddForce(reactVector * 5, ForceMode.Impulse);
                rb.AddTorque(reactVector * 15, ForceMode.Impulse);
            }
            else
            {
                reactVector = reactVector.normalized;
                reactVector += Vector3.up;
                rb.AddForce(reactVector * 5, ForceMode.Impulse);
            }



            Destroy(gameObject, 4);
        }
    }
}
