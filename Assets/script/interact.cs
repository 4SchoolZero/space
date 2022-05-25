using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class interact : MonoBehaviour
{
    [SerializeField] private Text customText;
    bool key;
    bool x;
    void Update(){
        if(x = true && Input.GetKey(KeyCode.E)){
            SceneManager.LoadScene("space");
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
