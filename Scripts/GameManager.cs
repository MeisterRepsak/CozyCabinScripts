using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject playerPrefab;
    public List<GameObject> trees = new List<GameObject>();

    public bool hasSpawnedPlayer = false;
    public bool hasBuiltBridge = false;
    public bool gatheringAreaUnlocked = false;


    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasSpawnedPlayer && SceneTracker.instance.currentScene == Scenes.Cabin)
            SpawnPlayer();

        
    }

    public void FindTrees()
    {
        trees.Clear();
        print("yuh");
        for (int i = 0; i < GameObject.Find("PænereTræHolder").transform.childCount; i++)
        {
            trees.Add(GameObject.Find("PænereTræHolder").transform.GetChild(i).gameObject);
        }
    }

    public void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, GameObject.Find("SpawnPointCabin").transform.position,Quaternion.identity);
        DontDestroyOnLoad(player);
        hasSpawnedPlayer = true;
    }
    public void CheckIfTreesGone()
    {
        print(trees.Count);
        if(trees.Count == 0)
        {
            gatheringAreaUnlocked = true;
            GameObject.Find("BeachTrigger").GetComponent<BoxCollider>().enabled = true;
        }
    }

    public void ActivateBridge()
    {
        GameObject bridge = GameObject.Find("bridge");
        bridge.GetComponent<MeshCollider>().enabled = true;
        bridge.GetComponent<MeshRenderer>().enabled = true;
    }
}
