using UnityEngine;

namespace FSM.States
{
    public class FsmReact : IState
    {
        public void Enter()
        {
            Debug.Log("REACTING TO PLAYER");
        }

        public void Tick()
        {            
        }

        public void Exit()
        {            
        }

        public Context Context { get; set; }
    }
}