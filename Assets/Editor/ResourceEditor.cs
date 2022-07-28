using System.Collections;
using UnityEngine;
using UnityEditor;
using Assets.Resources;

namespace Assets.Editor
{
    [CustomEditor(typeof(Grass))]
    public class GrassEditor : UnityEditor.Editor
    {
        SerializedProperty SpotList;
        SerializedProperty SpotPrefab;

        void OnEnable()
        {
            SpotList = serializedObject.FindProperty("SpotList");
            SpotPrefab = serializedObject.FindProperty("SpotPrefab");
        }

        public override void OnInspectorGUI()
        {
            //serializedObject.Update();
            //EditorGUILayout.PropertyField(SpotList);
            //serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();

            // custom
            if (GUILayout.Button("Add spot"))
            {
                SpotList.arraySize++;

                var item = SpotList.GetArrayElementAtIndex(SpotList.arraySize - 1);
                var spot = item.FindPropertyRelative("SpotObject");
                var free = item.FindPropertyRelative("IsFree");
                var obj = Instantiate((GameObject)SpotPrefab.objectReferenceValue);
                obj.transform.position = ((Grass)target).gameObject.transform.position;
                spot.objectReferenceValue = obj;
                free.boolValue = true;

                serializedObject.ApplyModifiedProperties();
            }
            if (GUILayout.Button("Remove spot"))
            {
                var item = SpotList.GetArrayElementAtIndex(SpotList.arraySize - 1);
                var spot = item.FindPropertyRelative("SpotObject");
                DestroyImmediate(spot.objectReferenceValue);

                SpotList.arraySize--;

                serializedObject.ApplyModifiedProperties();
            }
        }


    }

    [CustomEditor(typeof(Water))]
    public class WaterEditor : UnityEditor.Editor
    {
        SerializedProperty SpotList;
        SerializedProperty SpotPrefab;

        void OnEnable()
        {
            SpotList = serializedObject.FindProperty("SpotList");
            SpotPrefab = serializedObject.FindProperty("SpotPrefab");
        }

        public override void OnInspectorGUI()
        {
            //serializedObject.Update();
            //EditorGUILayout.PropertyField(SpotList);
            //serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();

            // custom
            if (GUILayout.Button("Add spot"))
            {
                SpotList.arraySize++;

                var item = SpotList.GetArrayElementAtIndex(SpotList.arraySize - 1);
                var spot = item.FindPropertyRelative("SpotObject");
                var free = item.FindPropertyRelative("IsFree");
                var obj = Instantiate((GameObject)SpotPrefab.objectReferenceValue);
                obj.transform.position = new Vector3(((Water)target).gameObject.transform.position.x, .5f, ((Water)target).gameObject.transform.position.z);
                obj.transform.parent = ((Water)target).gameObject.transform;
                spot.objectReferenceValue = obj;
                free.boolValue = true;

                serializedObject.ApplyModifiedProperties();
            }
            if (GUILayout.Button("Remove spot"))
            {
                var item = SpotList.GetArrayElementAtIndex(SpotList.arraySize - 1);
                var spot = item.FindPropertyRelative("SpotObject");
                DestroyImmediate(spot.objectReferenceValue);

                SpotList.arraySize--;

                serializedObject.ApplyModifiedProperties();
            }
        }


    }
}