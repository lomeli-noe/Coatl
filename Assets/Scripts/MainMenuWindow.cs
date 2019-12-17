using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MainMenuWindow : MonoBehaviour
{
    private IEnumerator coroutine;
    GameObject soundGameObject;
    AudioSource audioSource;

    private void Awake()
    {
        transform.Find("PlayButton").GetComponent<Button_UI>().AddButtonSounds();
        transform.Find("PlayButton").GetComponent<Button_UI>().ClickFunc = () =>
        {
            coroutine = WaitAndPrint(1.0f);
            StartCoroutine(coroutine);
        };

        transform.Find("QuitButton").GetComponent<Button_UI>().ClickFunc = () => Application.Quit();

    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(GameAssets.instance.ButtonClick);
        yield return new WaitForSeconds(waitTime);
        Loader.Load(Loader.Scene.GameScene);
    }

    private void Start()
    {
        soundGameObject = new GameObject("Sound");
        audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GameAssets.instance.BGMusic);
    }
}
