using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosionParticleEndAction : MonoBehaviour
{
    void OnParticleSystemStopped()
    {
        Destroy(transform.parent.gameObject);
    }
}
