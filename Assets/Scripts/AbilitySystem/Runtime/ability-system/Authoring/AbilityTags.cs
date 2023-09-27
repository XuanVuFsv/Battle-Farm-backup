using UnityEngine;
using GameplayTag.Authoring;
using System;
using UnityEngine.Serialization;

namespace AbilitySystem.Authoring
{
    [Serializable]
    public struct AbilityTags
    {
        /// <summary>
        /// This tag describes the Gameplay Ability
        /// </summary>
        [SerializeField] public GameplayTagScriptableObject AssetTag;

        /// <summary>
        /// Active Gameplay Abilities (on the same character) that have these tags will be cancelled.
        /// </summary>
        /// <remarks>
        /// Example: a player's aimming and choose direction when throwing a grenade. If that player has stun Ability Tag, cancel.
        /// </remarks>
        [SerializeField] public GameplayTagScriptableObject[] CancelAbilitiesWithTags;

        /// <summary>
        /// Gameplay Abilities that have these tags will be blocked from activating on the same character
        /// </summary>
        /// <remarks>
        /// Example: a player try to press button to throw a grenade. If that player has stun Ability Tag, block.
        /// </remarks>
        [SerializeField] public GameplayTagScriptableObject[] BlockAbilitiesWithTags;

        /// <summary>
        /// These tags are granted to the character while the ability is active
        /// </summary>
        /// <remarks>
        /// Example: a player has a shield can block all hostile projectiles that hit it, that player also has can't take damage Ability Tag. 
        /// </remarks>
        [SerializeField] public GameplayTagScriptableObject[] ActivationOwnedTags;

        /// <summary>
        /// This ability can only be activated if the owner character has all of the Required tags
        /// and none of the Ignore tags.  Usually, the owner is the source as well.
        /// </summary>
        [SerializeField] public GameplayTagRequireIgnoreContainer OwnerTags;

        /// <summary>
        /// This ability can only be activated if the source character has all of the Required tags
        /// and none of the Ignore tags
        /// </summary>
        [SerializeField] public GameplayTagRequireIgnoreContainer SourceTags;

        /// <summary>
        /// This ability can only be activated if the target character has all of the Required tags
        /// and none of the Ignore tags
        /// </summary>
        [SerializeField] public GameplayTagRequireIgnoreContainer TargetTags;

    }

}