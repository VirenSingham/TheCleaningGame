using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTimer : MonoBehaviour
{
    [SerializeField] float KillTime;

    private void Update()
    {
        KillTime -= Time.deltaTime;
        if (KillTime < 0)
            GameObject.Destroy(gameObject);
    }
}
