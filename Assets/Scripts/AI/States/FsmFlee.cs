using UnityEngine;

namespace FSM.States
{
    public class FsmFlee : IState
    {
        public void Enter()
        {            
        }

        public void Tick()
        {            
            Context.MoveTo(Context.SelfTransform.position - Context.GetPlayerTransform.position);
        }

        public void Exit()
        {            
        }

        public Context Context { get; set; }
    }
}