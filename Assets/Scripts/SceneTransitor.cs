using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransitor : SingletonComponent<SceneTransitor>
{
    private int nextSceneIndex = 1;
    Transform player;

    private List<Transform> sceneRoots = new List<Transform>();

    protected override void Awake()
    {
        base.Awake();
        //StartCoroutine(StartSequence());
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

    public void GoToNextScene()
    {
        sceneRoots[nextSceneIndex - 1].gameObject.SetActive(false);
        SceneManager.MoveGameObjectToScene(player.gameObject, SceneManager.GetSceneAt(nextSceneIndex+1));
        sceneRoots[nextSceneIndex].gameObject.SetActive(true);

        SceneManager.UnloadSceneAsync(nextSceneIndex);

        nextSceneIndex++;
    }
}

