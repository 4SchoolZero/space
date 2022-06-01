using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class collect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Text customText;
    public ParticleSystem collEffect;
    public Slider sliderInv;
    bool key;
    bool x;
    void Update(){
        if(x = true && Input.GetKey(KeyCode.E)){
            Destroy(gameObject);
            Debug.Log("collected 20kg debris");
            // Instantiate(collEffect);
            sliderInv.value += 20;
            if (sliderInv.value >= 100){
                Debug.Log("Inventory is full");
            }
            customText.enabled = false;
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
