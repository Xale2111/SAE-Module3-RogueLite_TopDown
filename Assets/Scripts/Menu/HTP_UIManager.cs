using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HtpUIManager : MonoBehaviour
{
    [SerializeField] private UIDocument _menu;

    private void Start()
    {
        var mainMenu = _menu.rootVisualElement.Q<Button>("MainMenuButton");
        mainMenu.clicked += OnMainMenuClicked;
    }

    void OnMainMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
