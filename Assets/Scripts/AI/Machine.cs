using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class Machine
    {
        IState _currentState;

        private List<Transition> _anyStateTransitions = new List<Transition>();
        
        private Dictionary<IState, List<Transition>> _transitions = new Dictionary<IState, List<Transition>>();
        
        float delay = 0.2f;
        float timer = 0;
        
        public void SetState(IState state, bool fromAnyState = false)//Add bool fromAnyState
        {
            if (state == null) return;
            //If !fromAnyState -> DO 
            //if timer < delay -> return   
            //reset timer
            if (!fromAnyState)
            {
                if (timer < delay) return;
            }
            timer = 0;   
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }
        public void SetFirstState(IState state)
        {
            if (state == null) return;

            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }

        public IState GetCurrentState()
        { 
            return _currentState;
        }
        
        public void Tick()
        {
            //Increment timer
            timer += Time.deltaTime;
            CheckTransitions();
           
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

        private void CheckTransitions()
        {
            IState newState = null;
            //ANY TRANSITIONS 
            foreach (Transition anyStateTransition in _anyStateTransitions)
            {
                if (anyStateTransition.IsVerified())
                { 
                    newState = anyStateTransition.State;
                    //SET STATE INSTEAD OF RETURN
                    if (newState != _currentState)
                    {
                        SetState(newState, true);
                    }
                
                }
            }

            //TRANSITIONS
            if (_transitions.TryGetValue(_currentState, out List<Transition> possibleTransitions))
            {
                foreach (Transition possibleTransition in possibleTransitions)
                {
                    if (possibleTransition.IsVerified())
                    {
                        //SET STATE INSTEAD OF RETURN
                        newState = possibleTransition.State;
                        //SET STATE INSTEAD OF RETURN
                        if (newState != _currentState)
                        {
                            SetState(newState);
                        }
                    }
                }
            }
                       
        }
    }
}