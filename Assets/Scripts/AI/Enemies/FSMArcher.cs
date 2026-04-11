using FSM.States;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



namespace FSM
{
    public class FSMArcher : FSMEnemy
    {
        protected FsmPatrol _patrol = new FsmPatrol(); 
        protected FsmChase _chase = new FsmChase();
        protected FsmAttack _attack = new FsmAttack();
        protected FsmFlee _flee = new FsmFlee();

        bool _sawPlayerOnce = false;
        
        protected override void OnStart()
        {
            _patrol.Context = _context;
            _chase.Context = _context;
            _attack.Context = _context;
            _flee.Context = _context;
            base.OnStart();
            
            _fsmMachine.AddTransition(_idle, () => true, _patrol);
            //_fsmMachine.AddTransition(_patrol, () => CheckPlayerSeen() && !_sawPlayerOnce, _react);

            _fsmMachine.AddTransition(_patrol, () => _context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange), _flee);
            _fsmMachine.AddTransition(_flee, () => !_context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange), _patrol);
            
            /*           
            react to chase
            react to flee

            Patrol to chase
            Patrol to flee
            chase to patrol
            flee to patrol
            chase to flee
            flee to chase
            chase to attack
            attack to chase
            attack to flee
            attack to patrol
            
            
             */
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (!_sawPlayerOnce && CheckPlayerSeen())
            {
                _sawPlayerOnce = true;
            }
        }

        //TODO : PUT IN CONTEXT ??
        private bool CheckPlayerSeen()
        {
            return _context.CheckPlayerInGivenRange(_context.EnemyStat.DetectionRadius);
        }
    }
}