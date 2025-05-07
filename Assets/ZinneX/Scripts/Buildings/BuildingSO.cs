using UnityEngine;

[System.Serializable]
public class BuildingSO : ScriptableObject {
    public string buildingName;
    [TextArea(3,4)] public string description;
    public int price;
}
