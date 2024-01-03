using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    // Enums: fixed set of options that you can have of something
    private enum Mode {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted
    }

    // This will be shown in the editor as a select field because of the enum
    [SerializeField] private Mode mode;
    
    // System function that runs afterward the Update function
    private void LateUpdate() {
        // In old unity versions Camera.main did not used to be cached
        // so every time you use this it would cycle through every single
        // gameobject in the scene until it found the main camera (obviously bad performance)
        // Nowadays this field is cache by default in the Unity backend
        switch (mode) {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;

            case Mode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;

            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
        
    }
}
