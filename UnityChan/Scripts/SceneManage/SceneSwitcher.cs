using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private static SceneSwitcher instance;
    public static SceneSwitcher Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneSwitcher>();
                DontDestroyOnLoad(instance);
            }

            return instance;
        }
    }

    private GameObject playerBeforeScene;
    
    public GameObject[] GateInsideLabList { get; private set; }
    public GameObject[] GateOutsideLabList { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            GameObject map = GameObject.Find("Lab");
            GateInsideLabList = new GameObject[map.transform.childCount];
            GateOutsideLabList = new GameObject[map.transform.childCount];
            for (int i = 0; i < GateInsideLabList.Length; i++)
            {
                GateInsideLabList[i] = map.transform.GetChild(i).gameObject;
                GateInsideLabList[i].AddComponent<GateScript>();
            }
            
            DontDestroyOnLoad(gameObject);
        }
    }


    public void SwitchToScene()
    {
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        
        if (GameObject.Find("Player_Lab"))
        {
            playerBeforeScene = GameObject.Find("Player_Lab");
            SceneManager.LoadSceneAsync("SampleScene");
        }
        else if (GameObject.Find("Player_Lumia"))
        {
            playerBeforeScene = GameObject.Find("Player_Lumia");
            SceneManager.LoadSceneAsync("Laboratory");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
        if (playerBeforeScene != null)
        {
            Destroy(playerBeforeScene);
            playerBeforeScene = null;
        }
        
        GameObject playerInNewScene = null;

        if (scene.name.Equals("SampleScene"))
        {
            playerInNewScene = GameObject.Find("Player_Lumia");  // Find player in Lumia scene
            Debug.Log(playerInNewScene.tag);//여기는 플레이어인데 ?
        }
        else if (scene.name.Equals("Laboratory"))
        {
            playerInNewScene = GameObject.Find("Player_Lab");  // Find player in Lab scene
        }
        
        /*// Once the new scene (BS) is loaded, find the new "Player" in the scene
        GameObject newPlayerObject = GameObject.Find("Player");
        if (newPlayerObject == null)
        {
            Debug.LogError("No Player object found in the new scene!");
            return;
        }

        // Reassign existing singleton components to the new "Player"
        ReassignSingletonToNewPlayer(newPlayerObject);*/


        // Unsubscribe from the sceneLoaded event to prevent future calls
        
    }

    private void ReassignSingletonToNewPlayer(GameObject newPlayer)
    {
        // Move SingletonA to the new Player
        Inventory_Player.Instance.transform.SetParent(newPlayer.transform);
        Inventory_Player.Instance.transform.localPosition = Vector3.zero; // Reset position or as needed

        // Move SingletonB to the new Player
        PlayerQuestWindow.Instance.transform.SetParent(newPlayer.transform);
        PlayerQuestWindow.Instance.transform.localPosition = Vector3.zero;

        // Move SingletonC to the new Player
        CharacterInput.Instance.transform.SetParent(newPlayer.transform);
        CharacterInput.Instance.transform.localPosition = Vector3.zero;

        // Optionally adjust other properties or behaviors for the new scene...
    }
}
