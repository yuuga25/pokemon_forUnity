using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class BelongingsScript : MonoBehaviour
{
    public Menu_PokéController pokéController;

    public GameObject parentObj;
    public GameObject item_Unit;

    public AudioClip Decision;

    public int changeItemNum;

    public void SetItems(int itemNum)
    {
        pokéController.loading_Image.SetActive(true);

        foreach (Transform u in parentObj.transform)
        {
            Destroy(u.gameObject);
        }

        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest()
        {
            CatalogVersion = "Items"
        },
        result =>
        {
            DataLists.catalogData = result.Catalog;

            var belongings = DataLists.playerData_Inventry.Where(x => x.ItemClass == "Belongings");
            foreach(var i in belongings)
            {
                GameObject unit = Instantiate(item_Unit, parentObj.transform);

                unit.transform.Find("Text_ItemName").GetComponent<Text>().text = i.DisplayName;
                unit.transform.Find("Text_ItemValue").gameObject.SetActive(false);
                unit.transform.Find("Text_ItemCount").gameObject.SetActive(false);

                var item = DataLists.catalogData.Find(x => x.ItemId == i.ItemId);
                unit.transform.Find("Text_Item").GetComponent<Text>().text = item.Description;

                unit.transform.Find("Button_LevelUpItem").GetComponent<Button>().onClick.AddListener(() => pokéController.audioManager_SE.PlayOneShot(Decision));
                string num = "";
                switch (itemNum)
                {
                    case 0:
                        num = DataLists.playerData.teamDatas.b_item1;
                        break;
                    case 1:
                        num = DataLists.playerData.teamDatas.b_item2;
                        break;
                    case 2:
                        num = DataLists.playerData.teamDatas.b_item3;
                        break;
                    case 3:
                        num = DataLists.playerData.teamDatas.b_item4;
                        break;
                    case 4:
                        num = DataLists.playerData.teamDatas.b_item5;
                        break;
                    case 5:
                        num = DataLists.playerData.teamDatas.b_item6;
                        break;
                }
                if (i.ItemId == num)
                {
                    unit.transform.Find("Button_LevelUpItem").gameObject.SetActive(false);
                }
                else
                {
                    unit.transform.Find("Button_LevelUpItem").gameObject.transform.Find("Text").GetComponent<Text>().text = "持たせる";
                    var tmp = item.ItemId;
                    unit.transform.Find("Button_LevelUpItem").GetComponent<Button>().onClick.AddListener(() => HaveAnItem(tmp));
                }

                var data = DataLists.catalogData.Find(x => x.ItemId == i.ItemId);
                var imageID = PlayFabSimpleJson.DeserializeObject<CustomData>(data.CustomData);
                var image = pokéController.imageData_Belongings.sheet.Find(x => x.imageId == imageID.icon);
                unit.transform.Find("Item_Icon").GetComponent<Image>().sprite = image.item_Image;

            }

            changeItemNum = itemNum;
            pokéController.loading_Image.SetActive(false);
        },
        error => { pokéController.error_Image.SetActive(true); });
    }

    public void HaveAnItem(string itemId)
    {
        string num = "";
        switch (changeItemNum)
        {
            case 0:
                num = DataLists.playerData.teamDatas.b_item1;
                break;
            case 1:
                num = DataLists.playerData.teamDatas.b_item2;
                break;
            case 2:
                num = DataLists.playerData.teamDatas.b_item3;
                break;
            case 3:
                num = DataLists.playerData.teamDatas.b_item4;
                break;
            case 4:
                num = DataLists.playerData.teamDatas.b_item5;
                break;
            case 5:
                num = DataLists.playerData.teamDatas.b_item6;
                break;
        }
        var d = DataLists.playerData.teamDatas;
        if (d.b_item1 == itemId)
        {
            (num, d.b_item1) = (d.b_item1, num);
        }
        else if(d.b_item2 == itemId)
        {
            (num, d.b_item2) = (d.b_item2, num);
        }
        else if(d.b_item3 == itemId)
        {
            (num, d.b_item3) = (d.b_item3, num);
        }
        else if(d.b_item4 == itemId)
        {
            (num, d.b_item4) = (d.b_item4, num);
        }
        else if(d.b_item5 == itemId)
        {
            (num, d.b_item5) = (d.b_item5, num);
        }
        else if(d.b_item6 == itemId)
        {
            (num, d.b_item6) = (d.b_item6, num);
        }
        else
        {
            num = DataLists.catalogData.Find(x => x.ItemId == itemId).ItemId;
        }
        switch (changeItemNum)
        {
            case 0:
                DataLists.playerData.teamDatas.b_item1 = num;
                break;
            case 1:
                DataLists.playerData.teamDatas.b_item2 = num;
                break;
            case 2:
                DataLists.playerData.teamDatas.b_item3 = num;
                break;
            case 3:
                DataLists.playerData.teamDatas.b_item4 = num;
                break;
            case 4:
                DataLists.playerData.teamDatas.b_item5 = num;
                break;
            case 5:
                DataLists.playerData.teamDatas.b_item6 = num;
                break;
        }

        pokéController.loading_Image.SetActive(true);
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>()
                {
                        {"PlayerData", PlayFabSimpleJson.SerializeObject(DataLists.playerData) },
                }
            },
            result =>
            {
                pokéController.Button_Organization();
                pokéController.SetOrganization();
                pokéController.loading_Image.SetActive(false);
            },
            error => { Debug.Log(error.GenerateErrorReport()); pokéController.error_Image.SetActive(true); });
    }

    public void NoBelongings()
    {
        string num = "";
        switch (changeItemNum)
        {
            case 0:
                num = DataLists.playerData.teamDatas.b_item1;
                break;
            case 1:
                num = DataLists.playerData.teamDatas.b_item2;
                break;
            case 2:
                num = DataLists.playerData.teamDatas.b_item3;
                break;
            case 3:
                num = DataLists.playerData.teamDatas.b_item4;
                break;
            case 4:
                num = DataLists.playerData.teamDatas.b_item5;
                break;
            case 5:
                num = DataLists.playerData.teamDatas.b_item6;
                break;
        }
        num = null;
        switch (changeItemNum)
        {
            case 0:
                DataLists.playerData.teamDatas.b_item1 = num;
                break;
            case 1:
                DataLists.playerData.teamDatas.b_item2 = num;
                break;
            case 2:
                DataLists.playerData.teamDatas.b_item3 = num;
                break;
            case 3:
                DataLists.playerData.teamDatas.b_item4 = num;
                break;
            case 4:
                DataLists.playerData.teamDatas.b_item5 = num;
                break;
            case 5:
                DataLists.playerData.teamDatas.b_item6 = num;
                break;
        }

        pokéController.loading_Image.SetActive(true);
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>()
                {
                        {"PlayerData", PlayFabSimpleJson.SerializeObject(DataLists.playerData) },
                }
            },
            result =>
            {
                pokéController.Button_Organization();
                pokéController.SetOrganization();
                pokéController.loading_Image.SetActive(false);
            },
            error => { Debug.Log(error.GenerateErrorReport()); pokéController.error_Image.SetActive(true); });
    }
}
