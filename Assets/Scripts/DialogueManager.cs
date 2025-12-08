using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    private string[] lines;
    private int currentIndex = 0;
    private bool isDialogueOpen = false;
    public bool IsDialogueOpen => isDialogueOpen;   // ✅ getter public

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        // Ici on gère juste la progression dans le dialogue
        if (isDialogueOpen && Input.GetKeyDown(KeyCode.E))
        {
            NextLine();
        }
    }

    public void StartDialogue(string[] newLines)
    {
        if (newLines == null || newLines.Length == 0)
            return;

        lines = newLines;
        currentIndex = 0;
        isDialogueOpen = true;

        dialoguePanel.SetActive(true);
        dialogueText.text = lines[currentIndex];
    }

    void NextLine()
    {
        currentIndex++;

        if (currentIndex >= lines.Length)
        {
            EndDialogue();
        }
        else
        {
            dialogueText.text = lines[currentIndex];
        }
    }

    void EndDialogue()
    {
        isDialogueOpen = false;
        dialoguePanel.SetActive(false);
    }
}
