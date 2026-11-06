using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200029F RID: 671
	public abstract class PointerManipulator : MouseManipulator
	{
		// Token: 0x06001344 RID: 4932 RVA: 0x00043530 File Offset: 0x00041730
		protected bool CanStartManipulation(IPointerEvent e)
		{
			foreach (ManipulatorActivationFilter manipulatorActivationFilter in base.activators)
			{
				bool flag = manipulatorActivationFilter.Matches(e);
				if (flag)
				{
					this.m_CurrentPointerId = e.pointerId;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x000435A4 File Offset: 0x000417A4
		protected bool CanStopManipulation(IPointerEvent e)
		{
			bool flag = e == null;
			return !flag && e.pointerId == this.m_CurrentPointerId;
		}

		// Token: 0x040008BC RID: 2236
		private int m_CurrentPointerId;
	}
}
