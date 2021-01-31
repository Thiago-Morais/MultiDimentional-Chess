using UnityEngine;

[CreateAssetMenu(menuName = nameof(ScriptableObject) + "/" + nameof(GameSettings))]
public class GameSettings : ScriptableObject
{
    public float hoverSensitivity;
}