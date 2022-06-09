using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Photon.MonoBehaviour
{
    public PhotonView photonView;
    public Rigidbody2D rb;
    public Animator anim;
    public GameObject PlayerCamera;
    public SpriteRenderer sr;
    public Text PlayerNameText;

    public bool IsGrounded = false;
    public float MoveSpeed;

    public float moveInput;
    public float JumpSpeed;
    private bool isOnGround;
    public Transform playerPos;
    public float positionRadius;
    public LayerMask ground;
    private float airTimeCount;
    public float airTime;
    private bool inAir;
    private bool doubleJump;

    public GameObject BulletObject;
    public Transform FirePos;

    public bool DisableInput = false;


    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.isMine)
        {
            PlayerCamera.SetActive(true);
            PlayerNameText.text = PhotonNetwork.playerName;
        } 
        else
        {
            PlayerNameText.text = photonView.owner.name;
            PlayerNameText.color = Color.red;
        }
    }

    private void Update()
    {
        if (photonView.isMine && !DisableInput)
        {
            
            CheckInput();
            isOnGround = Physics2D.OverlapCircle(playerPos.position, positionRadius, ground);


            if (Input.GetKeyDown(KeyCode.W) )
            {
                if(isOnGround == true || doubleJump)
                {
                    inAir = true;
                    airTimeCount = airTime;
                    rb.velocity = Vector2.up * JumpSpeed;
                    doubleJump = !doubleJump;


                }
            }

            if (Input.GetKeyDown(KeyCode.W) && inAir == true)
            {
                
                if(airTimeCount > 0)
                {
                    rb.velocity = Vector2.up * JumpSpeed;
                    airTimeCount -= Time.deltaTime;
                } else
                {
                    inAir = false;
                }
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                inAir = false;
  
            }

            if (inAir == true)
            {
                anim.SetBool("isJumping", true);
                //anim.Play("jump");
            }
            else
            {
;                anim.SetBool("isJumping", false);
            }

        }
    }

    private void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * MoveSpeed, rb.velocity.y);
    }



    private void CheckInput()
    {
        var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        transform.position += move * MoveSpeed * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.J))
        {
            anim.SetTrigger("shootTrigger");
            Shoot();
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            photonView.RPC("FlipTrue", PhotonTargets.AllBuffered);        
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            photonView.RPC("FlipFalse", PhotonTargets.AllBuffered);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }


    }

    private void Shoot()
    {
        if(sr.flipX == false)
        {
            GameObject obj = PhotonNetwork.Instantiate(BulletObject.name, new Vector2(FirePos.transform.position.x, FirePos.transform.position.y), Quaternion.identity, 0);
        }

        if (sr.flipX == true)
        {
            GameObject obj = PhotonNetwork.Instantiate(BulletObject.name, new Vector2(FirePos.transform.position.x, FirePos.transform.position.y), Quaternion.identity, 0);
            obj.GetComponent<PhotonView>().RPC("ChangeDir_left", PhotonTargets.AllBuffered);
        }


    
    }

    [PunRPC]
    private void FlipTrue()
    {
        sr.flipX = true;
    }

    [PunRPC]
    private void FlipFalse()
    {
        sr.flipX = false;
    }
}


