using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Book))]
public class AutoFlip : MonoBehaviour
{
    public FlipMode Mode;
    public float PageFlipTime = 1;
    public float TimeBetweenPages = 1;
    public float DelayBeforeStarting = 0;
    public bool AutoStartFlip = true;
    public Book ControledBook;
    public int AnimationFramesCount = 40;

    bool isFlipping = false;

    [Header("Zoom + Scene Transition")]
    public Animator animator;
    public string sceneToLoad = "FinalColinTimeo";
    public float delayAfterZoom = 2f;

    private bool hasTriggeredTransition = false;

    void Start()
    {
        if (!ControledBook)
            ControledBook = GetComponent<Book>();

        if (AutoStartFlip)
            StartFlipping();

        ControledBook.OnFlip.AddListener(new UnityEngine.Events.UnityAction(PageFlipped));
    }

    void PageFlipped()
    {
        isFlipping = false;
    }

    public void StartFlipping()
    {
        StartCoroutine(FlipToEnd());
    }

    public void FlipRightPage()
    {
        // ✅ Dernière page → zoom + transition
        if (ControledBook.currentPage >= ControledBook.TotalPageCount - 1)
        {
            if (!hasTriggeredTransition)
            {
                hasTriggeredTransition = true;

                if (animator != null)
                    animator.SetTrigger("zoom");

                StartCoroutine(ZoomThenLoad());
            }
            return;
        }

        isFlipping = true;
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;
        StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx));
    }

    public void FlipLeftPage()
    {
        if (isFlipping) return;
        if (ControledBook.currentPage <= 0) return;

        isFlipping = true;
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;
        StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx));
    }

    IEnumerator ZoomThenLoad()
    {
        // ✅ Attendre 2 secondes après le zoom
        yield return new WaitForSeconds(delayAfterZoom);

        // ✅ Charger la scène
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            if (Application.CanStreamedLevelBeLoaded(sceneToLoad))
                SceneManager.LoadScene(sceneToLoad);
            else
                Debug.LogError("❌ Scene not in Build Settings : " + sceneToLoad);
        }
        else
        {
            Debug.LogError("❌ sceneToLoad est vide !");
        }
    }

    IEnumerator FlipToEnd()
    {
        yield return new WaitForSeconds(DelayBeforeStarting);

        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;

        switch (Mode)
        {
            case FlipMode.RightToLeft:
                while (ControledBook.currentPage < ControledBook.TotalPageCount)
                {
                    StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx));
                    yield return new WaitForSeconds(TimeBetweenPages);
                }
                break;

            case FlipMode.LeftToRight:
                while (ControledBook.currentPage > 0)
                {
                    StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx));
                    yield return new WaitForSeconds(TimeBetweenPages);
                }
                break;
        }
    }

    IEnumerator FlipRTL(float xc, float xl, float h, float frameTime, float dx)
    {
        float x = xc + xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);

        ControledBook.DragRightPageToPoint(new Vector3(x, y, 0));

        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookRTLToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x -= dx;
        }

        ControledBook.ReleasePage();
    }

    IEnumerator FlipLTR(float xc, float xl, float h, float frameTime, float dx)
    {
        float x = xc - xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);

        ControledBook.DragLeftPageToPoint(new Vector3(x, y, 0));

        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookLTRToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x += dx;
        }

        ControledBook.ReleasePage();
    }
}
