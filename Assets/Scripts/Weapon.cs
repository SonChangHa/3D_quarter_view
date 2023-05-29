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

    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
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
}
