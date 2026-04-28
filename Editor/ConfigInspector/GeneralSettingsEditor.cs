using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Eagle
{
    [CustomEditor(typeof(GeneralSetting))]
    public class GeneralSettingsEditor : EagleSettingEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            HideScript(root);

            return root;
        }
    }
}