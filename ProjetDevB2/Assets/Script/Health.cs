using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : Photon.MonoBehaviour
{
    public float HealthAmount;
    public Image FillImage;

    public Player plMove;
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public SpriteRenderer sr;
    public GameObject PlayerCanvas;




    private void Awake()
    {
        if (photonView.isMine)
        {
            GameManager.Instance.LocalPlayer = this.gameObject;
        }
    }

    [PunRPC] 
    public void ReduceHealth(float amount)
    {
        ModifyHealth(amount);
    }

    public int CheckHealth()
    {
        FillImage.fillAmount = HealthAmount / 100f;
        if(photonView.isMine && HealthAmount <= 0)
        {
            plMove.live -= 1;
            GameManager.Instance.EnableRespawn();
            plMove.DisableInput = true;
            this.GetComponent<PhotonView>().RPC("Dead", PhotonTargets.AllBuffered);
        }
        return plMove.live;
    }

    public void EnableInput()
    {
        plMove.DisableInput = false;
    }

    [PunRPC]
    private void Dead()
    {
        rb.gravityScale = 0;
        bc.enabled = false;
        sr.enabled = false;
        PlayerCanvas.SetActive(false);
    }

    [PunRPC]
    private void Respawn()
    {
        rb.gravityScale = 5;
        bc.enabled = true;
        sr.enabled = true;
        PlayerCanvas.SetActive(true);
        FillImage.fillAmount = 1f;
        HealthAmount = 100f;
    }

    private void ModifyHealth(float amount)
    {
        if(photonView.isMine)
        {
            HealthAmount -= amount;
            FillImage.fillAmount -= amount;
        }
        else
        {
            HealthAmount -= amount;
            FillImage.fillAmount -= amount;
        }

        CheckHealth();
    }
}
