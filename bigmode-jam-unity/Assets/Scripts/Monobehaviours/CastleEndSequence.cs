using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CastleEndSequence : MonoBehaviour
{
    public Image FadeToBlack;
    public GameObject YouWinUI;
    public GameObject CreditsUI;
    public GameObject Crown;
    public CastleCamera CastleCam;

    public MovingKeep[] Keeps;
    bool end;
    bool win;
    bool credits;

    private void Update()
    {
        if (win)
        {
            if (Input.anyKeyDown)
            {
                if (credits)
                {
                    FadeToBlack.DOFade(1f, 1f).OnComplete(() => SceneManager.LoadScene("MainMenu"));
                }

                credits = true;
                YouWinUI.SetActive(false);
                CreditsUI.SetActive(true);
            }

            return;
        }

        if (!CastleCam.BeginningOfTheEnd)
            return;

        foreach (var keep in Keeps)
        {
            if (keep.enabled)
                return;
        }

        // all keeps disabled
        EndSequence();
    }

    private void EndSequence()
    {
        end = true;
        Crown.SetActive(true);
        Crown.transform.DOLocalMoveY(10f, 7f).OnComplete(WinState);
    }

    private void WinState()
    {
        YouWinUI.SetActive(true);
        win = true;
    }
}
