using UnityEngine;
using UnityEngine.AI;

namespace Behaviors.Tests
{
    public class NavAgentTest : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Transform _target;
        
        private void Update()
        {
            _navMeshAgent.destination = _target.position;
        }
    }
}