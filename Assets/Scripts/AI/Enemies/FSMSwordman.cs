using FSM.States;
using Unity.VisualScripting;

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
            //_fsmMachine.AddTransition(_patrol, () => true, _react); IF PLAYER IN CHASE RANGE && KNOW PLAYER == FALSE
            //_fsmMachine.AddTransition(_react, () => true, _chase); CHECK ANIMATION COMPLETE + MAKE KNOW PLAYER TRUE
            //_fsmMachine.AddTransition(_patrol, () => true, _chase); IF PLAYER IN CHASE RANGE AND KNOW PLAYER == TRUE
            //_fsmMachine.AddTransition(_chase, () => true, _attack); IF PLAYER IN ATTACK RANGE
            //_fsmMachine.AddTransition(_chase, () => true, _patrol); LOST PLAYER
            //_fsmMachine.AddTransition(_attack, () => true, _patrol); LOST PLAYER
            //_fsmMachine.AddTransition(_attack, () => true, _chase); PLAYER NOT IN ATTACK RANGE



        }
    }
}