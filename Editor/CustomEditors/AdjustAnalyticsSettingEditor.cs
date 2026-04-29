using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eagle
{
    [CustomEditor(typeof(AdjustAnalyticsSetting))]
    public class AdjustAnalyticsSettingEditor : EagleSettingEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);
#if HAS_EAGLE_ANALYTICS
            ScriptableObject adjustSetting = EagleServices.GetSetting<SdkInitSettingBase>("AdjustSetting");
            SerializedObject adjustSettingSerialized = new SerializedObject(adjustSetting);
            InspectorElement inspector = new InspectorElement(adjustSettingSerialized);
            inspector.Bind(adjustSettingSerialized);
            inspector.style.flexGrow = 0;
            inspector.style.paddingLeft = 0;
            root.Add(inspector);

            ScriptableObject adjustBuildConfig =
                EagleServices.GetBuildConfig<EagleBuildReflectionConfigBase>("AdjustBuildConfig");
            SerializedObject adjustBuildConfigSerialized = new SerializedObject(adjustBuildConfig);
            InspectorElement adjustBuildConfigInspector = new InspectorElement(adjustBuildConfigSerialized);
            adjustBuildConfigInspector.Bind(adjustBuildConfigSerialized);
            adjustBuildConfigInspector.style.flexGrow = 0;
            adjustBuildConfigInspector.style.paddingLeft = 0;
            root.Add(adjustBuildConfigInspector);
#else
            Label label = new Label("Eagle Analytics Sdk is not install")
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
            Button installEagleAnalytics = new Button(InstallEagleAnalyticsSDK)
            {
                text = "Install Eagle Analytics Sdk",
                style =
                {
                    marginTop = 20
                }
            };
            root.Add(installEagleAnalytics);
#endif

            HideScript(root);

            return root;
        }

        private void InstallEagleAnalyticsSDK()
        {
            string token = EagleServices.GetSetting<GeneralSetting>().SDKToken;
            if (string.IsNullOrEmpty(token))
            {
                EagleLog.Log("Nhập Token trước khi cài package");
                return;
            }

            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/EagleAnalytics.git");
        }
    }
}