using System;
using UnityEngine;

namespace PopUp
{
    public class InitialPopUp : MonoBehaviour
    {
        private void Start()
        {
            PopUpUI.Instance.SetTitle("Initial title").SetMessage("Initial message").OnClose(() => Debug.Log("Closed")).Show();
        }
    }
}
