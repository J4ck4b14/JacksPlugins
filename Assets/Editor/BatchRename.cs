using UnityEditor;
using UnityEngine;

public class BatchRename : EditorWindow
{
    // Variables
    private string prefix = "";
    private string suffix = "";
    private string baseName = "";

    private bool numbered = false;
    private int startingNum = 1;

    private bool trimFront = false;
    private bool trimBack = false;
    private int frontTrim = 0;
    private int backTrim = 0;

    private int repetitionAmount = 0;
    private bool solidarity;
    private string[] prefixes;
    private string[] names;
    private string[] suffixes;

    private GameObject[] selectedObjects = null;

    [MenuItem("Tools/BatchRenamer")]
    public static void ShowWindow()
    {
        GetWindow<BatchRename>("BatchRenamer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Batch Rename Settings", EditorStyles.boldLabel);

        prefix = EditorGUILayout.TextField("Prefix", prefix);
        baseName = EditorGUILayout.TextField("Name", baseName);
        suffix = EditorGUILayout.TextField("Suffix", suffix);

        numbered = EditorGUILayout.Toggle("Numbered", numbered);
        if (numbered)
        {
            startingNum = EditorGUILayout.IntField("Start from", startingNum);
        }

        trimFront = EditorGUILayout.Toggle("Trim the end?", trimFront);
        if (trimFront)
        {
            frontTrim = EditorGUILayout.IntField("Number of characters to trim", frontTrim);
        }

        trimBack = EditorGUILayout.Toggle("Trim the beggining?", trimBack);
        if (trimBack)
        {
            backTrim = EditorGUILayout.IntField("Number of characters to trim", backTrim);
        }

        if (GUILayout.Button("Apply Changes"))
        {
            RenameObjects();
        }

        /* I have a project here to make a pattern renamer, but it's a bit more difficult than anticipated
        GUILayout.Space(10);
        GUILayout.Label("Pattern maker", EditorStyles.boldLabel);
        
        repetitionAmount = EditorGUILayout.IntField("Number of variants", repetitionAmount);
        solidarity = EditorGUILayout.Toggle("Share base name?", solidarity);
        if (solidarity) names[0] = EditorGUILayout.TextField("Base name", names[0]);
        for (int i = 0; i < repetitionAmount; i++)
        {
            prefixes[i] = EditorGUILayout.TextField("Prefix" + (i+1), prefixes[i]);
            if (!solidarity)
            {
                names[i] = EditorGUILayout.TextField("Name" + (i + 1), names[i]);
            }
            else names[i] = names[0];
            suffixes[i] = EditorGUILayout.TextField("Suffix" + (i+1), suffixes[i]);
        }

        if (GUILayout.Button("Apply pattern"))
        {
            PatternObjects();
        }

        */
    }

    private void RenameObjects()
    {
        selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length <= 0)
        {
            Debug.LogWarning("No GameObjects selected. Please, select at least one GameObjects to rename.");
            return;
        }

        for (int i = 0; i < selectedObjects.Length; i++)
        {
            string trimedFrontName = "";
            if (trimFront)
            {
                trimedFrontName = selectedObjects[i].name.Substring(0, selectedObjects[i].name.Length - backTrim);
                Undo.RecordObject(selectedObjects[i], trimedFrontName);
                selectedObjects[i].name = trimedFrontName;
            }

            string trimedBackName = "";
            {
                if (trimBack) trimedBackName = selectedObjects[i].name.Substring(frontTrim);
                Undo.RecordObject(selectedObjects[i], trimedBackName);
                selectedObjects[i].name = trimedBackName;
            }

            // The $ symbol makes the string interpolated so the parts between brackets {} are replaced
            string newName = $"{prefix}{(string.IsNullOrEmpty(baseName) ? selectedObjects[i].name : baseName)}{suffix}";

            if (numbered) newName += $"_{startingNum + i}";

            Undo.RecordObject(selectedObjects[i], newName);
            selectedObjects[i].name = newName;
        }

        Debug.Log($"Renamed {selectedObjects.Length} GameObjects");
    }

    private void PatternObjects()
    {
        selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length <= 0)
        {
            Debug.LogWarning("No GameObjects selected. Please, select at least one GameObjects to rename.");
            return;
        }

        int j = -1;
        for (int i = 0; i < selectedObjects.Length; i++)
        {
            if (j++ >= repetitionAmount) j = 0;
            else j++;

            string newName = $"{prefixes[j]}{(string.IsNullOrEmpty(baseName) ? selectedObjects[i].name : names[j])}{suffixes[j]}";

            Undo.RecordObject(selectedObjects[i], newName);
            selectedObjects[i].name = newName;

        }

        Debug.Log($"Renamed {selectedObjects.Length} GameObjects");
    }

}

/* FOR FUTURE REFERENCE
IF THE BASE NAME IS EMPTY, DON'T MODIFY IT
PATTERN MAKER
*/
