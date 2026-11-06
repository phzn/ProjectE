using System;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Unity.Entities.Content
{
	// Token: 0x020004BB RID: 1211
	public static class RuntimeContentSystem
	{
		// Token: 0x06001D2B RID: 7467 RVA: 0x000660DE File Offset: 0x000642DE
		[RuntimeInitializeOnLoadMethod]
		private static void RuntimeInitialize()
		{
			RuntimeContentSystem.LoadContentCatalog(null, null, null, false);
			RuntimeContentSystem.AddToPlayerLoop();
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x000660EE File Offset: 0x000642EE
		private static void RemoveFromPlayerLoop()
		{
			ScriptBehaviourUpdateOrder.RemoveFromCurrentPlayerLoop(new PlayerLoopSystem.UpdateFunction(RuntimeContentSystem.Update));
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x00066104 File Offset: 0x00064304
		private static void AddToPlayerLoop()
		{
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			int num = ((currentPlayerLoop.subSystemList != null) ? currentPlayerLoop.subSystemList.Length : 0);
			PlayerLoopSystem[] array = new PlayerLoopSystem[num + 1];
			for (int i = 0; i < num; i++)
			{
				array[i] = currentPlayerLoop.subSystemList[i];
			}
			array[num] = new PlayerLoopSystem
			{
				type = typeof(RuntimeContentSystem),
				updateDelegate = new PlayerLoopSystem.UpdateFunction(RuntimeContentSystem.Update)
			};
			currentPlayerLoop.subSystemList = array;
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x00066196 File Offset: 0x00064396
		private static void Update()
		{
			if (RuntimeContentManager.IsReady)
			{
				RuntimeContentManager.ProcessQueuedCommands();
			}
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x000661A4 File Offset: 0x000643A4
		public static void LoadContentCatalog(string remoteUrlRoot, string localCachePath, string initialContentSet, bool allowOverrideArgs = false)
		{
			RuntimeContentSystem.LoadCatalogFunc((string p) => Application.streamingAssetsPath + "/" + p);
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x000661CC File Offset: 0x000643CC
		private static void LoadCatalogFunc(Func<string, string> remapFunc)
		{
			string text = remapFunc(RuntimeContentManager.RelativeCatalogPath);
			if (ContentDeliveryGlobalState.FileExists(text))
			{
				RuntimeContentManager.LoadLocalCatalogData(text, new Func<string, string>(RuntimeContentManager.DefaultContentFileNameFunc), (string p) => remapFunc(RuntimeContentManager.DefaultArchivePathFunc(p)));
			}
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x0006621E File Offset: 0x0006441E
		private static bool TryGetAppArg(string name, ref string value)
		{
			if (!Application.HasARGV(name))
			{
				return false;
			}
			value = Application.GetValueForARGV(name);
			return true;
		}
	}
}
