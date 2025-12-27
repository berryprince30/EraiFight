using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleDisappear : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ByeParticle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ByeParticle()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);

        yield return null;
    }
}
