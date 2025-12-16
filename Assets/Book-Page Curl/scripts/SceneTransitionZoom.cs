using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionZoom : MonoBehaviour
{
    [Header("Camera")]
    public Camera mainCamera;

    [Header("Zoom Settings")]
    public float targetFOV = 20f;
    public float zoomDuration = 1f;

    [Header("Scene Transition")]
    public string sceneToLoad;
    public float delayAfterZoom = 2f;

    private float startFOV;
    private bool isTransitioning = false;

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null)
            startFOV = mainCamera.fieldOfView;
    }

    public void StartTransition()
    {
        if (isTransitioning) return;

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("❌ SceneTransitionZoom : sceneToLoad est vide !");
            return;
        }

        if (mainCamera == null)
        {
            Debug.LogError("❌ SceneTransitionZoom : mainCamera est NULL !");
            return;
        }

        // Vérifie que la scène existe dans le build
        if (!Application.CanStreamedLevelBeLoaded(sceneToLoad))
        {
            Debug.LogError("❌ SceneTransitionZoom : Scene NOT in Build Settings -> " + sceneToLoad);
            return;
        }

        Debug.Log("✅ StartTransition() -> Zoom puis load : " + sceneToLoad);
        isTransitioning = true;
        StartCoroutine(ZoomAndLoadScene());
    }

    IEnumerator ZoomAndLoadScene()
    {
        float t = 0f;

        // ZOOM
        while (t < zoomDuration)
        {
            t += Time.unscaledDeltaTime; // marche même si Time.timeScale change
            float k = Mathf.Clamp01(t / zoomDuration);
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, k);
            yield return null;
        }

        Debug.Log("✅ Zoom fini -> attente : " + delayAfterZoom + "s");
        yield return new WaitForSecondsRealtime(delayAfterZoom);

        Debug.Log("🚀 LoadScene : " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}
