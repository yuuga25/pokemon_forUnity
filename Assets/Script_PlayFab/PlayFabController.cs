using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using UnityEngine;

public class PlayFabController : MonoBehaviour
{
    private void Start()
    {
        /*　ログイン処理　初期
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
        {
            CustomId = "GettingStaetedGuide",
            CreateAccount = true
        }
        , result =>
        {
            Debug.Log("ログイン成功");
        }
        , error =>
        {
            Debug.Log("ログイン失敗");
        }) ;
        */

        //匿名ログイン
        PlayFabClientAPI.LoginWithCustomID(
            new LoginWithCustomIDRequest
            {
                TitleId = PlayFabSettings.TitleId,
                CustomId = "100",
                CreateAccount = true
            }
        , result => { Debug.Log("ログイン成功"); }
        , error => { Debug.Log(error.GenerateErrorReport()); });
    }

    #region//プレイヤーデータの管理一覧
    //プレイヤーデータ（タイトル）をスクリプト上から作成して保存する
    public void SetUserData()
    {
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>()
                {
                    {"Name", "nekojoker"},
                    {"Exp",  "500" }
                }
            },
        result => { Debug.Log("プレイヤーデータの登録成功！"); },
        error  => { Debug.Log(error.GenerateErrorReport()); });
    }

    //他人のプレイヤーデータ（タイトル）を読み込む（アクセス許可が"パブリック"の時のみ）
    public void GetUserData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = "F63A63C659A18AB5"　//指定したいプレイヤーIDを入力（カスタムIDでは不可）
        }, 
        result =>{ Debug.Log(result.Data["Name"].Value);}, 
        error =>{ Debug.Log(error.GenerateErrorReport());});
    }

    //プレイヤーデータの更新（ほぼプレイヤーデータを作成して保存すると同じ動き　UpdateUserDataRequestが未登録の場合は新規、登録済みの場合は更新する仕組みだから）
    public void UpdateUserData()
    {
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>()
                {
                    {"Name", "kimidori25252" },
                    {"Exp" , "1000"}
                }
            },
        result => { Debug.Log("ユーザーデータの更新成功"); },
        error => { Debug.Log(error.GenerateErrorReport()); });
    }

    //プレイヤーデータの削除
    public void UpdateUserData_Delete()
    {
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>()
                {
                    {"Name", "kimidori25252"},
                    //{"Exp", "1000"}　Valueをnullにすることでも削除することができる
                },
                KeysToRemove = new List<string> { "Exp" } //KeyをRemoveすることで削除することができる
            },
        result => { Debug.Log("プレイヤーデータのExp削除成功"); },
        error => { Debug.Log(error.GenerateErrorReport()); });
    }

    /*クエストデータの登録（悪い例）
    public void SetUserData_Quest()
    {
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>()
                {
                    {"Quest1", "100" },
                    {"Quest2", "200" }
                }
            },
        result => { Debug.Log("プレイヤーデータの登録成功（クエスト）"); },
        error => { Debug.Log(error.GenerateErrorReport()); });
    }*/


    #region//プレイヤーデータにJson形式で保存する方法

    /// <summary>
    /// プレイヤーデータの登録
    /// </summary>
    public void SetUserData_Quest()
    {
        var questInfos = new List<QuestInfo>
        {
            new QuestInfo{ Id = 1, ClearTime = 20, Score = 100},
            new QuestInfo{ Id = 2, ClearTime = 30, Score = 200},
        };
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>()
                {
                    { "QuestInfo", PlayFabSimpleJson.SerializeObject(questInfos) }
                }
            },
        result => { Debug.Log("プレイヤーデータの登録成功"); },
        error => { Debug.Log(error.GenerateErrorReport()); });
    }
    /// <summary>
    /// プレイヤーデータの取得
    /// </summary>
    public void GetUserData_Quest()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
            result =>
            {
                var questInfos = PlayFabSimpleJson.DeserializeObject<List<QuestInfo>>(result.Data["QuestInfo"].Value);
                foreach (var quest in questInfos)
                {
                    Debug.Log($"Id: {quest.Id}");
                    Debug.Log($"ClearTime: {quest.ClearTime}");
                    Debug.Log($"Score: {quest.Score}");
                }
            },
            error => { Debug.Log(error.GenerateErrorReport()); });
    }

    /// <summary>
    /// クエスト情報のプレイヤーデータ
    /// </summary>
    public class QuestInfo
    {
        public int Id { get; set; }
        public int ClearTime { get; set; }
        public int Score { get; set; }
    }
    #endregion
    #endregion

    #region//タイトルデータの管理一覧
    //タイトルデータを取得する
    public void GetTitleData()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result =>
            {
                if (result.Data.ContainsKey("DeveloperWebSite"))
                {
                    Debug.Log("DeveloperWebSite: "
                        + result.Data["DeveloperWebSite"]);
                }
            },
            error =>
            {
                Debug.Log(error.GenerateErrorReport());
            });
    }

    //クエスト情報のJSON
    public void GetTitleData_Json() 
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result =>
            {
                if (result.Data.ContainsKey("QuestMaster"))
                {
                    var questMaster = PlayFabSimpleJson.DeserializeObject<List<QuestMaster>>(result.Data["QuestMaster"]);
                    foreach(var quest in questMaster)
                    {
                        Debug.Log($"Id: {quest.Id}");
                        Debug.Log($"Title: {quest.Title}");
                        Debug.Log($"Gold: {quest.Gold}");
                    }
                    //Listに変換しておくことで、Linqが使えるようになる
                    var quest_2 = questMaster.Find(x => x.Id == 2);
                    Debug.Log($"Id == 2 is {quest_2.Title}");
                }
            },
            error => { Debug.Log(error.GenerateErrorReport()); }); 
    }

    /// <summary>
    /// クエスト情報のマスタデータ
    /// </summary>
    public class QuestMaster
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Gold { get; set; }
    }
    #endregion
}
