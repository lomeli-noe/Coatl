using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{

    private LevelGrid levelGrid;

    private void Start()
    {
        levelGrid = new LevelGrid(20, 20);

    }
}
