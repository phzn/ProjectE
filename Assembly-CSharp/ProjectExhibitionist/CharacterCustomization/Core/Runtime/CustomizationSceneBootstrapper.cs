using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000AFE RID: 2814
	public class CustomizationSceneBootstrapper : MonoBehaviour
	{
		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06004FAD RID: 20397 RVA: 0x001EDF79 File Offset: 0x001EC179
		// (set) Token: 0x06004FAE RID: 20398 RVA: 0x001EDF81 File Offset: 0x001EC181
		public CustomizationSessionContext InitialContext { get; private set; }

		// Token: 0x06004FAF RID: 20399 RVA: 0x001EDF8A File Offset: 0x001EC18A
		private void Awake()
		{
			CustomizationAuthoredLabels.BindRig(base.transform);
			this.CacheSpawnProviders();
		}

		// Token: 0x06004FB0 RID: 20400 RVA: 0x001EDFA0 File Offset: 0x001EC1A0
		private void Start()
		{
			if (!this.autoBootstrapOnStart)
			{
				return;
			}
			CustomizationSessionContext customizationSessionContext = CustomizationSessionStore.ConsumePendingContext();
			if (customizationSessionContext == null)
			{
				customizationSessionContext = this.CreateFallbackContext();
			}
			if (customizationSessionContext == null)
			{
				if (this.verboseLogs)
				{
					Debug.Log("[CustomizationBootstrapper] No pending context found.");
				}
				return;
			}
			this.InitialContext = customizationSessionContext;
			if (customizationSessionContext.openInHub)
			{
				if (this.verboseLogs)
				{
					Debug.Log("[CustomizationBootstrapper] Hub-mode context received. Waiting for target selection.");
				}
				return;
			}
			this.Bootstrap(customizationSessionContext);
		}

		// Token: 0x06004FB1 RID: 20401 RVA: 0x001EE008 File Offset: 0x001EC208
		private CustomizationSessionContext CreateFallbackContext()
		{
			switch (this.fallbackEntryMode)
			{
			case CustomizationSceneBootstrapper.FallbackEntryMode.Hub:
				return CustomizationSessionContext.CreateHub(string.Empty);
			case CustomizationSceneBootstrapper.FallbackEntryMode.MainCharacter:
				return CustomizationSessionContext.Create(CustomizationTargetType.MainCharacter, this.fallbackMainCharacterId, true, string.Empty);
			case CustomizationSceneBootstrapper.FallbackEntryMode.Npc:
				return CustomizationSessionContext.Create(CustomizationTargetType.Npc, this.fallbackNpcCharacterId, false, string.Empty);
			default:
				return null;
			}
		}

		// Token: 0x06004FB2 RID: 20402 RVA: 0x001EE064 File Offset: 0x001EC264
		public bool Bootstrap(CustomizationSessionContext context)
		{
			if (this.coreOrchestrator == null)
			{
				Debug.LogError("[CustomizationBootstrapper] Missing CustomizationCoreOrchestrator reference.");
				return false;
			}
			if (context == null || !context.IsValid())
			{
				Debug.LogError("[CustomizationBootstrapper] Invalid customization session context.");
				return false;
			}
			this.DespawnExistingRuntime(context);
			Transform fallbackSpawnPoint = this.GetFallbackSpawnPoint(context.targetType);
			GameObject gameObject = this.SpawnRuntimeInstance(context, fallbackSpawnPoint);
			if (gameObject != null)
			{
				context.runtimeInstance = gameObject;
			}
			this.ApplyStateManagerBindings(context.targetType);
			if (!this.coreOrchestrator.InitializeSession(context))
			{
				if (gameObject != null)
				{
					this.DespawnRuntimeInstance(gameObject);
					context.runtimeInstance = null;
				}
				return false;
			}
			if (this.verboseLogs)
			{
				Debug.Log(string.Format("[CustomizationBootstrapper] Session bootstrapped for {0}, characterId: {1}", context.targetType, context.characterId));
			}
			return true;
		}

		// Token: 0x06004FB3 RID: 20403 RVA: 0x001EE12B File Offset: 0x001EC32B
		public void CloseActiveSession()
		{
			if (this.coreOrchestrator != null)
			{
				this.coreOrchestrator.CloseSession();
			}
		}

		// Token: 0x06004FB4 RID: 20404 RVA: 0x001EE146 File Offset: 0x001EC346
		private void OnDestroy()
		{
			if (this.closeSessionOnDestroy && this.coreOrchestrator != null)
			{
				this.coreOrchestrator.CloseSession();
			}
		}

		// Token: 0x06004FB5 RID: 20405 RVA: 0x001EE16C File Offset: 0x001EC36C
		private void CacheSpawnProviders()
		{
			this._spawnProviders.Clear();
			for (int i = 0; i < this.spawnProviderBehaviours.Count; i++)
			{
				ICustomizationSpawnProvider customizationSpawnProvider = this.spawnProviderBehaviours[i] as ICustomizationSpawnProvider;
				if (customizationSpawnProvider != null)
				{
					this._spawnProviders.Add(customizationSpawnProvider);
				}
			}
			MonoBehaviour[] componentsInChildren = base.GetComponentsInChildren<MonoBehaviour>(true);
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				ICustomizationSpawnProvider customizationSpawnProvider2 = componentsInChildren[j] as ICustomizationSpawnProvider;
				if (customizationSpawnProvider2 != null && !this._spawnProviders.Contains(customizationSpawnProvider2))
				{
					this._spawnProviders.Add(customizationSpawnProvider2);
				}
			}
		}

		// Token: 0x06004FB6 RID: 20406 RVA: 0x001EE1FB File Offset: 0x001EC3FB
		private Transform GetFallbackSpawnPoint(CustomizationTargetType targetType)
		{
			if (targetType != CustomizationTargetType.MainCharacter)
			{
				return this.defaultNpcSpawnPoint;
			}
			return this.defaultMainCharacterSpawnPoint;
		}

		// Token: 0x06004FB7 RID: 20407 RVA: 0x001EE210 File Offset: 0x001EC410
		private GameObject SpawnRuntimeInstance(CustomizationSessionContext context, Transform fallbackSpawnPoint)
		{
			this._activeSpawnProvider = null;
			for (int i = 0; i < this._spawnProviders.Count; i++)
			{
				ICustomizationSpawnProvider customizationSpawnProvider = this._spawnProviders[i];
				if (customizationSpawnProvider.CanSpawn(context))
				{
					GameObject gameObject = customizationSpawnProvider.Spawn(context, fallbackSpawnPoint);
					if (!(gameObject == null))
					{
						this._activeSpawnProvider = customizationSpawnProvider;
						return gameObject;
					}
				}
			}
			return null;
		}

		// Token: 0x06004FB8 RID: 20408 RVA: 0x001EE26C File Offset: 0x001EC46C
		private void DespawnExistingRuntime(CustomizationSessionContext context)
		{
			if (((context != null) ? context.runtimeInstance : null) == null)
			{
				return;
			}
			this.DespawnRuntimeInstance(context.runtimeInstance);
			context.runtimeInstance = null;
		}

		// Token: 0x06004FB9 RID: 20409 RVA: 0x001EE296 File Offset: 0x001EC496
		private void DespawnRuntimeInstance(GameObject runtimeInstance)
		{
			if (this._activeSpawnProvider != null)
			{
				this._activeSpawnProvider.Despawn(runtimeInstance);
				return;
			}
			Object.Destroy(runtimeInstance);
		}

		// Token: 0x06004FBA RID: 20410 RVA: 0x001EE2B4 File Offset: 0x001EC4B4
		private void ApplyStateManagerBindings(CustomizationTargetType targetType)
		{
			if (this.stateManager == null)
			{
				return;
			}
			CustomizationSceneBootstrapper.CharacterTargetBindings characterTargetBindings = ((targetType == CustomizationTargetType.MainCharacter) ? this.mainCharacterBindings : this.npcBindings);
			this.stateManager.ConfigureRuntimeBindings((characterTargetBindings != null) ? characterTargetBindings.styleManager : null, (characterTargetBindings != null) ? characterTargetBindings.generalInfoManager : null, (characterTargetBindings != null) ? characterTargetBindings.characterCreatorManager : null, this.sharedCharactersManager, true, false);
		}

		// Token: 0x040050A1 RID: 20641
		[Header("References")]
		[SerializeField]
		private CustomizationCoreOrchestrator coreOrchestrator;

		// Token: 0x040050A2 RID: 20642
		[Header("State Bindings")]
		[SerializeField]
		private CharacterStateManager stateManager;

		// Token: 0x040050A3 RID: 20643
		[SerializeField]
		private CharactersManager sharedCharactersManager;

		// Token: 0x040050A4 RID: 20644
		[SerializeField]
		private CustomizationSceneBootstrapper.CharacterTargetBindings mainCharacterBindings = new CustomizationSceneBootstrapper.CharacterTargetBindings();

		// Token: 0x040050A5 RID: 20645
		[SerializeField]
		private CustomizationSceneBootstrapper.CharacterTargetBindings npcBindings = new CustomizationSceneBootstrapper.CharacterTargetBindings();

		// Token: 0x040050A6 RID: 20646
		[Header("Spawn")]
		[SerializeField]
		private Transform defaultMainCharacterSpawnPoint;

		// Token: 0x040050A7 RID: 20647
		[SerializeField]
		private Transform defaultNpcSpawnPoint;

		// Token: 0x040050A8 RID: 20648
		[SerializeField]
		private List<MonoBehaviour> spawnProviderBehaviours = new List<MonoBehaviour>();

		// Token: 0x040050A9 RID: 20649
		[Header("Flow")]
		[SerializeField]
		private bool autoBootstrapOnStart = true;

		// Token: 0x040050AA RID: 20650
		[SerializeField]
		private bool closeSessionOnDestroy = true;

		// Token: 0x040050AB RID: 20651
		[SerializeField]
		private CustomizationSceneBootstrapper.FallbackEntryMode fallbackEntryMode = CustomizationSceneBootstrapper.FallbackEntryMode.Hub;

		// Token: 0x040050AC RID: 20652
		[SerializeField]
		private string fallbackMainCharacterId = "Ciel";

		// Token: 0x040050AD RID: 20653
		[SerializeField]
		private string fallbackNpcCharacterId = "Npc_Debug";

		// Token: 0x040050AE RID: 20654
		[Header("Debug")]
		[SerializeField]
		private bool verboseLogs;

		// Token: 0x040050AF RID: 20655
		private readonly List<ICustomizationSpawnProvider> _spawnProviders = new List<ICustomizationSpawnProvider>();

		// Token: 0x040050B0 RID: 20656
		private ICustomizationSpawnProvider _activeSpawnProvider;

		// Token: 0x02000AFF RID: 2815
		[Serializable]
		private class CharacterTargetBindings
		{
			// Token: 0x040050B2 RID: 20658
			public StyleManager styleManager;

			// Token: 0x040050B3 RID: 20659
			public CharacterGeneralInfoManager generalInfoManager;

			// Token: 0x040050B4 RID: 20660
			public CharacterCreatorManager characterCreatorManager;
		}

		// Token: 0x02000B00 RID: 2816
		public enum FallbackEntryMode
		{
			// Token: 0x040050B6 RID: 20662
			None,
			// Token: 0x040050B7 RID: 20663
			Hub,
			// Token: 0x040050B8 RID: 20664
			MainCharacter,
			// Token: 0x040050B9 RID: 20665
			Npc
		}
	}
}
