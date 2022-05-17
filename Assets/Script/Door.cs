using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsOpen=false;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        
    }
    private void Start()
    {
        GameManager.RegisterDoor(this);
    }

    private void FixedUpdate()
    {
        anim.SetBool("IsOpen", IsOpen);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.tag == "RedItem" && collision.tag == "RedMan" || this.tag == "BlueItem" && collision.tag == "BlueMan")
        {
            IsOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.tag == "RedItem" && collision.tag == "RedMan" || this.tag == "BlueItem" && collision.tag == "BlueMan")
        {
            IsOpen = false;
        }
    }
}
