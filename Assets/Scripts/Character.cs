using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    static readonly int baseHitChance = 60;
    static readonly int maximumArmor = 100;

    static readonly float accuracyHitChanceRatio = 2.0f;
    static readonly float criticalHitChanceRatio = 1.0f;

    public static readonly float criticalDamageModifier = 2.0f;
    static readonly float damageModifier = 1.0f;
    static readonly float armorModifier = 1.0f;
    static readonly float constitutionHealthMultiplier = 10.0f;

    public static readonly Weapon unarmedWeapon;

    public enum Allegiance {  Player, Enemy }
    public Allegiance allegiance;

    public Armor[] armor;
    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;

    [Space]
    public Image characterIcon;

    public float HealthPercentage{get{ return Health.Value / Health.BaseValue;}}

    public CharacterStat Power { get; private set; }
    public CharacterStat Accuracy { get; private set; }
    public CharacterStat Constitution { get; private set; }
    public CharacterStat Speed { get; private set; }
    public CharacterStat Luck { get; private set; }

    public CharacterStat Health { get; private set; }
    public CharacterStat ArmorRating { get; private set; }
    public CharacterStat Damage { get; private set; }

    public float DamageResistance { get { return ((Constitution.Value * armorModifier) + ArmorRating.Value) / maximumArmor; } }
    public float HitChance { get { return Accuracy.Value * accuracyHitChanceRatio; } }
    public float CritChance { get { return Accuracy.Value * criticalHitChanceRatio; } }

    CharacterStat TravelSpeed;
    CharacterStat DropChance;
    CharacterStat ShopDiscount;

    private void Start()
    {
        InitializeStats();
    }

    private void OnEnable()
    {
        Power.OnStatsUpdated += UpdatePower;
        Accuracy.OnStatsUpdated += UpdateAccuracy;
        Constitution.OnStatsUpdated += UpdateAccuracy;
        Speed.OnStatsUpdated += UpdateSpeed;
        Luck.OnStatsUpdated += UpdateLuck;
    }

    private void OnDisable()
    {
        Power.OnStatsUpdated -= UpdatePower;
        Accuracy.OnStatsUpdated -= UpdateAccuracy;
        Constitution.OnStatsUpdated -= UpdateAccuracy;
        Speed.OnStatsUpdated -= UpdateSpeed;
        Luck.OnStatsUpdated -= UpdateLuck;
    }

    void InitializeStats()
    {
        Power.BaseValue = 1;
        Accuracy.BaseValue = 1;
        Constitution.BaseValue = 1;
        Speed.BaseValue = 1;
        Luck.BaseValue = 1;

        Health.BaseValue = Constitution.Value * constitutionHealthMultiplier;
        ArmorRating.BaseValue = Constitution.Value * armorModifier;
        Damage.BaseValue = Power.Value * damageModifier;
    }

    void UpdatePower()
    {
        Damage.BaseValue = Power.Value * damageModifier;
    }

    void UpdateAccuracy()
    {

    }

    void UpdateConstitution()
    {
        Health.BaseValue = Constitution.Value * constitutionHealthMultiplier;
        ArmorRating.BaseValue = Constitution.Value * armorModifier;
    }

    void UpdateSpeed()
    {

    }

    void UpdateLuck()
    {
        
    }
}
//Set the ArmorRating stat to cumulative armorValue from all armor