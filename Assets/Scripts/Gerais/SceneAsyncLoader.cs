using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneAsyncLoader : MonoBehaviour
{
    private AsyncOperation asyncLoad;
    [SerializeField] private Button skipButtom;


    // Start is called before the first frame update
    private void Start()
    {
        int indexScene;
        try { indexScene = SceneManager.GetActiveScene().buildIndex + 1; }
        catch { indexScene = 0; }
        asyncLoad = SceneManager.LoadSceneAsync(indexScene);
        asyncLoad.allowSceneActivation = false;
        StartCoroutine(SceneIsDone());
    }

    private IEnumerator SceneIsDone()
    {
        yield return new WaitForSeconds(1);
        if (asyncLoad.progress >= 0.9f)
            skipButtom.gameObject.SetActive(true);
        else
            StartCoroutine(SceneIsDone());
        //Debug.Log("Load Progress " + asyncLoad.progress);
    }

    public void LoadScene()
    {
        //Debug.Log("Carregar");
        asyncLoad.allowSceneActivation = true;
    }
}