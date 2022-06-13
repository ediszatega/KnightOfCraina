using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public abstract void Attack();
    public abstract int TakeDamage(int dmg);
    public abstract void Die();
}
