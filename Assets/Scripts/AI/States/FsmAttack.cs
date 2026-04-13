using UnityEngine;

namespace FSM.States
{
    public class FsmAttack : IState
    {
        public void Enter()
        {
            Debug.Log("ATTACKING");
            Context.StopMove();
        }

        public void Tick()
        {
            Context.LookAt(Context.GetPlayerTransform.position-Context.SelfTransform.position);
        }

        public void Exit()
        {
            
        }

        public Context Context { get; set; }
    }
}