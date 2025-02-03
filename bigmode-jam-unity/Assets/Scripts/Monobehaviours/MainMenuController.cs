using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Image FadeToBlack;
    bool starting;

    void Update()
    {
        if (Input.anyKeyDown && !starting)
        {
            starting = true;
            FadeToBlack.DOFade(1f, 1f).OnComplete(()=>SceneManager.LoadScene("Game"));
        }
    }
}
