using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Weapons.Bow
{
    public class StunThrowable : Throwable
    {
        public float StunDuration;

        public override void AttackEnemy(EnemyInstance enemy)
        {            
            enemy.Stun(StunDuration);
        }
    }
}