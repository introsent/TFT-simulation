using UnityEngine;

namespace FSM
{
    public class PatrolState : UnitState
    {
        private Vector3 _center;
        private float   _centerRadius;
        private Vector3 _origin;
        private Vector3 _patrolPoint;
        private Vector3 _fieldForward;

        private float _maxAngleDeg = 30f;
        private float _minDist     = 2f;
        private float _maxDist     = 5f;
        private float _reachThresh = 0.2f;

        public PatrolState(Unit unit) : base(unit)
        {
            // Define map center and radius
            _center       = new Vector3(5, 0, 6);
            _centerRadius = 1.0f;

            // Forward direction per side
            _fieldForward = (unit.Side == Faction.Player) ? Vector3.back: Vector3.forward;
        }

        public override void OnEnter()
        {
            _unit.GetComponent<UnitAnimator>().TriggerMove();
            _patrolPoint    = Vector3.zero;
            _origin         = Vector3.zero;
        }

        public override void Execute()
        {
            if (_origin == Vector3.zero)
            {
                // not yet reached center
                MoveTowards(_center);
                if (Vector3.Distance(_unit.transform.position, _center) <= _centerRadius)
                {
                    // reached center, lock origin and pick first patrol point
                    _origin      = _unit.transform.position;
                    PickNewPatrolPoint();
                }
            }
            else
            {
                // patrol around origin within wedge
                MoveTowards(_patrolPoint);
                if (Vector3.Distance(_unit.transform.position, _patrolPoint) <= _reachThresh)
                    PickNewPatrolPoint();
            }
        }

        public override UnitState CheckTransitions()
        {
            if (EnemyInSight(out GameObject enemy))
                return new SeekState(_unit, enemy.transform);
            return null;
        }

        private void PickNewPatrolPoint()
        {
            float angleDeg = Random.Range(-_maxAngleDeg, _maxAngleDeg);
            Vector3 dir    = Quaternion.AngleAxis(angleDeg, Vector3.up) * _fieldForward;
            float d        = Random.Range(_minDist, _maxDist);
            _patrolPoint   = _origin + dir.normalized * d;
        }

        private void MoveTowards(Vector3 dest)
        {
            Vector3 dir = (dest - _unit.transform.position).normalized;
            _unit.transform.rotation = Quaternion.Slerp(
                _unit.transform.rotation,
                Quaternion.LookRotation(dir),
                _unit.RotationSpeed * Time.deltaTime);
            _unit.transform.position += dir * _unit.Speed * Time.deltaTime;
        }

        private bool EnemyInSight(out GameObject enemyFound)
        {
            float best = float.MaxValue;
            enemyFound = null;
            foreach (var u in Object.FindObjectsByType<Unit>(FindObjectsSortMode.None))
            {
                if (u.Side != _unit.Side)
                {
                    float d = Vector3.Distance(_unit.transform.position, u.transform.position);
                    if (d < _unit.DetectionRange && d < best)
                    {
                        best = d;
                        enemyFound = u.gameObject;
                    }
                }
            }
            return enemyFound != null;
        }
    }
}
