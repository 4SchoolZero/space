using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class returnBase : MonoBehaviour
{
 [SerializeField] private Text customText;
    bool key;
    bool x;
    void Update(){
        if(x = true && Input.GetKey(KeyCode.R)){
            SceneManager.LoadScene("homePlanet");
        }
    }
    void OnTriggerEnter(Collider other){
         customText.enabled = true;
         x = true;
    }
    void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            customText.enabled = false;
            x = false;
        }
    }
}
