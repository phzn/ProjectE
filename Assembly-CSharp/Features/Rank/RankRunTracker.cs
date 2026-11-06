using System;
using System.Collections.Generic;
using ExhibitionistDemo.UI;
using Features.Detection;
using Features.PlayerEmotes;
using Features.RunModes;
using UnityEngine;

namespace Features.Rank
{
	// Token: 0x02000DB6 RID: 3510
	public class RankRunTracker : MonoBehaviour
	{
		// Token: 0x17001110 RID: 4368
		// (get) Token: 0x0600677E RID: 26494 RVA: 0x00296188 File Offset: 0x00294388
		public float PeakDistance
		{
			get
			{
				return this._peakDistance;
			}
		}

		// Token: 0x17001111 RID: 4369
		// (get) Token: 0x0600677F RID: 26495 RVA: 0x00296190 File Offset: 0x00294390
		public int ValidEmotes
		{
			get
			{
				return this._validEmotes;
			}
		}

		// Token: 0x06006780 RID: 26496 RVA: 0x00296198 File Offset: 0x00294398
		private void OnEnable()
		{
			this.ResolveSources();
			if (this._emoteController != null)
			{
				this._emoteController.OnEmoteStarted += this.HandleEmoteStarted;
			}
		}

		// Token: 0x06006781 RID: 26497 RVA: 0x002961C5 File Offset: 0x002943C5
		private void OnDisable()
		{
			if (this._emoteController != null)
			{
				this._emoteController.OnEmoteStarted -= this.HandleEmoteStarted;
			}
		}

		// Token: 0x06006782 RID: 26498 RVA: 0x002961EC File Offset: 0x002943EC
		public void Freeze()
		{
			this._frozen = true;
		}

		// Token: 0x06006783 RID: 26499 RVA: 0x002961F8 File Offset: 0x002943F8
		private void Update()
		{
			if (this._frozen || LongWayHomeRuntimeController.IsPreparing)
			{
				return;
			}
			if (Time.time < this._nextDistanceSampleTime)
			{
				return;
			}
			this._nextDistanceSampleTime = Time.time + 0.5f;
			if (this._distanceTracker == null)
			{
				this._distanceTracker = Object.FindObjectOfType<DistanceTracker>();
				if (this._distanceTracker == null)
				{
					return;
				}
			}
			float distance = this._distanceTracker.GetDistance();
			if (distance > this._peakDistance)
			{
				this._peakDistance = distance;
			}
		}

		// Token: 0x06006784 RID: 26500 RVA: 0x00296278 File Offset: 0x00294478
		public void ResetForNewRun()
		{
			this._frozen = false;
			this._peakDistance = 0f;
			this._validEmotes = 0;
			this._nextDistanceSampleTime = 0f;
			this.ResolveSources();
			if (this._emoteController != null)
			{
				this._emoteController.OnEmoteStarted -= this.HandleEmoteStarted;
				this._emoteController.OnEmoteStarted += this.HandleEmoteStarted;
			}
		}

		// Token: 0x06006785 RID: 26501 RVA: 0x002962EC File Offset: 0x002944EC
		public RunResult BuildRunResult(int timesSeen, int scorePoints)
		{
			return new RunResult
			{
				peakDistanceMeters = this._peakDistance,
				timesSeen = timesSeen,
				validEmotes = this._validEmotes,
				scorePoints = scorePoints
			};
		}

		// Token: 0x06006786 RID: 26502 RVA: 0x0029632C File Offset: 0x0029452C
		private void ResolveSources()
		{
			if (this._distanceTracker == null)
			{
				this._distanceTracker = Object.FindObjectOfType<DistanceTracker>();
			}
			if (this._emoteController == null)
			{
				this._emoteController = Object.FindObjectOfType<GameplayEmoteController>();
			}
		}

		// Token: 0x06006787 RID: 26503 RVA: 0x00296360 File Offset: 0x00294560
		private void HandleEmoteStarted(EmoteDefinitionSO emote)
		{
			if (this._frozen || LongWayHomeRuntimeController.IsPreparing)
			{
				return;
			}
			if (this.IsAnyNpcNearby())
			{
				this._validEmotes++;
			}
		}

		// Token: 0x06006788 RID: 26504 RVA: 0x00296388 File Offset: 0x00294588
		private bool IsAnyNpcNearby()
		{
			RankProgressionConfigSO config = RankProgressionService.Config;
			float num = ((config != null) ? config.emoteValidityRadius : 15f);
			float num2 = num * num;
			PlayerProximityProvider instance = PlayerProximityProvider.Instance;
			if (instance == null)
			{
				return false;
			}
			Vector3 position = instance.Position;
			SensoryManager instance2 = SensoryManager.Instance;
			if (instance2 == null)
			{
				return false;
			}
			IReadOnlyList<NPCSenses> registeredNPCs = instance2.GetRegisteredNPCs();
			if (registeredNPCs == null)
			{
				return false;
			}
			for (int i = 0; i < registeredNPCs.Count; i++)
			{
				NPCSenses npcsenses = registeredNPCs[i];
				if (!(npcsenses == null) && (npcsenses.transform.position - position).sqrMagnitude <= num2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04006A16 RID: 27158
		private const float DistanceSampleInterval = 0.5f;

		// Token: 0x04006A17 RID: 27159
		private DistanceTracker _distanceTracker;

		// Token: 0x04006A18 RID: 27160
		private GameplayEmoteController _emoteController;

		// Token: 0x04006A19 RID: 27161
		private float _peakDistance;

		// Token: 0x04006A1A RID: 27162
		private int _validEmotes;

		// Token: 0x04006A1B RID: 27163
		private float _nextDistanceSampleTime;

		// Token: 0x04006A1C RID: 27164
		private bool _frozen;
	}
}
