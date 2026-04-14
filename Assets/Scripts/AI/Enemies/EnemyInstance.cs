using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyInstance : MonoBehaviour 
{
    [SerializeField] Enemy_SO enemyBaseStat;

    private int _health = 1;
    private int _damage = 1;

    private void Start()
    {        
        _health = enemyBaseStat.Health;
        _damage = enemyBaseStat.Damage;
    }

    /*
     * TODO : Think about how the upgrade will work (add default value, multiply by a number, add a direct value, add a percentage of the default value)
     * public void UpgradeHealthWithMultiplicator(float multiplicator)
    {
        _health = (int)(_health* multiplicator);
    }

    public void UpgradeHealthWithValue(int valueToAdd)
    {
        _health += valueToAdd;
    }*/


    public Enemy_SO GetEnemyBaseStat()
    { 
        return enemyBaseStat;
    }

    public int GetHealth() => _health;
    public int GetDamage() => _damage;


    public void TakeDamage(int damageDelt)
    {
        _health -= damageDelt;
    }    

}