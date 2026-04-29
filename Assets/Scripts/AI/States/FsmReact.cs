using UnityEngine;

namespace FSM.States
{
    public class FsmReact : IState
    {
        public void Enter()
        {
            Debug.Log("REACTING TO PLAYER");
            Context.StopMove();
            Context.LaunchReactAnimation();
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