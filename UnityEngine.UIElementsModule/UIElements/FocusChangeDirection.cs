using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000246 RID: 582
	public class FocusChangeDirection : IDisposable
	{
		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001092 RID: 4242 RVA: 0x0003C0F3 File Offset: 0x0003A2F3
		public static FocusChangeDirection unspecified { get; } = new FocusChangeDirection(-1);

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001093 RID: 4243 RVA: 0x0003C0FA File Offset: 0x0003A2FA
		public static FocusChangeDirection none { get; } = new FocusChangeDirection(0);

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x0003C101 File Offset: 0x0003A301
		protected static FocusChangeDirection lastValue { get; } = FocusChangeDirection.none;

		// Token: 0x06001095 RID: 4245 RVA: 0x0003C108 File Offset: 0x0003A308
		protected FocusChangeDirection(int value)
		{
			this.m_Value = value;
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0003C11C File Offset: 0x0003A31C
		public static implicit operator int(FocusChangeDirection fcd)
		{
			return (fcd != null) ? fcd.m_Value : 0;
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0003C13A File Offset: 0x0003A33A
		void IDisposable.Dispose()
		{
			this.Dispose();
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x00003CD2 File Offset: 0x00001ED2
		protected virtual void Dispose()
		{
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0003C143 File Offset: 0x0003A343
		internal virtual void ApplyTo(FocusController focusController, Focusable f)
		{
			focusController.SwitchFocus(f, this, false, DispatchMode.Default);
		}

		// Token: 0x0400074B RID: 1867
		private readonly int m_Value;
	}
}
