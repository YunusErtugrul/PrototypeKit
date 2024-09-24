using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public GameObject Flashlight;
    public bool flashActive = true;
    public AudioSource FlashlightSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flashActive && Input.GetKeyDown(KeyCode.F))
        {
            Flashlight.SetActive(false);
            flashActive = false;
            FlashlightSource.Play();
        }
        else if(flashActive == false && Input.GetKeyDown(KeyCode.F)){
            Flashlight.SetActive(true);
            flashActive = true;
            FlashlightSource.Play();
        }
    }
}
