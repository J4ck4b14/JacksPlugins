using UnityEngine;
using UnityEditor;

public class ProceduralStructureEditor : EditorWindow
{
    private Vector3 startPoint;
    private Vector3 endPoint;
    private bool isDrawing = false;

    private GameObject selectedPrefab;
    private float objectSpacing = 1.0f;


    [MenuItem("Tools/Procedural Structure Editor")]
    public static void ShowWindow()
    {
        GetWindow<ProceduralStructureEditor>("Procedural Structure Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Procedural Structure Settings", EditorStyles.boldLabel);

        // Add your GUI elements here
        GUILayout.Label("Procedural Structure Settings", EditorStyles.boldLabel);
        selectedPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab", selectedPrefab, typeof(GameObject), false);
        objectSpacing = EditorGUILayout.FloatField("Spacing", objectSpacing);
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            if (!isDrawing)
            {
                startPoint = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
                isDrawing = true;
            }
            else
            {
                endPoint = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
                isDrawing = false;

                // Create the procedural structure here
            }

            e.Use();
        }

        if (isDrawing)
        {
            Handles.DrawLine(startPoint, HandleUtility.GUIPointToWorldRay(e.mousePosition).origin);
            SceneView.RepaintAll();
        }
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void CreateProceduralStructure()
    {
        float distance = Vector3.Distance(startPoint, endPoint);
        int numberOfObjects = Mathf.CeilToInt(distance / objectSpacing);

        for (int i = 0; i < numberOfObjects; i++)
        {
            float t = i / (float)(numberOfObjects - 1);
            Vector3 position = Vector3.Lerp(startPoint, endPoint, t);
            Instantiate(selectedPrefab, position, Quaternion.identity);
        }
    }
}
