using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
#if HAS_MAX_SDK
using AppLovinMax.Scripts.IntegrationManager.Editor;
#endif

namespace Eagle
{
    [CustomEditor(typeof(MAXSetting))]
    public class MaxSettingEditor : EagleSettingEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            var defaultInspector = new VisualElement();
            InspectorElement.FillDefaultInspector(defaultInspector, serializedObject, this);
            root.Add(defaultInspector);

            Button installMediatedNetworksButton = new Button(InstallMediatedNetworks)
            {
                text = "Install Mediated Networks"
            };
            root.Add(installMediatedNetworksButton);

            Button setupMaxSdkButton = new Button
            {
                text = "Setup MAX Sdk"
            };
            setupMaxSdkButton.RegisterCallback<ClickEvent>(evt => { SetupMaxSdk(); });
            root.Add(setupMaxSdkButton);

            HideScript(root);

            return root;
        }

        private void InstallMediatedNetworks()
        {
#if HAS_MAX_SDK
            EagleLog.Log($"Đang bắt đầu cài đặt MAX Mediated Networks ...", LogLevel.Verbose);
            MAXMediatedNetworks mediatedNetworks =
                Resources.Load<MAXMediatedNetworks>($"{Constant.SettingsFolder}/MAXMediatedNetworks");
            InstallPackageHelper.Install(mediatedNetworks.GetAllPackages(),
                () => { EagleLog.Log("Đã cài đặt xong tất cả các Mediated Networks"); });
#endif
        }

        private void SetupMaxSdk()
        {
#if HAS_MAX_SDK
            serializedObject.Update();
            AppLovinSettings settings = AppLovinSettings.Instance;
            settings.SdkKey = serializedObject.FindProperty("MaxSdkKey").stringValue;
            settings.AdMobAndroidAppId = serializedObject.FindProperty("GoogleAdmobAppId").stringValue;
            settings.AdMobIosAppId = serializedObject.FindProperty("GoogleAdmobAppId").stringValue;

            AppLovinInternalSettings.Instance.ConsentFlowEnabled =
                serializedObject.FindProperty("ConsentFlowEnabled").boolValue;

            AppLovinInternalSettings.Instance.ConsentFlowPrivacyPolicyUrl =
                serializedObject.FindProperty("ConsentFlowPrivacyPolicyUrl").stringValue;

            MaxSdkBase.ConsentFlowUserGeography value =
                (MaxSdkBase.ConsentFlowUserGeography)serializedObject.FindProperty("DebugUserGeography").enumValueIndex;
            AppLovinInternalSettings.Instance.DebugUserGeography = value;

            AppLovinInternalSettings.Instance.Save();
            EditorUtility.SetDirty(AppLovinInternalSettings.Instance);
            AssetDatabase.SaveAssets();
#endif
        }
    }
}