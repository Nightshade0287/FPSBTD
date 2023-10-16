using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 1;
    public GameObject[] ChildBloons;

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
            GetComponent<Pathing>().UpdateChild(bloon);
            bloon.GetComponent<Health>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
