using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Node : MonoBehaviour {
    [SerializeField] private List<Node> nextNodes;
    public bool isRoundabout;
    [ShowIf("isRoundabout")] [InfoBox("Add trigger (finish line) and RoundaboutNode.cs")] public Transform roundaboutCenter;

    public Node GetNextNode() => nextNodes.Count == 0 ? null : nextNodes[Random.Range(0, nextNodes.Count)];
}
