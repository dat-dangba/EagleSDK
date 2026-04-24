using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Eagle
{
    [CustomEditor(typeof(GeneralConfig))]
    public class GeneralConfigEditor : EagleConfigEditor
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