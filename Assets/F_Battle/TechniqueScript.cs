using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class TechniqueScript : MonoBehaviour
{
    public BattleController battleController;

    public Dictionary<string, Action> actions = new Dictionary<string, Action>();

    public TurnScript turnScript;

    void Start()
    {
        actions.Add("10まんボルト", Thunderbolt);
        actions.Add("アイアンテール", IronTail);
        actions.Add("アイアンヘッド", IronHead);
        actions.Add("アクアジェット", AquaJet);
        actions.Add("あくび", Yawn);
        actions.Add("あまえる", Charm);
        actions.Add("インファイト", CloseCombat);
        actions.Add("エアスラッシュ", AirSlash);
        actions.Add("エナジーボール", EnergyBall);
        actions.Add("エレキネット", Electroweb);
        actions.Add("おにび", Will_O_Wisp);
        actions.Add("かえんほうしゃ",Flamethrower);
        actions.Add("かみくだく",Crunch);
        actions.Add("かみつく",Bite);
        actions.Add("かみなり",Thunder);
        actions.Add("かみなりパンチ", ThunderPunch);
        actions.Add("からをやぶる",ShellSmash);
        actions.Add("きあいだま", FocusBlast);
        actions.Add("ギガドレイン",GigaDrain);
        actions.Add("グロウパンチ",Power_UpPunch);
        actions.Add("クロスサンダー",FusionBolt);
        actions.Add("こうそくいどう",Agility);
        actions.Add("こごえるかぜ",IcyWind);
        actions.Add("さいみんじゅつ",Hypnosis);
        actions.Add("じしん",Earthquake);
        actions.Add("しねんのずつき",ZenHeadbutt);
        actions.Add("シャドーボール",ShadowBall);
        actions.Add("じゃれつく",PlayRough);
        actions.Add("しんそく",ExtremeSpeed);
        actions.Add("すてみタックル",Double_Edge);
        actions.Add("ステルスロック",StealthRock);
        actions.Add("ストーンエッジ",StoneEdge);
        actions.Add("スピードスター",Swift);
        actions.Add("だいちのちから",EarthPower);
        actions.Add("だいもんじ",FireBlast);
        actions.Add("つるぎのまい",SwordsDance);
        actions.Add("てっぺき",IronDefense);
        actions.Add("でんこうせっか",QuickAttack);
        actions.Add("でんじは",ThunderWave);
        actions.Add("どくづき",PoisonJab);
        actions.Add("どくどく",Toxic);
        actions.Add("ドラゴンクロー",DragonClaw);
        actions.Add("ドラゴンダイブ",DragonRush);
        actions.Add("トリックルーム",TrickRoom);
        actions.Add("ドレインパンチ",DrainPunch);
        actions.Add("なみのり",Surf);
        actions.Add("のしかかり",BodySlam);
        actions.Add("ハイドロポンプ",HydroPump);
        actions.Add("ハイパーボイス",HyperVoice);
        actions.Add("はどうだん",AuraSphere);
        actions.Add("パワーウィップ",PowerWhip);
        actions.Add("ビルドアップ",BulkUp);
        actions.Add("ふるいたてる",WorkUp);
        actions.Add("ヘドロばくだん",SludgeBomb);
        actions.Add("ほっぺすりすり",Nuzzle);
        actions.Add("ほのおのキバ",FireFang);
        actions.Add("マジカルシャイン",DazzlingGleam);
        actions.Add("マジカルフレイム",MysticalFire);
        actions.Add("マジカルリーフ",MagicalLeaf);
        actions.Add("まもる",Protect);
        actions.Add("ムーンフォース",Moonblast);
        actions.Add("めいそう",CalmMind);
        actions.Add("メタルクロー",MetalClaw);
        actions.Add("らいげき",BoltStrike);
        actions.Add("ラスターカノン",FlashCannon);
        actions.Add("りゅうせいぐん",DracoMeteor);
        actions.Add("りゅうのはどう",DragonPulse);
        actions.Add("りゅうのまい",DragonDance);
        actions.Add("れいとうビーム",IceBeam);
        actions.Add("わるだくみ",NastyPlot);

        print("技の数"+actions.Count);
    }

    public int DamageCalculation()
    {
        float damage = 0;
        var p1_data = turnScript.p1_Storage.userData_Pokémon;
        var p2_data = turnScript.p2_Storage.userData_Pokémon;

        var tech = turnScript.p1_Storage.technique_Pokémon;

        //攻撃側のレベル × 2 ÷ 5 ＋ 2 → 切り捨て
        damage = Mathf.Floor(p1_data.userP_Level * 2 / 5 + 2);

        float attack = 0;
        float defence = 0;
        if (turnScript.p1_Storage.technique_Pokémon.t_Classification == "Physics")
        {
            attack = p1_data.userP_Real_A;
            if(turnScript.p1_Storage.battleStatus.AscendingRank_Atk < 0 && turnScript.KeyPoint == 3)
            {
                attack = Mathf.Floor(attack * RankValue(0));
            }
            else attack = Mathf.Floor(attack * RankValue(turnScript.p1_Storage.battleStatus.AscendingRank_Atk));
            defence = p2_data.userP_Real_B;
            if(turnScript.p2_Storage.battleStatus.AscendingRank_Def > 0 && turnScript.KeyPoint == 3)
            {
                defence = Mathf.Floor(defence * RankValue(0));
            }
            else defence = Mathf.Floor(defence * RankValue(turnScript.p2_Storage.battleStatus.AscendingRank_Def));
        }
        else
        {
            attack = p1_data.userP_Real_C;
            if(turnScript.p1_Storage.battleStatus.AscendingRank_Sat < 0 && turnScript.KeyPoint == 3)
            {
                attack = Mathf.Floor(attack * RankValue(0));
            }
            else attack = Mathf.Floor(attack * RankValue(turnScript.p1_Storage.battleStatus.AscendingRank_Sat));
            defence = p2_data.userP_Real_D;
            if(turnScript.p2_Storage.battleStatus.AscendingRank_Sde > 0 && turnScript.KeyPoint == 3)
            {
                defence = Mathf.Floor(defence * RankValue(0));
            }
            else defence = Mathf.Floor(defence * RankValue(turnScript.p2_Storage.battleStatus.AscendingRank_Sde));
        }
        #region 最終攻撃
        if (turnScript.p1_Storage.othersStatus.hp < p1_data.userP_Real_Hp / 3)
        {
            if (tech.t_Type == "くさ" && p1_data.userP_Characteristic == "しんりょく")
            {
                attack = Mathf.Round(attack * 1.5f);
            }
            if (tech.t_Type == "ほのお" && p1_data.userP_Characteristic == "もうか")
            {
                attack = Mathf.Round(attack * 1.5f);
            }
            if (tech.t_Type == "みず" && p1_data.userP_Characteristic == "げきりゅう")
            {
                attack = Mathf.Round(attack * 1.5f);
            }
        }
        if (turnScript.p1_Storage.othersStatus.b_item == "Choice_Band" && turnScript.p1_Storage.technique_Pokémon.t_Classification == "Physics")
        {
            attack = Mathf.Round(attack * 1.5f);
        }
        if (turnScript.p1_Storage.othersStatus.b_item == "Choice_Specs" && turnScript.p1_Storage.technique_Pokémon.t_Classification == "Special")
        {
            attack = Mathf.Round(attack * 1.5f);
        }
        #endregion

        #region 最終防御
        var isEvo = DataLists.titleData_Pokémon.Find(x => x.p_Id == p2_data.userP_Id).p_EvolutionDN;
        if(isEvo != 0 && turnScript.p1_Storage.othersStatus.b_item == "Eviolite")
        {
            defence = Mathf.Round(defence * 1.5f);
        }
        if(turnScript.p1_Storage.othersStatus.b_item == "Assault_Vest")
        {
            defence = Mathf.Round(defence * 1.5f);
        }
        #endregion

        if (attack <= 0) attack = 1;
        if (defence <= 0) defence = 1;

        damage = Mathf.Floor(damage * tech.t_Power * attack / defence);
        damage = Mathf.Floor(damage / 50 + 2);

        //急所
        if (turnScript.KeyPoint == 3)
        {
            turnScript.message_Tech.Add($"急所に　あたった！");
            damage = Mathf.Floor(damage * 1.5f);
        }

        //乱数
        damage = Mathf.Floor(damage * turnScript.randomNumber);

        //タイプ一致
        var pokeData = DataLists.titleData_Pokémon.Find(x => x.p_Id == p1_data.userP_Id);
        if(pokeData.p_Type1 == tech.t_Type || pokeData.p_Type2 == tech.t_Type)
        {
            if (turnScript.p1_Storage.userData_Pokémon.userP_Characteristic == "てきおうりょく")
            {
                damage = Mathf.Floor(damage * 2);
            }
            else
            {
                damage = Mathf.Floor(damage * 1.5f);
            }
        }

        //タイプ相性
        var c_Data = battleController.c_Data.sheet.Find(x => x.typeName == tech.t_Type);
        int magnification = 100;
        if (c_Data.twice.Contains(p2_data.userP_Type1)) magnification *= 2;
        else if (c_Data.half.Contains(p2_data.userP_Type1)) magnification = magnification / 2;
        if (c_Data.twice.Contains(p2_data.userP_Type2)) magnification *= 2;
        else if (c_Data.half.Contains(p2_data.userP_Type2)) magnification = magnification / 2;

        damage = Mathf.Floor(damage * (magnification / 100));

        //やけど
        if(turnScript.p1_Storage.othersStatus.condition == BattleEnum.condition.burn)
        {
            damage = Mathf.Floor(damage / 2);
        }

        //ダメージ補正値
        if(turnScript.p1_Storage.othersStatus.b_item == "Life_Orb")
        {
            damage = Mathf.Floor(damage * 1.3f);
        }
        if(pokeData.p_Type1 == tech.t_Type || pokeData.p_Type2 == tech.t_Type)
        {
            if(turnScript.p1_Storage.othersStatus.b_item == "Expert_Belt")
            {
                damage = Mathf.Floor(damage * 1.2f);
            }
        }

        if(damage <= 0) { damage = 1; }

        damage = Mathf.FloorToInt(damage);
        print(damage);
        return (int)damage;
    }

    private float RankValue(int ascendingRank)
    {
        float value = 1;

        switch (ascendingRank)
        {
            case -6:
                value = (25 / 100);
                break;
            case -5:
                value = (28 / 100);
                break;
            case -4:
                value = (33 / 100);
                break;
            case -3:
                value = (40 / 100);
                break;
            case -2:
                value = (50 / 100);
                break;
            case -1:
                value = (66 / 100);
                break;
            case 0:
                value = (100 / 100);
                break;
            case 1:
                value = (150 / 100);
                break;
            case 2:
                value = (200 / 100);
                break;
            case 3:
                value = (250 / 100);
                break;
            case 4:
                value = (300 / 100);
                break;
            case 5:
                value =  (350 / 100);
                break;
            case 6:
                value = (400 / 100);
                break;
        }

        return value;
    }

    #region 10万ボルト
    public void Thunderbolt()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if(r < 10 && turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Electric && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Ground && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Electric && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Ground)
        {
            turnScript.p2_Storage.othersStatus.condition = BattleEnum.condition.paralysis;
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}は\n体が　しびれてしまった！");
        }
    }
    #endregion

    #region アイアンテール
    public void IronTail()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 30)
        {
            if(turnScript.p2_Storage.battleStatus.AscendingRank_Def <= -6)
            {
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n防御は　これ以上　下がらない！");
            }
            else
            {
                turnScript.p2_Storage.battleStatus.AscendingRank_Def--;
                Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Def, -6, 6);
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n防御が　下がった！");
            }
        }
    }
    #endregion

    #region アイアンヘッド
    public void IronHead()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 30)
        {
            turnScript.p2_Storage.battleStatus.frightened = true;
        }
    }
    #endregion

    #region アクアジェット
    public void AquaJet()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
    }
    #endregion

    #region あくび
    public void Yawn()
    {
        if (turnScript.p2_Storage.battleStatus.sleepinessTurn == 0 && turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none)
        {
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n眠気を　誘った！");
            turnScript.p2_Storage.battleStatus.sleepinessTurn = 2;
        }
        else
        {
            turnScript.message_Tech.Add("しかし　うまくきまらなかった");
        }
        turnScript.isDone = true;
    }
    #endregion

    #region あまえる
    public void Charm()
    {
        if (turnScript.p2_Storage.battleStatus.AscendingRank_Atk > -6)
        {
            turnScript.p2_Storage.battleStatus.AscendingRank_Atk -= 2;
            Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Atk, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n攻撃が　がくっと　下がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n攻撃は　これ以上　下がらない！");
        }
        turnScript.isDone = true;
    }
    #endregion

    #region インファイト
    public void CloseCombat()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        if (turnScript.p2_Storage.battleStatus.AscendingRank_Def > -6)
        {
            turnScript.p2_Storage.battleStatus.AscendingRank_Def--;
            Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Def, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n防御が　下がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n防御は　これ以上　下がらない！");
        }

        if (turnScript.p2_Storage.battleStatus.AscendingRank_Sde > -6)
        {
            turnScript.p2_Storage.battleStatus.AscendingRank_Sde--;
            Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Sde, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特防が　下がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特防は　これ以上　下がらない！");
        }

    }
    #endregion

    #region エアスラッシュ
    public void AirSlash()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 30)
        {
            turnScript.p2_Storage.battleStatus.frightened = true;
        }

    }
    #endregion

    #region エナジーボール
    public void EnergyBall()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 10)
        {
            if (turnScript.p2_Storage.battleStatus.AscendingRank_Sde > -6)
            {
                turnScript.p2_Storage.battleStatus.AscendingRank_Sde--;
                Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Sde, -6, 6);
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特防が　下がった！");
            }
            else
            {
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特防は　これ以上　下がらない！");
            }
        }
    }
    #endregion

    #region エレキネット
    public void Electroweb()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 100)
        {
            if (turnScript.p2_Storage.battleStatus.AscendingRank_Spe > -6)
            {
                turnScript.p2_Storage.battleStatus.AscendingRank_Spe--;
                Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Spe, -6, 6);
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n素早さが　下がった！");
            }
            else
            {
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n素早さは　これ以上　下がらない！");
            }
        }
    }
    #endregion

    #region おにび
    public void Will_O_Wisp()
    {
        if (turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Fire && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Fire)
        {
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}は\nやけどを　おった！");
            turnScript.p2_Storage.othersStatus.condition = BattleEnum.condition.burn;
        }
        else
        {
            turnScript.message_Tech[0] = $"しかし　うまくきまらなかった";
        }
        turnScript.isDone = true;
    }
    #endregion

    #region かえんほうしゃ
    public void Flamethrower()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 10)
        {
            if (turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Fire && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Fire)
            {
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}は\nやけどを　おった！");
                turnScript.p2_Storage.othersStatus.condition = BattleEnum.condition.burn;
            }
        }
    }
    #endregion

    #region かみくだく
    public void Crunch()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 20)
        {
            if (turnScript.p2_Storage.battleStatus.AscendingRank_Def > -6)
            {
                turnScript.p2_Storage.battleStatus.AscendingRank_Def--;
                Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Def, -6, 6);
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n防御が　下がった！");
            }
            else
            {
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n防御は　これ以上　下がらない！");
            }
        }
    }
    #endregion

    #region かみつく
    public void Bite()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 30)
        {
            turnScript.p2_Storage.battleStatus.frightened = true;
        }
    }
    #endregion

    #region かみなり
    public void Thunder()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 30 && turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Electric && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Ground && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Electric && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Ground)
        {
            turnScript.p2_Storage.othersStatus.condition = BattleEnum.condition.paralysis;
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}は\n体が　しびれてしまった！");
        }
    }
    #endregion

    #region かみなりパンチ
    public void ThunderPunch()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 10 && turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Electric && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Ground && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Electric && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Ground)
        {
            turnScript.p2_Storage.othersStatus.condition = BattleEnum.condition.paralysis;
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}は\n体が　しびれてしまった！");
        }
    }
    #endregion

    #region からをやぶる
    public void ShellSmash()
    {
        if (turnScript.p1_Storage.battleStatus.AscendingRank_Def > -6)
        {
            turnScript.p1_Storage.battleStatus.AscendingRank_Def--;
            Mathf.Clamp(turnScript.p1_Storage.battleStatus.AscendingRank_Def, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n防御が　下がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n特防は　これ以上　下がらない！");
        }
        if (turnScript.p1_Storage.battleStatus.AscendingRank_Sde > -6)
        {
            turnScript.p1_Storage.battleStatus.AscendingRank_Sde--;
            Mathf.Clamp(turnScript.p1_Storage.battleStatus.AscendingRank_Sde, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n特防が　下がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n特防は　これ以上　下がらない！");
        }
        if (turnScript.p1_Storage.battleStatus.AscendingRank_Atk < 6)
        {
            turnScript.p1_Storage.battleStatus.AscendingRank_Atk += 2;
            Mathf.Clamp(turnScript.p1_Storage.battleStatus.AscendingRank_Atk, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n攻撃が　ぐーんと　上がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n攻撃は　これ以上　上がらない！");
        }
        if (turnScript.p1_Storage.battleStatus.AscendingRank_Sat < 6)
        {
            turnScript.p1_Storage.battleStatus.AscendingRank_Sat += 2;
            Mathf.Clamp(turnScript.p1_Storage.battleStatus.AscendingRank_Sat, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n特攻が　ぐーんと　上がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n特攻は　これ以上　上がらない！");
        }
        turnScript.isDone = true;
    }
    #endregion

    #region きあいだま
    public void FocusBlast()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 10)
        {
            if (turnScript.p2_Storage.battleStatus.AscendingRank_Sde > -6)
            {
                turnScript.p2_Storage.battleStatus.AscendingRank_Sde--;
                Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Sde, -6, 6);
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特防が　下がった！");
            }
            else
            {
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特防は　これ以上　下がらない！");
            }
        }
    }
    #endregion

    #region ギガドレイン
    public void GigaDrain()
    {
        var damage = DamageCalculation();
        turnScript.Damage(damage,turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        float heal = damage / 2;
        turnScript.p1_Storage.othersStatus.hp += (int)Math.Round(heal, 0, MidpointRounding.AwayFromZero);
        turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}から\nHPを　吸い取った！");

        if (turnScript.p1_Storage.othersStatus.hp >= turnScript.p1_Storage.userData_Pokémon.userP_Real_Hp)
        {
            turnScript.p1_Storage.othersStatus.hp = turnScript.p1_Storage.userData_Pokémon.userP_Real_Hp;
        }

        if (turnScript.p1_Storage.userData_Pokémon == BattleDatas.user_PokemonData[battleController.player_PokeTeamNum])
        {
            battleController.player_HpSlider.value = turnScript.p1_Storage.othersStatus.hp;
            battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
            if (battleController.player_HpSlider.value <= battleController.player_HpSlider.maxValue / 5)
            {
                battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
            }
            else if (battleController.player_HpSlider.value <= battleController.player_HpSlider.maxValue / 2)
            {
                battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
            }
            else
            {
                battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
            }
        }
        else if(turnScript.p1_Storage.userData_Pokémon == BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum])
        {
            battleController.enemy_HpSlider.value = turnScript.p1_Storage.othersStatus.hp;
            if (battleController.enemy_HpSlider.value <= battleController.enemy_HpSlider.maxValue / 5)
            {
                battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
            }
            else if (battleController.enemy_HpSlider.value <= battleController.enemy_HpSlider.maxValue / 2)
            {
                battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
            }
            else
            {
                battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
            }
        }
    }
    #endregion

    #region グロウパンチ
    public void Power_UpPunch()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 100)
        {
            if (turnScript.p1_Storage.battleStatus.AscendingRank_Atk < 6)
            {
                turnScript.p1_Storage.battleStatus.AscendingRank_Atk++;
                Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Atk, -6, 6);
                turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n攻撃が　上がった！");
            }
            else
            {
                turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n攻撃は　これ以上　上がらない！");
            }
        }
    }
    #endregion

    #region クロスサンダー
    public void FusionBolt()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
    }
    #endregion

    #region こうそくいどう
    public void Agility()
    {
        if (turnScript.p1_Storage.battleStatus.AscendingRank_Spe < 6)
        {
            turnScript.p1_Storage.battleStatus.AscendingRank_Spe += 2;
            Mathf.Clamp(turnScript.p1_Storage.battleStatus.AscendingRank_Spe, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n素早さが　ぐーんと　上がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n素早さは　これ以上　上がらない！");
        }
        turnScript.isDone = true;
    }
    #endregion

    #region こごえるかぜ
    public void IcyWind()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 100)
        {
            if (turnScript.p2_Storage.battleStatus.AscendingRank_Spe > -6)
            {
                turnScript.p2_Storage.battleStatus.AscendingRank_Spe--;
                Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Spe, -6, 6);
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n素早さが　下がった！");
            }
            else
            {
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n素早さは　これ以上　下がらない！");
            }
        }
    }
    #endregion

    #region さいみんじゅつ
    public void Hypnosis()
    {
        if (turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none)
        {
            turnScript.p2_Storage.othersStatus.condition = BattleEnum.condition.sleep;
            turnScript.p2_Storage.othersStatus.sleepTurn = UnityEngine.Random.Range(2, 5);
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}は　眠って　しまった！");
        }
        else
        {
            turnScript.message_Tech.Add("しかし　うまくきまらなかった");
        }
        turnScript.isDone = true;
    }
    #endregion

    #region じしん
    public void Earthquake()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
    }
    #endregion

    #region しねんのずつき
    public void ZenHeadbutt()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 20)
        {
            turnScript.p2_Storage.battleStatus.frightened = true;
        }
    }
    #endregion

    #region シャドーボール
    public void ShadowBall()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 20)
        {
            if (turnScript.p2_Storage.battleStatus.AscendingRank_Sde > -6)
            {
                turnScript.p2_Storage.battleStatus.AscendingRank_Sde--;
                Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Sde, -6, 6);
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特防が　下がった！");
            }
            else
            {
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特防は　これ以上　下がらない！");
            }
        }
    }
    #endregion

    #region じゃれつく
    public void PlayRough()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 20)
        {
            if (turnScript.p2_Storage.battleStatus.AscendingRank_Atk > -6)
            {
                turnScript.p2_Storage.battleStatus.AscendingRank_Atk--;
                Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Atk, -6, 6);
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n攻撃が　下がった！");
            }
            else
            {
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n攻撃は　これ以上　下がらない！");
            }
        }
    }
    #endregion

    #region しんそく
    public void ExtremeSpeed()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
    }
    #endregion

    #region すてみタックル
    public void Double_Edge()
    {
        var damage = DamageCalculation();

        if(damage >= turnScript.p2_Storage.othersStatus.hp)
        {
            damage = turnScript.p2_Storage.othersStatus.hp;
        }

        turnScript.Damage(damage, turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        float p1_damage = damage * 0.33f;
        turnScript.p1_Storage.othersStatus.hp -= (int)Math.Round(p1_damage, 0, MidpointRounding.AwayFromZero);
        turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}も\n反動で　ダメージを　受けた！");

        if (turnScript.p1_Storage.othersStatus.hp <= 0) turnScript.p1_Storage.othersStatus.hp = 0;

        if (turnScript.p1_Storage.userData_Pokémon == BattleDatas.user_PokemonData[battleController.player_PokeTeamNum])
        {
            battleController.player_HpSlider.value = turnScript.p1_Storage.othersStatus.hp;
            battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
            if (battleController.player_HpSlider.value <= battleController.player_HpSlider.maxValue / 5)
            {
                battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
            }
            else if (battleController.player_HpSlider.value <= battleController.player_HpSlider.maxValue / 2)
            {
                battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
            }
            else
            {
                battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
            }
        }
        else if (turnScript.p1_Storage.userData_Pokémon == BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum])
        {
            battleController.enemy_HpSlider.value = turnScript.p1_Storage.othersStatus.hp;
            if (battleController.enemy_HpSlider.value <= battleController.enemy_HpSlider.maxValue / 5)
            {
                battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
            }
            else if (battleController.enemy_HpSlider.value <= battleController.enemy_HpSlider.maxValue / 2)
            {
                battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
            }
            else
            {
                battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
            }
        }
    }
    #endregion

    #region ステルスロック
    public void StealthRock()
    {
        if(turnScript.p2_Storage.individualFields.stealthRock == false)
        {
            turnScript.p2_Storage.individualFields.stealthRock = true;
        }
        else
        {
            turnScript.message_Tech.Add("しかし　うまくきまらなかった");
        }
        turnScript.isDone = true;
    }
    #endregion

    #region ストーンエッジ
    public void StoneEdge()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
    }
    #endregion

    #region スピードスター
    public void Swift()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
    }
    #endregion

    #region だいちのちから
    public void EarthPower()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 10)
        {
            if (turnScript.p2_Storage.battleStatus.AscendingRank_Sde > -6)
            {
                turnScript.p2_Storage.battleStatus.AscendingRank_Sde--;
                Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Sde, -6, 6);
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特防が　下がった！");
            }
            else
            {
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特防は　これ以上　下がらない！");
            }
        }
    }
    #endregion

    #region だいもんじ
    public void FireBlast()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 10 && turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Fire && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Fire)
        {
            turnScript.p2_Storage.othersStatus.condition = BattleEnum.condition.burn;
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}は\nやけどを　おった！");
        }
    }
    #endregion

    #region つるぎのまい
    public void SwordsDance()
    {
        if (turnScript.p1_Storage.battleStatus.AscendingRank_Atk < 6)
        {
            turnScript.p1_Storage.battleStatus.AscendingRank_Atk += 2;
            Mathf.Clamp(turnScript.p1_Storage.battleStatus.AscendingRank_Atk, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n攻撃が　ぐーんと　上がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n攻撃は　これ以上　上がらない！");
        }
        turnScript.isDone = true;
    }
    #endregion

    #region てっぺき
    public void IronDefense()
    {
        if (turnScript.p1_Storage.battleStatus.AscendingRank_Def < 6)
        {
            turnScript.p1_Storage.battleStatus.AscendingRank_Def += 2;
            Mathf.Clamp(turnScript.p1_Storage.battleStatus.AscendingRank_Def, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n防御が　ぐーんと　上がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n防御は　これ以上　上がらない！");
        }
        turnScript.isDone = true;
    }
    #endregion

    #region でんこうせっか
    public void QuickAttack()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
    }
    #endregion

    #region でんじは
    public void ThunderWave()
    {
        if (turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Electric && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Ground && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Electric && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Ground)
        {
            turnScript.p2_Storage.othersStatus.condition = BattleEnum.condition.paralysis;
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}は\n体が　しびれてしまった！");
        }
        else
        {
            turnScript.message_Tech.Add("しかし　うまくきまらなかった");
        }
        turnScript.isDone = true;
    }
    #endregion

    #region どくづき
    public void PoisonJab()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 30 && turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Poison && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Steel && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Poison && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Steel)
        {
            turnScript.p2_Storage.othersStatus.condition = BattleEnum.condition.poison;
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}は\nどくに　なってしまった！");
        }
    }
    #endregion

    #region どくどく
    public void Toxic()
    {
        if (turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Poison && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Steel && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Poison && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Steel)
        {
            turnScript.p2_Storage.othersStatus.condition = BattleEnum.condition.poison;
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}は\nどくに　なってしまった！");
        }
        else
        {
            turnScript.message_Tech.Add("しかし　うまくきまらなかった");
        }
        turnScript.isDone = true;
    }
    #endregion

    #region ドラゴンクロー
    public void DragonClaw()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
    }
    #endregion

    #region ドラゴンダイブ
    public void DragonRush()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 20)
        {
            turnScript.p2_Storage.battleStatus.frightened = true;
        }
    }
    #endregion

    #region トリックルーム
    public void TrickRoom()
    {
        if(BattleDatas.commonField.trickRoom == false)
        {
            BattleDatas.commonField.trickRoom = true;
            BattleDatas.commonField.trickRoomTurn = 5;
            turnScript.message_Tech.Add("時空が　ゆがんだ！");
        }
        else
        {
            turnScript.message_Tech.Add("しかし　うまくきまらなかった");
        }
        turnScript.isDone = true;
    }
    #endregion

    #region ドレインパンチ
    public void DrainPunch()
    {
        var damage = DamageCalculation();
        turnScript.Damage(damage, turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        float heal = damage / 2;
        turnScript.p1_Storage.othersStatus.hp += (int)Math.Round(heal, 0, MidpointRounding.AwayFromZero);
        turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}から\nHPを　吸い取った！");

        if(turnScript.p1_Storage.othersStatus.hp >= turnScript.p1_Storage.userData_Pokémon.userP_Real_Hp)
        {
            turnScript.p1_Storage.othersStatus.hp = turnScript.p1_Storage.userData_Pokémon.userP_Real_Hp;
        }

        if (turnScript.p1_Storage.userData_Pokémon == BattleDatas.user_PokemonData[battleController.player_PokeTeamNum])
        {
            battleController.player_HpSlider.value = turnScript.p1_Storage.othersStatus.hp;
            battleController.Pokemon1.transform.Find("HPBer_Player1").Find("Text_HPValue").GetComponent<Text>().text = $"{battleController.player_HpSlider.value}/{BattleDatas.user_PokemonData[battleController.player_PokeTeamNum].userP_Real_Hp}";
            if (battleController.player_HpSlider.value <= battleController.player_HpSlider.maxValue / 5)
            {
                battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
            }
            else if (battleController.player_HpSlider.value <= battleController.player_HpSlider.maxValue / 2)
            {
                battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
            }
            else
            {
                battleController.player_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
            }
        }
        else if (turnScript.p1_Storage.userData_Pokémon == BattleDatas.enemy_PokemonData[battleController.enemy_PokeTeamNum])
        {
            battleController.enemy_HpSlider.value = turnScript.p1_Storage.othersStatus.hp;

            if (battleController.enemy_HpSlider.value <= battleController.enemy_HpSlider.maxValue / 5)
            {
                battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[2];
            }
            else if (battleController.enemy_HpSlider.value <= battleController.enemy_HpSlider.maxValue / 2)
            {
                battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[1];
            }
            else
            {
                battleController.enemy_HpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = battleController.Color_HpBer[0];
            }
        }
    }
    #endregion

    #region なみのり
    public void Surf()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
    }
    #endregion

    #region のしかかり
    public void BodySlam()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 30 && turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Electric && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Ground && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Electric && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Ground)
        {
            turnScript.p2_Storage.othersStatus.condition = BattleEnum.condition.paralysis;
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}は\n体が　しびれてしまった！");
        }
    }
    #endregion

    #region ハイドロポンプ
    public void HydroPump()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
    }
    #endregion

    #region ハイパーボイス
    public void HyperVoice()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
    }
    #endregion

    #region はどうだん
    public void AuraSphere()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
    }
    #endregion

    #region パワーウィップ
    public void PowerWhip()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
    }
    #endregion

    #region ビルドアップ
    public void BulkUp()
    {
        if (turnScript.p1_Storage.battleStatus.AscendingRank_Atk < 6)
        {
            turnScript.p1_Storage.battleStatus.AscendingRank_Atk++;
            Mathf.Clamp(turnScript.p1_Storage.battleStatus.AscendingRank_Atk, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n攻撃が　上がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n攻撃は　これ以上　上がらない！");
        }
        if (turnScript.p1_Storage.battleStatus.AscendingRank_Def < 6)
        {
            turnScript.p1_Storage.battleStatus.AscendingRank_Def++;
            Mathf.Clamp(turnScript.p1_Storage.battleStatus.AscendingRank_Def, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n防御が　上がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n防御は　これ以上　上がらない！");
        }
        turnScript.isDone = true;
    }
    #endregion

    #region ふるいたてる
    public void WorkUp()
    {
        if (turnScript.p1_Storage.battleStatus.AscendingRank_Atk < 6)
        {
            turnScript.p1_Storage.battleStatus.AscendingRank_Atk++;
            Mathf.Clamp(turnScript.p1_Storage.battleStatus.AscendingRank_Atk, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n攻撃が　上がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n攻撃は　これ以上　上がらない！");
        }
        if (turnScript.p1_Storage.battleStatus.AscendingRank_Sat < 6)
        {
            turnScript.p1_Storage.battleStatus.AscendingRank_Sat++;
            Mathf.Clamp(turnScript.p1_Storage.battleStatus.AscendingRank_Sat, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n特攻が　上がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n特攻は　これ以上　上がらない！");
        }
        turnScript.isDone = true;
    }
    #endregion

    #region ヘドロばくだん
    public void SludgeBomb()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 30 && turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Poison && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Steel && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Poison && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Steel)
        {
            turnScript.p2_Storage.othersStatus.condition = BattleEnum.condition.poison;
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}は\nどくに　なってしまった！");
        }
    }
    #endregion

    #region ほっぺすりすり
    public void Nuzzle()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        if (turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Electric && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Ground && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Electric && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Ground)
        {
            turnScript.p2_Storage.othersStatus.condition = BattleEnum.condition.paralysis;
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}は\n体が　しびれてしまった！");
        }
    }
    #endregion

    #region ほのおのキバ
    public void FireFang()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 10 && turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Fire && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Fire)
        {
            turnScript.p2_Storage.othersStatus.condition = BattleEnum.condition.burn;
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}は\nやけどを　おった！");
        }
    }
    #endregion

    #region マジカルシャイン
    public void DazzlingGleam()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
    }
    #endregion

    #region マジカルフレイム
    public void MysticalFire()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 100)
        {
            if (turnScript.p2_Storage.battleStatus.AscendingRank_Sat > -6)
            {
                turnScript.p2_Storage.battleStatus.AscendingRank_Sat--;
                Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Sat, -6, 6);
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特攻が　下がった！");
            }
            else
            {
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特攻は　これ以上　下がらない！");
            }
        }
    }
    #endregion

    #region マジカルリーフ
    public void MagicalLeaf()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
    }
    #endregion

    #region まもる
    public void Protect()
    {
        if (turnScript.p1_Storage.battleStatus.protectTrun == 0 && turnScript.p2_Storage.battleStatus.protect == false)
        {
            turnScript.p1_Storage.battleStatus.protect = true;
        }
        else
        {
            turnScript.p1_Storage.battleStatus.protectTrun = 0;
            turnScript.message_Tech.Add("しかし　うまくきまらなかった");
        }
        turnScript.isDone = true;
    }
    #endregion

    #region ムーンフォース
    public void Moonblast()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 30)
        {
            if (turnScript.p2_Storage.battleStatus.AscendingRank_Sat > -6)
            {
                turnScript.p2_Storage.battleStatus.AscendingRank_Sat--;
                Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Sat, -6, 6);
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特攻が　下がった！");
            }
            else
            {
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特攻は　これ以上　下がらない！");
            }
        }
    }
    #endregion

    #region めいそう
    public void CalmMind()
    {
        if (turnScript.p1_Storage.battleStatus.AscendingRank_Sat < 6)
        {
            turnScript.p1_Storage.battleStatus.AscendingRank_Sat++;
            Mathf.Clamp(turnScript.p1_Storage.battleStatus.AscendingRank_Sat, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n特攻が　上がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n特攻は　これ以上　上がらない！");
        }
        if (turnScript.p1_Storage.battleStatus.AscendingRank_Sde < 6)
        {
            turnScript.p1_Storage.battleStatus.AscendingRank_Sde++;
            Mathf.Clamp(turnScript.p1_Storage.battleStatus.AscendingRank_Sde, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n特防が　上がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n特防は　これ以上　上がらない！");
        }
        turnScript.isDone = true;
    }
    #endregion

    #region メタルクロー
    public void MetalClaw()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 10)
        {
            if (turnScript.p1_Storage.battleStatus.AscendingRank_Atk < 6)
            {
                turnScript.p1_Storage.battleStatus.AscendingRank_Atk++;
                Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Atk, -6, 6);
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n攻撃が　上がった！");
            }
            else
            {
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n攻撃は　これ以上　上がらない！");
            }
        }
    }
    #endregion

    #region らいげき
    public void BoltStrike()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 20 && turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Electric && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Ground && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Electric && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Ground)
        {
            turnScript.p2_Storage.othersStatus.condition = BattleEnum.condition.paralysis;
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}は\n体が　しびれてしまった！");
        }
    }
    #endregion

    #region ラスターカノン
    public void FlashCannon()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 10)
        {
            if (turnScript.p2_Storage.battleStatus.AscendingRank_Sde > -6)
            {
                turnScript.p2_Storage.battleStatus.AscendingRank_Sde--;
                Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Sde, -6, 6);
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特防が　下がった！");
            }
            else
            {
                turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特防は　これ以上　下がらない！");
            }
        }
    }
    #endregion

    #region りゅうせいぐん
    public void DracoMeteor()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        if (turnScript.p2_Storage.battleStatus.AscendingRank_Sat > -6)
        {
            turnScript.p2_Storage.battleStatus.AscendingRank_Sat--;
            Mathf.Clamp(turnScript.p2_Storage.battleStatus.AscendingRank_Sat, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特攻が　がくっと　下がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}の\n特攻は　これ以上　下がらない！");
        }
    }
    #endregion

    #region りゅうのはどう
    public void DragonPulse()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
    }
    #endregion

    #region りゅうのまい
    public void DragonDance()
    {
        if (turnScript.p1_Storage.battleStatus.AscendingRank_Atk < 6)
        {
            turnScript.p1_Storage.battleStatus.AscendingRank_Atk++;
            Mathf.Clamp(turnScript.p1_Storage.battleStatus.AscendingRank_Atk, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n攻撃が　上がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n攻撃は　これ以上　上がらない！");
        }
        if (turnScript.p1_Storage.battleStatus.AscendingRank_Spe < 6)
        {
            turnScript.p1_Storage.battleStatus.AscendingRank_Spe++;
            Mathf.Clamp(turnScript.p1_Storage.battleStatus.AscendingRank_Spe, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n素早さが　上がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n素早さは　これ以上　上がらない！");
        }
        turnScript.isDone = true;
    }
    #endregion

    #region れいとうビーム
    public void IceBeam()
    {
        turnScript.Damage(DamageCalculation(), turnScript.isEnemy, turnScript.p1_Storage.technique_Pokémon.t_Name);
        int r = UnityEngine.Random.Range(1, 101);
        if (r < 10 && turnScript.p2_Storage.othersStatus.condition == BattleEnum.condition.none && turnScript.p2_Storage.userData_Pokémon.userP_Type1 != Pokémon_Type.Type.Ice && turnScript.p2_Storage.userData_Pokémon.userP_Type2 != Pokémon_Type.Type.Ice)
        {
            turnScript.p2_Storage.othersStatus.condition = BattleEnum.condition.ice;
            turnScript.message_Tech.Add($"{turnScript.p2_Storage.userData_Pokémon.userP_NickName}は\n体が　凍ってしまった！");
        }
    }
    #endregion

    #region わるだくみ
    public void NastyPlot()
    {
        if (turnScript.p1_Storage.battleStatus.AscendingRank_Sat < 6)
        {
            turnScript.p1_Storage.battleStatus.AscendingRank_Sat += 2;
            Mathf.Clamp(turnScript.p1_Storage.battleStatus.AscendingRank_Sat, -6, 6);
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n特攻が　ぐーんと　上がった！");
        }
        else
        {
            turnScript.message_Tech.Add($"{turnScript.p1_Storage.userData_Pokémon.userP_NickName}の\n特攻は　これ以上　上がらない！");
        }
        turnScript.isDone = true;
    }
    #endregion
}
