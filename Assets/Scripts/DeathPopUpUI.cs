using System;
using System.Collections;
using PlayerScripts;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DeathPopUp
{
    public string Title = "Hello adventurer";
    public UnityAction OnClose;
}
    
public class DeathPopUpUI : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private TMP_Text titleUIText;
    [SerializeField] private Button returnUIButton;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject levelLoader;

    DeathPopUp popUp = new DeathPopUp();

    public static DeathPopUpUI Instance;
    private PlayerController playerController;
    private string item;
    private void Awake()
    {
        Instance = this;
        playerController = player.GetComponent<PlayerController>();
            
        returnUIButton.onClick.RemoveAllListeners();
        returnUIButton.onClick.AddListener(ReturnToMenu);

        canvas.SetActive(false);
    }

    private void ReturnToMenu()
    {
        StartCoroutine(LoadMenu());
        Close();
    }

    private IEnumerator LoadMenu()
    {
        Time.timeScale = 1;
        levelLoader.GetComponent<CrossfadeController>().FadeToBlack();
        yield return new WaitForSecondsRealtime(1.1f);
        SceneManager.LoadScene(0);
    }

    public DeathPopUpUI SetTitle(string title)
    {
        popUp.Title = title;
        return Instance;
    }

    public DeathPopUpUI OnClose(UnityAction action)
    {
        popUp.OnClose = action;
        return Instance;
    }

    public void Show()
    {
        titleUIText.text = popUp.Title;
        canvas.SetActive(true);
    }
        
    public void Close()
    {
        canvas.SetActive(false);
            
        if(popUp.OnClose != null)
            popUp.OnClose.Invoke();
            
        // Reset pop-up
        popUp = new DeathPopUp();
    }
}