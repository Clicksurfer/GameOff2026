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
    [SerializeField] private float _pinDamage = 10f;
    
    ThumbCollisionHandler _collisionHandler;

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
        if (_collisionHandler == null)
        {
            _collisionHandler = new ThumbCollisionHandler(_playerCombatant, _opponentCombatant, _collisionDamage, _pinDamage);
        }
        
        _collisionHandler.HandleCollision();
    }
}