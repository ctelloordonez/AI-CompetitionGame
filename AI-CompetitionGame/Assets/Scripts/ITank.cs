using System.Collections;
using UnityEngine;

public interface ITank
{
    float GetHealth();
    void TakeDamage(float damage);

    void Move();
    void Turn();
    void TurnTurret();
    void Fire();
    void ChangeAmmo();
    void CheckSurface();
}