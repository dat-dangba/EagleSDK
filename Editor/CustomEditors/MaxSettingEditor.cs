using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
#if HAS_MAX_SDK
using AppLovinMax.Scripts.IntegrationManager.Editor;
#endif

namespace Eagle
{
    [CustomEditor(typeof(MAXSetting))]
    public class MaxSettingEditor : EagleSettingEditor
    {
        private const string Tag = "[SetupMAXSdk]";
        private const string MAXPackage = "com.applovin.mediation.ads@8.6.2";
        private VisualElement root;

        public override VisualElement CreateInspectorGUI()
        {
            root = new VisualElement();
            DrawInspector();

            HideScript(root);

            return root;
        }

        private void DrawInspector()
        {
            root.Clear();
            if (!EDM4UManager.IsEDM4UInstalled())
            {
                DrawInstallEDM4U();
                return;
            }

            if (!IsMAXInstalled())
            {
                DrawInstallMAX();
                return;
            }

            DrawMAXSetting();
        }

        private void DrawInstallEDM4U()
        {
            Label label = new Label("EDM4U is not install")
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
            Button installMediatedNetworksButton = new Button(EDM4UManager.ShowDialogSetupEDM4U)
            {
                text = "Install EDM4U",
                style =
                {
                    marginTop = 20
                }
            };
            root.Add(installMediatedNetworksButton);
        }

        private void DrawMAXSetting()
        {
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
        }

        private void InstallMediatedNetworks()
        {
#if HAS_MAX_SDK
            Debug.Log($"{Tag} - Đang bắt đầu cài đặt MAX Mediated Networks ...");
            MAXMediatedNetworks mediatedNetworks =
                Resources.Load<MAXMediatedNetworks>($"{Constant.SettingsFolder}/MAXMediatedNetworks");
            InstallPackageHelper.Install(mediatedNetworks.GetAllPackages(),
                () => { EagleLog.Log("Đã cài đặt xong tất cả các Mediated Networks"); });
#endif
        }

        private void SetupMaxSdk()
        {
#if HAS_MAX_SDK
            AppLovinSettings settings = AppLovinSettings.Instance;
            settings.SdkKey = serializedObject.FindProperty("MaxSdkKey").stringValue;
            settings.AdMobAndroidAppId = serializedObject.FindProperty("GoogleAdmobAppId").stringValue;
            settings.AdMobIosAppId = serializedObject.FindProperty("GoogleAdmobAppId").stringValue;

            AppLovinInternalSettings.Instance.ConsentFlowEnabled =
                serializedObject.FindProperty("ConsentFlowEnabled").boolValue;

            AppLovinInternalSettings.Instance.ConsentFlowPrivacyPolicyUrl =
                serializedObject.FindProperty("ConsentFlowPrivacyPolicyUrl").stringValue;
#endif
        }

        private static bool IsMAXInstalled()
        {
            return Directory.Exists($"Packages/com.applovin.mediation.ads");
        }

        private void DrawInstallMAX()
        {
            Label label = new Label("MAX Sdk is not install")
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
            Button installSdkButton = new Button(() =>
            {
                AddRegistry();
                InstallMAX();
            })
            {
                text = "Install MAX Sdk",
                style =
                {
                    marginTop = 20
                }
            };
            root.Add(installSdkButton);
        }

        private void InstallMAX()
        {
            Debug.Log($"{Tag} - Đang bắt đầu cài đặt MAX ...");

            InstallPackageHelper.Install(MAXPackage, InstallMaxCompleted);
        }

        private void InstallMaxCompleted()
        {
            MAXMediatedNetworks mediatedNetworks =
                CreateAssets.CreateAsset<MAXMediatedNetworks>(Constant.SettingsFolder);
            mediatedNetworks.CreateMediatedNetworks();
        }

        private void AddRegistry()
        {
            var maxRegistry = new ScopedRegistry
            {
                name = "AppLovin MAX Unity",
                url = "https://unity.packages.applovin.com",
                scopes = new List<string>
                {
                    "com.applovin.mediation.ads",
                    "com.applovin.mediation.adapters",
                    "com.applovin.mediation.dsp"
                }
            };
            RegistryHelper.AddRegistry(maxRegistry);
        }
    }
}