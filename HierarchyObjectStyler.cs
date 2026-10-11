using UnityEditor;
using UnityEngine;

namespace HierarchyObjectStyler
{
#if UNITY_EDITOR
    public class HierarchyObjectStyler : MonoBehaviour
    {
        // =========================
        // BACKGROUND
        // =========================
        [Header("Hierarchy Style")]
        [SerializeField]
        private Color backgroundColor = Color.cadetBlue;

        // =========================
        // TEXT STYLE
        // =========================
        [Header("Text Style")]
        [SerializeField]
        private Color textColor = Color.white;

        [SerializeField]
        private FontStyle fontStyle = FontStyle.Bold;

        [SerializeField]
        private int fontSize = 12;

        [SerializeField]
        private TextAnchor alignment = TextAnchor.MiddleLeft;

        [SerializeField]
        private TextCase textCase = TextCase.Uppercase;

        [SerializeField]
        private Vector2 offset = new(0f, 0f);

        // =========================
        // ICON
        // =========================
        [Header("Icon")]
        [SerializeField]
        private bool showIcon = true;

        [SerializeField]
        private HierarchyIconType iconType = HierarchyIconType.GameObject;

        [SerializeField]
        private float iconSize = 16f;

        [SerializeField]
        private float iconSpacing = 1f;


        static HierarchyObjectStyler()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }

        private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var obj = EditorUtility.EntityIdToObject(instanceID) as GameObject;
            if (obj == null) return;

            var settings = obj.GetComponent<HierarchyObjectStyler>();
            if (settings == null) return;

            // =========================
            // BACKGROUND
            // =========================
            Rect bgRect = new Rect(
                selectionRect.x,
                selectionRect.y,
                selectionRect.width + 50,
                selectionRect.height
            );

            EditorGUI.DrawRect(bgRect, settings.backgroundColor);

            // =========================
            // ICON
            // =========================
            float x = selectionRect.x + settings.offset.x;

            if (settings.showIcon)
            {
                Texture icon = GetIcon(settings.iconType);

                if (icon != null)
                {
                    Rect iconRect = new Rect(
                        x,
                        selectionRect.y + (selectionRect.height - settings.iconSize) * 0.5f,
                        settings.iconSize,
                        settings.iconSize
                    );

                    GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);
                    x += settings.iconSize + settings.iconSpacing;
                }
            }

            // =========================
            // TEXT
            // =========================
            Rect textRect = new Rect(
                x,
                selectionRect.y + settings.offset.y,
                selectionRect.width,
                selectionRect.height
            );

            var style = new GUIStyle(EditorStyles.label)
            {
                normal = { textColor = settings.textColor },
                fontStyle = settings.fontStyle,
                fontSize = settings.fontSize,
                alignment = settings.alignment
            };

            string displayName = ApplyTextCase(obj.name, settings.textCase);
            EditorGUI.LabelField(textRect, displayName, style);
        }

        private static Texture GetIcon(HierarchyIconType type)
        {
            return type switch
            {
                HierarchyIconType.GameObject => EditorGUIUtility.IconContent("GameObject Icon")?.image,
                HierarchyIconType.Prefab => EditorGUIUtility.IconContent("Prefab Icon")?.image,
                HierarchyIconType.Script => EditorGUIUtility.IconContent("cs Script Icon")?.image,
                HierarchyIconType.Favorite => EditorGUIUtility.IconContent("d_Favorite")?.image,
                HierarchyIconType.Play => EditorGUIUtility.IconContent("d_PlayButton")?.image,
                HierarchyIconType.Warning => EditorGUIUtility.IconContent("console.warnicon")?.image,
                HierarchyIconType.Error => EditorGUIUtility.IconContent("console.erroricon")?.image,
                _ => null
            };
        }

        private static string ApplyTextCase(string text, TextCase textCase)
        {
            return textCase switch
            {
                TextCase.Uppercase => text.ToUpperInvariant(),
                TextCase.Lowercase => text.ToLowerInvariant(),
                _ => text
            };
        }
    }
#endif

    public enum TextCase
    {
        Original,
        Uppercase,
        Lowercase
    }

    public enum HierarchyIconType
    {
        None,
        GameObject,
        Prefab,
        Script,
        Favorite,
        Play,
        Warning,
        Error
    }
}
