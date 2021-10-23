using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class GetCatalogData : MonoBehaviour
{
    /// <summary>
    /// カタログアイテムの一覧
    /// </summary>
    public List<CatalogItem> CatalogItems { get; private set; }

    /// <summary>
    /// カタログアイテムの一覧
    /// </summary>
    public List<StoreItem> StoreItems { get; private set; }

    /// <summary>
    /// 購入するアイテム
    /// </summary>
    StoreItem purchaseItem;

    /// <summary>
    /// カタログのバージョン
    /// </summary>
    const string CATALOG_VERSION = "Items";

    /// <summary>
    /// ストアID
    /// </summary>
    const string GOLD_STORE_ID = "candy_store";

    public void GetCatalogDatas()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest()
        {
            CatalogVersion = CATALOG_VERSION,
        }
        , result =>
        {
            Debug.Log("カタログデータ取得成功");
            CatalogItems = result.Catalog;
        }
        , error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void GetStoreData()
    {
        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest()
        {
            CatalogVersion = CATALOG_VERSION,
            StoreId = GOLD_STORE_ID
        }
        , result =>
        {
            Debug.Log("ストアデータ取得成功");
            StoreItems = result.Store;

            //購入するアイテムを保持
            purchaseItem = StoreItems.Find(x => x.ItemId == "Candy_Experience_XS");
            PurchaseItem();
        }
        , error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    /// <summary>
    /// ゴールドの通貨
    /// </summary>
    const string VC_GD = "GD";

    public void PurchaseItem()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest()
        {
            CatalogVersion = CATALOG_VERSION,
            StoreId = GOLD_STORE_ID,
            ItemId = purchaseItem.ItemId,
            VirtualCurrency = VC_GD,
            Price = (int)purchaseItem.VirtualCurrencyPrices[VC_GD]
        }
        , result =>
        {
            Debug.Log($"{result.Items[0].DisplayName}:購入成功");
        }
        , error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
