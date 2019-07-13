public class Weapon : Item
{
    public enum ActionType { Damage, Heal}
    public ActionType actionType;

    public int weaponDamage;
    public int targetCount;
}
