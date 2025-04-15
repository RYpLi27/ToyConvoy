using System.Collections;
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

    private void Update() {
        Move();

        Rotate();
    }

    private void Move() {
        if (currentNode == null) return;
        
        transform.position = Vector3.MoveTowards(transform.position, currentNode.position + nodeOffset, Time.deltaTime * enemySO.moveSpeed);
        
        if(Vector3.Distance(transform.position, currentNode.position + nodeOffset) <= .1f) FindNextNode();
    }

    private void FirstNodeAndOffset() {
        currentNodeIndex = -1;
        FindNextNode();
        // nodeOffset = Vector3.ClampMagnitude(transform.position - currentNode.position, 4.5f);
        nodeOffset = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
    }
    
    private void FindNextNode() {
        currentNode = EnemyPathManager.instance.GetNode(++currentNodeIndex);
        if(currentNode == null) { ReachPlayerBase();}
    }

    private void ReachPlayerBase() {
        // DEAL DAMAGE TO BASE
        GetComponent<HealthManager>().Death();
    }

    private void Rotate() {
        if (currentNode == null) return;
        
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentNode.position + nodeOffset - transform.position), .2f);
    }
}
