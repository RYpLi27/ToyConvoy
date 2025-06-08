using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    [SerializeField] [InfoBox("To see values click on pen at the right side")] private EnemyStatsSO statsSO;
    private Node currentNode, previousNode;
    private Vector3 nodeOffset;

    public Vector3 MoveDir => (currentNode.transform.position + nodeOffset - transform.position).normalized;

    private bool isRoundabout;
    public bool backtrack, canMove;
    
    private void OnEnable() {
        canMove = true;
        FirstNodeAndOffset();
        EnemyManager.instance.AddEnemy(this);
    }

    private void OnDisable() {
        currentNode = null;
        EnemyManager.instance.RemoveEnemy(this);
    }

    public void CustomUpdate() {
        if (canMove == false) return;
        
        if(backtrack == true && currentNode != previousNode) FindPreviousNode();

        if (isRoundabout == false) {
            MoveStraight();
        }
        else { MoveInCircle(); }
        
    }

    private void FirstNodeAndOffset() {
        currentNode = EnemyPathManager.instance.startingNode;
        if (currentNode != null) { isRoundabout = currentNode.isRoundabout; } 
        nodeOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
    }
    
    private void FindNextNode() {
        previousNode = currentNode;
        if (backtrack == false) {
            currentNode = currentNode.GetNextNode();
        } else {
            currentNode = currentNode.GetBacktrackNode();
            backtrack = false;
        }
        if (backtrack == true || currentNode != null) { isRoundabout = currentNode.isRoundabout; } 
        else { ReachPlayerBase(); }
    }

    private void FindPreviousNode() {
        currentNode = previousNode;
    }
    
    public void TryFindNode(Node node) {
        if(node == currentNode && backtrack == false) FindNextNode();
    }
    
    private void ReachPlayerBase() {
        GameManager.instance.DealDamageToBase(statsSO.damageToBase);
        WaveManager.instance.EnemyCount--;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    private void MoveStraight() {
        if (currentNode == null) return;
        
        transform.position = Vector3.MoveTowards(transform.position, currentNode.transform.position + nodeOffset, Time.fixedDeltaTime * statsSO.moveSpeed);
        
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentNode.transform.position + nodeOffset - transform.position), .15f);
        
        if(Vector3.Distance(transform.position, currentNode.transform.position + nodeOffset) <= 0f) FindNextNode();
    }

    private void MoveInCircle() {
        if (currentNode == null) return;

        float circumference = 2 * Mathf.PI * Vector3.Distance(transform.position, currentNode.roundaboutCenter.position);
        float degreesPerSecond = (statsSO.moveSpeed / circumference) * 360f;

        Vector3 previousPos = transform.position;
        transform.RotateAround(currentNode.roundaboutCenter.position, Vector3.up, -degreesPerSecond * Time.fixedDeltaTime);
        Vector3 afterPos = transform.position;
        
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(afterPos - previousPos), .15f);
        // FINDING NEXT NODE IS HANDLED IN SCRIPT RoundaboutNode.cs by collision
    }
    
}
