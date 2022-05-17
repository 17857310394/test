using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    [SerializeField] private Animator Anim;
    public AudioSource audioSource;

    [Header("移动参数")]
    public float speed = 6;
    public float velocityX;
    public float velocityY;

    [Header("跳跃参数")]
    public float jumpForce = 6.3f;
    public float jumpHoldForce = 1.9f;
    public float jumpHoldDuration = 0.1f;

    [Header("状态")]
    public bool IsJump; //在holdDuration内为true
    public bool IsOnGround;
    public bool IsFall;
    public bool IsLand;
    public bool IsLeave;

    [Header("环境")]
    public LayerMask groundLayer;
    private float footOffset = 0.2f;
    private float footDistance = 0.15f;

    private bool jumpPress;
    private bool jumpHeld;
    private float jumpTime;

    [Header("Sounds")]
    public AudioClip deathSound;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip stepSound;
    [System.NonSerialized] public int whichHurtSound;
    public float soundPitch=1.5f;
    public float SoundIncreaseTime = 1f;
    public bool CanIncrease = false;

    [Header("Particle")]
    //[SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private ParticleSystem jumpParticles;
    public ParticleSystem stepParticles;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameManager.RegisterPlayer(this);
    }

    private void Update()
    {
        velocityY = rb.velocity.y;
        if (Input.GetButtonDown(gameObject.name+"Jump"))
        {
            jumpPress = true;
        }

        jumpHeld = Input.GetButton(gameObject.name + "Jump");
        if (Anim)
        {
            SetAnim();
        }
        SoundIncrease();
    }

    private void FixedUpdate()
    {
        GroundMovement();
        AirMovement();
        PhysicsCheck();
    }

    private void GroundMovement()
    {
        velocityX = Input.GetAxisRaw(gameObject.name+"Horizontal");
        rb.AddForce(new Vector2(velocityX * speed * Time.fixedDeltaTime, 0f));
        //rb.velocity = new Vector2(velocityX * speed * Time.fixedDeltaTime, rb.velocity.y);

        FlipDirction();
    }

    private void AirMovement()
    {
        if (jumpPress && IsOnGround && !IsJump)
        {
            IsOnGround = false;
            IsJump = true;

            jumpTime = Time.time + jumpHoldDuration;
            jumpPress = false;

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            JumpEffect();
        }

        else if (IsJump)
        {
            if (jumpHeld)
            {
                rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            }
            if (jumpTime < Time.time)
            {
                IsJump = false;
            }
        }

        if (velocityY < 0)
        {
            IsFall = true;
        }
        else
        {
            IsFall = false;
        }
    }

    private void PhysicsCheck()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, -coll.size.y/2 + coll.offset.y), Vector2.down, footDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, -coll.size.y / 2 + coll.offset.y), Vector2.down, footDistance, groundLayer);

        if (leftCheck || rightCheck)
        {
            IsOnGround = true;
            if (!IsLand)
            {
                IsLand=true;
                LandEffect();
            }
        }
        else
        {
            IsOnGround = false;
            IsLand = false;
        }
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 direction, float distance, LayerMask groundLayer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, direction, distance, groundLayer);

        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, direction * distance, color);

        return hit;
    }

    private void FlipDirction()
    {
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.y);
        }
    }

    private void SetAnim()
    {
        Anim.SetBool("IsOnGround", IsOnGround);
        Anim.SetFloat("velocityX", Mathf.Abs(velocityX));
        Anim.SetFloat("velocityY", velocityY);
        Anim.SetBool("IsJump", IsJump);
        Anim.SetBool("IsFall", IsFall);
        Anim.SetBool("IsLeave", IsLeave);
    }

    public void PlayStepSound()
    {
        //Play a step sound at a random pitch between two floats, while also increasing the volume based on the Horizontal axis
        audioSource.pitch = 1f;
        audioSource.volume = 1f;
        audioSource.PlayOneShot(stepSound, Mathf.Abs(Input.GetAxis(gameObject.name + "Horizontal") / 20));
    }

    public void JumpEffect()
    {
        jumpParticles.Emit(1);
        audioSource.pitch = Random.Range(0.6f, 1f);
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(jumpSound);
    }

    public void LandEffect()
    {
        jumpParticles.Emit(1);
        audioSource.pitch = (Random.Range(0.6f, 1f));
        audioSource.volume = 1f;
        audioSource.PlayOneShot(landSound);
    }

    public void CollectSound(AudioClip sound)
    {
        CanIncrease = true;
        SoundIncreaseTime = 1f;
        audioSource.pitch = soundPitch;
        soundPitch = soundPitch + 0.1f;
        audioSource.volume = 1f;
        audioSource.PlayOneShot(sound);
    }

    public void SoundIncrease()
    {
        if (CanIncrease)
        {
            SoundIncreaseTime -= Time.deltaTime;
            if (SoundIncreaseTime <= 0)
            {
                CanIncrease = false;
                soundPitch = 1.5f;
                SoundIncreaseTime = 1f;
            }
        }
    }
}
