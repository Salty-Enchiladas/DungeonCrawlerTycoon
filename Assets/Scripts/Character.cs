using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Character : MonoBehaviour
{
    static readonly int baseHitChance = 60;
    static readonly int maximumArmor = 400;

    static readonly float accuracyHitChanceRatio = 1.0f;
    static readonly float criticalHitChanceRatio = .5f;

    public static readonly float criticalDamageModifier = 1.5f;
    static readonly float damageModifier = 1.0f;
    static readonly float armorModifier = 1.0f;
    static readonly float constitutionHealthMultiplier = 50.0f;

    //Find a way to lower healing

    public static readonly Weapon unarmedWeapon;

    public string characterName;
    public enum Allegiance {  Player, Enemy }
    public Allegiance allegiance;
    public Specialization specialization;

    public List<Armor> armor;
    public List<Transform> armorSlots;

    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;
    public Transform primaryWeaponSlot;
    public Transform secondaryWeaponSlot;
    public List<Sprite> characterIcons;

    [Space, Header("UI")]
    public Image characterIcon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI classText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI constitutionText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI luckText;
    public Image healthBar;

    public float HealthPercentage{get{ return Health.Value / Health.BaseValue;}}

    [Space, Header("Stats")]
    public CharacterStat Power;
    public CharacterStat Accuracy;
    public CharacterStat Constitution;
    public CharacterStat Speed;
    public CharacterStat Luck;

    public CharacterStat Health;
    public CharacterStat ArmorRating;
    public CharacterStat Damage;

    public float DamageResistance { get { return ((Constitution.Value * armorModifier) + ArmorRating.Value) / maximumArmor; } }
    public float HitChance { get { return Accuracy.Value * accuracyHitChanceRatio; } }
    public float CritChance { get { return Accuracy.Value * criticalHitChanceRatio; } }

    CharacterStat TravelSpeed;
    CharacterStat DropChance;
    CharacterStat ShopDiscount;

    private void OnEnable()
    {
        Power.OnStatsUpdated += UpdatePower;
        Accuracy.OnStatsUpdated += UpdateAccuracy;
        Constitution.OnStatsUpdated += UpdateAccuracy;
        Speed.OnStatsUpdated += UpdateSpeed;
        Luck.OnStatsUpdated += UpdateLuck;
        Health.OnStatsUpdated += UpdateHealth;
    }

    private void OnDisable()
    {
        Power.OnStatsUpdated -= UpdatePower;
        Accuracy.OnStatsUpdated -= UpdateAccuracy;
        Constitution.OnStatsUpdated -= UpdateAccuracy;
        Speed.OnStatsUpdated -= UpdateSpeed;
        Luck.OnStatsUpdated -= UpdateLuck;
        Health.OnStatsUpdated -= UpdateHealth;
    }

    public void InitializeStats()
    {
        Power.BaseValue = 1;
        Accuracy.BaseValue = 1;
        Constitution.BaseValue = 1;
        Speed.BaseValue = 1;
        Luck.BaseValue = 1;

        Health.BaseValue = Constitution.Value * constitutionHealthMultiplier;
        ArmorRating.BaseValue = Constitution.Value * armorModifier;
        Damage.BaseValue = Power.Value * damageModifier;

        characterIcon.sprite = characterIcons[Random.Range(0, characterIcons.Count)];

        UpdateUI();
    }

    void UpdatePower()
    {
        Damage.BaseValue = Power.Value * damageModifier;
        UpdateUI();
    }

    void UpdateAccuracy()
    {
        UpdateUI();
    }

    void UpdateConstitution()
    {
        Health.BaseValue = Constitution.Value * constitutionHealthMultiplier;
        ArmorRating.BaseValue = Constitution.Value * armorModifier;
        UpdateUI();
    }

    void UpdateSpeed()
    {
        UpdateUI();
    }

    void UpdateLuck()
    {
        UpdateUI();
    }

    void UpdateHealth()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        nameText.text = "Name: " + characterName;
        classText.text = "Class: " + specialization.specName;

        powerText.text = "Pow: \n" + Power.Value;
        accuracyText.text = "Acc: \n" + Accuracy.Value;
        constitutionText.text = "Con: \n" + Constitution.Value;
        speedText.text = "Spd: \n" + Speed.Value;
        luckText.text = "Lck: \n" + Luck.Value;

        healthBar.fillAmount = HealthPercentage;
    }
}
//Set the ArmorRating stat to cumulative armorValue from all armor