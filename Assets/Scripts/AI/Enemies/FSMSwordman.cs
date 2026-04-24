using FSM.States;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



namespace FSM
{
    public class FSMSwordman : FSMEnemy
    {
        protected FsmPatrol _patrol = new FsmPatrol(); 
        protected FsmChase _chase = new FsmChase();
        protected FsmAttack _attack = new FsmAttack();        
        protected override void OnStart()
        {
            _patrol.Context = _context;
            _chase.Context = _context;
            _attack.Context = _context;
            base.OnStart();
            
            _fsmMachine.AddTransition(_idle, () => true, _patrol);
            _fsmMachine.AddTransition(_patrol, () => CheckPlayerSeen() && !_sawPlayerOnce, _react); //IF PLAYER IN CHASE RANGE && KNOW PLAYER == FALSE
            _fsmMachine.AddTransition(_react, () => true, _chase); //CHECK ANIMATION COMPLETE + MAKE KNOW PLAYER TRUE : TODO ADD ANIMATION CHECK
            _fsmMachine.AddTransition(_patrol, () => CheckPlayerSeen() && _sawPlayerOnce, _chase); //IF PLAYER IN CHASE RANGE AND KNOW PLAYER == TRUE
            _fsmMachine.AddTransition(_chase, () => !CheckPlayerSeen(), _patrol); //LOST PLAYER
            _fsmMachine.AddTransition(_chase, () => _context.CheckPlayerInGivenRange(_context.EnemyStat.AttackRange), _attack); //IF PLAYER IN ATTACK RANGE
            _fsmMachine.AddTransition(_attack, () => !_context.IsPlayingAttackAnimation() && !CheckPlayerSeen(), _patrol); //LOST PLAYER
            _fsmMachine.AddTransition(_attack, () => !_context.IsPlayingAttackAnimation() && !_context.CheckPlayerInGivenRange(_context.EnemyStat.AttackRange) && CheckPlayerSeen(), _chase);// PLAYER NOT IN ATTACK RANGE            
        }      
        
    }
}