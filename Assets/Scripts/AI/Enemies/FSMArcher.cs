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

        
        protected override void OnStart()
        {
            _patrol.Context = _context;
            _chase.Context = _context;
            _attack.Context = _context;
            _flee.Context = _context;
            base.OnStart();
            
            _fsmMachine.AddTransition(_idle, () => true, _patrol);
            _fsmMachine.AddTransition(_patrol, () => CheckPlayerSeen() && !_sawPlayerOnce, _react);

            _fsmMachine.AddTransition(_react, () => _context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange), _flee);
            _fsmMachine.AddTransition(_react, () => _context.CheckPlayerInGivenRange(_context.EnemyStat.DetectionRadius) && !_context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange), _chase);

            _fsmMachine.AddTransition(_patrol, () => _context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange) && _sawPlayerOnce, _flee);
            _fsmMachine.AddTransition(_flee, () => !_context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange), _patrol);

            _fsmMachine.AddTransition(_patrol, () => _context.CheckPlayerInGivenRange(_context.EnemyStat.DetectionRadius) && _sawPlayerOnce, _chase);
            _fsmMachine.AddTransition(_chase, () => !_context.CheckPlayerInGivenRange(_context.EnemyStat.DetectionRadius), _patrol);

            _fsmMachine.AddTransition(_chase,() => _context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange), _flee);
            _fsmMachine.AddTransition(_flee, () => !_context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange) && _context.CheckPlayerInGivenRange(_context.EnemyStat.DetectionRadius), _chase);

            _fsmMachine.AddTransition(_chase, ()=>_context.CheckPlayerInGivenRange(_context.EnemyStat.AttackRange), _attack);
            
            _fsmMachine.AddTransition(_attack, ()=>!_context.CheckPlayerInGivenRange(_context.EnemyStat.AttackRange) && _context.CheckPlayerInGivenRange(_context.EnemyStat.DetectionRadius), _chase);
            _fsmMachine.AddTransition(_attack, ()=>_context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange), _flee);
            _fsmMachine.AddTransition(_attack, ()=> !_context.CheckPlayerInGivenRange(_context.EnemyStat.DetectionRadius), _patrol);
            
        }
            
    }
}