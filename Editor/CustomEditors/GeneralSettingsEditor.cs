using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Eagle
{
    [CustomEditor(typeof(GeneralSetting))]
    public class GeneralSettingsEditor : EagleSettingEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            Button createProjectStructure = new Button(CreateProjectStructure.CreateFolders)
            {
                text = "Create Project Structure",
                style =
                {
                    marginTop = 20
                }
            };

            root.Add(createProjectStructure);

            // Button installAllPackage = new Button(InstallAllPackage)
            // {
            //     text = "Install All Package",
            // };
            //
            // root.Add(installAllPackage);

            HideScript(root);

            return root;
        }

        // private void InstallAllPackage()
        // {
        //     string token = EagleServices.GetToken();
        //     if (string.IsNullOrEmpty(token))
        //     {
        //         EagleLog.Log($"Nhập token trước khi cài");
        //         return;
        //     }
        //
        //     RegistryHelper.AddRegistryMAX();
        //
        //     var package = new List<string>
        //     {
        //         "com.unity.purchasing",
        //         "com.applovin.mediation.ads",
        //         AdjustSettingEditor.LinkInstall,
        //         $"https://{token}@github.com/dat-dangba/EagleAnalytics.git",
        //         $"https://{token}@github.com/dat-dangba/EagleAds.git",
        //         $"https://{token}@github.com/dat-dangba/EagleIAP.git",
        //         $"https://{token}@github.com/dat-dangba/EagleFirebaseApp.git",
        //         $"https://{token}@github.com/dat-dangba/EagleFirebaseAnalytics.git",
        //         $"https://{token}@github.com/dat-dangba/EagleFirebaseCrashlytics.git",
        //         $"https://{token}@github.com/dat-dangba/EagleFirebaseRemoteConfig.git",
        //         $"https://{token}@github.com/dat-dangba/EagleFirebase.git",
        //         $"https://{token}@github.com/dat-dangba/EagleBaseGame.git",
        //     };
        //     InstallPackageHelper.Install(package,
        //         () => { CreateAssets.CreateAsset<MAXSetting>(Constant.SettingsFolder); });
        // }
    }
}