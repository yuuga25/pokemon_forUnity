using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;

public class Controller_AC : MonoBehaviour
{
    public GameObject error_Image;

    [SerializeField] private GameObject inputObject;
    [SerializeField] private GameObject confirmationObject;
    [SerializeField] private GameObject errorObject;

    [SerializeField] private GameObject nextExplanationObject;

    [SerializeField] private InputField inputField_NameSpace;

    [SerializeField] private GameObject text_error;
    [SerializeField] private Text text_Name;
    
    public void Decision_Input()
    {
        string name = inputField_NameSpace.text;
        bool isMatch = false;
        foreach(var i in errorName)
        {
            if (Regex.IsMatch(name, i, RegexOptions.IgnoreCase))
            {
                isMatch = true;
            }
        }
        if(isMatch)
        {
            errorObject.SetActive(true);
            inputObject.SetActive(false);
        }
        else
        {
            if(name.Length > 2)
            {
                confirmationObject.SetActive(true);
                inputObject.SetActive(false);
                text_Name.text = name;
            }
            else
            {
                StartCoroutine(falseMessage());
                text_error.SetActive(true);
            }
        }
    }

    public void Decision_Confirmation()
    {
        DataLists.playerData.user_Name = text_Name.text;
        DataLists.playerData.user_Rank = 1;
        print(text_Name.text);
        print(DataLists.playerData.user_Name);

        //プレイヤー表示名の更新
        PlayFabClientAPI.UpdateUserTitleDisplayName(new PlayFab.ClientModels.UpdateUserTitleDisplayNameRequest
        {
            DisplayName = DataLists.playerData.user_Name
        }, result =>
        {
            Debug.Log("プレイヤー名：" + result.DisplayName);
        }, error => { Debug.LogError(error.GenerateErrorReport()); error_Image.SetActive(true); });

        CloseObject();
        inputObject.SetActive(false);
        nextExplanationObject.SetActive(true);
    }

    public void CloseObject()
    {
        confirmationObject.SetActive(false);
        errorObject.SetActive(false);
        inputObject.SetActive(true);
    }

    private IEnumerator falseMessage()
    {
        yield return new WaitForSeconds(1.5f);
        text_error.SetActive(false);
    }

    public static List<string> errorName = new List<string>
    {
    #region//NGワード
        "アナル", "あなる", "アヘン", "あへん", "インポ", "いんぽ", "いんも", "陰毛", "いんもう",
      "淫乱", "いんらん", "インラン", "うんこ", "ウンコ", "うんち", "ウンチ", "エロ", "えろ",
      "多い日も安心", "おしっこ", "オシッコ", "おしり", "お尻", "尻", "おっぱい", "オッパイ", "おめこ",
      "がんしゃ", "顔射", "きくもん", "キャバ嬢", "きゃばじょう", "キャバジョウ", "巨乳", "きょにゅう", "キョニュウ",
      "クンニ", "くんに", "支那", "処女", "しょじょ", "射精", "しゃせい", "すけべ", "スケベ", "すまた", "スカトロ", "すかとろ",
      "セクハラ", "せくはら", "セックス", "せっくす", "sex", "絶倫", "前立腺", "チェリーボーイ", "ちんこ", "チンコ", "おせちんこ",
      "つんぽ", "ていんた", "てまん", "手マン", "童貞", "どうてい", "トップレス", "とっぷれす", "なかだし", "中だし",
      "にがー", "呪い", "ぱいおつ", "π乙", "発情期", "はつじょうき", "びっこ", "貧乳", "ひんにゅう", "ヒンニュウ", "ファック",
      "ふぁっく", "fuck", "風俗", "ふうぞく", "ふぇら", "フェラ", "部落", "変質者", "ほも", "ホモ", "まりふぁな", "マリファナ",
      "まんこ", "マンコ", "まんちょ", "めくら", "もっこり", "モッコリ", "よここてい", "レイプ", "ロリコン", "assww", "Adolf Hitler",
      "DIO", "FAI", "fuk", "FA!", "jap", "JOINT", "PEE", "SHEMALE", "shit", "T.T", "XX XY", "072", "206", "310", "455", "114", "514", "1919",
      "愛液", "愛奴", "青姦","あきめくら","明盲","朝勃ち","足コキ","穴兄弟","アナニー","ああ、なるほど","ああなるから","阿片","アホ","アホども",
      "粗チン","いけぬま","池沼","イラマチオ","淫","陰核","陰茎","陰唇","陰嚢","陰部","ヴァギナ","穢多","エネマグラ","エフェドリン","援助交際",
      "援交","オーガズム","黄金水","オナニー","オナヌー","オナネタ","オナホ","オナホール","オナル","おお、なるほど","オナル",
      "オピオイドペプチド","おまんちょ","おめこ","オルガズム","がいじ","害児","私がいじめられて","顔騎","顔射","覚せい剤","カスども","我慢汁",
      "姦淫","監禁","姦通","亀甲縛り","きちがい","キチガイ","ｷﾁｶﾞｲ","気違い","基地外","逆援","近親相姦","キンタマ","キンタマーニ","クズども",
      "屎","クソども","クリトリス","くんに","クンニリングス","クンニ","けちゃまん","毛唐","抗うつ剤","強姦","後背位","口内射精","口内発射","合法ハーブ",
      "コカイン","乞食","ゴミカス","ゴミクズ","ゴミども","粗大ゴミ","ゴミ","そだいゴミ","生ゴミ","なまゴミ","ゴ有","殺す","ころす","コロス","56す","５６す","殺す気か",
      "コロナウイルス","コンドーム","ザー汁","ザーメン","ザコ","ざこ","ザッコ","サセ子","三国人","自慰","Gスポット","死姦","視姦","シコシコ","じさつ",
      "自殺","しっこ","しつこ","死ね","しね","シネ","4ね","４ね","タヒね","ﾀﾋね","獣姦","娼婦","しょうべん","小便","女体盛り","女体","素股","性愛",
      "性交","性奴隷","性行為","精液","セフレ","鮮人","全身舐め","ぜつりん","せんずり","賤民","大麻","ダウンしょう","ダッチワイフ","玉責め","玉舐め",
      "だいべん","大便","だっぷん","脱糞","脱法ハーブ","痴漢","痴女","痴汁","地下鉄サリン","ちかてつサリン","ちしょう","ちしょー","チャンコロ","直アド",
      "直メ","チンカス","ちんちん","チンチン","ちんぼ","ちんぽ","手淫","手コキ","手マン","ディープスロート","低能","ディルド","デカチン","デカマラ","デブ専","電動コケシ",
      "屠殺","土人","床上手","南極2号","肉壺","肉奴隷","乳頭","乳輪","尿道球腺液","売春","買春","売女","パイズリ","梅毒","パイパン","背面座位","バカ","白痴","バター犬",
      "花びら回転","ハメ撮り","一人エッチ","ファッキュー","プッシー","部落","へたくそ","ペッティング","ペニス","ヘロイン","包茎","ぼっき","勃起","ホ別","ポルチオ",
      "マジキチ","マットプレイ","まな板本番","麻薬","マンカス","マン毛","まんげ","マン臭","マン汁","まんすじ","ミコスリ半","メアド","めくら","メクラ","めこすじ",
      "メスカリン","メンヘラ","野獣先輩","やめろ","ヤリマン","よわい","ラブジュース","乱交","リスカ","リストカット","凌辱","陵辱","輪姦","淋病","ルンペン","レイプ",
      "レキソタン","レンドルミン","ロリコン","ロンパリ","和姦","わるもし"
    #endregion
    };
}
