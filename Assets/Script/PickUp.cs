using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private int playerLayer;
    public ParticleSystem ExplosionParticle;
    public AudioClip CollectSound;

    private void Start()
    {
        GameManager.RegisterGem(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.tag=="RedItem" && collision.tag=="RedMan" || this.tag == "BlueItem" && collision.tag == "BlueMan")
        {
            GameManager.RemoveGem(this);
            transform.DetachChildren();
            ExplosionParticle.Play();
            collision.GetComponent<playerController>().CollectSound(CollectSound);
            Destroy(gameObject);
        }
    }
}
