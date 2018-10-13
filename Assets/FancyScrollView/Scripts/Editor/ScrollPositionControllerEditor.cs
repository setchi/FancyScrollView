using UnityEditor;

// For manteinance, every new [SerializeField] variable in ScrollPositionController must be declared here

namespace FancyScrollView
{
    [CustomEditor(typeof(ScrollPositionController))]
    [CanEditMultipleObjects]
    public class ScrollPositionControllerEditor : Editor
    {
        SerializedProperty viewport;
        SerializedProperty directionOfRecognize;
        SerializedProperty movementType;
        SerializedProperty elasticity;
        SerializedProperty scrollSensitivity;
        SerializedProperty inertia;
        SerializedProperty decelerationRate;
        SerializedProperty snap;
        SerializedProperty snapEnable;
        SerializedProperty snapVelocityThreshold;
        SerializedProperty snapDuration;
        SerializedProperty dataCount;

        void OnEnable()
        {
            viewport = serializedObject.FindProperty("viewport");
            directionOfRecognize = serializedObject.FindProperty("directionOfRecognize");
            movementType = serializedObject.FindProperty("movementType");
            elasticity = serializedObject.FindProperty("elasticity");
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
            EditorGUILayout.PropertyField(elasticity);
            EditorGUILayout.PropertyField(scrollSensitivity);
            EditorGUILayout.PropertyField(inertia);
            DrawInertiaRelatedValues();
            EditorGUILayout.PropertyField(dataCount);
            serializedObject.ApplyModifiedProperties();
        }

        void DrawInertiaRelatedValues()
        {
            if (inertia.boolValue)
            {
                EditorGUILayout.PropertyField(decelerationRate);
                EditorGUILayout.PropertyField(snap);

                using (new EditorGUI.IndentLevelScope())
                {
                    DrawSnapRelatedValues();
                }
            }
        }

        void DrawSnapRelatedValues()
        {
            if (snap.isExpanded)
            {
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
