using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameObject nearObject;
    public GameObject[] weapons;
    public GameObject[] bombs;
    public bool[] hasWeapons;

    public GameObject bombObj;

    public int ammo;
    public int coin;
    public int hp;
    public int bomb;

    public int maxAmmo;
    public int maxCoin;
    public int maxHp;
    public int maxBomb;


    bool weaponNum1;
    bool weaponNum2;
    bool weaponNum3;

    bool reloadInput;

    bool attackInput;
    float attackDelay;
    bool isAttackReady;
    bool isReload;
    bool bombInput;

    Weapon equipWeapon;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetItem();
        GetInput();
        WeaponSwap();
        Attack();
        Reload();
        ThrowBomb();
    }

    void ThrowBomb()
    {
        if (bomb <= 0)
            return;

        if (bombInput == true)
        {
            Debug.Log("¼ö·ùÅº");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Debug.DrawRay(transform.position, rayHit.point, Color.red);
                Vector3 nextVector = rayHit.point - transform.position;
                nextVector.y = 0;

                GameObject instantBomb = Instantiate(bombObj, transform.position, Quaternion.identity);
                Rigidbody rbBomb = instantBomb.GetComponent<Rigidbody>();

                rbBomb.AddForce(nextVector, ForceMode.Impulse);
                rbBomb.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                bomb--;
                bombs[bomb].SetActive(false);
            }
        }
    }

    void WeaponSwap()
    {
        if (weaponNum1 && !hasWeapons[0])
            return;
        if (weaponNum2 && !hasWeapons[1])
            return;
        if (weaponNum3 && !hasWeapons[2])
            return;


        int equipIndex = -1;
        if (weaponNum1)
            equipIndex = 0;
        if (weaponNum2)
            equipIndex = 1;
        if (weaponNum3)
            equipIndex = 2;

        if (weaponNum1 || weaponNum2 || weaponNum3)
        {
            if (equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            equipWeapon = weapons[equipIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");
        }
    }

    void Attack()
    {
        if (equipWeapon == null)
            return;

        attackDelay += Time.deltaTime;
        isAttackReady = equipWeapon.rate < attackDelay;

        if(attackInput && isAttackReady)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            attackDelay = 0;
        }
    }

    void Reload()
    {
        if (equipWeapon == null || equipWeapon.type == Weapon.Type.Melee)
            return;
        if (ammo == 0)
            return;

        if (reloadInput)
        {
            anim.SetTrigger("doReload");
            isReload = true;

            Invoke("ReloadOut", 3f);
        }
    }

    void ReloadOut()
    {
        isReload = false;
        int reAmmo = equipWeapon.maxAmmo - equipWeapon.curAmmo;
        equipWeapon.curAmmo += reAmmo;
        ammo -= reAmmo;
    }

    void GetInput()
    {
        weaponNum1 = Input.GetKeyDown(KeyCode.Alpha1);
        weaponNum2 = Input.GetKeyDown(KeyCode.Alpha2);
        weaponNum3 = Input.GetKeyDown(KeyCode.Alpha3);

        attackInput = Input.GetMouseButton(0);

        reloadInput = Input.GetKeyDown(KeyCode.R);

        bombInput = Input.GetKeyDown(KeyCode.G);
    }

    void GetItem()
    {
        if(nearObject != null)
        {
            if(nearObject.tag == "Weapon" && Input.GetKeyDown(KeyCode.F))
            {
                Item item = nearObject.GetComponent<Item>();
                int index = item.value;
                hasWeapons[index] = true;

                Destroy(nearObject);
            }
            if (nearObject.tag == "Bomb")
            {
                Item item = nearObject.GetComponent<Item>();
                int index = item.value;
                bomb++;

                Destroy(nearObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            if(item.type == Item.Type.Ammo)
            {
                ammo += item.value;
                if(ammo > maxAmmo)
                {
                    ammo = maxAmmo;
                }
            } 
            else if(item.type == Item.Type.Coin)
            {
                coin += item.value;
                if (coin > maxCoin)
                {
                    coin = maxCoin;
                }
            }
            else if (item.type == Item.Type.Heart)
            {
                hp += item.value;
                if (hp > maxHp)
                {
                    hp = maxHp;
                }
            }
            else if (item.type == Item.Type.Bomb)
            {
                bombs[bomb].SetActive(true);
                bomb += item.value;
                if (bomb > maxBomb)
                {
                    bomb = maxBomb;
                }
            }
            Destroy(other.gameObject);

        }
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Weapon")
        {
            nearObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;
        }
    }
}
