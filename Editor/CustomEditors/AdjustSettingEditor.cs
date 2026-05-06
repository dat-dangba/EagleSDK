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
        private const string LinkInstall = "https://github.com/adjust/unity_sdk.git?path=Assets/Adjust";

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            if (InstallPackageHelper.IsPackageInstalled(PackageId))
            {
                root.Add(new PackageInstalledVisualElement("Adjust"));
            }
            else
            {
                root.Add(new InstallPackageVisualElement("Adjust", InstallAdjust));
            }

            HideScript(root);

            return root;
        }

        private void InstallAdjust()
        {
            InstallPackageHelper.Install(LinkInstall, () => { });
        }
    }
}