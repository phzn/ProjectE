using System;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x0200012F RID: 303
	public class DebugUIHandlerObjectList : DebugUIHandlerField<DebugUI.ObjectListField>
	{
		// Token: 0x06000905 RID: 2309 RVA: 0x00029F3F File Offset: 0x0002813F
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Index = 0;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00029F4F File Offset: 0x0002814F
		public override void OnIncrement(bool fast)
		{
			this.m_Index++;
			this.UpdateValueLabel();
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00029F65 File Offset: 0x00028165
		public override void OnDecrement(bool fast)
		{
			this.m_Index--;
			this.UpdateValueLabel();
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00029F7C File Offset: 0x0002817C
		public override void UpdateValueLabel()
		{
			string text = "Empty";
			Object[] value = this.m_Field.GetValue();
			if (value != null)
			{
				this.m_Index = Math.Clamp(this.m_Index, 0, value.Length - 1);
				text = value[this.m_Index].name;
			}
			base.SetLabelText(text);
		}

		// Token: 0x04000548 RID: 1352
		private int m_Index;
	}
}
