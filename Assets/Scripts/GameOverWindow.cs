using UnityEngine;
using CodeMonkey.Utils;

public class GameOverWindow : MonoBehaviour
{

    private static GameOverWindow instance;

    private void Awake()
    {
        instance = this;

        transform.Find("RetryButton").GetComponent<Button_UI>().ClickFunc = () =>
        {
            Loader.Load(Loader.Scene.GameScene);
            FindObjectOfType<AudioManager>().Play("cumbia");
        };

        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public static void ShowStatic()
    {
        instance.Show();
    }
}
