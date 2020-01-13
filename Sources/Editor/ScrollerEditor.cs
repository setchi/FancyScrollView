/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEditor;
using UnityEditor.AnimatedValues;

// For manteinance, every new [SerializeField] variable in Scroller must be declared here

namespace FancyScrollView
{
    [CustomEditor(typeof(Scroller))]
    [CanEditMultipleObjects]
    public class ScrollerEditor : Editor
    {
        SerializedProperty viewport;
        SerializedProperty scrollDirection;
        SerializedProperty movementType;
        SerializedProperty elasticity;
        SerializedProperty scrollSensitivity;
        SerializedProperty inertia;
        SerializedProperty decelerationRate;
        SerializedProperty snap;
        SerializedProperty snapEnable;
        SerializedProperty snapVelocityThreshold;
        SerializedProperty snapDuration;
        SerializedProperty snapEasing;
        SerializedProperty draggable;
        SerializedProperty scrollbar;

        AnimBool showElasticity;
        AnimBool showInertiaRelatedValues;
        AnimBool showSnapEnableRelatedValues;

        void OnEnable()
        {
            viewport = serializedObject.FindProperty("viewport");
            scrollDirection = serializedObject.FindProperty("scrollDirection");
            movementType = serializedObject.FindProperty("movementType");
            elasticity = serializedObject.FindProperty("elasticity");
            scrollSensitivity = serializedObject.FindProperty("scrollSensitivity");
            inertia = serializedObject.FindProperty("inertia");
            decelerationRate = serializedObject.FindProperty("decelerationRate");
            snap = serializedObject.FindProperty("snap");
            snapEnable = serializedObject.FindProperty("snap.Enable");
            snapVelocityThreshold = serializedObject.FindProperty("snap.VelocityThreshold");
            snapDuration = serializedObject.FindProperty("snap.Duration");
            snapEasing = serializedObject.FindProperty("snap.Easing");
            draggable = serializedObject.FindProperty("draggable");
            scrollbar = serializedObject.FindProperty("scrollbar");

            showElasticity = new AnimBool(Repaint);
            showInertiaRelatedValues = new AnimBool(Repaint);
            showSnapEnableRelatedValues = new AnimBool(Repaint);
            SetAnimBools(true);
        }

        void OnDisable()
        {
            showElasticity.valueChanged.RemoveListener(Repaint);
            showInertiaRelatedValues.valueChanged.RemoveListener(Repaint);
            showSnapEnableRelatedValues.valueChanged.RemoveListener(Repaint);
        }

        void SetAnimBools(bool instant)
        {
            SetAnimBool(showElasticity, !movementType.hasMultipleDifferentValues && movementType.enumValueIndex == (int)MovementType.Elastic, instant);
            SetAnimBool(showInertiaRelatedValues, !inertia.hasMultipleDifferentValues && inertia.boolValue, instant);
            SetAnimBool(showSnapEnableRelatedValues, !snapEnable.hasMultipleDifferentValues && snapEnable.boolValue, instant);
        }

        void SetAnimBool(AnimBool a, bool value, bool instant)
        {
            if (instant)
            {
                a.value = value;
            }
            else
            {
                a.target = value;
            }
        }

        public override void OnInspectorGUI()
        {
            SetAnimBools(false);

            serializedObject.Update();
            EditorGUILayout.PropertyField(viewport);
            EditorGUILayout.PropertyField(scrollDirection);
            EditorGUILayout.PropertyField(movementType);
            DrawMovementTypeRelatedValue();
            EditorGUILayout.PropertyField(scrollSensitivity);
            EditorGUILayout.PropertyField(inertia);
            DrawInertiaRelatedValues();
            EditorGUILayout.PropertyField(draggable);
            EditorGUILayout.PropertyField(scrollbar);
            serializedObject.ApplyModifiedProperties();
        }

        void DrawMovementTypeRelatedValue()
        {
            using (var group = new EditorGUILayout.FadeGroupScope(showElasticity.faded))
            {
                if (!group.visible)
                {
                    return;
                }

                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(elasticity);
                }
            }
        }

        void DrawInertiaRelatedValues()
        {
            using (var group = new EditorGUILayout.FadeGroupScope(showInertiaRelatedValues.faded))
            {
                if (!group.visible)
                {
                    return;
                }

                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(decelerationRate);
                    EditorGUILayout.PropertyField(snap);

                    using (new EditorGUI.IndentLevelScope())
                    {
                        DrawSnapRelatedValues();
                    }
                }
            }
        }

        void DrawSnapRelatedValues()
        {
            if (snap.isExpanded)
            {
                EditorGUILayout.PropertyField(snapEnable);

                using (var group = new EditorGUILayout.FadeGroupScope(showSnapEnableRelatedValues.faded))
                {
                    if (!group.visible)
                    {
                        return;
                    }

                    EditorGUILayout.PropertyField(snapVelocityThreshold);
                    EditorGUILayout.PropertyField(snapDuration);
                    EditorGUILayout.PropertyField(snapEasing);
                }
            }
        }
    }
}
