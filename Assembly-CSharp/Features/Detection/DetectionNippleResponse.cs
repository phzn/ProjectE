using System;
using System.Collections.Generic;
using BoZo.CharacterCreator;
using Features.NPC;
using Features.TimeFreeze;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001272 RID: 4722
	public sealed class DetectionNippleResponse
	{
		// Token: 0x060092D7 RID: 37591 RVA: 0x003C424A File Offset: 0x003C244A
		public static bool IsObserved(bool hasVisualContact, float attention)
		{
			return hasVisualContact && attention > 0.2f;
		}

		// Token: 0x060092D8 RID: 37592 RVA: 0x003C425C File Offset: 0x003C245C
		public void Update(StealthActor player, SensoryManager sensory)
		{
			if (player == null || !player.isActiveAndEnabled || sensory == null || !sensory.isActiveAndEnabled || !sensory.IsRunModeDetectionEnabled || DetectionTransitionGate.IsBlocked)
			{
				this.Clear();
				return;
			}
			BlendShapeManager blendShapeManager = BlendShapeManager.ResolveForRig(player);
			if (sensory.IsPaused || NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f || TimeFreezeController.IsAnyActive)
			{
				this.Apply(blendShapeManager, false, Time.deltaTime, true);
				return;
			}
			if (PlayerStealthBridge.IsFullyHidden)
			{
				this.Apply(blendShapeManager, false, Time.deltaTime, false);
				return;
			}
			bool flag = false;
			IReadOnlyList<NPCSenses> registeredNPCs = sensory.GetRegisteredNPCs();
			for (int i = 0; i < registeredNPCs.Count; i++)
			{
				NPCSenses npcsenses = registeredNPCs[i];
				if (!(npcsenses == null) && npcsenses.isActiveAndEnabled && !npcsenses.IsInCooldown)
				{
					IDetectionUISource detectionUISource = ((npcsenses.AttentionUI != null) ? npcsenses.AttentionUI.UISource : null);
					if (DetectionNippleResponse.IsObserved((detectionUISource != null) ? detectionUISource.VisualActive : npcsenses.HasLineOfSight, (detectionUISource != null) ? detectionUISource.DetectionLevel : npcsenses.CurrentAttention))
					{
						flag = true;
						break;
					}
				}
			}
			this.Apply(blendShapeManager, flag, Time.deltaTime, false);
		}

		// Token: 0x060092D9 RID: 37593 RVA: 0x003C4398 File Offset: 0x003C2598
		public void Apply(BlendShapeManager manager, bool observed, float deltaTime, bool paused = false)
		{
			if (this._manager != manager)
			{
				this.Clear();
				this._manager = manager;
			}
			if (manager == null)
			{
				this.Clear();
				return;
			}
			if (paused || deltaTime <= 0f)
			{
				return;
			}
			float level = this._level;
			if (observed)
			{
				this._level = Mathf.MoveTowards(this._level, 1f, deltaTime / 3.5f);
				this._holdRemaining = 50f;
			}
			else
			{
				float num = Mathf.Min(this._holdRemaining, deltaTime);
				this._holdRemaining -= num;
				this._level = Mathf.MoveTowards(this._level, 0f, (deltaTime - num) / 2.5f);
			}
			if (this._level <= 0f)
			{
				this.ReleaseOverrides();
				return;
			}
			if (this._applied && Mathf.Approximately(level, this._level))
			{
				return;
			}
			manager.SetOverride("Nipple Depth", 30f * this._level, "DetectionNippleResponse", BlendShapePriority.Emotion, false);
			manager.SetOverride("Areola Depth", 35f * this._level, "DetectionNippleResponse", BlendShapePriority.Emotion, false);
			this._applied = true;
		}

		// Token: 0x060092DA RID: 37594 RVA: 0x003C44BB File Offset: 0x003C26BB
		public void Clear()
		{
			this.ReleaseOverrides();
			this._manager = null;
			this._level = 0f;
			this._holdRemaining = 0f;
		}

		// Token: 0x060092DB RID: 37595 RVA: 0x003C44E0 File Offset: 0x003C26E0
		private void ReleaseOverrides()
		{
			if (this._applied && this._manager != null)
			{
				this._manager.RemoveAllOverridesFromSource("DetectionNippleResponse");
			}
			this._applied = false;
		}

		// Token: 0x04009771 RID: 38769
		public const float AttentionThreshold = 0.2f;

		// Token: 0x04009772 RID: 38770
		public const string NippleShape = "Nipple Depth";

		// Token: 0x04009773 RID: 38771
		public const string AreolaShape = "Areola Depth";

		// Token: 0x04009774 RID: 38772
		public const float NippleWeight = 30f;

		// Token: 0x04009775 RID: 38773
		public const float AreolaWeight = 35f;

		// Token: 0x04009776 RID: 38774
		public const float RiseSeconds = 3.5f;

		// Token: 0x04009777 RID: 38775
		public const float HoldSeconds = 50f;

		// Token: 0x04009778 RID: 38776
		public const float FallSeconds = 2.5f;

		// Token: 0x04009779 RID: 38777
		private const string SourceId = "DetectionNippleResponse";

		// Token: 0x0400977A RID: 38778
		private BlendShapeManager _manager;

		// Token: 0x0400977B RID: 38779
		private bool _applied;

		// Token: 0x0400977C RID: 38780
		private float _level;

		// Token: 0x0400977D RID: 38781
		private float _holdRemaining;
	}
}
