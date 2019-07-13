using UnityEngine;

public class Character : MonoBehaviour
{
    public enum Allegiance {  Player, Enemy }
    public Allegiance allegiance;

    public Armor armor;
    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;
    public float HealthPercentage{get{ return Health.Value / Health.BaseValue;}}

    public CharacterStat Power;
    public CharacterStat Accuracy;
    public CharacterStat Constitution;
    public CharacterStat Speed;
    public CharacterStat Luck;

    public CharacterStat DamageResistance;
    public CharacterStat Health;
    public CharacterStat WeaponAccuracy;
    public CharacterStat CritChance;

    CharacterStat TravelSpeed;
    CharacterStat DropChance;
    CharacterStat ShopDiscount;
}
