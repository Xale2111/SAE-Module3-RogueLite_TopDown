using System;
using System.Collections.Generic;

namespace FSM
{
    public class Machine
    {
        IState _currentState;

        private List<Transition> _anyStateTransitions = new List<Transition>();
        
        private Dictionary<IState, List<Transition>> _transitions = new Dictionary<IState, List<Transition>>();
        
        public void SetState(IState state)
        {
            if (state == null) return;
            
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }
        
        public void Tick()
        {
            IState newState = CheckTransitions();
            
            if (newState != _currentState)
            {
                SetState(newState);
            }
            
            _currentState?.Tick();
            
        }
        
        public void AddAnyTransition(Func<bool> condition, IState state)
        {
            _anyStateTransitions.Add(new Transition(condition, state));
        }

        public void AddTransition(IState fromState, Func<bool> condition, IState toState)
        {
            if (_transitions.TryGetValue(fromState, out List<Transition> transitions))
            {
                transitions.Add(new Transition(condition, toState));
            }
            else
            {
                _transitions.Add(fromState, new List<Transition> {new Transition(condition, toState)});
            }
        }

        private IState CheckTransitions()
        {
            foreach (Transition anyStateTransition in _anyStateTransitions)
            {
                if(anyStateTransition.IsVerified()) return anyStateTransition.State;
            }

            if (_transitions.TryGetValue(_currentState, out List<Transition> possibleTransitions))
            {
                foreach (Transition possibleTransition in possibleTransitions)
                {
                    if (possibleTransition.IsVerified()) return possibleTransition.State;
                }
            }
            return _currentState;
        }
    }
}