using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000278 RID: 632
	public abstract class MouseManipulator : Manipulator
	{
		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x00040FA1 File Offset: 0x0003F1A1
		// (set) Token: 0x060011E5 RID: 4581 RVA: 0x00040FA9 File Offset: 0x0003F1A9
		public List<ManipulatorActivationFilter> activators { get; private set; }

		// Token: 0x060011E6 RID: 4582 RVA: 0x00040FB2 File Offset: 0x0003F1B2
		protected MouseManipulator()
		{
			this.activators = new List<ManipulatorActivationFilter>();
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x00040FC8 File Offset: 0x0003F1C8
		protected bool CanStartManipulation(IMouseEvent e)
		{
			foreach (ManipulatorActivationFilter manipulatorActivationFilter in this.activators)
			{
				bool flag = manipulatorActivationFilter.Matches(e);
				if (flag)
				{
					this.m_currentActivator = manipulatorActivationFilter;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x00041038 File Offset: 0x0003F238
		protected bool CanStopManipulation(IMouseEvent e)
		{
			bool flag = e == null;
			return !flag && e.button == (int)this.m_currentActivator.button;
		}

		// Token: 0x040007F0 RID: 2032
		private ManipulatorActivationFilter m_currentActivator;
	}
}
