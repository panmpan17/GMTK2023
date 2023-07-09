using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public static End ins;

    [SerializeField]
    private GameObject success;
    [SerializeField]
    private GameObject fail;

    void Awake()
    {
        ins = this;

        GetComponent<Canvas>().enabled = false;
        success.SetActive(false);
        fail.SetActive(false);
    }

    public void Show(bool isSuccess)
    {
        Tutorial.IsSkip = true;

        // PlayerDisplay.ins.gameObject.SetActive(false);
        CardChooser.ins.HideAllCards();
        PlayerDisplay.ins.Hide();

        if (isSuccess)
        {
            success.SetActive(true);
        }
        else
        {
            fail.SetActive(true);
        }

        GetComponent<Canvas>().enabled = true;
    }

    public void Replay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
