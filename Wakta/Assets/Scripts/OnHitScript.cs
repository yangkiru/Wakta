using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitScript : MonoBehaviour
{
    public AudioClip[] attackClips;
    public AudioSource source;

    public void OnHitEvent()
    {
        
        Panzee panzee = (Wakta.Instance.selected as MonoBehaviour)?.GetComponent<Panzee>();
        if (panzee != null) {
            source.PlayOneShot(attackClips[Random.Range(0, attackClips.Length)]);
            panzee.Damage(Wakta.Instance.damage);
            panzee.impulseSource.GenerateImpulse();
        }
    }
}
