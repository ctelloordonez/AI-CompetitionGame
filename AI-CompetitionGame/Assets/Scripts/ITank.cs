using System.Collections;
using UnityEngine;

public interface ITank
{
    float GetHealth();
    void TakeDamage(float damage);

    void Move(float movementInputValue);
    void Turn(float turnInputValue);
    void TurnTurret();
    void Fire();
    string ObstacleAhead();
    string ObstacleRight();
    string ObstacleLeft();
}