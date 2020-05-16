using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Placementtool : EditorWindow
{
    public GameObject gameObject;

    [MenuItem("Window/Placement")]
     public static void ShowWindow()
     {  
        GetWindow<Placementtool>("Place Object");
     }

    private void OnFocus()
    {
        SceneView.duringSceneGui += Placement;
    }

    private void OnDestroy()
    {
        SceneView.duringSceneGui -= Placement;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.ObjectField(gameObject, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
    }

    public void Placement(SceneView screenView)
    {
        Event e = Event.current;
        
        if ((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && e.button == 0)
        {
            RaycastHit hit;
            //Tools.current = Tool.View;
            int layer = 1 << 16;

            if (Physics.Raycast(Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y, 0)), out hit, Mathf.Infinity, layer))
            {
                GameObject objectPlace = (GameObject)PrefabUtility.InstantiatePrefab(PrefabUtility.GetCorrespondingObjectFromSource(gameObject));

                objectPlace.transform.position = hit.point;

                e.Use();
            }

        }
    }

}