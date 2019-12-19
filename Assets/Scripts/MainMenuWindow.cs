using UnityEngine;
using CodeMonkey.Utils;

public class MainMenuWindow : MonoBehaviour
{

    private enum Sub
    {
        Main,
        Instructions
    }

    private void Awake()
    {
        transform.Find("MainSub").Find("PlayButton").GetComponent<Button_UI>().ClickFunc = () =>
        {
            Loader.Load(Loader.Scene.GameScene);
        };

        transform.Find("MainSub").Find("QuitButton").GetComponent<Button_UI>().ClickFunc = () => Application.Quit();

        transform.Find("MainSub").Find("HowToPlayButton").GetComponent<Button_UI>().ClickFunc = () => ShowSub(Sub.Instructions);

        transform.Find("InstructionsSub").Find("InstructionsWindow").GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        transform.Find("InstructionsSub").Find("InstructionsWindow").Find("BackButton").GetComponent<Button_UI>().ClickFunc = () => ShowSub(Sub.Main);

        ShowSub(Sub.Main);
    }

    private void ShowSub(Sub sub)
    {
        transform.Find("MainSub").gameObject.SetActive(false);
        transform.Find("InstructionsSub").gameObject.SetActive(false);

        switch (sub)
        {
            case Sub.Main:
                transform.Find("MainSub").gameObject.SetActive(true);
                break;
            case Sub.Instructions:
                transform.Find("InstructionsSub").gameObject.SetActive(true);
                break;
        }
    }
}
