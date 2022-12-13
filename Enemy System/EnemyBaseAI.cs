using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseAI : ScriptableObject
{
    public abstract void OnEnter(EnemyScript _enemyScript);
    public abstract void OnUpdate(EnemyScript _enemyScript);
    public abstract void OnFixedUpdate(EnemyScript _enemyScript);
    public abstract void OnCollision(EnemyScript _enemyScript);
    public abstract void OnExit(EnemyScript _enemyScript);
   
}
