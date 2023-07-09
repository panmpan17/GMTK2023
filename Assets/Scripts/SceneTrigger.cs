using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    public string SceneName;
    public void Trigger()
    {
        SceneManager.LoadScene(SceneName);
    }
}
