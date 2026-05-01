using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] private UIDocument _menu;

    private void Start()
    {
        var newGameButton = _menu.rootVisualElement.Q<Button>("NewGameButton");
        newGameButton.clicked += OnNewGameClicked;
        
        var HTPButton = _menu.rootVisualElement.Q<Button>("HowToPlayButton");
        HTPButton.clicked += OnHTPButtonClicked;
        
        var quitButton = _menu.rootVisualElement.Q<Button>("QuitButton");
        quitButton.clicked += OnQuitButtonClicked;
    }

    

    void OnNewGameClicked()
    {
        SceneManager.LoadScene("MainDungeon");
    }
    
    void OnQuitButtonClicked()
    {
        Application.Quit();
    }
    
    void OnHTPButtonClicked()
    {
        SceneManager.LoadScene("HowToPlay");
    }
}
