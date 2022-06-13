using System;
using System.Collections;
using System.Collections.Generic;
using PopUp;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProductShopping : MonoBehaviour
{
    public Transform productPoint;
    //public float productRange = 0.5f;
    public LayerMask playerLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            BuyProduct();
        }
    }

    void BuyProduct()
    {
        var productColliders = Physics.OverlapBox(productPoint.position, Vector3.one * 0.5f, Quaternion.identity, playerLayer);

        if (productColliders.Length != 0)
        {
            if (gameObject.name == "Heart")
            {
                PopUpUI.Instance.SetTitle("Health upgrade (15 coins)").SetMessage("By buying this item your health will upgrade by 1").Show("Heart", 15);
                
            } else if (gameObject.name == "Shield")
            {
                PopUpUI.Instance.SetTitle("Shield upgrade (10 coins)").SetMessage("By buying this item your shield will upgrade by 2").Show("Shield", 10);
            } else if (gameObject.name == "Sword")
            {
                PopUpUI.Instance.SetTitle("Sword upgrade (25 coins)").SetMessage("By buying this item your sword will double your attack").Show("Sword", 25);
            } else if (gameObject.name == "Bottle")
            {
                PopUpUI.Instance.SetTitle("Speed upgrade (10 coins)").SetMessage("By buying this item your speed will upgrade by 1").Show("Speed", 10);
            } else if (gameObject.name == "Fireball")
            {
                PopUpUI.Instance.SetTitle("Fireball upgrade (40 coins)").SetMessage("By buying this item your attacks will fire a fireball (mutually exclusive with iceball)").Show("Fireball", 40);
            } else if (gameObject.name == "Iceball")
            {
                PopUpUI.Instance.SetTitle("Iceball upgrade (40 coins)").SetMessage("By buying this item your attacks will fire a iceball (mutually exclusive with fireball)").Show("Iceball", 40);
            }
        }
        
    }
}
