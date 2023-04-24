using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public enum Scenes
{
    // 0
    MainMenu,
    // 1
    Cabin,
    // 2
    Beach,
    // 3
    CabinInside,
    // 4
    GatheringArea,
};
public class SceneTracker : MonoBehaviour
{
    public Scenes currentScene;
    int lastScene;
    public static SceneTracker instance;
    
    void Start()
    {
        instance = this;
        SceneManager.sceneUnloaded += GetNextScene;
        SceneManager.sceneLoaded += LoadNewScene;
        DontDestroyOnLoad(FindObjectOfType<EventSystem>());
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(FindObjectOfType<GameManager>());
        SceneManager.LoadScene(1);
    }

    public void LoadNewScene(Scene _nextScene, LoadSceneMode mode)
    {
        currentScene = (Scenes)_nextScene.buildIndex;
       
        switch (currentScene)
        {
            case Scenes.Beach:
                if (!GameManager.instance.hasSpawnedPlayer)
                    return;

                GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.Find("SpawnPointBeach").transform.position;

                break;

            case Scenes.Cabin:
               
                if (!GameManager.instance.gatheringAreaUnlocked)
                {
                    print("asd");
                    GameManager.instance.FindTrees();
                }

                if (GameManager.instance.hasBuiltBridge)
                    GameManager.instance.ActivateBridge();
                if (!GameManager.instance.hasSpawnedPlayer)
                    return;
                if (lastScene == (int)Scenes.Beach)
                    GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.Find("SpawnPointBeachToCabin").transform.position;
                else if(lastScene == (int)Scenes.CabinInside)
                    GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.Find("SpawnPointCabin").transform.position;


                break;
            case Scenes.CabinInside:
                if (!GameManager.instance.hasSpawnedPlayer)
                    return;
                GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.Find("SpawnPointCabinInside").transform.position;

                break;
            case Scenes.GatheringArea:
                if (!GameManager.instance.hasSpawnedPlayer)
                    return;
                GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.Find("SpawnPoint").transform.position;
                break;
        }
    }
    public void GetNextScene(Scene current)
    {
        lastScene = SceneManager.GetSceneByName(current.name).buildIndex;
        print(lastScene);
    }
}
