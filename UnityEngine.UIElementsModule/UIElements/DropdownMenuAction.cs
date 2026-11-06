using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200017C RID: 380
	public class DropdownMenuAction : DropdownMenuItem
	{
		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000C28 RID: 3112 RVA: 0x00030E7C File Offset: 0x0002F07C
		public string name { get; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000C29 RID: 3113 RVA: 0x00030E84 File Offset: 0x0002F084
		// (set) Token: 0x06000C2A RID: 3114 RVA: 0x00030E8C File Offset: 0x0002F08C
		public DropdownMenuAction.Status status { get; private set; }

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x00030E95 File Offset: 0x0002F095
		// (set) Token: 0x06000C2C RID: 3116 RVA: 0x00030E9D File Offset: 0x0002F09D
		public DropdownMenuEventInfo eventInfo { get; private set; }

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000C2D RID: 3117 RVA: 0x00030EA6 File Offset: 0x0002F0A6
		// (set) Token: 0x06000C2E RID: 3118 RVA: 0x00030EAE File Offset: 0x0002F0AE
		public object userData { get; private set; }

		// Token: 0x06000C2F RID: 3119 RVA: 0x00030EB8 File Offset: 0x0002F0B8
		public static DropdownMenuAction.Status AlwaysEnabled(DropdownMenuAction a)
		{
			return DropdownMenuAction.Status.Normal;
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x00030ECC File Offset: 0x0002F0CC
		public static DropdownMenuAction.Status AlwaysDisabled(DropdownMenuAction a)
		{
			return DropdownMenuAction.Status.Disabled;
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x00030EDF File Offset: 0x0002F0DF
		public DropdownMenuAction(string actionName, Action<DropdownMenuAction> actionCallback, Func<DropdownMenuAction, DropdownMenuAction.Status> actionStatusCallback, object userData = null)
		{
			this.name = actionName;
			this.actionCallback = actionCallback;
			this.actionStatusCallback = actionStatusCallback;
			this.userData = userData;
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x00030F07 File Offset: 0x0002F107
		public void UpdateActionStatus(DropdownMenuEventInfo eventInfo)
		{
			this.eventInfo = eventInfo;
			Func<DropdownMenuAction, DropdownMenuAction.Status> func = this.actionStatusCallback;
			this.status = ((func != null) ? func(this) : DropdownMenuAction.Status.Hidden);
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x00030F2C File Offset: 0x0002F12C
		public void Execute()
		{
			Action<DropdownMenuAction> action = this.actionCallback;
			if (action != null)
			{
				action(this);
			}
		}

		// Token: 0x040005CB RID: 1483
		private readonly Action<DropdownMenuAction> actionCallback;

		// Token: 0x040005CC RID: 1484
		private readonly Func<DropdownMenuAction, DropdownMenuAction.Status> actionStatusCallback;

		// Token: 0x0200017D RID: 381
		[Flags]
		public enum Status
		{
			// Token: 0x040005CE RID: 1486
			None = 0,
			// Token: 0x040005CF RID: 1487
			Normal = 1,
			// Token: 0x040005D0 RID: 1488
			Disabled = 2,
			// Token: 0x040005D1 RID: 1489
			Checked = 4,
			// Token: 0x040005D2 RID: 1490
			Hidden = 8
		}
	}
}
