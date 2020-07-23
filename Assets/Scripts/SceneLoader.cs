using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private int levelSelected;

    private void Awake()
    {
        int levelCount = FindObjectsOfType<SceneLoader>().Length;
        if (levelCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else 
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            QuitGame();
        }
    }

    public int GetSelectedLevel()
    {
        return levelSelected;
    }

    public void LoadNextScene(int level)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
        levelSelected = level;
    }

   public void LoadGameOverScene()
    {
        StartCoroutine(WaitAndLoad());
    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
