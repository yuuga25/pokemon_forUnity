using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class Menu_ShopController : MonoBehaviour
{
    public GameObject shopItem_Unit;
    public GameObject parentObj;
    public Menu_PokéController pokéController;

    public GameObject ScrollView;
    public GameObject errorMessage;
    public GameObject PurchaseConfirmationObj;

    public Text Text_GDValue;
    public Text Text_BPValue;

    [Space(15)]
    public ImageData_Item imageData;

    public AudioClip Decision;

    private List<StoreItem> storeItems = new List<StoreItem>();


    public Transform itemListbar;

    [HideInInspector]
    public float scrollValue_y;

    public void GetCatalogData()
    {
        pokéController.loading_Image.SetActive(true);

        errorMessage.SetActive(false);
        PurchaseConfirmationObj.SetActive(false);
        ScrollView.SetActive(true);

        foreach(Transform u in parentObj.transform)
        {
            Destroy(u.gameObject);
        }

        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest()
        {
            CatalogVersion = "Items"
        }, result =>
        {
            DataLists.catalogData = result.Catalog;
            GetVCData();
        }
        , error => { pokéController.error_Image.SetActive(true); });
    }

    private void GetVCData()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        result =>
        {
            DataLists.playerData_Inventry = result.Inventory;
            DataLists.player_Money = result.VirtualCurrency["GD"];
            DataLists.player_BattlePoint = result.VirtualCurrency["BP"];

            Text_GDValue.text = $"{DataLists.player_Money}円";
            Text_BPValue.text = $"{DataLists.player_BattlePoint}BP";

            GetGDShopData();
        }
        , error => { pokéController.error_Image.SetActive(true); });
    }

    private void GetGDShopData()
    {
        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest()
        {
            CatalogVersion = "Items",
            StoreId = "GD_store",
        }, result =>
        {
            storeItems = result.Store;
            SetStoreItem("GD", "GD_store");
        }
        , error => { pokéController.error_Image.SetActive(false); });
    }
    private void GetBPShopData()
    {
        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest()
        {
            CatalogVersion = "Items",
            StoreId = "BP_store",
        }, result =>
        {
            storeItems = result.Store;
            SetStoreItem("BP", "BP_store");
        }
        , error => { pokéController.error_Image.SetActive(false); });
    }

    private void SetStoreItem(string VCP, string storeId)
    {
        foreach (var item in storeItems)
        {
            GameObject unit = Instantiate(shopItem_Unit, parentObj.transform);

            var data = DataLists.catalogData.Find(x => x.ItemId == item.ItemId);
            unit.transform.Find("Text_ItemName").GetComponent<Text>().text = data.DisplayName;
            if(VCP == "GD")
            {
                unit.transform.Find("Text_ItemPrice").GetComponent<Text>().text = $"{item.VirtualCurrencyPrices[VCP]}円";
            }
            else unit.transform.Find("Text_ItemPrice").GetComponent<Text>().text = $"{item.VirtualCurrencyPrices[VCP]}BP";
            var imageID = PlayFabSimpleJson.DeserializeObject<CustomData>(data.CustomData);
            if (imageData.sheet.Find(x => x.imageId == imageID.icon) != null)
            {
                unit.transform.Find("Item_Icon").gameObject.GetComponent<Image>().sprite = imageData.sheet.Find(x => x.imageId == imageID.icon).item_Image;
            }
            else if (pokéController.imageData_Belongings.sheet.Find(x => x.imageId == imageID.icon) != null)
            {
                unit.transform.Find("Item_Icon").gameObject.GetComponent<Image>().sprite = pokéController.imageData_Belongings.sheet.Find(x => x.imageId == imageID.icon).item_Image;
            }

            var value = DataLists.playerData_Inventry.Find(x => x.ItemId == item.ItemId);
            int Num = 0;
            if (value != null)
            {
                if (data.ItemClass == "Belongings")
                {
                    Num = 0;
                }
                else Num = value.RemainingUses.Value;
            }
            unit.transform.Find("Text_ItemValue").GetComponent<Text>().text = $"{Num}";

            var button = unit.transform.Find("Button_LevelUpItem").GetComponent<Button>();
            button.onClick.AddListener(() => pokéController.audioManager_SE.PlayOneShot(Decision));

            button.onClick.AddListener(() => PurchaseConfirmation(storeId, item.ItemId, VCP, (int)item.VirtualCurrencyPrices[VCP]));

            if(data.ItemClass == "Belongings")
            {
                if (DataLists.playerData_Inventry.Find(x => x.ItemId == data.ItemId) != null)
                {
                    Destroy(unit);
                }
            }
        }

        if (VCP == "GD")
        {
            GetBPShopData();
        }
        else
        {
            pokéController.loading_Image.SetActive(true);
            StartCoroutine(s());
        }

    }

    IEnumerator s()
    {
        yield return new WaitForSeconds(0.5f);
        var pos = itemListbar.localPosition;
        pos.y = scrollValue_y;
        itemListbar.localPosition = pos;
        pokéController.loading_Image.SetActive(false);
    }

    public void PurchaseConfirmation(string StoreId, string ItemId, string VC, int price)
    {
        ScrollView.SetActive(false);
        var item = DataLists.catalogData.Find(x => x.ItemId == ItemId);
        PurchaseConfirmationObj.transform.Find("Text_ItemName").GetComponent<Text>().text = $"{item.DisplayName}";
        PurchaseConfirmationObj.transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
        PurchaseConfirmationObj.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => BuyStoreItem(StoreId, ItemId, VC, price));
        if (VC == "GD")
        {
            if(DataLists.player_Money >= price)
            {
                PurchaseConfirmationObj.transform.Find("Text_Money").GetComponent<Text>().text = $"おこずかい：{DataLists.player_Money}円\n↓\nおこずかい：{DataLists.player_Money - price}円";
                PurchaseConfirmationObj.SetActive(true);
            }
            else
            {
                errorMessage.SetActive(true);
            }
        }
        else if(VC == "BP")
        {
            if(DataLists.player_BattlePoint >= price)
            {
                PurchaseConfirmationObj.transform.Find("Text_Money").GetComponent<Text>().text = $"バトルポイント：{DataLists.player_BattlePoint}BP\n↓\nバトルポイント：{DataLists.player_BattlePoint - price}BP";
                PurchaseConfirmationObj.SetActive(true);
            }
            else
            {
                errorMessage.SetActive(true);
            }
        }
    }

    public void BuyStoreItem(string storeID, string ItemId, string VC, int price)
    {
        pokéController.loading_Image.SetActive(true);

        print($"{storeID}：{ItemId}：{VC}：{price}");

        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest()
        {
            CatalogVersion = "Items",
            StoreId = storeID,
            ItemId = ItemId,
            VirtualCurrency = VC,
            Price = price
        },result =>
        {
            if(VC == "GD")
            {
                DataLists.player_Money -= price;
            }
            else if(VC == "BP")
            {
                DataLists.player_BattlePoint -= price;
            }
            GetCatalogData();
        },
        error => { Debug.Log(error.GenerateErrorReport()); pokéController.error_Image.SetActive(true); });
    }
}
