using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Weapons.Bow
{
    public class NormalArrow : Arrow
    {
        public override void Attack(EnemyInstance enemy)
        {
            enemy.TakeDamage(Damage);
        }       
    }
}
