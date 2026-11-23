using System;

[Flags]
public enum CombatantFlags
{
    Normal = 1 << 0,
    Stunned = 1 << 1,
    Pinned = 1 << 2,
    Dead = 1 << 3,
}