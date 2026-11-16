using UnityEngine;

public class VectorTestingView : MonoBehaviour
{
    [SerializeField] private CombantantController _playerCombatant;
    [SerializeField] private RectTransform _playerVelocityArrow;
    [SerializeField] private CombantantController _opponentCombatant;
    [SerializeField] private RectTransform _opponentVelocityArrow;

    private void FixedUpdate()
    {
        UpdateVelocityArrow(_playerCombatant, _playerVelocityArrow);
        UpdateVelocityArrow(_opponentCombatant, _opponentVelocityArrow);
    }
    
    private void UpdateVelocityArrow(CombantantController combantant, RectTransform arrow)
    {
        Vector3 velocity = combantant.GetThumbVector();
        
        arrow.localRotation = Quaternion.LookRotation(Vector3.forward, velocity);
    }
}
