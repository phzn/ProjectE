using System;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000F4 RID: 244
	internal static class InputUpdate
	{
		// Token: 0x06000E78 RID: 3704 RVA: 0x0004756D File Offset: 0x0004576D
		internal static void OnBeforeUpdate(InputUpdateType type)
		{
			InputUpdate.s_LatestUpdateType = type;
			if (type - InputUpdateType.Dynamic <= 1 || type == InputUpdateType.Manual)
			{
				InputUpdate.s_PlayerUpdateStepCount.OnBeforeUpdate();
				InputUpdate.s_UpdateStepCount = InputUpdate.s_PlayerUpdateStepCount.value;
			}
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00047599 File Offset: 0x00045799
		internal static void OnUpdate(InputUpdateType type)
		{
			InputUpdate.s_LatestUpdateType = type;
			if (type - InputUpdateType.Dynamic <= 1 || type == InputUpdateType.Manual)
			{
				InputUpdate.s_PlayerUpdateStepCount.OnUpdate();
				InputUpdate.s_UpdateStepCount = InputUpdate.s_PlayerUpdateStepCount.value;
			}
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x000475C8 File Offset: 0x000457C8
		public static InputUpdate.SerializedState Save()
		{
			return new InputUpdate.SerializedState
			{
				lastUpdateType = InputUpdate.s_LatestUpdateType,
				playerUpdateStepCount = InputUpdate.s_PlayerUpdateStepCount
			};
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x000475F8 File Offset: 0x000457F8
		public static void Restore(InputUpdate.SerializedState state)
		{
			InputUpdate.s_LatestUpdateType = state.lastUpdateType;
			InputUpdate.s_PlayerUpdateStepCount = state.playerUpdateStepCount;
			InputUpdateType inputUpdateType = InputUpdate.s_LatestUpdateType;
			if (inputUpdateType - InputUpdateType.Dynamic <= 1 || inputUpdateType == InputUpdateType.Manual)
			{
				InputUpdate.s_UpdateStepCount = InputUpdate.s_PlayerUpdateStepCount.value;
				return;
			}
			InputUpdate.s_UpdateStepCount = 0U;
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00047642 File Offset: 0x00045842
		public static InputUpdateType GetUpdateTypeForPlayer(this InputUpdateType mask)
		{
			if ((mask & InputUpdateType.Manual) != InputUpdateType.None)
			{
				return InputUpdateType.Manual;
			}
			if ((mask & InputUpdateType.Dynamic) != InputUpdateType.None)
			{
				return InputUpdateType.Dynamic;
			}
			if ((mask & InputUpdateType.Fixed) != InputUpdateType.None)
			{
				return InputUpdateType.Fixed;
			}
			return InputUpdateType.None;
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x0004765C File Offset: 0x0004585C
		public static bool IsPlayerUpdate(this InputUpdateType updateType)
		{
			return updateType != InputUpdateType.Editor && updateType > InputUpdateType.None;
		}

		// Token: 0x040005F5 RID: 1525
		public static uint s_UpdateStepCount;

		// Token: 0x040005F6 RID: 1526
		public static InputUpdateType s_LatestUpdateType;

		// Token: 0x040005F7 RID: 1527
		public static InputUpdate.UpdateStepCount s_PlayerUpdateStepCount;

		// Token: 0x0200021E RID: 542
		[Serializable]
		public struct UpdateStepCount
		{
			// Token: 0x170005B3 RID: 1459
			// (get) Token: 0x06001531 RID: 5425 RVA: 0x00062C48 File Offset: 0x00060E48
			// (set) Token: 0x06001532 RID: 5426 RVA: 0x00062C50 File Offset: 0x00060E50
			public uint value { readonly get; private set; }

			// Token: 0x06001533 RID: 5427 RVA: 0x00062C5C File Offset: 0x00060E5C
			public void OnBeforeUpdate()
			{
				this.m_WasUpdated = true;
				uint value = this.value;
				this.value = value + 1U;
			}

			// Token: 0x06001534 RID: 5428 RVA: 0x00062C80 File Offset: 0x00060E80
			public void OnUpdate()
			{
				if (!this.m_WasUpdated)
				{
					uint value = this.value;
					this.value = value + 1U;
				}
				this.m_WasUpdated = false;
			}

			// Token: 0x04000BBB RID: 3003
			private bool m_WasUpdated;
		}

		// Token: 0x0200021F RID: 543
		[Serializable]
		public struct SerializedState
		{
			// Token: 0x04000BBD RID: 3005
			public InputUpdateType lastUpdateType;

			// Token: 0x04000BBE RID: 3006
			public InputUpdate.UpdateStepCount playerUpdateStepCount;
		}
	}
}
