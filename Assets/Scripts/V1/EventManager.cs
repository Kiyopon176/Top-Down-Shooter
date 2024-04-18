

using System;

public class EventManager
{
    public static Action<int> OnDamaged;
    public static Action OnHpChanged;
    public static Action OnEnemyKilled;
    public static Action OutOfScreen;
    public static Action<String> OnSoundNeed;
}
