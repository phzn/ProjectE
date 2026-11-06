using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.LowLevel;

namespace Unity.Entities
{
	// Token: 0x02000273 RID: 627
	internal static class RuntimeApplication
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060010DA RID: 4314 RVA: 0x00047E58 File Offset: 0x00046058
		// (remove) Token: 0x060010DB RID: 4315 RVA: 0x00047E8C File Offset: 0x0004608C
		public static event Action PreFrameUpdate;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060010DC RID: 4316 RVA: 0x00047EC0 File Offset: 0x000460C0
		// (remove) Token: 0x060010DD RID: 4317 RVA: 0x00047EF4 File Offset: 0x000460F4
		public static event Action PostFrameUpdate;

		// Token: 0x060010DE RID: 4318 RVA: 0x00047F27 File Offset: 0x00046127
		internal static void InvokePreFrameUpdate()
		{
			Action preFrameUpdate = RuntimeApplication.PreFrameUpdate;
			if (preFrameUpdate == null)
			{
				return;
			}
			preFrameUpdate();
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x00047F38 File Offset: 0x00046138
		internal static void InvokePostFrameUpdate()
		{
			Action postFrameUpdate = RuntimeApplication.PostFrameUpdate;
			if (postFrameUpdate == null)
			{
				return;
			}
			postFrameUpdate();
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00047F4C File Offset: 0x0004614C
		internal static void RegisterFrameUpdateToCurrentPlayerLoop()
		{
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			List<PlayerLoopSystem> list = currentPlayerLoop.subSystemList.ToList<PlayerLoopSystem>();
			if (!list.Any<PlayerLoopSystem>((PlayerLoopSystem s) => s.type == typeof(RuntimeApplication.UpdatePreFrame)))
			{
				list.Insert(0, new PlayerLoopSystem
				{
					type = typeof(RuntimeApplication.UpdatePreFrame),
					updateDelegate = new PlayerLoopSystem.UpdateFunction(RuntimeApplication.InvokePreFrameUpdate)
				});
			}
			if (!list.Any<PlayerLoopSystem>((PlayerLoopSystem s) => s.type == typeof(RuntimeApplication.UpdatePostFrame)))
			{
				list.Add(new PlayerLoopSystem
				{
					type = typeof(RuntimeApplication.UpdatePostFrame),
					updateDelegate = new PlayerLoopSystem.UpdateFunction(RuntimeApplication.InvokePostFrameUpdate)
				});
			}
			currentPlayerLoop.subSystemList = list.ToArray();
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00048034 File Offset: 0x00046234
		internal static void UnregisterFrameUpdateToCurrentPlayerLoop()
		{
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			List<PlayerLoopSystem> list = currentPlayerLoop.subSystemList.ToList<PlayerLoopSystem>();
			list.RemoveAll((PlayerLoopSystem s) => s.type == typeof(RuntimeApplication.UpdatePreFrame));
			list.RemoveAll((PlayerLoopSystem s) => s.type == typeof(RuntimeApplication.UpdatePostFrame));
			currentPlayerLoop.subSystemList = list.ToArray();
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
		}

		// Token: 0x02000274 RID: 628
		private sealed class UpdatePreFrame
		{
		}

		// Token: 0x02000275 RID: 629
		private sealed class UpdatePostFrame
		{
		}
	}
}
