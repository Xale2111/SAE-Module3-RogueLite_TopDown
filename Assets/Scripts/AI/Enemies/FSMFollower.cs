using FSM.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSM
{
    public class FSMFollower : FSMEnemy
    {
        [SerializeField] private FSMLeader _leader;
        [SerializeField] private Transform positionToFollow;
        
        protected FsmFollow _follow = new FsmFollow();
        protected FsmPanic _panic = new FsmPanic();
        protected FsmFlee _flee = new FsmFlee();
        protected FsmAttackAtTarget _attackTarget = new FsmAttackAtTarget();

        protected override void OnStart()
        {
            _follow.Context = _context;
            _follow.Leader = _leader;
            _follow.PositionToFollow = positionToFollow;
            _panic.Context = _context;
            _flee.Context = _context;
            _attackTarget.Context = _context;
            base.OnStart();

            _fsmMachine.AddTransition(_idle, () => true, _follow);
            _fsmMachine.AddTransition(_follow, () => !LeaderIsAlive(), _panic);
            _fsmMachine.AddTransition(_follow,() => LeaderIsAttacking(),_attackTarget);
            _fsmMachine.AddTransition(_attackTarget,() => !LeaderIsAttacking(), _follow);
            _fsmMachine.AddTransition(_panic, () => _context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange), _flee);
            _fsmMachine.AddTransition(_flee, () => !_context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange), _panic);

        }

        protected override void OnUpdate()
        {
            base.OnUpdate();            
        }

        private bool LeaderIsAlive()
        {
            if (_leader)
            {
                return _leader.IsAlive();
            }

            return false;
        }

        private bool LeaderIsAttacking()
        { 
            if(_leader)
            {
                return _leader.IsAttacking;
            }
            return false;
        }
    }
}
