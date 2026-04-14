using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSM.States
{
    public class FsmAttackAtTarget : IState
    {
        private Vector3 playerLastKnowedPosition; 

        public Context Context { get; set; }

        public void Enter()
        {
            playerLastKnowedPosition = Context.GetPlayerTransform.position;
        }
        public void Tick()
        {
            if (Vector3.Distance(Context.SelfTransform.position, playerLastKnowedPosition) > Context.EnemyInstance.GetEnemyBaseStat().AttackRange)
            {
                Context.MoveTo(playerLastKnowedPosition - Context.SelfTransform.position);                
            }
            else
            {
                Debug.Log("ATTACKING");
                Context.StopMove();
            }
        }

        public void Exit()
        {
            
        }

        
    }
}
