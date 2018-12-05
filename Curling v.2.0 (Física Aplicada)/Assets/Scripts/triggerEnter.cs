using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerEnter : MonoBehaviour {
    public bool entrou = false;

    void OnTriggerEnter(Collider other){
        entrou = true;
    }

    void OnTriggerExit(Collider other){
        entrou = false;
    }
}
