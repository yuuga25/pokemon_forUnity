using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class Login_Title : MonoBehaviour
{
    [SerializeField] GetPlayerCombinedInfoRequestParams InfoRequestParams;

    [SerializeField] private string NewCreatedSceneName;
    [SerializeField] private string HomeSceneName;

    [Header("ロード画面")]
    [SerializeField] private GameObject LoadingObject;
    [SerializeField] private Text LoadText;

    [SerializeField] private GameObject audiomanager_BGM;

    private string loadSceneName;
    private bool isLogin = false;

    private void Start()
    {
        isLogin = false;
        LoadingObject.SetActive(false);

        if (PlayerPrefs.GetInt("key") == 1)
        {
            AudioController.Master_Volume = PlayerPrefs.GetFloat("Volume_Master");
            AudioController.BGM_Volume = PlayerPrefs.GetFloat("Volume_BGM");
            AudioController.SE_Volume = PlayerPrefs.GetFloat("Volume_SE");
            AudioController.Voice_Volume = PlayerPrefs.GetFloat("Volume_Voice");
        }
        else
        {
            PlayerPrefs.SetInt("key", 1);
            PlayerPrefs.SetFloat("Volume_Master", AudioController.Master_Volume);
            PlayerPrefs.SetFloat("Volume_BGM", AudioController.BGM_Volume);
            PlayerPrefs.SetFloat("Volume_SE", AudioController.SE_Volume);
            PlayerPrefs.SetFloat("Volume_Voice", AudioController.Voice_Volume);
        }
    }

    public void Login()
    {
        if (!isLogin)
        {
            isLogin = true;

            PlayFabAuthService.Instance.InfoRequestParams = InfoRequestParams;
            PlayFabAuthService.OnLoginSuccess += PlayFabAuthService_OnLoginSuccess;
            PlayFabAuthService.Instance.Authenticate(Authtypes.Silent);
        }
    }

    private void PlayFabAuthService_OnLoginSuccess(LoginResult success)
    {
        DataLists.titleData_Pokémon = PlayFabSimpleJson.DeserializeObject<List<SaveData_Pokémon>>(success.InfoResultPayload.TitleData["pokeData"]);
        DataLists.titleData_Remember = PlayFabSimpleJson.DeserializeObject<List<Remember_Pokémon>>(success.InfoResultPayload.TitleData["rememberData"]);
        DataLists.titleData_Technique = PlayFabSimpleJson.DeserializeObject<List<Technique_Pokémon>>(success.InfoResultPayload.TitleData["techniqueData"]);
        DataLists.titleData_helpDatas = PlayFabSimpleJson.DeserializeObject<List<HelpData>>(success.InfoResultPayload.TitleData["helpData"]);
        DataLists.titleData_StageData = PlayFabSimpleJson.DeserializeObject<List<StageData>>(success.InfoResultPayload.TitleData["stageData"]);
        DataLists.titleData_CharacteristicDatas = PlayFabSimpleJson.DeserializeObject<List<CharacteristicData>>(success.InfoResultPayload.TitleData["characteristicData"]);

        //新規作成をしたかどうか
        if (success.NewlyCreated)
        {
            //ユーザー名を入力する画面に移管
            TutorialDataRegistration();
        }
        else
        {
            //メインメニューに移管
            LoadingObject.SetActive(true);
            loadSceneName = HomeSceneName;
            DataLists.isTutorialCompleted = success.InfoResultPayload.UserData["isTutorialCompleted"].Value;
            while (true)
            {
                if(DataLists.isTutorialCompleted != null)
                {
                    if (DataLists.isTutorialCompleted == "false")
                    {
                        break;
                    }
                    else if (DataLists.isTutorialCompleted != "false")
                    {
                        DataLists.playerData = PlayFabSimpleJson.DeserializeObject<PlayerData>(success.InfoResultPayload.UserData["PlayerData"].Value);
                        DataLists.playerData_Inventry = success.InfoResultPayload.UserInventory;
                        DataLists.player_Money = success.InfoResultPayload.UserVirtualCurrency["GD"];
                        DataLists.player_BattlePoint = success.InfoResultPayload.UserVirtualCurrency["BP"];
                        Debug.Log("ユーザーデータ取得");
                        break;
                    }
                }
            }
            StartCoroutine(SceneLoad());
            print("メインメニューに移管");
        }
    }

    private void TutorialDataRegistration()
    {
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>()
                {
                    {"isTutorialCompleted", "false"}
                }
            }, result => 
            { 
                Debug.Log("チュートリアルデータ登録成功");
                DataLists.isTutorialCompleted = "false";
                LoadingObject.SetActive(true);
                loadSceneName = NewCreatedSceneName;
                StartCoroutine(SceneLoad());
                print("ユーザー名を入力する画面に移管");
            }
            , error => { Debug.Log(error.GenerateErrorReport()); });
    }

    IEnumerator SceneLoad()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            LoadText.text = "読み込み中.";
            yield return new WaitForSeconds(0.5f);
            LoadText.text = "読み込み中..";
            yield return new WaitForSeconds(0.5f);
            LoadText.text = "読み込み中...";
            yield return new WaitForSeconds(0.5f);
            LoadText.text = "読み込み中";
            var lastData = DataLists.titleData_Pokémon.Last(x => x.p_Id != 0);
            if(lastData != null)
            {
                if(loadSceneName == HomeSceneName)
                {
                    if(DataLists.playerData != null)
                    {
                        if (DataLists.isTutorialCompleted == "false")
                        {
                            loadSceneName = NewCreatedSceneName;
                            print("ユーザー名を入力する画面に移管に変更");
                            LoadText.text = "アカウント作成しています";
                            break;
                        }

                        else if(DataLists.isTutorialCompleted != "false")
                        {
                            LoadText.text = "ホームに移動します";
                            break;

                        }
                    }
                }
                else
                {
                    LoadText.text = "アカウント作成しています";
                    break;
                }
            }
        }

        AsyncOperation loadScene = SceneManager.LoadSceneAsync(loadSceneName);
        loadScene.allowSceneActivation = false;

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
                    LoadingObject.SetActive(false);
                    loadScene.allowSceneActivation = true;
                }
            }
        }
        else
        {
            LoadingObject.SetActive(false);
            loadScene.allowSceneActivation = true;
        }
    }
}