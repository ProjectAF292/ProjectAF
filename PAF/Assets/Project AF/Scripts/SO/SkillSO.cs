using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu(fileName = "SkillSO", menuName = "Scriptable Object/SkillSO")]
public class SkillSO : ScriptableObject
{
    [SerializeField, ReadOnly, PropertyTooltip("Skill Id, read only, Changing the SO's name will also update this variable")]
    int id;
    public int Id => id;

    string desc; //Description for Designer.

    [Space(15), SerializeField, PropertyTooltip("Name to be displayed in the skill UI, Input string key")]
    string _skillName;
    public string SkillName => _skillName;

    [SerializeField, PropertyTooltip("Description to be displayed in the skill UI, Input string key")]
    string _skillDesc;
    public string SkillDesc => _skillDesc;

    [Space(15), EnumPaging, SerializeField, PropertyTooltip("Skill Effect Type")]
    SkillEffectType _skillEffectType;
    public SkillEffectType SkillType => _skillEffectType;

    public enum SkillEffectType
    {
        Normal,
        Projectile,
        DomainExpansion,
        Passive,
        Heal
    }
    
    [Space(15), SerializeField, PropertyTooltip("If check this, must set Mp")]
    bool _isUsingMp;
    public bool IsUsingMp => _isUsingMp;

    [ShowIf("_isUsingMp"), SerializeField, PropertyTooltip("consumed mp when using the skill")]
    int _mpCost;
    public int MpCost => _mpCost;

    [SerializeField, PropertyTooltip("Cooldown after using the skill")]
    float _coolTime;
    public float CoolTime => _coolTime;

    [SerializeField, PropertyTooltip("Is Stack")]
    bool _isStacked;
    public bool IsStacked => _isStacked;

    [ShowIf("_isStacked"), SerializeField, PropertyTooltip("Stack Count")]
    int _stackCount;
    public int StackCount => _stackCount;

    [Space(15), EnumPaging, SerializeField, PropertyTooltip("Skill damage type used in damage calculation")]
    DmgType _dmgType;
    public DmgType DamageType => _dmgType;

    public enum DmgType
    {
        Atk,
        Def,
        Dex,
        Luk
    }

    [SerializeField, PropertyTooltip("Skill coefficient used in damage calculation")]
    float _coef;
    public float Coef => _coef;

    [Space(15), EnumPaging, SerializeField, PropertyTooltip("Defines the type of casting for the skill")]
    SkillCastType _skillCastType; 
    public SkillCastType CastType => _skillCastType;

    public enum SkillCastType
    {
        Instant,
        Charging,
        Target
    }

    [ShowIf("_skillCastType", SkillCastType.Target), SerializeField, PropertyTooltip("Target Type Detail Value")]
    TargetType _targetType;
    public TargetType Target => _targetType;

    public enum TargetType
    {
        SingleEnemy,
        MultyEnemy,
        SingleAlly,
        MultyAlly
    }

    [Space(15), SerializeField, PropertyTooltip("Can move while using skill")]
    bool _isMove;
    public bool IsMove => _isMove;

    [SerializeField, PropertyTooltip("Cancel motion while using skill")]
    bool _isCancel;
    public bool IsCancel => _isCancel;

    [Space(15), EnumPaging, SerializeField, PropertyTooltip("Use buff or debuff")]
    AdditionalEffects _additionalEffects;
    public AdditionalEffects AddEffect => _additionalEffects;

    [System.Flags]
    public enum AdditionalEffects
    {
        Buff = 1 << 0,
        Debuff = 1 << 1,
        All = Buff | Debuff
    }

    [TitleGroup("Assets")]
    [AssetsOnly, PreviewField, SerializeField, PropertyTooltip("Skill icon in Ui")]
    Sprite _iconSpirte;
    public Sprite IconSprite => _iconSpirte;

    [TitleGroup("Assets")]
    [AssetsOnly, PreviewField, SerializeField, PropertyTooltip("Character Animation to Play on Skill Use")]
    AnimationClip _skillAnim;
    public AnimationClip SkillAnim => _skillAnim;

    [TitleGroup("Assets")]
    [AssetsOnly, PreviewField, SerializeField, PropertyTooltip("Skill effect to Play on Skill Use")]
    AnimationClip _effectAnim;
    public AnimationClip EffectAnim => _effectAnim;

    private void OnEnable()
    {
        id = int.TryParse(this.name, out int parseId) ? parseId : id;
    }

}