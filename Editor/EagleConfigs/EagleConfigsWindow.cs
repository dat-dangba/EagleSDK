#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eagle
{
    public class EagleConfigsWindow : EditorWindow
    {
        private Label currentLabel;
        private Color menuColor = new(0.15f, 0.15f, 0.15f);
        private Color contentColor = new(0.2f, 0.2f, 0.2f);
        private Label header;
        private VisualElement content;

        private string[] tabs = { "General", "MAX" };

        [MenuItem("Eagle/Eagle Configs Manager")]
        public static void ShowWindow()
        {
            GetWindow<EagleConfigsWindow>("Eagle Configs Manager");
        }

        private void CreateGUI()
        {
            VisualElement root = rootVisualElement;
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

            ScriptableObject scriptableObject = null;
            switch (labelText)
            {
                case "General":
                    scriptableObject = EagleConfigService.GetConfig<GeneralConfig>();
                    break;
                case "MAX":
                    scriptableObject = EagleConfigService.GetConfig<MAXConfig>();
                    break;
            }

            if (scriptableObject == null) return;

            content.Clear();
            SerializedObject serialized = new SerializedObject(scriptableObject);
            InspectorElement inspector = new InspectorElement(serialized);
            inspector.Bind(serialized);
            content.Add(inspector);
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
            Label sdkName = new Label("Eagle Configs Manager")
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
#endif