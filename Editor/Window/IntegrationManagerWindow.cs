using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eagle
{
    public class IntegrationManagerWindow : EditorWindow
    {
        private Label currentLabel;
        private Color menuColor = new(0.15f, 0.15f, 0.15f);
        private Color contentColor = new(0.2f, 0.2f, 0.2f);
        private Label header;
        private VisualElement content;
        private const float width = 800;
        private const float height = 650;

        private string[] tabs =
            { "General", "Adjust", "MAX", "Eagle Analytics", "Eagle Ads", "Eagle IAP", "Eagle Firebase" };

        [MenuItem("Eagle/Eagle Integration Manager %#e")]
        public static void ShowWindow()
        {
            var main = EditorGUIUtility.GetMainWindowPosition();

            float x = main.x + (main.width - width) / 2f;
            float y = main.y + (main.height - height) / 2f;

            IntegrationManagerWindow window = GetWindow<IntegrationManagerWindow>("Eagle Integration Manager");
            window.position = new Rect(x, y, width, height);
            window.Show();
        }

        private void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            if (!EDM4UManager.IsEDM4UInstalled())
            {
                DrawInstallEDM4U(root);
                return;
            }

            root.style.flexDirection = FlexDirection.Column;
            root.style.backgroundColor = Color.black;

            var topPanel = GetTopPanel();
            var sdkName = GetSDKNameLabel();
            header = GetHeaderLabel();
            topPanel.Add(sdkName);
            topPanel.Add(header);
            root.Add(topPanel);

            var bottomContainer = GetBottomContainer();
            root.Add(bottomContainer);

            var menuBar = GetMenuBar();
            bottomContainer.Add(menuBar);

            content = GetContentContainer();
            bottomContainer.Add(content);

            ShowConfig(currentLabel.text);
        }

        private void ShowConfig(string labelText)
        {
            if (header != null)
            {
                header.text = labelText;
            }

            content.Clear();
            switch (labelText)
            {
                case "General":
                    DrawSetting<GeneralSetting>();
                    break;
                case "Adjust":
                    DrawSetting<AdjustSetting>();
                    break;
                case "MAX":
                    DrawMAXSettings();
                    break;
                case "Eagle Analytics":
                    DrawEagleAnalytics();
                    break;
                case "Eagle Ads":
                    DrawEagleAds();
                    break;
                case "Eagle IAP":
                    DrawEagleIAP();
                    break;
                case "Eagle Firebase":
                    DrawEagleFirebase();
                    break;
            }
        }

        #region EDM4U

        private void DrawInstallEDM4U(VisualElement root)
        {
            Label label = new Label("EDM4U is not installed.")
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
            Button installMediatedNetworksButton = new Button(EDM4UManager.InstallEDM4U)
            {
                text = "Install EDM4U",
                style =
                {
                    marginTop = 20
                }
            };
            root.Add(installMediatedNetworksButton);
        }

        #endregion

        #region MAXSetting

        private void DrawMAXSettings()
        {
#if HAS_MAX_SDK
            DrawSetting<MAXSetting>();
#else
            content.Add(new InstallPackageVisualElement("MAX Sdk", InstallMAXSdk));
#endif
        }

        private void InstallMAXSdk()
        {
            AddRegistry();
            InstallPackageHelper.Install("com.applovin.mediation.ads",
                () => { CreateAssets.CreateAsset<MAXSetting>(Constant.SettingsFolder); });
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

        #endregion

        #region EagleAnalytics

        private void DrawEagleAnalytics()
        {
#if HAS_EAGLE_ANALYTICS
            DrawSetting<EagleAnalyticsSetting>();
#elif !HAS_ADJUST_SDK
            DrawSetting<AdjustSetting>();
#else
            content.Add(new InstallPackageVisualElement("Eagle Analytics Sdk", InstallEagleAnalyticsSDK));
#endif
        }

        private void InstallEagleAnalyticsSDK()
        {
            string token = EagleServices.GetToken();
            if (string.IsNullOrEmpty(token)) return;
            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/EagleAnalytics.git");
        }

        #endregion

        #region EagleAds

        private void DrawEagleAds()
        {
#if !HAS_MAX_SDK
            DrawMAXSettings();
#elif !HAS_EAGLE_ADS
            content.Add(new InstallPackageVisualElement("Eagle Ads Sdk", InstallEagleAds));
#else
            DrawSetting<EagleAdsSetting>();
#endif
        }

        private void InstallEagleAds()
        {
            string token = EagleServices.GetToken();
            if (string.IsNullOrEmpty(token)) return;
            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/EagleAds.git");
        }

        #endregion

        #region EagleIAP

        private void DrawEagleIAP()
        {
#if HAS_EAGLE_IAP
            DrawSetting<EagleIAPSetting>();
#else
            content.Add(new InstallPackageVisualElement("Eagle IAP Sdk", InstallEagleIAP));
#endif
        }

        private void InstallEagleIAP()
        {
            string token = EagleServices.GetToken();
            if (string.IsNullOrEmpty(token)) return;
            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/EagleIAP.git");
        }

        #endregion

        #region EagleFirebase

        private void DrawEagleFirebase()
        {
#if HAS_EAGLE_FIREBASE
            DrawSetting<EagleFirebaseSetting>();
#else
            content.Add(new InstallPackageVisualElement("Eagle Firebase Sdk", InstallEagleFirebase));
#endif
        }

        private void InstallEagleFirebase()
        {
            string token = EagleServices.GetToken();
            if (string.IsNullOrEmpty(token)) return;

            var packages = new List<string>()
            {
                $"https://{token}@github.com/dat-dangba/EagleFirebase.git",
                $"https://{token}@github.com/dat-dangba/EagleFirebaseApp.git",
                $"https://{token}@github.com/dat-dangba/EagleFirebaseAnalytics.git",
                $"https://{token}@github.com/dat-dangba/EagleFirebaseCrashlytics.git"
            };
            InstallPackageHelper.Install(packages);
        }

        #endregion

        #region UI Window

        private Label GetHeaderLabel()
        {
            Label header = new Label("General")
            {
                style =
                {
                    fontSize = 25,
                    height = 50,
                    paddingLeft = 20,
                    paddingBottom = 5,
                    unityTextAlign = TextAnchor.LowerLeft,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            };
            return header;
        }

        private static Label GetSDKNameLabel()
        {
            Label sdkName = new Label("Eagle Integration Manager")
            {
                style =
                {
                    fontSize = 13,
                    height = 50,
                    width = 250,
                    paddingLeft = 20,
                    unityTextAlign = TextAnchor.MiddleLeft,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            };
            return sdkName;
        }

        private VisualElement GetContentContainer()
        {
            VisualElement contentContainer = new VisualElement
            {
                style =
                {
                    paddingTop = 10,
                    paddingBottom = 10,
                    paddingLeft = 20,
                    paddingRight = 20,
                    marginLeft = 1,
                    flexGrow = 1,
                    backgroundColor = contentColor
                }
            };
            return contentContainer;
        }

        private VisualElement GetMenuBar()
        {
            VisualElement menuBar = new VisualElement
            {
                style =
                {
                    width = 250,
                    backgroundColor = menuColor
                }
            };


            for (int i = 0; i < tabs.Length; i++)
            {
                string tabName = tabs[i];
                var label = GetLabel(tabName);
                if (i == 0)
                {
                    currentLabel = label;
                }

                label.RegisterCallback<ClickEvent>(evt =>
                {
                    if (currentLabel != null)
                    {
                        currentLabel.style.backgroundColor = menuColor;
                    }

                    label.style.backgroundColor = contentColor;
                    currentLabel = label;

                    ShowConfig(currentLabel.text);
                });
                menuBar.Add(label);
            }

            return menuBar;
        }

        private Label GetLabel(string tabName)
        {
            Label label = new Label(tabName)
            {
                style =
                {
                    width = Length.Percent(100),
                    height = 40,
                    paddingLeft = 20,
                    unityTextAlign = TextAnchor.MiddleLeft,
                    color = Color.white,
                    backgroundColor = tabName == "General" ? contentColor : menuColor
                }
            };
            return label;
        }

        private static VisualElement GetBottomContainer()
        {
            VisualElement bottomContainer = new VisualElement
            {
                style =
                {
                    marginTop = 1,
                    flexGrow = 1,
                    flexDirection = FlexDirection.Row
                }
            };
            return bottomContainer;
        }

        private VisualElement GetTopPanel()
        {
            VisualElement topPanel = new VisualElement
            {
                style =
                {
                    height = 50,
                    backgroundColor = contentColor,
                    flexDirection = FlexDirection.Row
                }
            };
            return topPanel;
        }

        private void DrawSetting<T>() where T : EagleEditorSettingBase
        {
            T setting = EagleServices.GetSetting<T>();
            SerializedObject settingSerialized = new SerializedObject(setting);
            InspectorElement settingInspector = new InspectorElement(settingSerialized);
            settingInspector.Bind(settingSerialized);
            settingInspector.style.paddingLeft = 0;
            settingInspector.style.paddingRight = 0;
            settingInspector.style.paddingTop = 0;
            settingInspector.style.paddingBottom = 0;
            content.Add(settingInspector);
        }

        #endregion
    }
}