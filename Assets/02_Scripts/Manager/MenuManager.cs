using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("메인메뉴")]
    public string sceneName;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GameStartBUT()
    {
        SceneManager.LoadScene(sceneName);
    }
}
