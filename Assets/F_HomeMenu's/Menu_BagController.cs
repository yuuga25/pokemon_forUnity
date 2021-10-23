using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using UnityEngine.UI;

public class Menu_BagController : MonoBehaviour
{
    public GameObject item_Unit;
    public GameObject parentObj;
    public Menu_PokéController pokéController;

    [Space(15)]
    public ImageData_Item imageData;
    public ImageData_Belongings belongings;

    public void GetItemList()
    {
        pokéController.loading_Image.SetActive(true);

        foreach (Transform u in parentObj.transform)
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
            GetUserInventory();
        }
        , error => { print(error.GenerateErrorReport()); pokéController.error_Image.SetActive(true); });
    }

    private void GetUserInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest()
        ,result =>
        {
            DataLists.playerData_Inventry = result.Inventory;
            SetItemList();
        }
        , error => { print(error.GenerateErrorReport()); pokéController.error_Image.SetActive(true); });
    }

    private void SetItemList()
    {
        foreach (var item in DataLists.playerData_Inventry)
        {
            GameObject unit = Instantiate(item_Unit, parentObj.transform);

            unit.transform.Find("Item_Text_Name").gameObject.GetComponent<Text>().text = item.DisplayName;
            unit.transform.Find("Item_Text_Quantity").gameObject.GetComponent<Text>().text = item.RemainingUses.ToString();

            var customData = DataLists.catalogData.Find(x => x.ItemId == item.ItemId);
            var imageID = PlayFabSimpleJson.DeserializeObject<CustomData>(customData.CustomData);

            print(imageID.icon);

            if (imageData.sheet.Find(x => x.imageId == imageID.icon) != null)
            {
                unit.transform.Find("Item_Image").gameObject.GetComponent<Image>().sprite = imageData.sheet.Find(x => x.imageId == imageID.icon).item_Image;
            }
            else if (belongings.sheet.Find(x => x.imageId == imageID.icon) != null)
            {
                unit.transform.Find("Item_Image").gameObject.GetComponent<Image>().sprite = pokéController.imageData_Belongings.sheet.Find(x => x.imageId == imageID.icon).item_Image;
                unit.transform.Find("Item_Text_Quantity").gameObject.GetComponent<Text>().text = "1";
            }

        }

        print("アイテム数:" + DataLists.playerData_Inventry.Count);

        pokéController.loading_Image.SetActive(false);
    }
}
