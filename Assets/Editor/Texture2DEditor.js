//@CustomEditor(TextureImporter) 
class Texture2DEditor extends Editor {
 
    var drawDefaultInspector : boolean;
    var drawExtendedInspector : boolean;
 
    function OnInspectorGUI() {
        GUI.enabled = true;
 
        drawDefaultInspector = GUILayout.Toggle(drawDefaultInspector, "Default Inspector", EditorStyles.toolbarDropDown);
        if (drawDefaultInspector) DrawDefaultInspector(); 
        drawExtendedInspector = GUILayout.Toggle(drawExtendedInspector, "Extended Inspector", EditorStyles.toolbarDropDown);
        if (drawExtendedInspector) DrawExtendedInspector();
    }
 
    function DrawExtendedInspector() {
        GUILayout.Label("Hello OnInspectorGUI!");
    }       
}