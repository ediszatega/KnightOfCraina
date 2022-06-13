using TMPro;
using UnityEngine;

namespace PopUpScripts
{
    public class PopUpSystem : MonoBehaviour
    {
        public GameObject popUpBox;
        public Animator animator;
        public TMP_Text popUpText;
    
        public void PopUp(string text)
        {
            popUpBox.SetActive(true);
            popUpText.text = text;
            animator.SetTrigger("pop");
        }
    }
}