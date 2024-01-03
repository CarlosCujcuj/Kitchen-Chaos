using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
    
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    // Unity doesn't show Interfaces in the editor so we need to 
    // Serialize a GameObject and the assign it to the desire interface
    private IHasProgress hasProgress;

    private Animator animator;


    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null) {
            Debug.LogError("Game object " + hasProgressGameObject + " doe not have a component that implements IHasProgress");
        }


        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        barImage.fillAmount = 0f;

        // Call this after we listen to our event, if we would have called on Awake()
        // that would disable the game object and the start would never run
        // and in consequence it would never be called by our event
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        barImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized == 0f || e.progressNormalized == 1f) {
            Hide();
        } else {
            Show();
        }
    }

    private void Show() {
        // gameObject refers to this object itself
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
    
}
