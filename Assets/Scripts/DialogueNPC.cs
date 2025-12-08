using UnityEngine;

public class DialogueNPC : MonoBehaviour
{
    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] dialogueLines;

    [Header("UI Hint")]
    public GameObject interactHint;   // "Appuie sur E pour parler"

    private bool isPlayerInRange = false;

    private void Start()
    {
        if (interactHint != null)
            interactHint.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (interactHint != null)
                interactHint.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            if (interactHint != null)
                interactHint.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (DialogueManager.instance != null)
            {
                // ✅ Ne démarrer le dialogue que s'il n'y en a pas déjà un d'ouvert
                if (!DialogueManager.instance.IsDialogueOpen)
                {
                    DialogueManager.instance.StartDialogue(dialogueLines);
                }
                // Sinon, on ne fait rien ici : 
                // c'est DialogueManager qui gère le E pour passer à la suite.
            }
        }
    }
}
