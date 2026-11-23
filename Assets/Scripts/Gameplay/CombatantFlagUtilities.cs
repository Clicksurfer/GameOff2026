namespace Gameplay
{
    public static class CombatantFlagUtilities
    {
        public static bool HasFlag(this CombatantFlags flag, CombatantFlags value)
        {
            return (flag & value) != 0;
        }
        
        public static void SetFlag(this ref CombatantFlags flag, CombatantFlags value)
        {
            flag |= value;
        }
        
        public static void ClearFlag(this ref CombatantFlags flag, CombatantFlags value)
        {
            flag &= ~value;
        }
        
        public static bool HasAnyFlag(this CombatantFlags flag)
        {
            return flag != CombatantFlags.Normal;
        }
    }
}