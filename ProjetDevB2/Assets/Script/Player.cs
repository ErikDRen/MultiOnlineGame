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
    public float JumpForce;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.isMine)
        {
            PlayerCamera.SetActive(true);
        }
    }

    private void Update()
    {
        if (photonView.isMine)
        {
            CheckInput();
            PlayAnim();
        }
    }

    private void CheckInput()
    {
        var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        transform.position += move * MoveSpeed * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.A))
        {
            sr.flipX = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            sr.flipX = false;
        }

    }

    private void PlayAnim()
    {
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("isMoving", false);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            anim.SetBool("isMoving", true);
        }
    }
}


