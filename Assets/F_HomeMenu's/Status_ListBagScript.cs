using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class Status_ListBagScript : MonoBehaviour
{
    public Menu_PokéController pokéController;

    public GameObject List_Bag;
    public GameObject parentObj;
    public GameObject PListItem_Unit;

    public ImageData_Item data_Item;

    public AudioClip Decision;

    public void close()
    {
        List_Bag.SetActive(false);
        pokéController.RealValueCalculation();
    }
    public void SetBagList()
    {
        pokéController.List_BasicStatus.SetActive(false);
        pokéController.List_BattleStatus.SetActive(false);
        pokéController.List_Technique.SetActive(false);
        pokéController.Image_AlwaysDisplayed.SetActive(false);

        pokéController.loading_Image.SetActive(true);

        foreach(Transform u in parentObj.transform)
        {
            Destroy(u.gameObject);
        }

        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest()
        {
            CatalogVersion = "Items"
        }
        , result =>
        {
            DataLists.catalogData = result.Catalog;

            var p_data = DataLists.playerData.pokémonsList.Find(x => x.unique_Id == pokéController.Change_Unique_ID);

            var useItems = DataLists.playerData_Inventry.Where(x => x.ItemClass == "useItem");
            foreach(var i in useItems)
            {
                GameObject unit = Instantiate(PListItem_Unit, parentObj.transform);

                unit.transform.Find("Text_ItemName").GetComponent<Text>().text = i.DisplayName;
                unit.transform.Find("Text_ItemValue").GetComponent<Text>().text = i.RemainingUses.ToString();

                var item = DataLists.catalogData.Find(x => x.ItemId == i.ItemId);
                unit.transform.Find("Text_Item").GetComponent<Text>().text = item.Description;

                if (p_data.isDifferentColors == true && i.ItemId == "Capsule_DifferentColors")
                {
                    unit.transform.Find("Button_LevelUpItem").gameObject.SetActive(false);
                }
                else
                {
                    unit.transform.Find("Button_LevelUpItem").GetComponent<Button>().onClick.AddListener(() => pokéController.audioManager_SE.PlayOneShot(Decision));
                    string itemId = i.ItemId;
                    string itemInstanceId = i.ItemInstanceId;
                    unit.transform.Find("Button_LevelUpItem").GetComponent<Button>().onClick.AddListener(() => UseItem(itemId, itemInstanceId));
                }

                var data = DataLists.catalogData.Find(x => x.ItemId == i.ItemId);
                var imageID = PlayFabSimpleJson.DeserializeObject<CustomData>(data.CustomData);
                var image = data_Item.sheet.Find(x => x.imageId == imageID.icon);
                unit.transform.Find("Item_Icon").GetComponent<Image>().sprite = image.item_Image;
            }

            List_Bag.SetActive(true);
            pokéController.loading_Image.SetActive(false);
        }
        , error => { pokéController.error_Image.SetActive(true);});
    }

    private void UpdateUserData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>()
            {
                {"PlayerData", PlayFabSimpleJson.SerializeObject(DataLists.playerData) },
            }
        },
        result =>
        {
            GetUserInventory();
        },
        error => { pokéController.error_Image.SetActive(true); });
    }
    private void GetUserInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        result =>
        {
            DataLists.playerData_Inventry = result.Inventory;

            pokéController.loading_Image.SetActive(false);
            List_Bag.SetActive(false);
            pokéController.RealValueCalculation();
            FlagManager.isDataChange = true;
        },
        error => { pokéController.error_Image.SetActive(true); });
    }

    public void UseItem(string itemId, string itemInstanceId)
    {
        pokéController.loading_Image.SetActive(true);

        var p_Data = DataLists.playerData.pokémonsList.Find(x => x.unique_Id == pokéController.Change_Unique_ID);

        if (itemId == "Mint_Lonely") p_Data.userP_Personality = 1;
        else if (itemId == "Mint_Stubborn") p_Data.userP_Personality = 2;
        else if (itemId == "Mint_Mischievous") p_Data.userP_Personality = 3;
        else if (itemId == "Mint_Brave") p_Data.userP_Personality = 4;
        else if (itemId == "Mint_Bold") p_Data.userP_Personality = 5;
        else if (itemId == "Mint_Naughty") p_Data.userP_Personality = 6;
        else if (itemId == "Mint_Carefree") p_Data.userP_Personality = 7;
        else if (itemId == "Mint_Swallowing") p_Data.userP_Personality = 8;
        else if (itemId == "Mint_Moderate") p_Data.userP_Personality = 9;
        else if (itemId == "Mint_Easygoing") p_Data.userP_Personality = 10;
        else if (itemId == "Mint_Inadvertently") p_Data.userP_Personality = 11;
        else if (itemId == "Mint_Composure") p_Data.userP_Personality = 12;
        else if (itemId == "Mint_Calm") p_Data.userP_Personality = 13;
        else if (itemId == "Mint_Meek") p_Data.userP_Personality = 14;
        else if (itemId == "Mint_Prudence") p_Data.userP_Personality = 15;
        else if (itemId == "Mint_Saucy") p_Data.userP_Personality = 16;
        else if (itemId == "Mint_Coward") p_Data.userP_Personality = 17;
        else if (itemId == "Mint_Impatient") p_Data.userP_Personality = 18;
        else if (itemId == "Mint_Cheerful") p_Data.userP_Personality = 19;
        else if (itemId == "Mint_Innocent") p_Data.userP_Personality = 20;

        else if(itemId == "Capsule_Characteristics")
        {
            var p_titleData = DataLists.titleData_Pokémon.Find(x => x.p_Id == p_Data.userP_Id);
            if (p_Data.isDreamCharacteristic == false)
            {
                p_Data.isDreamCharacteristic = true;
                p_Data.userP_Characteristic = p_titleData.p_Characteristic_Dream;
            }
            else if (p_Data.isDreamCharacteristic == true)
            { 
                p_Data.isDreamCharacteristic = false;
                p_Data.userP_Characteristic = p_titleData.p_Characteristic;
            }

        }
        else if(itemId == "Capsule_DifferentColors")
        {
            p_Data.isDifferentColors = true;
        }

        PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
        {
            ItemInstanceId = itemInstanceId,
            ConsumeCount = 1
        },
        result=> 
        {
            UpdateUserData();
        },
        error=> { pokéController.error_Image.SetActive(true); });

    }
}
