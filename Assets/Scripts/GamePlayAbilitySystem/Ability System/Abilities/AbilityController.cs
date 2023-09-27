using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using AbilitySystem.Authoring;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : MonoBehaviour
{
    public AbstractAbilityScriptableObject Abilities;
    public AbstractAbilityScriptableObject InitialisationAbilities;

    private AbilitySystemCharacter abilitySystemCharacter;

    private AbstractAbilitySpec abilitySpecs;

    public Image[] Cooldowns;

    // Start is called before the first frame update
    void Start()
    {
        this.abilitySystemCharacter = GetComponent<AbilitySystemCharacter>();
        var spec = Abilities.CreateSpec(this.abilitySystemCharacter);
        this.abilitySystemCharacter.GrantAbility(spec);
        ActivateInitialisationAbilities();
        //GrantCastableAbilities();
    }

    // Update is called once per frame
    void Update()
    {
        //for (var i = 0; i < Cooldowns.Length; i++)
        //{
        //    var durationRemaining = this.abilitySpecs.CheckCooldown();
        //    if (durationRemaining.TotalDuration > 0)
        //    {
        //        var percentRemaining = durationRemaining.TimeRemaining / durationRemaining.TotalDuration;
        //        Cooldowns[i].fillAmount = 1 - percentRemaining;
        //    }
        //    else
        //    {
        //        Cooldowns[i].fillAmount = 1;
        //    }
        //}

        //Handle Cooldown Image on UI
    }

    void ActivateInitialisationAbilities()
    {
        //for (var i = 0; i < InitialisationAbilities.Length; i++)
        //{
        //    var spec = InitialisationAbilities[i].CreateSpec(this.abilitySystemCharacter);
        //    this.abilitySystemCharacter.GrantAbility(spec);
        //    StartCoroutine(spec.TryActivateAbility());
        //}
        var spec = InitialisationAbilities.CreateSpec(this.abilitySystemCharacter);
        this.abilitySystemCharacter.GrantAbility(spec);
        //StartCoroutine(spec.TryActivateAbility());
        this.abilitySpecs = spec;
    }

    //Select to use a ability
    void GrantCastableAbilities()
    {
        //this.abilitySpecs = new AbstractAbilitySpec[Abilities.Length];
        //for (var i = 0; i < Abilities.Length; i++)
        //{
        //    var spec = Abilities[i].CreateSpec(this.abilitySystemCharacter);
        //    this.abilitySystemCharacter.GrantAbility(spec);
        //    this.abilitySpecs[i] = spec;
        //}
        var spec = Abilities.CreateSpec(this.abilitySystemCharacter);
        this.abilitySystemCharacter.GrantAbility(spec);
        this.abilitySpecs = spec;
    }

    public void UseAbility()
    {
        //var spec = abilitySpecs;
        //StartCoroutine(spec.TryActivateAbility());

        StartCoroutine(abilitySpecs.TryActivateAbility());
    }
}
