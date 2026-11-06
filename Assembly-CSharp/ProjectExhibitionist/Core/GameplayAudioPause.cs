using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A52 RID: 2642
	public static class GameplayAudioPause
	{
		// Token: 0x06004B3A RID: 19258 RVA: 0x001D47C0 File Offset: 0x001D29C0
		public static IDisposable Acquire()
		{
			if (GameplayAudioPause.Owners.Count == 0)
			{
				GameplayAudioPause._baseline = AudioListener.pause;
			}
			GameplayAudioPause.Scope scope = new GameplayAudioPause.Scope();
			GameplayAudioPause.Owners.Add(scope);
			AudioListener.pause = true;
			return scope;
		}

		// Token: 0x06004B3B RID: 19259 RVA: 0x001D47FC File Offset: 0x001D29FC
		public static bool ResolveAfterExternalRestore(bool baseline)
		{
			if (GameplayAudioPause.Owners.Count == 0)
			{
				return baseline;
			}
			GameplayAudioPause._baseline = baseline;
			return true;
		}

		// Token: 0x06004B3C RID: 19260 RVA: 0x001D4813 File Offset: 0x001D2A13
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRuntimeState()
		{
			GameplayAudioPause.Owners.Clear();
			GameplayAudioPause._baseline = false;
			AudioListener.pause = false;
		}

		// Token: 0x04004D19 RID: 19737
		private static readonly HashSet<IDisposable> Owners = new HashSet<IDisposable>();

		// Token: 0x04004D1A RID: 19738
		private static bool _baseline;

		// Token: 0x02000A53 RID: 2643
		private sealed class Scope : IDisposable
		{
			// Token: 0x06004B3E RID: 19262 RVA: 0x001D4837 File Offset: 0x001D2A37
			public void Dispose()
			{
				if (!GameplayAudioPause.Owners.Remove(this))
				{
					return;
				}
				AudioListener.pause = GameplayAudioPause.Owners.Count > 0 || GameplayAudioPause._baseline;
			}
		}
	}
}
