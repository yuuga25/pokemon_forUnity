using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;

public class GameManager : MonoBehaviour
{
    public void LoadTitle()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        SceneManager.LoadScene("TitleScene");
    }
}
