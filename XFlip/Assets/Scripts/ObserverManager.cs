using UnityEngine.Events;

public class ObserverManager
{
    public static UnityEvent GamePrepare = new UnityEvent();
    public static UnityEvent GamePlay = new UnityEvent();
    public static UnityEvent GameWin = new UnityEvent();
    public static UnityEvent GameLose = new UnityEvent();

    public static UnityEvent UpgradeSpeed = new UnityEvent();
    public static UnityEvent UpgradeAccelaration = new UnityEvent();
    public static UnityEvent UpgradeLeanSpeed = new UnityEvent();

    public static UnityEvent RefreshTexts = new UnityEvent();

}
