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
        public const string LinkInstall = "https://github.com/adjust/unity_sdk.git?path=Assets/Adjust";

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            if (InstallPackageHelper.IsPackageInstalled(PackageId))
            {
                root.Add(new PackageInstalledVisualElement("Adjust"));
                InspectorElement.FillDefaultInspector(root, serializedObject, this);
#if HAS_EAGLE_ANALYTICS
                var buildConfig = EagleServices.GetBuildConfig<AdjustBuildConfig>();
                if (buildConfig != null)
                {
                    var serializedConfig = new SerializedObject(buildConfig);
                    var configInspector = new InspectorElement(serializedConfig)
                    {
                        style =
                        {
                            paddingLeft = 0,
                            marginTop = 10
                        }
                    };
                    root.Add(configInspector);
                }
#endif
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