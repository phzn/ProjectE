using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x02000130 RID: 304
	public class DebugUIHandlerObjectPopupField : DebugUIHandlerField<DebugUI.ObjectPopupField>
	{
		// Token: 0x0600090A RID: 2314 RVA: 0x00029FD2 File Offset: 0x000281D2
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Index = 0;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00029FE4 File Offset: 0x000281E4
		private void ChangeSelectedObject()
		{
			if (this.m_Field == null)
			{
				return;
			}
			IEnumerable<Object> enumerable = this.m_Field.getObjects();
			if (enumerable == null)
			{
				return;
			}
			Object[] array = enumerable.ToArray<Object>();
			int num = array.Count<Object>();
			if (this.m_Index >= num)
			{
				this.m_Index = 0;
			}
			else if (this.m_Index < 0)
			{
				this.m_Index = num - 1;
			}
			Object @object = array[this.m_Index];
			this.m_Field.SetValue(@object);
			this.UpdateValueLabel();
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0002A05A File Offset: 0x0002825A
		public override void OnIncrement(bool fast)
		{
			this.m_Index++;
			this.ChangeSelectedObject();
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0002A070 File Offset: 0x00028270
		public override void OnDecrement(bool fast)
		{
			this.m_Index--;
			this.ChangeSelectedObject();
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0002A088 File Offset: 0x00028288
		public override void UpdateValueLabel()
		{
			Object value = this.m_Field.GetValue();
			string text = ((value != null) ? value.name : "Empty");
			base.SetLabelText(text);
		}

		// Token: 0x04000549 RID: 1353
		private int m_Index;
	}
}
