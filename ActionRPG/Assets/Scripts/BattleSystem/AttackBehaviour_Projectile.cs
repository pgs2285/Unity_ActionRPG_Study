using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour_Projectile : AttackBehaviour
{
    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        if(target == null)
        {
            return;
        }

        Vector3 projectilePosition = startPoint?.position ?? transform.position; // 삼항연산자. startPoint가 null이 아니면 startPoint.position을, null이면 transform.position을 projectilePosition에 저장한다.
        if(effectPrefab)
        {
            Debug.Log("Instantiate Projectile");
            GameObject projectileGO = Instantiate(effectPrefab, projectilePosition, Quaternion.identity) as GameObject;
            projectileGO.transform.forward = transform.forward;

            Projectile projectile = projectileGO.GetComponent<Projectile>();
            if(projectile)
            {
                projectile.owner = gameObject;
                projectile.target = target;
                projectile.attackBehaviour = this;
            }
        }

        calcCoolTime = 0.0f;
    }
}
