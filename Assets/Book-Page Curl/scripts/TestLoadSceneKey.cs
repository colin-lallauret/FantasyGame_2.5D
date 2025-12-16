using UnityEngine;
using UnityEngine.SceneManagement;

public class TestLoadSceneKey : MonoBehaviour
{
    public string sceneToLoad;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("✅ Touche L détectée -> LoadScene : " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
