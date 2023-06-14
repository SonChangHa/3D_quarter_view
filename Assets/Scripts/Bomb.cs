using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject mesh;
    public GameObject effect;
    public Rigidbody rigid;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        mesh.SetActive(false);
        effect.SetActive(true);

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 15, Vector3.up, 0f, LayerMask.GetMask("Enemy"));

        foreach(RaycastHit hit in hits)
        {
            hit.transform.GetComponent<Enemy>().HitByBomb(transform.position);
        }

        Destroy(gameObject, 5);
    }
}
