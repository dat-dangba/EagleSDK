using UnityEngine;
using UnityEngine.UIElements;

namespace Eagle
{
    public class PackageInstalledVisualElement : VisualElement
    {
        public PackageInstalledVisualElement(string sdkName)
        {
            Label label = new Label($"{sdkName} has been installed.")
            {
                style =
                {
                    height = 30,
                    backgroundColor = new Color(0.345098f, 0.345098f, 0.345098f),
                    color = Color.green,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    unityTextAlign = TextAnchor.MiddleCenter,
                }
            };
            Add(label);
        }
    }
}