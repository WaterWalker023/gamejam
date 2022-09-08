using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targe : MonoBehaviour
{
    public float health = 50f;
   public void takedamage (float amount)
    {
        health -= amount;
        if(health <= 0f)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }

}
