using System.Threading;
using UnityEngine;

namespace FSM.States
{
    public class FsmHit : IState
    {
        private float timer = 0;
        public void Enter()
        {
            timer = 0;            
            Context.LaunchHitAnimation();
        }

        public void Tick()
        {
            timer += Time.deltaTime;
            if (timer > Context.HitTime)
            {
                Debug.Log("HIT");
                
                timer = 0;
                Context.EnemyInstance.IsBeingHit = false;
            }
            Context.LookAt(Context.GetPlayerTransform.position - Context.SelfTransform.position);
        }

        public void Exit()
        {            
        }

        public Context Context { get; set; }
    }
}