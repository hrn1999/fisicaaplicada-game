using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour {

    private readonly string loadScene;

    public void ChangeScene(string loadScene){
        SceneManager.LoadScene(loadScene);
    }
}
