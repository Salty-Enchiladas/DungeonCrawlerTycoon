using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class Character : MonoBehaviour
{
    static readonly int baseHitChance = 60;
    static readonly int maximumArmor = 400;

    static readonly float extraAttackRatio = 1.0f;
    static readonly float criticalHitChanceRatio = .5f;

    public static readonly float criticalDamageModifier = 1.5f;
    static readonly float damageModifier = 1.0f;
    static readonly float healingModifier = .5f;
    static readonly float armorModifier = 1.0f;
    static readonly float constitutionHealthMultiplier = 5.0f;

    //Find a way to lower healing

    public static readonly Weapon unarmedWeapon = new Weapon();

    public string characterName;
    public enum Allegiance {  Player, Enemy }
    public Allegiance allegiance;
    public Specialization specialization;

    public List<Armor> armor;
    public List<Transform> armorSlots;

    public List<Weapon> weapons;
    public List<Transform> weaponSlots;
    public List<Sprite> characterIcons;

    [Space, Header("UI")]
    public Image characterIcon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI classText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI zealText;
    public TextMeshProUGUI constitutionText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI luckText;
    public Image healthBar;
    public TextMeshProUGUI healthValues;

    public float HealthPercentage{get{ return Health.Value / Health.BaseValue;}}

    [Space, Header("Stats")]
    public CharacterStat Power;
    public CharacterStat Zeal;
    public CharacterStat Constitution;
    public CharacterStat Speed;
    public CharacterStat Luck;

    public CharacterStat Health;
    public CharacterStat ArmorRating;
    public CharacterStat Damage;
    public CharacterStat Healing;

    public float DamageResistance { get { return ((Constitution.Value * armorModifier) + ArmorRating.Value) / maximumArmor; } }
    public float ExtraAttackChance { get { return Zeal.Value * extraAttackRatio; } }
    public float CritChance { get { return Luck.Value * criticalHitChanceRatio; } }

    CharacterStat TravelSpeed;
    CharacterStat DropChance;
    CharacterStat ShopDiscount;

    private void OnEnable()
    {
        Power.OnStatsUpdated += UpdatePower;
        Zeal.OnStatsUpdated += UpdateZeal;
        Constitution.OnStatsUpdated += UpdateConstitution;
        Speed.OnStatsUpdated += UpdateSpeed;
        Luck.OnStatsUpdated += UpdateLuck;
        Health.OnStatsUpdated += UpdateHealth;
    }

    private void OnDisable()
    {
        Power.OnStatsUpdated -= UpdatePower;
        Zeal.OnStatsUpdated -= UpdateZeal;
        Constitution.OnStatsUpdated -= UpdateConstitution;
        Speed.OnStatsUpdated -= UpdateSpeed;
        Luck.OnStatsUpdated -= UpdateLuck;
        Health.OnStatsUpdated -= UpdateHealth;
    }

    public void InitializeStats()
    {
        Power.BaseValue = 1;
        Zeal.BaseValue = 1;
        Constitution.BaseValue = 1;
        Speed.BaseValue = 1;
        Luck.BaseValue = 1;

        Health.BaseValue = Constitution.Value * constitutionHealthMultiplier;
        ArmorRating.BaseValue = Constitution.Value * armorModifier;
        Damage.BaseValue = Power.Value * damageModifier;
        Healing.BaseValue = Power.Value * healingModifier;

        characterIcon.sprite = characterIcons[Random.Range(0, characterIcons.Count)];

        UpdateUI();
    }

    void UpdatePower()
    {
        Damage.BaseValue = Power.Value * damageModifier;
        Healing.BaseValue = Power.Value * healingModifier;
        UpdateUI();
    }

    void UpdateZeal()
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
        zealText.text = "Zea: \n" + Zeal.Value;
        constitutionText.text = "Con: \n" + Constitution.Value;
        speedText.text = "Spd: \n" + Speed.Value;
        luckText.text = "Lck: \n" + Luck.Value;

        healthBar.fillAmount = HealthPercentage;
        healthValues.text = Health.Value + " / " + Health.BaseValue;
    }
    
    public int GetChallengeRating()
    {
        int statCount = (int)(Power.BaseValue + Zeal.BaseValue + Constitution.BaseValue + Speed.BaseValue + Luck.BaseValue);

        for(int i = 0; i < armor.Count; i++)
        {
            statCount += armor[i].power;
            statCount += armor[i].zeal;
            statCount += armor[i].constitution;
            statCount += armor[i].speed;
            statCount += armor[i].luck;
        }

        for(int i = 0; i < weapons.Count; i++)
        {
            statCount += weapons[i].power;
            statCount += weapons[i].zeal;
            statCount += weapons[i].constitution;
            statCount += weapons[i].speed;
            statCount += weapons[i].luck;
        }

        return Mathf.CeilToInt(statCount / 5.0f); //Divided by 5 because each challenge rating increases Stat Count by 5.
    }
}
//Set the ArmorRating stat to cumulative armorValue from all armor