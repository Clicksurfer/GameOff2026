using UnityEngine;

public class ThumbWarManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CombantantController _playerCombatant;
    [SerializeField] private CombantantController _opponentCombatant;
    [SerializeField] private CollisionDetector[] _collisionDetectors;

    [Header("Config")]
    [SerializeField] private float _startHp = 100f;
    [SerializeField] private float _collisionDamage = 10f;

    private void Start()
    {
        InitializePlayers();
        ListenToCollisions();
    }
    
    private void InitializePlayers()
    {
        _playerCombatant.Initialize(CombatantType.Player, _startHp);
        _opponentCombatant.Initialize(CombatantType.Opponent, _startHp);
    }
    
    private void ListenToCollisions()
    {
        foreach (var detector in _collisionDetectors)
        {
            detector.OnCollisionEntered += HandleCollision;
        }
    }
    
    private void HandleCollision(Collision collision)
    {
        // For simplicity, we assume any collision detected is between the player and opponent.
        Debug.Log($"Collision detected between {_playerCombatant.name} and {_opponentCombatant.name}. Applying {_collisionDamage} damage to both.");
        _playerCombatant.TakeDamage(_collisionDamage);
        _opponentCombatant.TakeDamage(_collisionDamage);
    }
}