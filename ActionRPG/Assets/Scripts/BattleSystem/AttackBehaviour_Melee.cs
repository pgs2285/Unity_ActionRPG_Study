using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour_Melee : AttackBehaviour
{

    public ManualCollision attackCollision;

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Collider[] colliders = attackCollision?.CheckOverlapBox(targetMask);
        Debug.Log("Colliders : " + colliders.Length);

        foreach(Collider collider in colliders)
        {
            collider.gameObject.GetComponent<IDamageable>()?.takeDamage(damage, effectPrefab);
        }
    }

}
