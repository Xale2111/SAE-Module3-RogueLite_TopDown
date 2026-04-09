using FSM.States;
using UnityEngine;

namespace FSM
{
    public class FSMEnemy : MonoBehaviour
    {
        [SerializeField] protected Context _context;
        
        protected Machine _fsmMachine = new Machine();

        protected FsmIdle _idle = new FsmIdle();

        protected FsmDie _die = new FsmDie();
        protected FsmHit _hit = new FsmHit();
        protected FsmReact _react = new FsmReact();
        
        private void Start()
        {
            OnStart();
        }

        protected virtual void OnStart()
        {
            _idle.Context = _context;
            _die.Context = _context;
            _hit.Context = _context;
            _react.Context = _context;
            
            //_fsmMachine.AddAnyTransition(,_die); => IF HP <= 0
            //_fsmMachine.AddAnyTransition(,_hit); => IF TOOK DAMAGE
            //_fsmMachine.Add
            
            _fsmMachine.AddTransition(_hit, ()=>true,_idle); //When Hit, go back to idle
            
            _fsmMachine.SetState(_idle);
        }

        private void Update()
        {
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
            _fsmMachine.Tick();
        }

    }
}