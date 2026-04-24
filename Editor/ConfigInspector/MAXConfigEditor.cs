using System.Collections.Generic;
using System.IO;
#if HAS_MAX_SDK
using AppLovinMax.Scripts.IntegrationManager.Editor;
#endif
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eagle
{
    [CustomEditor(typeof(MAXConfig))]
    public class MAXConfigEditor : EagleConfigEditor
    {
        private const string Tag = "[SetupMAXSdk]";
        private AddRequest addRequest;
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
            if (IsMAXInstalled())
            {
                var defaultInspector = new VisualElement();
                InspectorElement.FillDefaultInspector(defaultInspector, serializedObject, this);
                root.Add(defaultInspector);

                Button button = new Button()
                {
                    text = "Setup MAX Sdk"
                };
                button.RegisterCallback<ClickEvent>(evt =>
                {
#if HAS_MAX_SDK
                    AppLovinSettings settings = AppLovinSettings.Instance;
                    settings.SdkKey = serializedObject.FindProperty("MaxSdkKey").stringValue;

                    AppLovinInternalSettings.Instance.ConsentFlowEnabled =
                        serializedObject.FindProperty("ConsentFlowEnabled").boolValue;

                    AppLovinInternalSettings.Instance.ConsentFlowPrivacyPolicyUrl =
                        serializedObject.FindProperty("ConsentFlowPrivacyPolicyUrl").stringValue;
#endif
                });
                root.Add(button);
            }
            else
            {
                DrawInstallMAX(root);
            }
        }

        private static bool IsMAXInstalled()
        {
            return Directory.Exists($"Packages/com.applovin.mediation.ads");
        }

        private void DrawInstallMAX(VisualElement root)
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
            Button installSdkButton = new Button
            {
                text = "Install MAX Sdk",
                style =
                {
                    marginTop = 20
                }
            };
            installSdkButton.RegisterCallback<ClickEvent>(evt =>
            {
                AddRegistry();
                InstallPackage();
            });
            root.Add(installSdkButton);
        }

        private void InstallPackage()
        {
            Debug.Log($"{Tag} - Đang bắt đầu cài đặt MAX ...");
            addRequest = Client.Add($"com.applovin.mediation.ads@8.6.2");
            EditorApplication.update += Progress;
        }

        private void Progress()
        {
            if (!addRequest.IsCompleted) return;

            switch (addRequest.Status)
            {
                case StatusCode.Success:
                    Debug.Log($"{Tag} - Cài đặt MAX thành công: " + addRequest.Result.packageId);
                    break;
                case >= StatusCode.Failure:
                    Debug.LogError($"{Tag} - Cài đặt MAX thất bại: " + addRequest.Error.message);
                    break;
            }

            EditorApplication.update -= Progress;
        }

        private void AddRegistry()
        {
            string manifestPath = Path.Combine(Application.dataPath, "..", "Packages", "manifest.json");
            if (!File.Exists(manifestPath))
            {
                Debug.Log($"{Tag} - Không tìm thấy file {manifestPath}");
                return;
            }

            string content = File.ReadAllText(manifestPath);
            if (content.Contains("com.applovin.mediation.ads"))
            {
                return;
            }

            UnityManifest manifest = JsonConvert.DeserializeObject<UnityManifest>(content);
            manifest.scopedRegistries ??= new List<ScopedRegistry>();

            var openUPM = new ScopedRegistry
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

            manifest.scopedRegistries.Add(openUPM);

            string newJson = JsonConvert.SerializeObject(manifest, Formatting.Indented);
            File.WriteAllText(manifestPath, newJson);

            Debug.Log($"{Tag} - Đã thêm MAX Registry thành công!");
            AssetDatabase.Refresh();
        }
    }
}