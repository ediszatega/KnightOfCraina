using System;
using PlayerScripts;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace PopUp
{
    public class PopUp
    {
        public string Title = "Hello adventurer";
        public string Message = "This is a shop where you can buy different upgrades and power-ups";
        public UnityAction OnClose;
    }
    
    public class PopUpUI : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private TMP_Text titleUIText;
        [SerializeField] private TMP_Text messageUIText;
        [SerializeField] public Button buyUIButton;
        [SerializeField] private Button closeUIButton;
        [SerializeField] private GameObject player;

        PopUp popUp = new PopUp();

        public static PopUpUI Instance;
        private PlayerController playerController;
        private string item;
        private void Awake()
        {
            Instance = this;
            playerController = player.GetComponent<PlayerController>();
            
            buyUIButton.onClick.RemoveAllListeners();
            buyUIButton.onClick.AddListener(ItemPurchase);
            
            
            closeUIButton.onClick.RemoveAllListeners();
            closeUIButton.onClick.AddListener(Close);
            canvas.SetActive(false);

            
        }

        private void ItemPurchase()
        {
            switch (item)
            {
                case "Heart":
                    playerController.SetUpgrade(1);
                    break;
                case "Shield":
                    playerController.SetUpgrade(2);
                    break;
                case "Sword":
                    playerController.SetUpgrade(3);
                    break;
                case "Speed":
                    playerController.SetUpgrade(4);
                    break;
                case "Fireball":
                    playerController.SetUpgrade(5);
                    break;
                case "Iceball":
                    playerController.SetUpgrade(6);
                    break;
            }
            Close();
        }

        public PopUpUI SetTitle(string title)
        {
            popUp.Title = title;
            return Instance;
        }
        
        public PopUpUI SetMessage(string message)
        {
            popUp.Message = message;
            return Instance;
        }
        
        public PopUpUI OnClose(UnityAction action)
        {
            popUp.OnClose = action;
            return Instance;
        }

        public void Show(string item = null, int price = Int32.MaxValue)
        {
            if (playerController.currentCoins < price)
                buyUIButton.enabled = false;
            else
                buyUIButton.enabled = true;
            this.item = item;
            titleUIText.text = popUp.Title;
            messageUIText.text = popUp.Message;

            canvas.SetActive(true);
        }
        
        public void Close()
        {
            canvas.SetActive(false);
            
            if(popUp.OnClose != null)
                popUp.OnClose.Invoke();
            
            // Reset pop-up
            popUp = new PopUp();

        }
    }
}
