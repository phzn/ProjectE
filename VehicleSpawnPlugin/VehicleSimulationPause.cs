using System;
using System.Collections;
using UnityEngine;

namespace VehicleSpawnPlugin
{
	// Token: 0x02000006 RID: 6
	public static class VehicleSimulationPause
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00004EA0 File Offset: 0x000030A0
		public static bool IsPaused
		{
			get
			{
				return VehicleSimulationPause._manuallyPaused || VehicleSimulationPause._leaseCount > 0;
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004EB3 File Offset: 0x000030B3
		public static void SetPaused(bool paused)
		{
			VehicleSimulationPause._manuallyPaused = paused;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004EBB File Offset: 0x000030BB
		public static IDisposable AcquirePause()
		{
			VehicleSimulationPause._leaseCount++;
			return new VehicleSimulationPause.PauseLease(VehicleSimulationPause._generation);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004ED3 File Offset: 0x000030D3
		internal static IEnumerator WaitForSimulationSeconds(float seconds)
		{
			float remaining = Mathf.Max(0f, seconds);
			while (remaining > 0f || VehicleSimulationPause.IsPaused)
			{
				yield return null;
				if (!VehicleSimulationPause.IsPaused)
				{
					remaining -= Time.deltaTime;
				}
			}
			yield break;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004EE2 File Offset: 0x000030E2
		internal static IEnumerator WaitForSimulationResume()
		{
			while (VehicleSimulationPause.IsPaused)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004EEA File Offset: 0x000030EA
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			VehicleSimulationPause._manuallyPaused = false;
			VehicleSimulationPause._leaseCount = 0;
			VehicleSimulationPause._generation++;
		}

		// Token: 0x04000062 RID: 98
		private static bool _manuallyPaused;

		// Token: 0x04000063 RID: 99
		private static int _leaseCount;

		// Token: 0x04000064 RID: 100
		private static int _generation;

		// Token: 0x0200001C RID: 28
		private sealed class PauseLease : IDisposable
		{
			// Token: 0x060000D3 RID: 211 RVA: 0x00007936 File Offset: 0x00005B36
			public PauseLease(int leaseGeneration)
			{
				this._leaseGeneration = leaseGeneration;
			}

			// Token: 0x060000D4 RID: 212 RVA: 0x00007945 File Offset: 0x00005B45
			public void Dispose()
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				if (this._leaseGeneration == VehicleSimulationPause._generation && VehicleSimulationPause._leaseCount > 0)
				{
					VehicleSimulationPause._leaseCount--;
				}
			}

			// Token: 0x040000E1 RID: 225
			private readonly int _leaseGeneration;

			// Token: 0x040000E2 RID: 226
			private bool _disposed;
		}
	}
}
