using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

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

        private FieldInfo field;
        private IList list;

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

            EditorGUILayout.BeginHorizontal();
            int newvalue = EditorGUILayout.IntField("dataCount", dataCount.intValue);
            if( newvalue != dataCount.intValue  )
            {
                dataCount.intValue = newvalue;
            }

            if (Application.isPlaying && GUILayout.Button("Sure"))
            {
                ScrollPositionController controller = target as ScrollPositionController;
                BaseFancyScrollView view = controller.GetComponent<BaseFancyScrollView>();

                controller.SetDataCount(newvalue);

                if (view != null)
                {
                    if (field == null)
                    {
                        field = view.GetType().GetField("cellData", BindingFlags.Instance | BindingFlags.NonPublic);
                        list = (IList)field.GetValue(view);
                    }
                    int datacnt = controller.GetDataCount();
                    if (datacnt < list.Count)
                    {
                        for (int i = list.Count - 1; i >= datacnt; --i)
                        {
                            list.RemoveAt(i);
                        }
                    }
                    else
                    {
                        Type ListElementType = list.GetType().GetGenericArguments()[0];
                        for (int i = list.Count; i < datacnt; ++i)
                        {
                            list.Add(Activator.CreateInstance(ListElementType));
                        }
                    }

                    view.RefreshCells();
                }
            }

            EditorGUILayout.EndHorizontal();

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
