using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using TextEditor = UnityEditor.UI.TextEditor;

[CustomEditor(typeof(Label))]
public class LabelEditor : GraphicEditor
{
    private Label m_Label;
    private SerializedProperty m_UseLocalization;
    private SerializedProperty m_LocalizationKey;
    private SerializedProperty m_Text;
    private SerializedProperty m_FontData;
    private SerializedProperty m_FontType;
    private SerializedProperty m_TiltEffect;
    private SerializedProperty m_TiltAngle;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.m_UseLocalization = this.serializedObject.FindProperty("m_UseLocalization");
        this.m_LocalizationKey = this.serializedObject.FindProperty("m_LocalizationKey");
        this.m_Text = this.serializedObject.FindProperty("m_Text");
        this.m_FontData = this.serializedObject.FindProperty("m_FontData");
        this.m_FontType = this.serializedObject.FindProperty("m_FontType");
        this.m_TiltEffect = this.serializedObject.FindProperty("m_TiltEffect");
        this.m_TiltAngle = this.serializedObject.FindProperty("m_TiltAngle");
        this.m_Label = (Label) target;
    }
    
    
    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        EditorGUILayout.PropertyField(this.m_UseLocalization);
        if (this.m_UseLocalization.boolValue)
        {
            EditorGUILayout.PropertyField(this.m_LocalizationKey);
            GUI.enabled = false;
        }
        EditorGUILayout.PropertyField(this.m_Text);
        GUI.enabled = true;
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(this.m_FontType);
        EditorGUILayout.PropertyField(this.m_FontData);
        this.AppearanceControlsGUI();
        this.RaycastControlsGUI();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(this.m_TiltEffect);
        if (this.m_TiltEffect.boolValue)
        {
            float v = EditorGUILayout.FloatField(this.m_TiltAngle.floatValue);
            if (v != this.m_TiltAngle.floatValue)
                this.m_TiltAngle.floatValue = v;
        }
        EditorGUILayout.EndHorizontal();

        this.serializedObject.ApplyModifiedProperties();
    }
}
