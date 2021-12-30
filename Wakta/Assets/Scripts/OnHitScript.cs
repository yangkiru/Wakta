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
            float x;
            if (Input.GetKey(KeyCode.A)) x = -3;
            else if (Input.GetKey(KeyCode.D)) x = 3;
            else
                x = panzee.transform.position.x > Wakta.Instance.transform.position.x ? 3 : -3;
            panzee.rb.AddForce(new Vector2(x, 5), ForceMode2D.Impulse);
        }
    }
}
