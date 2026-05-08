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

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            HideScript(root);

            return root;
        }

        private void InstallBaseGame()
        {
            string token = EagleServices.GetToken();
            if (string.IsNullOrEmpty(token)) return;
            InstallPackageHelper.Install($"https://{token}@github.com/dat-dangba/EagleBaseGame.git");
        }
    }
}