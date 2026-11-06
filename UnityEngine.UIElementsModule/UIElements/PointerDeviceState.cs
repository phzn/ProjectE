using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000210 RID: 528
	internal static class PointerDeviceState
	{
		// Token: 0x06000F58 RID: 3928 RVA: 0x00039338 File Offset: 0x00037538
		internal static void Reset()
		{
			for (int i = 0; i < PointerId.maxPointers; i++)
			{
				PointerDeviceState.s_PlayerPointerLocations[i].SetLocation(Vector2.zero, null);
				PointerDeviceState.s_PressedButtons[i] = 0;
				PointerDeviceState.s_PlayerPanelWithSoftPointerCapture[i] = null;
			}
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x00039384 File Offset: 0x00037584
		internal static void RemovePanelData(IPanel panel)
		{
			for (int i = 0; i < PointerId.maxPointers; i++)
			{
				bool flag = PointerDeviceState.s_PlayerPointerLocations[i].Panel == panel;
				if (flag)
				{
					PointerDeviceState.s_PlayerPointerLocations[i].SetLocation(Vector2.zero, null);
				}
				bool flag2 = PointerDeviceState.s_PlayerPanelWithSoftPointerCapture[i] == panel;
				if (flag2)
				{
					PointerDeviceState.s_PlayerPanelWithSoftPointerCapture[i] = null;
				}
			}
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x000393EC File Offset: 0x000375EC
		public static void SavePointerPosition(int pointerId, Vector2 position, IPanel panel, ContextType contextType)
		{
			if (contextType > ContextType.Editor)
			{
			}
			PointerDeviceState.s_PlayerPointerLocations[pointerId].SetLocation(position, panel);
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x00039419 File Offset: 0x00037619
		public static void PressButton(int pointerId, int buttonId)
		{
			Debug.Assert(buttonId >= 0);
			Debug.Assert(buttonId < 32);
			PointerDeviceState.s_PressedButtons[pointerId] |= 1 << buttonId;
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x00039449 File Offset: 0x00037649
		public static void ReleaseButton(int pointerId, int buttonId)
		{
			Debug.Assert(buttonId >= 0);
			Debug.Assert(buttonId < 32);
			PointerDeviceState.s_PressedButtons[pointerId] &= ~(1 << buttonId);
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0003947A File Offset: 0x0003767A
		public static void ReleaseAllButtons(int pointerId)
		{
			PointerDeviceState.s_PressedButtons[pointerId] = 0;
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x00039488 File Offset: 0x00037688
		public static Vector2 GetPointerPosition(int pointerId, ContextType contextType)
		{
			if (contextType > ContextType.Editor)
			{
			}
			return PointerDeviceState.s_PlayerPointerLocations[pointerId].Position;
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x000394B4 File Offset: 0x000376B4
		public static IPanel GetPanel(int pointerId, ContextType contextType)
		{
			if (contextType > ContextType.Editor)
			{
			}
			return PointerDeviceState.s_PlayerPointerLocations[pointerId].Panel;
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x000394E0 File Offset: 0x000376E0
		private static bool HasFlagFast(PointerDeviceState.LocationFlag flagSet, PointerDeviceState.LocationFlag flag)
		{
			return (flagSet & flag) == flag;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x000394F8 File Offset: 0x000376F8
		public static bool HasLocationFlag(int pointerId, ContextType contextType, PointerDeviceState.LocationFlag flag)
		{
			if (contextType > ContextType.Editor)
			{
			}
			return PointerDeviceState.HasFlagFast(PointerDeviceState.s_PlayerPointerLocations[pointerId].Flags, flag);
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x0003952C File Offset: 0x0003772C
		public static int GetPressedButtons(int pointerId)
		{
			return PointerDeviceState.s_PressedButtons[pointerId];
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x00039548 File Offset: 0x00037748
		internal static bool HasAdditionalPressedButtons(int pointerId, int exceptButtonId)
		{
			return (PointerDeviceState.s_PressedButtons[pointerId] & ~(1 << exceptButtonId)) != 0;
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0003956C File Offset: 0x0003776C
		internal static void SetPlayerPanelWithSoftPointerCapture(int pointerId, IPanel panel)
		{
			PointerDeviceState.s_PlayerPanelWithSoftPointerCapture[pointerId] = panel;
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x00039578 File Offset: 0x00037778
		internal static IPanel GetPlayerPanelWithSoftPointerCapture(int pointerId)
		{
			return PointerDeviceState.s_PlayerPanelWithSoftPointerCapture[pointerId];
		}

		// Token: 0x040006ED RID: 1773
		private static PointerDeviceState.PointerLocation[] s_PlayerPointerLocations = new PointerDeviceState.PointerLocation[PointerId.maxPointers];

		// Token: 0x040006EE RID: 1774
		private static int[] s_PressedButtons = new int[PointerId.maxPointers];

		// Token: 0x040006EF RID: 1775
		private static readonly IPanel[] s_PlayerPanelWithSoftPointerCapture = new IPanel[PointerId.maxPointers];

		// Token: 0x02000211 RID: 529
		[Flags]
		internal enum LocationFlag
		{
			// Token: 0x040006F1 RID: 1777
			None = 0,
			// Token: 0x040006F2 RID: 1778
			OutsidePanel = 1
		}

		// Token: 0x02000212 RID: 530
		private struct PointerLocation
		{
			// Token: 0x17000331 RID: 817
			// (get) Token: 0x06000F67 RID: 3943 RVA: 0x000395C0 File Offset: 0x000377C0
			// (set) Token: 0x06000F68 RID: 3944 RVA: 0x000395C8 File Offset: 0x000377C8
			internal Vector2 Position { readonly get; private set; }

			// Token: 0x17000332 RID: 818
			// (get) Token: 0x06000F69 RID: 3945 RVA: 0x000395D1 File Offset: 0x000377D1
			// (set) Token: 0x06000F6A RID: 3946 RVA: 0x000395D9 File Offset: 0x000377D9
			internal IPanel Panel { readonly get; private set; }

			// Token: 0x17000333 RID: 819
			// (get) Token: 0x06000F6B RID: 3947 RVA: 0x000395E2 File Offset: 0x000377E2
			// (set) Token: 0x06000F6C RID: 3948 RVA: 0x000395EA File Offset: 0x000377EA
			internal PointerDeviceState.LocationFlag Flags { readonly get; private set; }

			// Token: 0x06000F6D RID: 3949 RVA: 0x000395F4 File Offset: 0x000377F4
			internal void SetLocation(Vector2 position, IPanel panel)
			{
				this.Position = position;
				this.Panel = panel;
				this.Flags = PointerDeviceState.LocationFlag.None;
				bool flag = panel == null || !panel.visualTree.layout.Contains(position);
				if (flag)
				{
					this.Flags |= PointerDeviceState.LocationFlag.OutsidePanel;
				}
			}
		}
	}
}
