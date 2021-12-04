using UnityEditor;
using UnityEngine;


namespace Gamekit2D
{
    [CustomEditor(typeof(AstralCopyController))]
    public class AstralCopyControllerEditor : Editor
    {
        SerializedProperty m_AstralCopyColorProp;
        SerializedProperty m_AstralCopyShieldSpawnDistanceProp;
        SerializedProperty m_AstralCopyShieldStrengthProp;
        SerializedProperty m_AstralCopyShieldCooldownProp;
        SerializedProperty m_AstralCopySpearSpawnDistanceProp;
        SerializedProperty m_MaxAstralCopiesCountProp;

        bool m_AstralCopySettingsFoldout;

        readonly GUIContent m_AstralCopySettingsContent = new GUIContent("Astral Copy Settings");

        readonly GUIContent m_AstralCopyColorContent = new GUIContent("Color");
        readonly GUIContent m_AstralCopyShieldSpawnDistanceContent = new GUIContent("Shield spawn distance");
        readonly GUIContent m_AstralCopyShieldStrengthContent = new GUIContent("Shield strenght");
        readonly GUIContent m_AstralCopyShieldCooldownContent = new GUIContent("Shield cooldown");
        readonly GUIContent m_AstralCopySpearSpawnDistanceContent = new GUIContent("Spear spawn distance");
        readonly GUIContent m_MaxastralCopiesCountContent = new GUIContent("Max astral copies");

        void OnEnable()
        {
            m_AstralCopyColorProp = serializedObject.FindProperty("AstralCopyColor");
            m_AstralCopyShieldSpawnDistanceProp = serializedObject.FindProperty("AstralCopyShieldSpawnDistance");
            m_AstralCopyShieldStrengthProp = serializedObject.FindProperty("AstralCopyShieldStrength");
            m_AstralCopyShieldCooldownProp = serializedObject.FindProperty("AstralCopyShieldCooldown");
            m_AstralCopySpearSpawnDistanceProp = serializedObject.FindProperty("AstralCopySpearSpawnDistance");
            m_MaxAstralCopiesCountProp = serializedObject.FindProperty("MaxAstralCopiesCount");
        }

        public override void OnInspectorGUI()
        {

            m_AstralCopySettingsFoldout = EditorGUILayout.Foldout(m_AstralCopySettingsFoldout, m_AstralCopySettingsContent);

            if (m_AstralCopySettingsFoldout)
            {
                EditorGUILayout.PropertyField(m_AstralCopyColorProp, m_AstralCopyColorContent);
                EditorGUILayout.PropertyField(m_AstralCopyShieldSpawnDistanceProp, m_AstralCopyShieldSpawnDistanceContent);
                EditorGUILayout.PropertyField(m_AstralCopyShieldStrengthProp, m_AstralCopyShieldStrengthContent);
                EditorGUILayout.PropertyField(m_AstralCopyShieldCooldownProp, m_AstralCopyShieldCooldownContent);
                EditorGUILayout.PropertyField(m_AstralCopySpearSpawnDistanceProp, m_AstralCopySpearSpawnDistanceContent);
                EditorGUILayout.PropertyField(m_MaxAstralCopiesCountProp, m_MaxastralCopiesCountContent);
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            serializedObject.ApplyModifiedProperties();
        }

    }
}