using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour {

    // Here we just want to wait for the first 'update' in order to show this
    // Loading scene at least in 1 frame because if the next scene is too big it will 
    // take time to load and would look like the game freezed
    
    private bool isFirstUpdate = true;

    private void Update() {
        if (isFirstUpdate) {
            isFirstUpdate = false;

            Loader.LoaderCallback();
        }
    }

}
