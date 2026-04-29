using FSM.States;
using UnityEngine;

namespace FSM
{
    public class FSMLeader : FSMEnemy
    {
        [SerializeField] private Transform weaponCenter;
        [SerializeField] private Vector3 weaponSize;

        protected FsmPatrol _patrol = new FsmPatrol();
        protected FsmChase _chase = new FsmChase();
        protected FsmAttack _attack = new FsmAttack();

        public bool IsAttacking;
        private bool _isAttacking = false;

        protected override void OnStart()
        {
            _patrol.Context = _context;
            _chase.Context = _context;
            _attack.Context = _context;
            base.OnStart();

            _fsmMachine.AddTransition(_idle, () => true, _patrol);
            _fsmMachine.AddTransition(_patrol, () => CheckPlayerSeen() && !_sawPlayerOnce, _react);
            _fsmMachine.AddTransition(_react, () => !_reactAnimationIsPlaying, _chase);
            _fsmMachine.AddTransition(_patrol, () => CheckPlayerSeen() && _sawPlayerOnce, _chase);
            _fsmMachine.AddTransition(_chase, () => !CheckPlayerSeen(), _patrol);
            _fsmMachine.AddTransition(_chase, () => _context.CheckPlayerInGivenRange(_context.EnemyStat.AttackRange), _attack);
            _fsmMachine.AddTransition(_attack, () => !_context.IsPlayingAttackAnimation() && !CheckPlayerSeen(), _patrol);
            _fsmMachine.AddTransition(_attack, () => !_context.IsPlayingAttackAnimation() && !_context.CheckPlayerInGivenRange(_context.EnemyStat.AttackRange) && CheckPlayerSeen(), _chase);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            IsAttacking = _fsmMachine.GetCurrentState() == _attack;

            if (_isAttacking)
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

        public IState GetCurrentState()
        {
            return _fsmMachine.GetCurrentState();
        }

        public bool IsAlive()
        {
            return _context.EnemyInstance.GetHealth() > 0 && gameObject.activeSelf;
        }

        private void OnDrawGizmos()
        {
            if (weaponCenter == null) return;
            Gizmos.color = new Color(0.75f, 0.0f, 0.0f, 0.75f);
            Gizmos.matrix = weaponCenter.transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, weaponSize);
        }
    }
}