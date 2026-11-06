using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000202 RID: 514
	public class NavigationMoveEvent : NavigationEventBase<NavigationMoveEvent>
	{
		// Token: 0x06000F29 RID: 3881 RVA: 0x00038E53 File Offset: 0x00037053
		static NavigationMoveEvent()
		{
			EventBase<NavigationMoveEvent>.SetCreateFunction(() => new NavigationMoveEvent());
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00038E6C File Offset: 0x0003706C
		internal static NavigationMoveEvent.Direction DetermineMoveDirection(float x, float y, float deadZone = 0.6f)
		{
			bool flag = new Vector2(x, y).sqrMagnitude < deadZone * deadZone;
			NavigationMoveEvent.Direction direction;
			if (flag)
			{
				direction = NavigationMoveEvent.Direction.None;
			}
			else
			{
				bool flag2 = Mathf.Abs(x) > Mathf.Abs(y);
				if (flag2)
				{
					bool flag3 = x > 0f;
					if (flag3)
					{
						direction = NavigationMoveEvent.Direction.Right;
					}
					else
					{
						direction = NavigationMoveEvent.Direction.Left;
					}
				}
				else
				{
					bool flag4 = y > 0f;
					if (flag4)
					{
						direction = NavigationMoveEvent.Direction.Up;
					}
					else
					{
						direction = NavigationMoveEvent.Direction.Down;
					}
				}
			}
			return direction;
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x00038ED7 File Offset: 0x000370D7
		// (set) Token: 0x06000F2C RID: 3884 RVA: 0x00038EDF File Offset: 0x000370DF
		public NavigationMoveEvent.Direction direction { get; private set; }

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000F2D RID: 3885 RVA: 0x00038EE8 File Offset: 0x000370E8
		// (set) Token: 0x06000F2E RID: 3886 RVA: 0x00038EF0 File Offset: 0x000370F0
		public Vector2 move { get; private set; }

		// Token: 0x06000F2F RID: 3887 RVA: 0x00038EFC File Offset: 0x000370FC
		public static NavigationMoveEvent GetPooled(Vector2 moveVector, EventModifiers modifiers = EventModifiers.None)
		{
			NavigationMoveEvent pooled = NavigationEventBase<NavigationMoveEvent>.GetPooled(NavigationDeviceType.Unknown, modifiers);
			pooled.direction = NavigationMoveEvent.DetermineMoveDirection(moveVector.x, moveVector.y, 0.6f);
			pooled.move = moveVector;
			return pooled;
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00038F3C File Offset: 0x0003713C
		internal static NavigationMoveEvent GetPooled(Vector2 moveVector, NavigationDeviceType deviceType, EventModifiers modifiers = EventModifiers.None)
		{
			NavigationMoveEvent pooled = NavigationEventBase<NavigationMoveEvent>.GetPooled(deviceType, modifiers);
			pooled.direction = NavigationMoveEvent.DetermineMoveDirection(moveVector.x, moveVector.y, 0.6f);
			pooled.move = moveVector;
			return pooled;
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00038F7C File Offset: 0x0003717C
		public static NavigationMoveEvent GetPooled(NavigationMoveEvent.Direction direction, EventModifiers modifiers = EventModifiers.None)
		{
			NavigationMoveEvent pooled = NavigationEventBase<NavigationMoveEvent>.GetPooled(NavigationDeviceType.Unknown, modifiers);
			pooled.direction = direction;
			pooled.move = Vector2.zero;
			return pooled;
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00038FAC File Offset: 0x000371AC
		internal static NavigationMoveEvent GetPooled(NavigationMoveEvent.Direction direction, NavigationDeviceType deviceType, EventModifiers modifiers = EventModifiers.None)
		{
			NavigationMoveEvent pooled = NavigationEventBase<NavigationMoveEvent>.GetPooled(deviceType, modifiers);
			pooled.direction = direction;
			pooled.move = Vector2.zero;
			return pooled;
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00038FDB File Offset: 0x000371DB
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00038FEC File Offset: 0x000371EC
		public NavigationMoveEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00038FFD File Offset: 0x000371FD
		private void LocalInit()
		{
			this.direction = NavigationMoveEvent.Direction.None;
			this.move = Vector2.zero;
		}

		// Token: 0x02000203 RID: 515
		public enum Direction
		{
			// Token: 0x040006DF RID: 1759
			None,
			// Token: 0x040006E0 RID: 1760
			Left,
			// Token: 0x040006E1 RID: 1761
			Up,
			// Token: 0x040006E2 RID: 1762
			Right,
			// Token: 0x040006E3 RID: 1763
			Down,
			// Token: 0x040006E4 RID: 1764
			Next,
			// Token: 0x040006E5 RID: 1765
			Previous
		}
	}
}
