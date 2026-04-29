using FSM.States;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



namespace FSM
{
    public class FSMSwordman : FSMEnemy
    {
        [SerializeField] private Transform weaponCenter;
        [SerializeField] private Vector3 weaponSize;

        protected FsmPatrol _patrol = new FsmPatrol(); 
        protected FsmChase _chase = new FsmChase();
        protected FsmAttack _attack = new FsmAttack();

        bool _isAttacking = false;
        protected override void OnStart()
        {
            _patrol.Context = _context;
            _chase.Context = _context;
            _attack.Context = _context;
            base.OnStart();
            
            _fsmMachine.AddTransition(_idle, () => true, _patrol);
            _fsmMachine.AddTransition(_patrol, () => CheckPlayerSeen() && !_sawPlayerOnce, _react); //IF PLAYER IN CHASE RANGE && KNOW PLAYER == FALSE
            _fsmMachine.AddTransition(_react, () => !_reactAnimationIsPlaying, _chase); //CHECK ANIMATION COMPLETE + MAKE KNOW PLAYER TRUE : TODO ADD ANIMATION CHECK
            _fsmMachine.AddTransition(_patrol, () => CheckPlayerSeen() && _sawPlayerOnce, _chase); //IF PLAYER IN CHASE RANGE AND KNOW PLAYER == TRUE
            _fsmMachine.AddTransition(_chase, () => !CheckPlayerSeen(), _patrol); //LOST PLAYER
            _fsmMachine.AddTransition(_chase, () => _context.CheckPlayerInGivenRange(_context.EnemyStat.AttackRange), _attack); //IF PLAYER IN ATTACK RANGE
            _fsmMachine.AddTransition(_attack, () => !_context.IsPlayingAttackAnimation() && !CheckPlayerSeen(), _patrol); //LOST PLAYER
            _fsmMachine.AddTransition(_attack, () => !_context.IsPlayingAttackAnimation() && !_context.CheckPlayerInGivenRange(_context.EnemyStat.AttackRange) && CheckPlayerSeen(), _chase);// PLAYER NOT IN ATTACK RANGE            
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();

            if(_isAttacking)
            {
                CheckCollision();
            }
        }

        public void StartAttacking()
        {
            _isAttacking = true;
        }

        public void EndAttacking()
        {
            _isAttacking = false;
        }

        private void CheckCollision()
        {
            Vector2 origin = weaponCenter.position;
            Vector2 size = new Vector2(weaponSize.x, weaponSize.y);
            float angle = weaponCenter.eulerAngles.z;

            RaycastHit2D hit = Physics2D.BoxCast(origin, size, angle, Vector2.zero, 0f);
            if (hit && hit.collider.TryGetComponent(out PlayerController player))
            {
                player.TakeDamage(_context.EnemyStat.Damage);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.75f, 0.0f, 0.0f, 0.75f);
            Gizmos.matrix = weaponCenter.transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, weaponSize);
        }
    }
}