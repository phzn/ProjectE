using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000403 RID: 1027
	internal class VisualElementFocusChangeTarget : FocusChangeDirection
	{
		// Token: 0x060020E9 RID: 8425 RVA: 0x0007C7F4 File Offset: 0x0007A9F4
		public static VisualElementFocusChangeTarget GetPooled(Focusable target)
		{
			VisualElementFocusChangeTarget visualElementFocusChangeTarget = VisualElementFocusChangeTarget.Pool.Get();
			visualElementFocusChangeTarget.target = target;
			return visualElementFocusChangeTarget;
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x0007C81A File Offset: 0x0007AA1A
		protected override void Dispose()
		{
			this.target = null;
			VisualElementFocusChangeTarget.Pool.Release(this);
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x0007C831 File Offset: 0x0007AA31
		internal override void ApplyTo(FocusController focusController, Focusable f)
		{
			focusController.selectedTextElement = null;
			f.Focus();
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x0007C843 File Offset: 0x0007AA43
		public VisualElementFocusChangeTarget()
			: base(FocusChangeDirection.unspecified)
		{
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x060020ED RID: 8429 RVA: 0x0007C857 File Offset: 0x0007AA57
		// (set) Token: 0x060020EE RID: 8430 RVA: 0x0007C85F File Offset: 0x0007AA5F
		public Focusable target { get; private set; }

		// Token: 0x04000DED RID: 3565
		private static readonly ObjectPool<VisualElementFocusChangeTarget> Pool = new ObjectPool<VisualElementFocusChangeTarget>(() => new VisualElementFocusChangeTarget(), 100);
	}
}
