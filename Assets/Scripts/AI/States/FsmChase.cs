using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace FSM.States
{
    public class FsmChase : IState
    {
        public void Enter()
        {
            
        }

        public void Tick()
        {
            Context.MoveTo(Context.GetPlayerTransform.position-Context.SelfTransform.position);
        }

        public void Exit()
        {
            
        }

        public Context Context { get; set; }

        
    }
}