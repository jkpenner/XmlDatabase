using UnityEngine;
using UnityEditor;

namespace UtilitySystem.XmlDatabase.Editor {
    public abstract class XmlDatabaseWindowComplex<DatabaseAssetType> : EditorWindow where DatabaseAssetType : class, IXmlDatabaseAsset {

        protected abstract AbstractXmlDatabase<DatabaseAssetType> GetDatabaseInstance();
        protected abstract DatabaseAssetType CreateNewDatabaseAsset();

        private Vector2 selectorScroll;

        private int _selectorIndex = -1;
        public int SelectedId {
            get {
                return _selectorIndex;
            }
            set {
                if (_selectorIndex != value) {
                    _selectorIndex = value;
                    EditorGUI.FocusTextInControl(string.Empty);
                }
            }
        }

        protected abstract void DisplayAssetGUI(DatabaseAssetType asset);

        private GUIStyle toggleButtonStyle;
        public virtual GUIStyle ToggleButtonStyle {
            get {
                if (toggleButtonStyle == null) {
                    // Create custom style for stat buttons
                    toggleButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
                    toggleButtonStyle.alignment = TextAnchor.MiddleLeft;
                }
                return toggleButtonStyle;
            }
        }

        private void OnGUI() {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Width(200));
            DisplaySelector();
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            DisplayContent();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            DisplayFooter();
        }

        protected virtual void OnDisplayDatabaseAssets(int selectedIndex) {
            var database = GetDatabaseInstance();

            foreach (var asset in database.GetAssets()) {
                bool isVisible = GUILayout.Toggle(asset.Id == SelectedId,
                    string.Format("[{0}]: {1}", asset.Id.ToString("D4"), 
                    (string.IsNullOrEmpty(asset.Name) ? "Unassigned Object" : asset.Name)),
                    ToggleButtonStyle);

                if (SelectedId == asset.Id || isVisible) {
                    SelectedId = asset.Id;
                }
            }
        }

        protected virtual void OnDisplayFooter() {
            GUILayout.FlexibleSpace();

            GUILayout.Label(string.Format("Assets: {0}",
                GetDatabaseInstance().GetCount().ToString("D3")),
                EditorStyles.centeredGreyMiniLabel);
        }

        private void DisplaySelector() {
            // Scroll view for the listed assets
            selectorScroll = GUILayout.BeginScrollView(selectorScroll, false, true);

            // List all the assets in the database
            GUILayout.BeginVertical("Box", GUILayout.ExpandWidth(true));

            OnDisplayDatabaseAssets(SelectedId);

            if (GetDatabaseInstance().GetCount() <= 0) {
                GUILayout.Label("No Assets in Database\nPress '+' to add an asset", EditorStyles.centeredGreyMiniLabel);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUILayout.EndScrollView();
        }

        private void DisplayContent() {
            GUILayout.BeginVertical();
            var asset = GetDatabaseInstance().GetWithId(SelectedId);
            if (asset != null) {
                DisplayAssetGUI(asset);
            }
            GUILayout.EndVertical();
        }

        private void DisplayFooter() {
            // Show the add and remove selected buttons
            GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(88))) {
                GetDatabaseInstance().Add(CreateNewDatabaseAsset());
                SelectedId = GetDatabaseInstance().GetCount() - 1;
            }
            if (GUILayout.Button("-", EditorStyles.toolbarButton, GUILayout.Width(88))) {
                if (SelectedId >= 0 && SelectedId < GetDatabaseInstance().GetCount()) {
                    GetDatabaseInstance().Remove(SelectedId--);
                    if (SelectedId == -1 && GetDatabaseInstance().GetCount() > 0) {
                        SelectedId = 0;
                    }
                }
            }

            OnDisplayFooter();

            GUILayout.EndHorizontal();

            GUILayout.Space(2);
        }
    }
}
