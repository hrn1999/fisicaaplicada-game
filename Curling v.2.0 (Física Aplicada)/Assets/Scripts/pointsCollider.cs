using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointsCollider : MonoBehaviour {
    GameObject[] penguins;

    void Update(){
        penguins = GameObject.FindGameObjectsWithTag("Pinguim");
    }

    public GameObject FindClosestPenguim(){
		
		GameObject closestPenguim=null;
		float distanceToClosestPenguim=Mathf.Infinity;

		foreach (GameObject currentPenguim in penguins){
            float distanceToPenguim = (currentPenguim.transform.position - this.transform.position).sqrMagnitude;
			if (distanceToPenguim<distanceToClosestPenguim){
				closestPenguim=currentPenguim;
				distanceToClosestPenguim=distanceToPenguim;
			}
		}
		return closestPenguim;
	}

    public void destroyAll(){
        for (int i = 0; i <= 5; i++){
            Destroy(penguins[i].gameObject);
        }
        
    }
}
