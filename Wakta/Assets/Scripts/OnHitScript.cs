using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitScript : MonoBehaviour
{
    public AudioClip[] attackClips;
    public AudioSource source;

    public void OnHitEvent()
    {
        source.PlayOneShot(attackClips[Random.Range(0, attackClips.Length)]);
        Panzee panzee = (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>();
        panzee.Damage(Wakta.Instance.damage);
        panzee.impulseSource.GenerateImpulse();
    }
}
