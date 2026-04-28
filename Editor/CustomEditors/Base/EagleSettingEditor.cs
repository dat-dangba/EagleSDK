using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Eagle
{
    public class EagleSettingEditor : Editor
    {
        protected void HideScript(VisualElement root)
        {
            VisualElement scriptField = root.Q<PropertyField>("PropertyField:m_Script");
            if (scriptField != null)
            {
                scriptField.style.display = DisplayStyle.None;
            }
        }
    }
}