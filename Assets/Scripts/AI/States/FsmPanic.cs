using UnityEngine;

namespace FSM.States
{
    public class FsmPanic : IState
    {
        Vector3 _patrolPoint;

        public void Enter()
        {
            _patrolPoint = GetNewPatrolPoint();
        }

        public void Tick()
        {
            if (Vector3.Distance(Context.SelfTransform.position, _patrolPoint) < 0.7f)
            {
                _patrolPoint = GetNewPatrolPoint();
            }

            Context.FleeTo(_patrolPoint - Context.SelfTransform.position);
        }

        public void Exit()
        {
        }

        public Context Context { get; set; }

        private Vector3 GetNewPatrolPoint()
        {
            Vector3 minBounds = Context.GetRoomBounds().min;
            Vector3 maxBounds = Context.GetRoomBounds().max;

            Vector3 newPoint = new Vector3
            {
                x = Random.Range(minBounds.x + .5f, maxBounds.x - .5f),
                y = Random.Range(minBounds.y + .5f, maxBounds.y - .5f),
                z = Random.Range(minBounds.z + .5f, maxBounds.z - .5f),
            };

            return newPoint;
        }
    }
}