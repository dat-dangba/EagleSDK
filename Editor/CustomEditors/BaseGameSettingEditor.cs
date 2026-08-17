using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Eagle
{
    [CustomEditor(typeof(BaseGameSetting))]
    public class BaseGameSettingEditor : EagleSettingEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

#if HAS_AUTO_REFERENCE
            InstallBaseGame(root);
#else
            root.Add(new InstallPackageVisualElement("Auto Reference", InstallAutoReference));
#endif

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            HideScript(root);

            return root;
        }

        private void InstallBaseGame(VisualElement root)
        {
#if HAS_BASE_GAME
            root.Add(new PackageInstalledVisualElement("Base Game"));
#else
            root.Add(new InstallPackageVisualElement("Base Game", InstallBaseGame));
#endif

#if HAS_CORE
            root.Add(new PackageInstalledVisualElement("Core")
            {
                style =
                {
                    marginTop = 20
                }
            });

            Button createProjectStructure = new Button(CreateProjectStructure.CopyProjectStructure)
            {
                text = "Create Project Structure",
                style =
                {
                    marginTop = 50
                }
            };

            root.Add(createProjectStructure);
#else
            root.Add(new InstallPackageVisualElement("Core", InstallCore)
            {
                style =
                {
                    marginTop = 20
                }
            });
#endif
        }

        private void InstallAutoReference()
        {
            string token = EagleServices.GetToken();
            if (string.IsNullOrEmpty(token)) return;

            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/EagleAutoReference.git");
        }

        private void InstallCore()
        {
            string token = EagleServices.GetToken();
            if (string.IsNullOrEmpty(token)) return;

            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/EagleCore.git");
        }

        private void InstallBaseGame()
        {
            string token = EagleServices.GetToken();
            if (string.IsNullOrEmpty(token)) return;

            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/EagleBaseGame.git");
        }
    }
}
