using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IAEnemy : MonoBehaviour
{
    AStarPathfinding _astar = new AStarPathfinding();
    List<Node> _path = new List<Node>();
    LayerMask _borderMask;
    enum IAStates { go, pathfinding }

    [SerializeField] Transform _chest;
    [SerializeField] Transform _goalPos;
    [SerializeField] float _speed;

    EventFSM<IAStates> _myFSM;
    private void Start()
    {
        _chest = transform.GetChild(0).transform;
        _borderMask = LayerMask.GetMask("Border");

        var go = new State<IAStates>("GO");
        var pathfinding = new State<IAStates>("PATHFINDING");

        StateConfigurer.Create(go).SetTransition(IAStates.pathfinding, pathfinding).Done();
        StateConfigurer.Create(pathfinding).SetTransition(IAStates.go, go).Done();

        go.OnEnter += x => Debug.Log("GO");
        go.OnUpdate += delegate
        {
            Goto(_goalPos.position);
            if (!InSight(_chest.position, _goalPos.position, _borderMask)) _myFSM.SendInput(IAStates.pathfinding);
        };

        pathfinding.OnEnter += x => { Debug.Log("Pathfinding"); SetPath(); };
        pathfinding.OnUpdate += delegate
        {
            if (!_path.Any()) SetPath();

            Goto(_path[0].hitPoint);
            if (Vector2.Distance(_path[0].hitPoint, transform.position) < .1f)
                _path.RemoveAt(0);

            if (InSight(_chest.position, _goalPos.position, _borderMask)) _myFSM.SendInput(IAStates.go);
        };

        _myFSM = new EventFSM<IAStates>(pathfinding);
    }
    void Update()
    {
        _myFSM?.Update();
    }
    void Goto(Vector3 destination)
    {
        Vector3 dir = destination - transform.position;

        transform.position += dir.normalized * _speed * Time.deltaTime;
    }
    void SetPath()
    {
        StartCoroutine(_astar.ConstructPathThetaStar(FindClosestNode(_chest.position), FindClosestNode(_goalPos.position), x => _path = x));
    }
    Node FindClosestNode(Vector3 targetPosition)
    {
        Node closest = null;
        float minDist = float.MaxValue;
        Node[] allNodes = FindObjectsOfType<Node>();

        for (int i = 0; i < allNodes.Length; i++)
        {
            float dist = Vector2.Distance(allNodes[i].transform.position, targetPosition);

            if (!InSight(targetPosition, allNodes[i].transform.position, _borderMask))
                continue;

            if (dist < minDist)
            {
                minDist = dist;
                closest = allNodes[i];
            }
        }

        return closest;
    }
    bool InSight(Vector3 start, Vector3 end, LayerMask mask)
    {
        //origen, direccion, distance, layerMask
        return !Physics2D.Raycast(start, end - start, Vector3.Distance(start, end), mask);
    }
}
