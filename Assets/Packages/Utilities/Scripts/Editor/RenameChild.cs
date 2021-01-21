#if UNITY_EDITOR
using TMPro;
using UnityEditor;
using UnityEngine;

public class RenameChild : MonoBehaviour
{
    [MenuItem("CONTEXT/Transform/Rename")]
    public static void Rename(MenuCommand command)
    {
        foreach (TextMeshProUGUI text in (command.context as Transform).GetComponentsInChildren<TextMeshProUGUI>())
            text.text = text.transform.name;
    }
}
#endif //UNITY_EDITOR