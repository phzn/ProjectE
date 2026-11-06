using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Backends
{
	// Token: 0x02000B1E RID: 2846
	public class BozoNpcCustomizationBackend : CharacterCustomizationBackendBase
	{
		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x06005096 RID: 20630 RVA: 0x001F324E File Offset: 0x001F144E
		public override string BackendId
		{
			get
			{
				return "bozo-npc";
			}
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x06005097 RID: 20631 RVA: 0x001F3255 File Offset: 0x001F1455
		public override CustomizationFeature SupportedFeatures
		{
			get
			{
				return CustomizationFeature.BlendShapes | CustomizationFeature.Styles | CustomizationFeature.Presets;
			}
		}

		// Token: 0x06005098 RID: 20632 RVA: 0x001F3259 File Offset: 0x001F1459
		public override bool CanInitialize(CustomizationSessionContext context)
		{
			return context != null && !(this.ResolveOutfitSystem() == null) && (context.targetType == CustomizationTargetType.Npc || (this.allowMainCharacterFallback && context.targetType == CustomizationTargetType.MainCharacter));
		}

		// Token: 0x06005099 RID: 20633 RVA: 0x001F3290 File Offset: 0x001F1490
		private OutfitSystem ResolveOutfitSystem()
		{
			if (this.outfitSystem != null)
			{
				return this.outfitSystem;
			}
			PrefabCustomizationSpawnProvider component = base.GetComponent<PrefabCustomizationSpawnProvider>();
			if (component != null && component.PreSpawnedTarget != null)
			{
				this.outfitSystem = component.PreSpawnedTarget.GetComponentInChildren<OutfitSystem>(true);
			}
			if (this.outfitSystem == null)
			{
				this.outfitSystem = Object.FindObjectOfType<OutfitSystem>(true);
			}
			return this.outfitSystem;
		}

		// Token: 0x0600509A RID: 20634 RVA: 0x001F3302 File Offset: 0x001F1502
		public override bool Initialize(CustomizationSessionContext context)
		{
			if (!base.Initialize(context))
			{
				return false;
			}
			this.ResolveOutfitSystem();
			this._runtimeBlendShapes.Clear();
			if (this.stateManager != null)
			{
				this.stateManager.SetActiveContext(CustomizationTargetType.Npc);
			}
			return true;
		}

		// Token: 0x0600509B RID: 20635 RVA: 0x001F333C File Offset: 0x001F153C
		public override void Shutdown()
		{
			this._runtimeBlendShapes.Clear();
			base.Shutdown();
		}

		// Token: 0x0600509C RID: 20636 RVA: 0x001F334F File Offset: 0x001F154F
		public override bool TrySetBlendShape(string blendShapeName, float value)
		{
			if (!base.IsInitialized || this.outfitSystem == null || string.IsNullOrWhiteSpace(blendShapeName))
			{
				return false;
			}
			this.outfitSystem.SetShape(blendShapeName, value);
			this._runtimeBlendShapes[blendShapeName] = value;
			return true;
		}

		// Token: 0x0600509D RID: 20637 RVA: 0x001F338C File Offset: 0x001F158C
		public override bool TryGetBlendShape(string blendShapeName, out float value)
		{
			value = 0f;
			if (!base.IsInitialized || this.outfitSystem == null || string.IsNullOrWhiteSpace(blendShapeName))
			{
				return false;
			}
			if (this._runtimeBlendShapes.TryGetValue(blendShapeName, out value))
			{
				return true;
			}
			float shapeValue = this.outfitSystem.GetShapeValue(blendShapeName);
			if (shapeValue < 0f)
			{
				return false;
			}
			value = shapeValue;
			this._runtimeBlendShapes[blendShapeName] = shapeValue;
			return true;
		}

		// Token: 0x0600509E RID: 20638 RVA: 0x001F33FC File Offset: 0x001F15FC
		public override CharacterStateData CaptureState()
		{
			if (this.stateManager != null)
			{
				return this.stateManager.ConvertToCharacterStateData(CustomizationTargetType.Npc);
			}
			CharacterStateData characterStateData = new CharacterStateData();
			foreach (KeyValuePair<string, float> keyValuePair in this._runtimeBlendShapes)
			{
				characterStateData.blendShapes.Add(new BlendShapeValue("Runtime", "Runtime", keyValuePair.Key, keyValuePair.Value));
			}
			return characterStateData;
		}

		// Token: 0x0600509F RID: 20639 RVA: 0x001F3494 File Offset: 0x001F1694
		public override bool ApplyState(CharacterStateData stateData)
		{
			if (!base.IsInitialized || stateData == null)
			{
				return false;
			}
			if (this.stateManager != null)
			{
				this.stateManager.SetActiveContext(CustomizationTargetType.Npc);
				this.stateManager.LoadFromCharacterStateData(stateData, CustomizationTargetType.Npc);
			}
			for (int i = 0; i < stateData.blendShapes.Count; i++)
			{
				BlendShapeValue blendShapeValue = stateData.blendShapes[i];
				if (blendShapeValue != null && !string.IsNullOrWhiteSpace(blendShapeValue.blendShapeName))
				{
					this.TrySetBlendShape(blendShapeValue.blendShapeName, blendShapeValue.value);
				}
			}
			return true;
		}

		// Token: 0x04005140 RID: 20800
		[Header("References")]
		[SerializeField]
		private OutfitSystem outfitSystem;

		// Token: 0x04005141 RID: 20801
		[SerializeField]
		private CharacterStateManager stateManager;

		// Token: 0x04005142 RID: 20802
		[Header("Config")]
		[SerializeField]
		private bool allowMainCharacterFallback;

		// Token: 0x04005143 RID: 20803
		private readonly Dictionary<string, float> _runtimeBlendShapes = new Dictionary<string, float>();
	}
}
