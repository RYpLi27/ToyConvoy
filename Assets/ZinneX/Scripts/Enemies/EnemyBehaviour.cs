using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    [SerializeField] [InfoBox("To see values click on pen at the right side")] private EnemySO enemySO;
    private int currentNodeIndex;
    private Transform currentNode;
    private Vector3 nodeOffset;

    public Vector3 MoveDir => (currentNode.position + nodeOffset - transform.position).normalized;
    
    private void Start() {
        FirstNodeAndOffset();
        
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        if (currentNode == null) return;
        
        transform.position = Vector3.MoveTowards(transform.position, currentNode.position + nodeOffset, Time.fixedDeltaTime * enemySO.moveSpeed);
        
        if(Vector3.Distance(transform.position, currentNode.position + nodeOffset) <= .1f) FindNextNode();
    }

    private void FirstNodeAndOffset() {
        currentNodeIndex = -1;
        FindNextNode();
        nodeOffset = transform.position - currentNode.position;
    }
    
    private void FindNextNode() { // THIS WILL DISABLE GAME OBJECT WHEN FINAL NODE IS REACHED
        currentNode = EnemyPathManager.instance.GetNode(++currentNodeIndex, gameObject);
    }

    private void OnDisable() {
        foreach (TurretBehaviour turret in FindObjectsByType<TurretBehaviour>(FindObjectsSortMode.None)) {
            turret.EnemyDeactivated(transform);
        }
        print("reached final node");
    }
}
