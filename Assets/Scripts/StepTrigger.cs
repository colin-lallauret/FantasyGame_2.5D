using UnityEngine;

public class StepTrigger : MonoBehaviour
{
    [Range(2, 6)]
    public int stepNumber = 2;

    public bool playOnlyOnce = true;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (playOnlyOnce && triggered) return;

        triggered = true;

        if (StepAudioManager.instance != null)
        {
            StepAudioManager.instance.PlayStep(stepNumber);
        }
        else
        {
            Debug.LogError("❌ StepAudioManager introuvable dans la scène !");
        }
    }
}
