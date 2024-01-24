using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mess : MonoBehaviour
{
    [SerializeField] Transform messRenderScale;
    public float health = 100f;

    /// <summary>
    /// Deals the given amount of damage to the mess. 
    /// <br>If the mess goes below zero than killMess() is called automatically</br>
    /// <br>Returns true if the mess is killed</br>
    /// </summary>
    public bool Damage(float dmg)
    {
        health -= dmg;
        rescaleMess();

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

    /// <summary>
    /// Rescales the mess render so that there is visual feedback on cleaning
    /// </summary>
    private void rescaleMess()
    {
        messRenderScale.localScale = new Vector3(health / 100, health / 100, health / 100);
    }
}
