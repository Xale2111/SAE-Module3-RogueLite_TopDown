using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons")]
public class Weapon : ScriptableObject
{
    [SerializeField] GameObject weaponPrefab;
    public string name;
    [SerializeField] Animator animator;

    public Weapon Equip(Transform parent)
    {
        Instantiate(weaponPrefab, parent);
        return this;
    }

    //Main Attack
    public virtual void LeftClick() {
        Debug.Log("Main attack with : " + name);
    }


    //Second Attack
    public virtual void RightClick() {
        Debug.Log("Second attack with : " + name);
    }

}
