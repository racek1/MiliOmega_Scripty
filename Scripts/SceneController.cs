using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneController : MonoBehaviour
{
    [Header("Variables")]
    public Animator sceneTransitionAnimator;
    public 
    void Start()
    {
        
    }

    public void GotoScene(string sceneName)
    {
        StartCoroutine(GotoSceneEnum(sceneName));
    }

    private IEnumerator GotoSceneEnum(string sceneName)
    {
        sceneTransitionAnimator.SetTrigger("close");
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene(sceneName);
    }

}
