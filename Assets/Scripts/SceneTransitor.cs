using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransitor : SingletonComponent<SceneTransitor>
{
    Transform player;

    [SerializeField] private int nextSceneIndex = 0;
    [SerializeField] private List<Transform> sceneRoots = new List<Transform>();
    private int atScene = 1;

    public void StartGame()
    {
        StartCoroutine(StartSequence());
    }


    private IEnumerator StartSequence()
    {
        yield return LoadScenesAsync();

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (i == 0) continue;

            var sceneRootObject = SceneManager.GetSceneAt(i).GetRootGameObjects()[0];

                sceneRoots.Add(sceneRootObject.transform);

            if (i != 0 && i != 1)
                sceneRootObject.SetActive(false);
        }

    }

    public IEnumerator LoadScenesAsync()
    {
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            yield return new WaitForSeconds(2);
            yield return SceneManager.LoadSceneAsync(i, LoadSceneMode.Additive);

            if (i == 1)
            {
                player = FindObjectOfType<PlayerCombatManager>().transform;
            }
        }
    }

    [Button]
    public void GoToNextScene()
    {
        Debug.Log(nextSceneIndex);
        Debug.Log(sceneRoots[nextSceneIndex - 1], sceneRoots[nextSceneIndex - 1].gameObject);

        sceneRoots[nextSceneIndex - 1].gameObject.SetActive(false);

        var nextScene = SceneManager.GetSceneAt(atScene + 1);
        Debug.Log(nextScene.name);
        SceneManager.MoveGameObjectToScene(player.gameObject, nextScene);
        sceneRoots[nextSceneIndex].gameObject.SetActive(true);

        //SceneManager.UnloadSceneAsync(nextSceneIndex);
        sceneRoots.RemoveAt(nextSceneIndex - 1);
        //nextSceneIndex++;

        atScene++;
    }
}

