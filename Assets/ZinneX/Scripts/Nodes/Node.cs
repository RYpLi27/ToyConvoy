using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Node : MonoBehaviour {
    [BoxGroup("Nodes")] [SerializeField] private List<Node> nextNodes;
    [Space(10)]
    [BoxGroup("Nodes")] [InfoBox("Can leave empty if this node is not in roadblock path")]
    [SerializeField] private List<Node> backtrackNodes;
    
    [Space(10)]
    public bool isRoundabout;
    [ShowIf("isRoundabout")] [InfoBox("Add trigger (finish line) and RoundaboutNode.cs")] 
    public Transform roundaboutCenter;

    public Node GetNextNode() => nextNodes.Count == 0 ? null : nextNodes[Random.Range(0, nextNodes.Count)];
    
    public Node GetBacktrackNode() => backtrackNodes.Count == 0 ? null : backtrackNodes[Random.Range(0, backtrackNodes.Count)];
}
