using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    [SerializeField] [InfoBox("To see values click on pen at the right side")] private EnemyStatsSO statsSO;
    private Node currentNode;
    private Vector3 nodeOffset;

    public Vector3 MoveDir => (currentNode.transform.position + nodeOffset - transform.position).normalized;

    private bool isRoundabout;
    
    private void OnEnable() {
        FirstNodeAndOffset();
    }

    private void OnDisable() {
        currentNode = null;
    }

    private void Update() {
        if (GameManager.gameState != GameManager.GameState.Ongoing) return;
        
        if (isRoundabout == false) { MoveStraight(); }
        else { MoveInCircle(); }
        
    }

    private void FirstNodeAndOffset() {
        currentNode = EnemyPathManager.instance.startingNode;
        if (currentNode != null) { isRoundabout = currentNode.isRoundabout; } 
        nodeOffset = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
    }
    
    private void FindNextNode() {
        currentNode = currentNode.GetNextNode();
        if (currentNode != null) { isRoundabout = currentNode.isRoundabout; } 
        else { ReachPlayerBase(); }
    }

    public void TryFindNode(Node node) {
        if(node == currentNode) FindNextNode();
    }
    
    private void ReachPlayerBase() {
        GameManager.instance.DealDamageToBase(1);
        WaveManager.instance.EnemyCount--;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    private void MoveStraight() {
        if (currentNode == null) return;
        
        transform.position = Vector3.MoveTowards(transform.position, currentNode.transform.position + nodeOffset, Time.deltaTime * statsSO.moveSpeed);
        
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentNode.transform.position + nodeOffset - transform.position), .05f);
        
        if(Vector3.Distance(transform.position, currentNode.transform.position + nodeOffset) <= 0f) FindNextNode();
    }

    private void MoveInCircle() {
        if (currentNode == null) return;

        float circumference = 2 * Mathf.PI * Vector3.Distance(transform.position, currentNode.roundaboutCenter.position);
        float degreesPerSecond = (statsSO.moveSpeed / circumference) * 360f;

        Vector3 previousPos = transform.position;
        transform.RotateAround(currentNode.roundaboutCenter.position, Vector3.up, -degreesPerSecond * Time.deltaTime);
        Vector3 afterPos = transform.position;
        
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(afterPos - previousPos), .05f);
        // FINDING NEXT NODE IS HANDLED IN SCRIPT RoundaboutNode.cs by collision
    }
}
