using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remember_Pokémon
{
    public string p_T_Name { get; set; }
    public int p_T_Id { get; set; }
    
    //技名
    public string Technique1 { get; set; }
    public string Technique2 { get; set; }
    public string Technique3 { get; set; }
    public string Technique4 { get; set; }
    public string Technique5 { get; set; }
    public string Technique6 { get; set; }
    public string Technique7 { get; set; }
    public string Technique8 { get; set; }
    public string Technique9 { get; set; }
    public string Technique10 { get; set; }

    //技ID
    public int TechniqueID1 { get; } = 1;
    public int TechniqueID2 { get; } = 2;
    public int TechniqueID3 { get; } = 3;
    public int TechniqueID4 { get; } = 4;
    public int TechniqueID5 { get; } = 5;
    public int TechniqueID6 { get; } = 6;
    public int TechniqueID7 { get; } = 7;
    public int TechniqueID8 { get; } = 8;
    public int TechniqueID9 { get; } = 9;
    public int TechniqueID10 { get; } = 10;
}
public class Technique_Pokémon
{
    public string t_Name { get; set; } //技名
    public string t_Explanation { get; set; } //技説明
    public string t_Type { get; set; } //技タイプ
    public string t_Classification { get; set; } //技分類
    public int t_Power { get; set; } //技威力
    public int t_Hit { get; set; } //技命中率
    public int t_PP { get; set; } //技PP
    public int t_Priority { get; set; } //技優先度
    public bool t_DirectAttack { get; set; } //直接攻撃か否か
}