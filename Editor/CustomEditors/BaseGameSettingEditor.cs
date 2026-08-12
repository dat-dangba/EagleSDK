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

#if HAS_BASE_GAME
            root.Add(new PackageInstalledVisualElement("Base Game"));
#else
            root.Add(new InstallPackageVisualElement("Base Game", InstallBaseGame));
#endif

#if HAS_BASE_GAME_V2
            root.Add(new PackageInstalledVisualElement("Base Game v2")
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
                    marginTop = 20
                }
            };

            root.Add(createProjectStructure);
#else
            root.Add(new InstallPackageVisualElement("Base Game v2", InstallBaseGameV2)
            {
                style =
                {
                    marginTop = 20
                }
            });
#endif
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            HideScript(root);

            return root;
        }

        private void InstallBaseGameV2()
        {
            string token = EagleServices.GetToken();
            if (string.IsNullOrEmpty(token)) return;
            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/EagleBaseGameV2.git");
        }

        private void InstallBaseGame()
        {
            string token = EagleServices.GetToken();
            if (string.IsNullOrEmpty(token)) return;
            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/EagleBaseGame.git");
        }
    }
}
