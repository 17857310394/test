using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimatorFunction : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private ParticleSystem stepParticle;
    [SerializeField] private Animator setBoolInAnimator;
    private playerController player;

    // If we don't specify what audio source to play sounds through, just use the one on player.
    void Start()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            audioSource = GetComponent<playerController>().audioSource;
            player = GetComponent<playerController>();
            stepParticle = GetComponent<playerController>().stepParticles;
        }
    }

    public void EmitParticles(int amount)
    {
        stepParticle.Emit(amount);
    }

    public void DestoryPlayer()
    {
        GameManager.DestoryPlayer();
    }

    //void PlaySound(AudioClip whichSound)
    //{
    //    audioSource.PlayOneShot(whichSound);
    //}

    //public void SetTimeScale(float time)
    //{
    //    Time.timeScale = time;
    //}

    //public void SetAnimBoolToFalse(string boolName)
    //{
    //    setBoolInAnimator.SetBool(boolName, false);
    //}

    //public void SetAnimBoolToTrue(string boolName)
    //{
    //    setBoolInAnimator.SetBool(boolName, true);
    //}

    public void loadscene(string whichlevel)
    {
        SceneManager.LoadScene(whichlevel);
    }

    ////Slow down or speed up the game's time scale!
    //public void SetTimeScaleTo(float timeScale)
    //{
    //    Time.timeScale = timeScale;
    //}
}
