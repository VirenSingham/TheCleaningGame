using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamTrigger : MonoBehaviour
{
    [SerializeField] List<AudioClip> screamClipList;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //if the object falls into the garbage disposal, then play this.
        if (other.gameObject.tag == "object")
        {
            if (other.gameObject.GetComponent<ObjectBehaviour>().isDeviant)
            {
                other.gameObject.AddComponent<AudioSource>();

                //generate a random clip.
                int rnd = Random.Range(0, 8);

                //change settings
                other.gameObject.GetComponent<AudioSource>().clip = screamClipList[rnd];
                other.gameObject.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
                other.gameObject.GetComponent<AudioSource>().maxDistance = 30;
                other.gameObject.GetComponent<AudioSource>().spatialBlend = 1;
                other.gameObject.GetComponent<AudioSource>().volume = 0.75f;
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }
}
