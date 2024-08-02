using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor(typeof(DamageCalculate))]
public class DemageCalculateEditor : Editor
{
    private SerializedProperty AtkWhite;
    private SerializedProperty AtkBlue;
    private SerializedProperty MultiDMG;
    private SerializedProperty DmgBoost;
    private SerializedProperty CritDMG;
    private SerializedProperty ReductionDMG;
    private SerializedProperty Vulnerable;
    private SerializedProperty Resistance;
    private SerializedProperty Penetration;
    private SerializedProperty AttributeRestraint;
    private SerializedProperty Breaked;
    private SerializedProperty AcotrLevel;
    private SerializedProperty EnemyLevel;
    private SerializedProperty mDamageResult;
    private SerializedProperty EnemyList;

    private void OnEnable()
    {
        AtkWhite = serializedObject.FindProperty("AtkWhite");
        AtkBlue = serializedObject.FindProperty("AtkBlue");
        DmgBoost = serializedObject.FindProperty("DmgBoost");
        CritDMG = serializedObject.FindProperty("CritDMG");
        ReductionDMG = serializedObject.FindProperty("ReductionDMG");
        Vulnerable = serializedObject.FindProperty("Vulnerable");
        Resistance = serializedObject.FindProperty("Resistance");
        Penetration = serializedObject.FindProperty("Penetration");
        AttributeRestraint = serializedObject.FindProperty("AttributeRestraint");
        Breaked = serializedObject.FindProperty("Breaked");
        AcotrLevel = serializedObject.FindProperty("AcotrLevel");
        EnemyLevel = serializedObject.FindProperty("EnemyLevel");
        mDamageResult = serializedObject.FindProperty("mDamageResult");
        EnemyList = serializedObject.FindProperty("EnemyList");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(AtkWhite, new GUIContent("攻击力白值"), true);
        EditorGUILayout.PropertyField(AtkBlue, new GUIContent("攻击力蓝值"), true);
        EditorGUILayout.PropertyField(CritDMG, new GUIContent("暴伤"), true);
        EditorGUILayout.PropertyField(DmgBoost, new GUIContent("增伤Buff"), true);
        EditorGUILayout.PropertyField(Penetration, new GUIContent("穿透"), true);
        EditorGUILayout.PropertyField(AcotrLevel, new GUIContent("玩家角色等级"), true);

        EditorGUILayout.PropertyField(EnemyList, new GUIContent("敌人列表"), true);

        EditorGUILayout.PropertyField(mDamageResult, new GUIContent("最终伤害"), true);

        DamageCalculate damageCalc = (DamageCalculate)target;
        if (GUILayout.Button("计算伤害"))
        {
            damageCalc.CalculateDemage();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
