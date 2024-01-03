using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake() {
        // playButton.onClick.AddListener(PlayClick);

        // Lambda expression approach
        playButton.onClick.AddListener(() => {
            // Because Loader is static we can call it right away
            Loader.Load(Loader.Scene.GameScene);
        });

        quitButton.onClick.AddListener(() => {
            // Inside Inspector (running the game in dev mode) won't do anything
            // Until the game is already built, this will work
            Application.Quit();
        });

        Time.timeScale = 1f;
    }
}
