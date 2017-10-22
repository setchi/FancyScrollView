using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


//For manteinance, every new [SerializeField] variable in ScrollPositionController must be declared here

namespace FancyScrollView
{
    [CustomEditor(typeof(ScrollPositionController))]
    [CanEditMultipleObjects]
    public class ScrollPositionControllerEditor : Editor
    {
        private SerializedProperty viewport;
        private SerializedProperty directionOfRecognize;
        private SerializedProperty movementType;
        private SerializedProperty scrollSensitivity;
        private SerializedProperty inertia;
        private SerializedProperty decelerationRate;
        private SerializedProperty snap;
        private SerializedProperty snapEnable;
        private SerializedProperty snapVelocityThreshold;
        private SerializedProperty snapDuration; 
        private SerializedProperty dataCount;
      

        private void OnEnable()
        {
            viewport = serializedObject.FindProperty("viewport");
            directionOfRecognize = serializedObject.FindProperty("directionOfRecognize");
            movementType = serializedObject.FindProperty("movementType");
            scrollSensitivity = serializedObject.FindProperty("scrollSensitivity");
            inertia = serializedObject.FindProperty("inertia");
            decelerationRate = serializedObject.FindProperty("decelerationRate");
            snap = serializedObject.FindProperty("snap");
            snapEnable = serializedObject.FindProperty("snap.Enable");
            snapVelocityThreshold = serializedObject.FindProperty("snap.VelocityThreshold");
            snapDuration = serializedObject.FindProperty("snap.Duration"); 
            dataCount = serializedObject.FindProperty("dataCount");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(viewport);
            EditorGUILayout.PropertyField(directionOfRecognize);
            EditorGUILayout.PropertyField(movementType);
            EditorGUILayout.PropertyField(scrollSensitivity);
            EditorGUILayout.PropertyField(inertia);
            DrawInertiaRelatedValues(); 
            EditorGUILayout.PropertyField(dataCount);
            serializedObject.ApplyModifiedProperties(); 
            
        }


        private void DrawInertiaRelatedValues()
        {
            if (inertia.boolValue)
            {
                EditorGUILayout.PropertyField(decelerationRate);
                EditorGUILayout.PropertyField(snap);
                DrawSnapRelatedValues(); 
                EditorGUI.indentLevel = 0;

            }
        }

        private void DrawSnapRelatedValues()
        {
            if (snap.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(snapEnable);
                if (snapEnable.boolValue)
                {
                    EditorGUILayout.PropertyField(snapVelocityThreshold);
                    EditorGUILayout.PropertyField(snapDuration);
                }
            }
        }

    



    }

    

}
