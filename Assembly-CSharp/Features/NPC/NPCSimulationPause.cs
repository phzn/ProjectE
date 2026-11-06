using System;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EED RID: 3821
	public static class NPCSimulationPause
	{
		// Token: 0x170012AA RID: 4778
		// (get) Token: 0x0600726F RID: 29295 RVA: 0x002E97BC File Offset: 0x002E79BC
		public static bool IsPaused
		{
			get
			{
				return NPCSimulationPause._compatibilityPause || NPCSimulationPause._pauseLeaseCount > 0;
			}
		}

		// Token: 0x170012AB RID: 4779
		// (get) Token: 0x06007270 RID: 29296 RVA: 0x002E97CF File Offset: 0x002E79CF
		public static float SimulationScale
		{
			get
			{
				return NPCSimulationPause._simulationScale;
			}
		}

		// Token: 0x170012AC RID: 4780
		// (get) Token: 0x06007271 RID: 29297 RVA: 0x002E97D6 File Offset: 0x002E79D6
		public static float SimulationDeltaTime
		{
			get
			{
				if (!NPCSimulationPause.IsPaused)
				{
					return Time.deltaTime * NPCSimulationPause._simulationScale;
				}
				return 0f;
			}
		}

		// Token: 0x170012AD RID: 4781
		// (get) Token: 0x06007272 RID: 29298 RVA: 0x002E97F0 File Offset: 0x002E79F0
		public static float FrameTimeLoss
		{
			get
			{
				if (!NPCSimulationPause.IsPaused)
				{
					return Time.deltaTime * (1f - NPCSimulationPause._simulationScale);
				}
				return 0f;
			}
		}

		// Token: 0x140000C5 RID: 197
		// (add) Token: 0x06007273 RID: 29299 RVA: 0x002E9810 File Offset: 0x002E7A10
		// (remove) Token: 0x06007274 RID: 29300 RVA: 0x002E9844 File Offset: 0x002E7A44
		public static event Action<bool> PauseChanged;

		// Token: 0x06007275 RID: 29301 RVA: 0x002E9877 File Offset: 0x002E7A77
		public static void SetPaused(bool paused)
		{
			if (NPCSimulationPause._compatibilityPause == paused)
			{
				return;
			}
			bool isPaused = NPCSimulationPause.IsPaused;
			NPCSimulationPause._compatibilityPause = paused;
			NPCSimulationPause.ApplyEffectiveStateIfChanged(isPaused);
		}

		// Token: 0x06007276 RID: 29302 RVA: 0x002E9892 File Offset: 0x002E7A92
		public static IDisposable AcquirePause()
		{
			bool isPaused = NPCSimulationPause.IsPaused;
			NPCSimulationPause._pauseLeaseCount++;
			NPCSimulationPause.ApplyEffectiveStateIfChanged(isPaused);
			return new NPCSimulationPause.PauseLease();
		}

		// Token: 0x06007277 RID: 29303 RVA: 0x002E98AF File Offset: 0x002E7AAF
		public static void RegisterParticipant(Component component)
		{
			if (!NPCSimulationPause.IsPaused || component == null)
			{
				return;
			}
			NPCSimulationPauseDriver.GetOrCreate().Register(component);
		}

		// Token: 0x06007278 RID: 29304 RVA: 0x002E98CD File Offset: 0x002E7ACD
		internal static void UnregisterParticipant(Transform root)
		{
			if (!NPCSimulationPause.IsPaused || root == null)
			{
				return;
			}
			NPCSimulationPauseDriver.ParticipantDisabledCurrent(root);
		}

		// Token: 0x06007279 RID: 29305 RVA: 0x002E98E6 File Offset: 0x002E7AE6
		internal static void PrepareParticipantForRagdoll(NPCRagdollRecovery recovery)
		{
			if (!NPCSimulationPause.IsPaused || recovery == null)
			{
				return;
			}
			NPCSimulationPauseDriver.PrepareForRagdollCurrent(recovery.transform);
		}

		// Token: 0x0600727A RID: 29306 RVA: 0x002E9904 File Offset: 0x002E7B04
		internal static void RestoreParticipantAfterRagdoll(NPCRagdollRecovery recovery)
		{
			if (recovery == null)
			{
				return;
			}
			NPCSimulationPauseDriver.RestoreAfterRagdollCurrent(recovery.transform, recovery);
		}

		// Token: 0x0600727B RID: 29307 RVA: 0x002E991C File Offset: 0x002E7B1C
		internal static void SetSimulationScale(float scale)
		{
			NPCSimulationPause._simulationScale = Mathf.Clamp01(scale);
		}

		// Token: 0x0600727C RID: 29308 RVA: 0x002E9929 File Offset: 0x002E7B29
		private static void ReleasePauseLease()
		{
			if (NPCSimulationPause._pauseLeaseCount <= 0)
			{
				return;
			}
			bool isPaused = NPCSimulationPause.IsPaused;
			NPCSimulationPause._pauseLeaseCount--;
			NPCSimulationPause.ApplyEffectiveStateIfChanged(isPaused);
		}

		// Token: 0x0600727D RID: 29309 RVA: 0x002E994C File Offset: 0x002E7B4C
		private static void ApplyEffectiveStateIfChanged(bool wasPaused)
		{
			bool isPaused = NPCSimulationPause.IsPaused;
			if (isPaused == wasPaused)
			{
				return;
			}
			if (isPaused)
			{
				NPCSimulationPauseDriver.GetOrCreate().BeginPause();
			}
			else
			{
				NPCSimulationPauseDriver.EndCurrentPause();
			}
			Action<bool> pauseChanged = NPCSimulationPause.PauseChanged;
			if (pauseChanged == null)
			{
				return;
			}
			pauseChanged(isPaused);
		}

		// Token: 0x0600727E RID: 29310 RVA: 0x002E9988 File Offset: 0x002E7B88
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			NPCSimulationPause._compatibilityPause = false;
			NPCSimulationPause._pauseLeaseCount = 0;
			NPCSimulationPause._simulationScale = 1f;
			NPCSimulationPause.PauseChanged = null;
			NPCSimulationPauseDriver.ResetStaticState();
		}

		// Token: 0x0400759F RID: 30111
		private static bool _compatibilityPause;

		// Token: 0x040075A0 RID: 30112
		private static int _pauseLeaseCount;

		// Token: 0x040075A1 RID: 30113
		private static float _simulationScale = 1f;

		// Token: 0x02000EEE RID: 3822
		private sealed class PauseLease : IDisposable
		{
			// Token: 0x06007280 RID: 29312 RVA: 0x002E99B7 File Offset: 0x002E7BB7
			public void Dispose()
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				NPCSimulationPause.ReleasePauseLease();
			}

			// Token: 0x040075A3 RID: 30115
			private bool _disposed;
		}
	}
}
