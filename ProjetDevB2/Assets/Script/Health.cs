using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : Photon.MonoBehaviour
{
    public Image FillImage;

    [PunRPC] 
    public void ReduceHealth(float amount)
    {
        ModifyHealth(amount);
    }

    private void ModifyHealth(float amount)
    {
        if(photonView.isMine)
        {
            FillImage.fillAmount -= amount;
        }
        else
        {
            FillImage.fillAmount -= amount;
        }
    }
}
