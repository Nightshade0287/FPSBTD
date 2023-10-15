using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject[] ChildBloons;

    public void Popped2(int damage)
    {
        if (damage > 0)
        {
            for(int x = 0; x < ChildBloons.Length; x++)
            {
                GameObject bloon = Instantiate(ChildBloons[x], transform.position, transform.rotation);
                bloon.GetComponent<Health>().Popped2(damage -1);
            }
            Destroy(gameObject);
        }
    }
}
