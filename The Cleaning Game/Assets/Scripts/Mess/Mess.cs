using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mess : MonoBehaviour
{
    public float health = 100f;

    /// <summary>
    /// Deals the given amount of damage to the mess. 
    /// <br>If the mess goes below zero than killMess() is called automatically</br>
    /// <br>Returns true if the mess is killed</br>
    /// </summary>
    public bool Damage(float dmg)
    {
        health -= dmg;

        if (health < 0)
        {
            KillMess();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Destroys this Mess GameObject
    /// </summary>
    public void KillMess()
    {
        GameObject.Destroy(gameObject);
    }
}
