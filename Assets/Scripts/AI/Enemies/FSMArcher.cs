using FSM.States;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



namespace FSM
{
    public class FSMArcher : FSMEnemy
    {
        [SerializeField] GameObject arrowPrefab;
        [SerializeField] Transform bowTransform;

        protected FsmPatrol _patrol = new FsmPatrol(); 
        protected FsmChase _chase = new FsmChase();
        protected FsmAttack _attack = new FsmAttack();
        protected FsmFlee _flee = new FsmFlee();
        bool attackAnimationIsPlaying => _context.IsPlayingAttackAnimation();


        protected override void OnStart()
        {
            _patrol.Context = _context;
            _chase.Context = _context;
            _attack.Context = _context;
            _flee.Context = _context;
            base.OnStart();
            
            _fsmMachine.AddTransition(_idle, () => true, _patrol);
            _fsmMachine.AddTransition(_patrol, () => CheckPlayerSeen() && !_sawPlayerOnce, _react);

            _fsmMachine.AddTransition(_react, () => !_reactAnimationIsPlaying && _context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange), _flee);
            _fsmMachine.AddTransition(_react, () => !_reactAnimationIsPlaying && _context.CheckPlayerInGivenRange(_context.EnemyStat.DetectionRadius) && !_context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange), _chase);

            _fsmMachine.AddTransition(_patrol, () => _context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange) && _sawPlayerOnce, _flee);
            _fsmMachine.AddTransition(_flee, () => !_context.CheckPlayerInGivenRange(_context.EnemyStat.DetectionRadius), _patrol);

            _fsmMachine.AddTransition(_patrol, () => _context.CheckPlayerInGivenRange(_context.EnemyStat.DetectionRadius) && _sawPlayerOnce, _chase);
            _fsmMachine.AddTransition(_chase, () => !_context.CheckPlayerInGivenRange(_context.EnemyStat.DetectionRadius), _patrol);

            _fsmMachine.AddTransition(_chase,() => _context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange), _flee);
            _fsmMachine.AddTransition(_flee, () => !_context.CheckPlayerInGivenRange(_context.EnemyStat.AttackRange) && _context.CheckPlayerInGivenRange(_context.EnemyStat.DetectionRadius), _chase);
            _fsmMachine.AddTransition(_flee, () => !_context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange) && _context.CheckPlayerInGivenRange(_context.EnemyStat.AttackRange), _attack);

            _fsmMachine.AddTransition(_chase, ()=>_context.CheckPlayerInGivenRange(_context.EnemyStat.AttackRange), _attack);
            
            _fsmMachine.AddTransition(_attack, ()=> !attackAnimationIsPlaying && !_context.CheckPlayerInGivenRange(_context.EnemyStat.AttackRange) && _context.CheckPlayerInGivenRange(_context.EnemyStat.DetectionRadius), _chase);
            _fsmMachine.AddTransition(_attack, ()=>!attackAnimationIsPlaying && _context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange), _flee);
            _fsmMachine.AddTransition(_attack, ()=> !attackAnimationIsPlaying && !_context.CheckPlayerInGivenRange(_context.EnemyStat.DetectionRadius), _patrol);            
        }

        public void ShootArrow()
        {
            Vector3 direction = _context.SelfTransform.up;

            // Pour un jeu 2D, calculer l'angle avec Atan2
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion aimDirection = Quaternion.Euler(0, 0, angle);

            GameObject newArrow = arrowPrefab;
            Throwable throwableStat = newArrow.GetComponent<Throwable>();
            throwableStat.SetDamage(_context.EnemyStat.Damage);

            Instantiate(newArrow, bowTransform.position, aimDirection);
        }

    }
}