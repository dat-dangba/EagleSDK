using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eagle
{
    [CustomEditor(typeof(AdjustSetting))]
    public class AdjustSettingEditor : EagleSettingEditor
    {
        private const string PackageId = "com.adjust.sdk";
        private const string LinkInstall = "https://github.com/adjust/unity_sdk.git?path=Assets/Adjust#v5.6.0";

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            if (InstallPackageHelper.IsPackageInstalled(PackageId))
            {
                Label label = new Label("Adjust has been installed.")
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
                root.Add(label);
            }
            else
            {
                Label label = new Label("Adjust is not installed.")
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
                root.Add(label);
                Button installAdjust = new Button(InstallAdjust)
                {
                    text = "Install Adjust",
                    style =
                    {
                        marginTop = 20
                    }
                };
                root.Add(installAdjust);
            }

            HideScript(root);

            return root;
        }

        private void InstallAdjust()
        {
            InstallPackageHelper.Install(LinkInstall, () =>
            {
            });
        }
    }
}