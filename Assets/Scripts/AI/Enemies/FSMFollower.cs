using FSM.States;
using UnityEngine;

namespace FSM
{
    public class FSMFollower : FSMEnemy
    {
        [SerializeField] private FSMLeader _leader;
        [SerializeField] private Transform positionToFollow;

        [SerializeField] private Transform weaponCenter;
        [SerializeField] private Vector3 weaponSize;

        protected FsmFollow _follow = new FsmFollow();
        protected FsmPanic _panic = new FsmPanic();
        protected FsmFlee _flee = new FsmFlee();
        protected FsmAttackAtTarget _attackTarget = new FsmAttackAtTarget();

        private bool _isAttacking = false;

        protected override void OnStart()
        {
            _follow.Context = _context;
            _follow.Leader = _leader;
            _follow.PositionToFollow = positionToFollow;
            _panic.Context = _context;
            _flee.Context = _context;
            _attackTarget.Context = _context;
            base.OnStart();

            _fsmMachine.AddTransition(_idle, () => true, _follow);
            _fsmMachine.AddTransition(_follow, () => !LeaderIsAlive(), _panic);
            _fsmMachine.AddTransition(_follow, () => LeaderIsAttacking(), _attackTarget);
            _fsmMachine.AddTransition(_attackTarget, () => !LeaderIsAttacking(), _follow);
            _fsmMachine.AddTransition(_attackTarget, () => !LeaderIsAlive(), _panic);
            _fsmMachine.AddTransition(_panic, () => _context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange), _flee);
            _fsmMachine.AddTransition(_flee, () => !_context.CheckPlayerInGivenRange(_context.EnemyStat.FleeRange), _panic);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

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

        private bool LeaderIsAlive()
        {
            if (_leader)
                return _leader.IsAlive();
            return false;
        }

        private bool LeaderIsAttacking()
        {
            if (_leader)
                return _leader.IsAttacking;
            return false;
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