using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BloonEffects
{
    regrow,
    reinforced,
    camo
}
public class Health : MonoBehaviour
{
    public List<BloonTypes> bloonTypes = new List<BloonTypes>();
    //[HideInInspector]
    public List<BloonEffects> bloonEffects = new List<BloonEffects>();
    public int health = 1;
    public GameObject[] ChildBloons;
    public GameObject dart;
    public Economy_Health economy;
    void Awake()
    {
        economy = GameObject.Find("GameManager").GetComponent<Economy_Health>();
    }

    void Update()
    {
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Popped(-health);
        }
    }

    public void Popped(int damage)
    {
        foreach(GameObject child in ChildBloons)
        {
            GameObject bloon = Instantiate(child, transform.position, transform.rotation);
            Health bloonHealth = bloon.GetComponent<Health>();
            GetComponent<BloonMovement>().UpdateChild(bloon);
            bloonHealth.dart = dart;
            dart.GetComponent<DartBehavior>().bloonHitList.Add(bloon.GetInstanceID());
            bloonHealth.TakeDamage(damage);
        }
        economy.UpdateMoney(1);
        Destroy(gameObject);
    }
}
