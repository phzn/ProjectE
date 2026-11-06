using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000275 RID: 629
	public abstract class Manipulator : IManipulator
	{
		// Token: 0x060011DA RID: 4570
		protected abstract void RegisterCallbacksOnTarget();

		// Token: 0x060011DB RID: 4571
		protected abstract void UnregisterCallbacksFromTarget();

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x060011DC RID: 4572 RVA: 0x00040E48 File Offset: 0x0003F048
		// (set) Token: 0x060011DD RID: 4573 RVA: 0x00040E60 File Offset: 0x0003F060
		public VisualElement target
		{
			get
			{
				return this.m_Target;
			}
			set
			{
				bool flag = this.target != null;
				if (flag)
				{
					this.UnregisterCallbacksFromTarget();
				}
				this.m_Target = value;
				bool flag2 = this.target != null;
				if (flag2)
				{
					this.RegisterCallbacksOnTarget();
				}
			}
		}

		// Token: 0x040007E8 RID: 2024
		private VisualElement m_Target;
	}
}
