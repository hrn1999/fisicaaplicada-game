using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    public GameObject bombLocal;
    public float power = 10.0f;
    public float radius = 5.0f;
    public float upForce = 1.0f;

    void Start()
    {
        Explode();
    }

	public void Explode()
    {
        Vector3 explosionPos = bombLocal.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPos, radius, upForce, ForceMode.VelocityChange);
                print("entrou");
            }
        }
    }
}
