using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using UnityEngine.SceneManagement;
using PlayFab.ClientModels;

public class ResultScript : MonoBehaviour
{
    public GameObject loading_Image;

    private const string VC_GD = "GD";
    private const string VC_BP = "BP";

    private void Start()
    {
        loading_Image.SetActive(false);
    }

    public void AddMoney(int GD, int BP)
    {
        loading_Image.SetActive(true);

        PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = VC_GD,
            Amount = GD
        }, result =>
         {
             AddBP(BP);
         },
        error => { Debug.Log(error.GenerateErrorReport()); });
    }

    public void AddBP(int BP)
    {
        PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = VC_BP,
            Amount = BP
        }, result =>
        {
            GetVCData();
        },
        error => { Debug.Log(error.GenerateErrorReport()); });
    }
    private void GetVCData()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        result =>
        {
            DataLists.playerData_Inventry = result.Inventory;
            DataLists.player_Money = result.VirtualCurrency["GD"];
            DataLists.player_BattlePoint = result.VirtualCurrency["BP"];
            loading_Image.SetActive(false);

        }
        , error => { Debug.Log(error.GenerateErrorReport()); });
    }

    public void LoadHome()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
