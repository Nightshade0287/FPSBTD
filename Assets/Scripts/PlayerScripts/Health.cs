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
        for(int x = 0; x < ChildBloons.Length; x++)
        {
            GameObject bloon = Instantiate(ChildBloons[x], transform.position, transform.rotation);
            bloon.GetComponent<Health>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
