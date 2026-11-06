using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000190 RID: 400
	public abstract class MouseCaptureEventBase<T> : PointerCaptureEventBase<T>, IMouseCaptureEvent where T : MouseCaptureEventBase<T>, new()
	{
		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x00031DB3 File Offset: 0x0002FFB3
		public new IEventHandler relatedTarget
		{
			get
			{
				return base.relatedTarget;
			}
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x00031DBC File Offset: 0x0002FFBC
		public static T GetPooled(IEventHandler target, IEventHandler relatedTarget)
		{
			return PointerCaptureEventBase<T>.GetPooled(target, relatedTarget, 0);
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x00031DD8 File Offset: 0x0002FFD8
		protected override void Init()
		{
			base.Init();
		}
	}
}
