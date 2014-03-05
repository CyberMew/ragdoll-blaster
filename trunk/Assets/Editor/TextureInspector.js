class TextureInspector extends EditorWindow {
    var scrollView : Vector2;
 
    @MenuItem("Window/Texture Inspector")
    static function OpenWindow() {
        var window : TextureInspector = EditorWindow.GetWindow (TextureInspector);
        window.title = "T2D Inspector";
    }
 
    function OnGUI() {
        var textures = Selection.GetFiltered(Texture2D, SelectionMode.Assets);
        scrollView = GUILayout.BeginScrollView(scrollView);
        GUILayout.Label ("Selected Textures", EditorStyles.boldLabel);
        for (var texture : Texture2D in textures) {
            EditorGUILayout.InspectorTitlebar(true, texture);
            DrawInspector(texture);
        }
        GUILayout.EndScrollView();
        Repaint();
    }
 
    function DrawInspector(texture : Texture2D) {
        GUILayout.Label("Add your custom inspection here");
    }
}