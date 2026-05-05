using System;
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
            { "General", "Adjust", "MAX", "Eagle Analytics", "Eagle Ads", "Eagle IAP" };

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
            }
        }

        private void DrawEagleIAP()
        {
#if HAS_EAGLE_IAP
            DrawSetting<EagleIAPSetting>();
#else
            InstallPackage(
                "Eagle IAP Sdk is not installed.",
                "Install Eagle IAP Sdk",
                () =>
                {
                    string token = EagleServices.GetSetting<GeneralSetting>().SDKToken;
                    if (string.IsNullOrEmpty(token))
                    {
                        EagleLog.Log("Nhập Token trước khi cài package");
                        return;
                    }

                    InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/EagleIAP.git");
                });
#endif
        }

        private void DrawEagleAds()
        {
#if !HAS_MAX_SDK
            DrawMAXSettings();
#elif !HAS_EAGLE_ADS
            InstallPackage(
                "Eagle Ads Sdk is not installed.",
                "Install Eagle Ads Sdk",
                () =>
                {
                    string token = EagleServices.GetSetting<GeneralSetting>().SDKToken;
                    if (string.IsNullOrEmpty(token))
                    {
                        EagleLog.Log("Nhập Token trước khi cài package");
                        return;
                    }

                    InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/EagleAds.git");
                });
#else
            DrawSetting<EagleAdsSetting>();
#endif
        }

        private void DrawMAXSettings()
        {
#if HAS_MAX_SDK
            DrawSetting<MAXSetting>();
#else
            InstallPackage(
                "MAX Sdk is not installed.",
                "Install MAX Sdk",
                () =>
                {
                    AddRegistry();
                    InstallPackageHelper.Install("com.applovin.mediation.ads@8.6.2",
                        () => { CreateAssets.CreateAsset<MAXSetting>(Constant.SettingsFolder); });
                });
#endif
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

        private void DrawEagleAnalytics()
        {
#if HAS_EAGLE_ANALYTICS
            DrawSetting<EagleAnalyticsSetting>();
#elif !HAS_ADJUST_SDK
            DrawSetting<AdjustSetting>();
#else
            InstallPackage(
                "Eagle Analytics Sdk is not installed.",
                "Install Eagle Analytics Sdk",
                InstallEagleAnalyticsSDK);
#endif
        }

        private void InstallPackage(string mess, string textButton, Action clickEvent)
        {
            Label label = new Label(mess)
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
            content.Add(label);
            Button installEagleAnalytics = new Button(clickEvent)
            {
                text = textButton,
                style =
                {
                    marginTop = 20
                }
            };
            content.Add(installEagleAnalytics);
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
    }
}