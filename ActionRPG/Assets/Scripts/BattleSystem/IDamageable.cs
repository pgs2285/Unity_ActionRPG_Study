using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    #region properties
    bool isAlive
    {
        get;
    }
    #endregion properties
    
    void takeDamage(int damage, GameObject hitEffectPrefab = null);
    
}
