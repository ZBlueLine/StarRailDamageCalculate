using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;

[Serializable]
public class Enemy
{
    [Rename("启用")]
    public bool Enable = true;

    //敌方属性
    public float[] 减伤;

    public float[] 易伤;

    public float[] 减防;

    [Rename("触发暴击"), Min(0)]
    public bool Crit = false;

    [Rename("技能倍率"), Min(0)]
    public float MultiDMG = 0;

    [Rename("抗性"), Min(0)]
    public float Resistance;

    [Rename("被弱点克制")]
    public bool AttributeRestraint = false;

    [Rename("击破状态")]
    public bool Breaked = false;

    [Rename("等级"), Min(0)]
    public float EnemyLevel;

    [Rename("受到伤害")]
    public float DamageResult = 0;
}

public class DamageCalculate : MonoBehaviour
{
    [Min(0)]
    public float AtkWhite = 0;
    public float AtkBlue = 0;

    public float[] DmgBoost;

    [Min(0)]
    public float CritDMG = 0.5f;

    [Min(0)]
    public float Penetration;

    [Min(0)]
    public float AcotrLevel;

    public Enemy[] EnemyList;


    [Space(20)]
    [SerializeField]
    private float mDamageResult;
    public void CalculateDemage()
    {
        //攻击
        float basicDMG = AtkWhite + AtkBlue;


        //增伤
        float dmgBoost = 1;
        foreach (var boost in DmgBoost)
        {
            dmgBoost += boost;
        }
        basicDMG *= dmgBoost;


        float finalDemage = 0;
        foreach (var enemy in EnemyList)
        {
            if (!enemy.Enable)
                continue;
            float perEnemyDMG = basicDMG;

            //暴击
            if (enemy.Crit)
            {
                perEnemyDMG *= 1 + CritDMG;
            }

            //伤害倍率
            perEnemyDMG *= enemy.MultiDMG;
            //减伤
            float reductionDMG = 1;
            reductionDMG -= enemy.Breaked ? 0f : 0.1f;
            foreach (var reduce in enemy.减伤)
            {
                reductionDMG -= reduce;
            }
            reductionDMG = Mathf.Max(reductionDMG, 0);
            perEnemyDMG *= reductionDMG;

            //易伤
            float vulnerable = 1;
            foreach (var v in enemy.易伤)
            {
                vulnerable += v;
            }

            //抗性 穿透
            float resis = enemy.Resistance + (enemy.AttributeRestraint ? 0f : 0.2f);
            perEnemyDMG *= 1 - (resis - Penetration);

            float reductionDEF = 1;
            foreach (var reduce in enemy.减防)
            {
                reductionDEF -= reduce;
            }
            //防御力效果
            float defEffect = (AcotrLevel * 10 + 200) / (AcotrLevel * 10 + 200 + (enemy.EnemyLevel * 10 + 200) * reductionDEF);
            perEnemyDMG *= defEffect;
            enemy.DamageResult = perEnemyDMG;
            finalDemage += perEnemyDMG;
        }

        mDamageResult = finalDemage;
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class RenameAttribute : PropertyAttribute
{
    //用来显示中文的字符串
    public string name;

    public RenameAttribute(string name)
    {
        this.name = name;
    }
}

[CustomPropertyDrawer(typeof(RenameAttribute))] //用到RenameAttribute的地方都会被重绘
public class RenameDrawer : PropertyDrawer //相对于Editor类可以修改MonoBehaviour的外观，我们可以简单的理解PropertyDrawer为修改struct/class的外观的Editor类
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //替换属性名称
        RenameAttribute rename = (RenameAttribute)attribute;
        label.text = rename.name;

        //重绘GUI
        EditorGUI.PropertyField(position, property, label);
    }

}