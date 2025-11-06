using UnityEngine;

public interface ICombatant
{
    string DisplayName { get; }
    bool IsAlive { get; }
    int CalculateAttackSpeed();
    
    void TakeDamage(int amount);
}
