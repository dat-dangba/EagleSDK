using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eagle
{
    public class InstallPackageVisualElement : VisualElement
    {
        public InstallPackageVisualElement(string sdkName, Action clickEvent)
        {
            Label label = new Label($"{sdkName} is not installed.")
            {
                style =
                {
                    height = 30,
                    backgroundColor = new Color(0.345098f, 0.345098f, 0.345098f),
                    color = Color.red,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    unityTextAlign = TextAnchor.MiddleCenter,
                }
            };
            Add(label);
            Button installEagleAnalytics = new Button(clickEvent)
            {
                text = $"Install {sdkName}",
                style =
                {
                    marginTop = 10
                }
            };
            Add(installEagleAnalytics);
        }
    }
}