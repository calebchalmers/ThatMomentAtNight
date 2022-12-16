using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteParticles : MonoBehaviour
{
    public ParticleSystem[] systems;
    public float minWaitTime = 1f;
    public float maxWaitTime = 2f;

    void Start()
    {
        StartCoroutine("Loop");
    }

    private IEnumerator Loop()
    {
        while (true)
        {
            float wait = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(wait);
            EmitParticle();
        }
    }

    private void EmitParticle()
    {
        int i = Random.Range(0, systems.Length);
        systems[i].Emit(1);
    }
}
