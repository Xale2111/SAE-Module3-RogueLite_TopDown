namespace FSM.States
{
    public class FsmDie : IState
    {
        public void Enter()
        {
            Context.KillEntity();
            
        }

        public void Tick()
        {            
        }

        public void Exit()
        {            
        }

        public Context Context { get; set; }
    }
}