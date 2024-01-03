using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Because is static, it means that it's not attached to any instance of an object
// Cannot be attached to any object and cannot have any instances constructed
// Making the class 'static' is a good approach if everything in this class is 
// also meant to be static (and this requires the class to have all element inside 
// to be static, otherwise it will throw an error)
public static class Loader {

    public enum Scene {
        MainMenuScene,
        GameScene,
        LoadingScene
    }

    private static Scene targetScene;

    public static void Load(Scene targetScene) {
        // This is a static class so in order to access 'targetScene' we need to access
        // through the class name because inside this function we alsohave a local variable
        // with the same name
        Loader.targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback() {
        // Because Unity doesn't allow us to call out custom enum
        // we can convert the enum to string
        SceneManager.LoadScene(targetScene.ToString());
    }
}
