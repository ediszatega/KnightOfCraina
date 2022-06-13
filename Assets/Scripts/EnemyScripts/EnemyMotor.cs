using UnityEngine;
using UnityEngine.AI;

namespace EnemyScripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMotor : MonoBehaviour
    {
        private NavMeshAgent agent;
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public void MoveToPoint(Vector3 point)
        {
            if (!agent.enabled)
                return;
            agent.isStopped = false;
            agent.SetDestination(point);
        }

        public void StopMoving()
        {
            if (!agent.enabled)
                return;
            agent.isStopped = true;
            agent.ResetPath();
        }
    }
}
