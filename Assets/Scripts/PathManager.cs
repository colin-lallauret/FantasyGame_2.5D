using UnityEngine;

public class PathManager : MonoBehaviour
{
    [Header("Les Chemins Lumineux")]
    public GameObject path1;
    public GameObject path2;
    public GameObject path3;

    void Start()
    {
        // Au début, seul le chemin 1 est visible
        path1.SetActive(true);
        path2.SetActive(false);
        path3.SetActive(false);
    }

    // Cette fonction sera appelée par les zones de validation
    public void ReachValidation(int step)
    {
        if (step == 1)
        {
            path1.SetActive(false);
            path2.SetActive(true);
            Debug.Log("Validation 1 atteinte : Chemin 2 activé");
        }
        else if (step == 2)
        {
            path2.SetActive(false);
            path3.SetActive(true);
            Debug.Log("Validation 2 atteinte : Chemin 3 activé");
        }
        else if (step == 3)
        {
            path3.SetActive(false);
            Debug.Log("Validation 3 atteinte : Tous les chemins sont terminés");
        }
    }
}