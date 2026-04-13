using UnityEngine;

namespace FSM.States
{
    public class FsmFollow : IState
    {
        public void Enter()
        {
        }

        public void Tick()
        {
            if (Vector3.Distance(PositionToFollow.position, Context.SelfTransform.position) > 0.2f)
            {
                Context.MoveTo(PositionToFollow.position - Context.SelfTransform.position);
            }
            else
             {
                 Context.StopMove();
                 Context.LookAt(Leader.transform.position - Context.SelfTransform.position);
             }
            
        }

        public void Exit()
        {
        }

        public Context Context { get; set; }

        public FSMLeader Leader { get; set; }
        public Transform PositionToFollow { get; set; }
    }
}