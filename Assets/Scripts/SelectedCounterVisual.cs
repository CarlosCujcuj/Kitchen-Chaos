using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour {

    [SerializeField] private BaseCounter baseCounter; // the counter with normal material
    [SerializeField] private GameObject[] visualGameObjectArray;

    // In the Player script we make an assigment in the Awake method. 
    // So we use Start here this code runs after that assigment in Player
    private void Start() {
        // That Instance is 'static' that's why we can reach that variable here
        // otherwise we would have to reference that script in another way 
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e) {

        // Because the event publishes the hitted counter to all of his suscribers (Counter instances)
        // we have to check if the hitted Counter passed in Args is ourselves so we can light-up the gameObject
        if (e.selectedCounter == baseCounter){
            Show();
        }else{
            Hide();
        }
    }


    private void Show() {
        foreach (GameObject visualGameObject in visualGameObjectArray) {
            visualGameObject.SetActive(true);
        }
    }

    private void Hide() {
        foreach (GameObject visualGameObject in visualGameObjectArray) {
            visualGameObject.SetActive(false);
        }
    }
}
