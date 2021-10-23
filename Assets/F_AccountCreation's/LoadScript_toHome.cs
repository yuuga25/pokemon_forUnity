using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class LoadScript_toHome : MonoBehaviour
{
    public GameObject error_Image;

    [SerializeField] private Text loadText;
    [SerializeField] private GameObject audiomanager_BGM;
    [SerializeField] private string loadSceneName;
    void Start()
    {
        StartCoroutine(loadHome());
    }

    private void UpdatePlayerDataNext(AsyncOperation loadScene)
    {
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>()
                {
                    {"PlayerData", PlayFabSimpleJson.SerializeObject(DataLists.playerData) },
                    {"isTutorialCompleted", "true" }
                }
            },
            result =>
            {
                Debug.Log("プレイヤーデータの登録成功");
                loadScene.allowSceneActivation = true;
            },
            error => { Debug.Log(error.GenerateErrorReport()); error_Image.SetActive(true); });
    }

    private IEnumerator loadHome()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(loadSceneName);
        loadScene.allowSceneActivation = false;

        while(true)
        {
            loadText.text = "Now Loading";
            if(loadScene != null)
            {
                break;
            }
        }

        var bgm = audiomanager_BGM.GetComponent<AudioSource>();
        var audioController = audiomanager_BGM.GetComponent<AudioController>();
        audioController.enabled = false;
        if(bgm.volume != 0)
        {
            var reduce = bgm.volume / 10;
            while (true)
            {
                bgm.volume = bgm.volume - reduce;
                yield return new WaitForSeconds(0.3f);
                if(bgm.volume == 0)
                {
                    UpdatePlayerDataNext(loadScene);
                }
            }
        }
        else
        {
            UpdatePlayerDataNext(loadScene);
        }
    }
}
