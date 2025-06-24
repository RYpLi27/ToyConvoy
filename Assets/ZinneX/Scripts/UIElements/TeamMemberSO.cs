using UnityEngine;

[CreateAssetMenu(fileName = "TeamMemberSO", menuName = "Scriptable Objects/TeamMemberSO")]
public class TeamMemberSO : ScriptableObject {
    public string fullName;
    [TextArea(3,5)] public string description;
    public Sprite sprite;
}
