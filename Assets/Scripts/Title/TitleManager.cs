using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleManager : MonoBehaviour
{
    [SerializeField] string nextScene = default;

    public void OnStartButton()
    {
        SceneManager.LoadScene(nextScene);
    }
    public void OnContinueButton()
    {
        SceneManager.LoadScene(nextScene);
    }
}
