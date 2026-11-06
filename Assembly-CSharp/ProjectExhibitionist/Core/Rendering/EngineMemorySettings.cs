using System;
using UnityEngine;

namespace ProjectExhibitionist.Core.Rendering
{
	// Token: 0x02000A8E RID: 2702
	public static class EngineMemorySettings
	{
		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06004CE0 RID: 19680 RVA: 0x001DDE73 File Offset: 0x001DC073
		public static int SystemMemoryMB
		{
			get
			{
				if (EngineMemorySettings._simulatedMemoryMB < 0)
				{
					EngineMemorySettings._simulatedMemoryMB = EngineMemorySettings.ReadSimulatedMemoryMB();
				}
				if (EngineMemorySettings._simulatedMemoryMB <= 0)
				{
					return SystemInfo.systemMemorySize;
				}
				return EngineMemorySettings._simulatedMemoryMB;
			}
		}

		// Token: 0x06004CE1 RID: 19681 RVA: 0x001DDE9C File Offset: 0x001DC09C
		private static int ReadSimulatedMemoryMB()
		{
			try
			{
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				int num = 0;
				while (num + 1 < commandLineArgs.Length)
				{
					int num2;
					if (string.Equals(commandLineArgs[num], "-simulateSystemMemoryMB", StringComparison.OrdinalIgnoreCase) && int.TryParse(commandLineArgs[num + 1], out num2) && num2 > 0)
					{
						return num2;
					}
					num++;
				}
			}
			catch (Exception)
			{
			}
			return 0;
		}

		// Token: 0x06004CE2 RID: 19682 RVA: 0x001DDF00 File Offset: 0x001DC100
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Apply()
		{
			if (Application.isEditor)
			{
				return;
			}
			int systemMemoryMB = EngineMemorySettings.SystemMemoryMB;
			bool flag = systemMemoryMB > 0 && systemMemoryMB < 24576;
			if (flag)
			{
				Shader.maximumChunksOverride = 64;
			}
			QualitySettings.asyncUploadPersistentBuffer = false;
			Debug.Log(string.Concat(new string[]
			{
				string.Format("[EngineMemorySettings] RAM {0} MB", systemMemoryMB),
				(EngineMemorySettings._simulatedMemoryMB > 0) ? string.Format(" (simulated, real {0} MB)", SystemInfo.systemMemorySize) : "",
				": shader chunk limit ",
				flag ? string.Format("{0} per shader", Shader.maximumChunksOverride) : "unchanged (unlimited)",
				string.Format(", async upload persistent buffer {0}.", QualitySettings.asyncUploadPersistentBuffer)
			}));
		}

		// Token: 0x04004E66 RID: 20070
		private const int LowMemoryThresholdMB = 24576;

		// Token: 0x04004E67 RID: 20071
		private const int LowMemoryShaderChunkLimit = 64;

		// Token: 0x04004E68 RID: 20072
		private const string SimulatedMemoryArgument = "-simulateSystemMemoryMB";

		// Token: 0x04004E69 RID: 20073
		private static int _simulatedMemoryMB = -1;
	}
}
