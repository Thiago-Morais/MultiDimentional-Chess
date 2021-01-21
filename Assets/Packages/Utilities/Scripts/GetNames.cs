#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;

public class GetNames
{
    [MenuItem("Tools/" + nameof(GetObjectsNames))]
    static void GetObjectsNames()
    {
        string[] objectsNames = GetNamesFrom(Selection.gameObjects);
        string selectedNames = string.Join("\n", objectsNames);
        GUIUtility.systemCopyBuffer = selectedNames;
        Debug.Log($"Putted on clipboard:{selectedNames}");
    }
    static string[] GetNamesFrom(GameObject[] selectedObjects) => selectedObjects.Select(obj => obj.name).ToArray();
}
#endif //UNITY_EDITOR