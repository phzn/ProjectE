using System;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x02000123 RID: 291
	public class DebugUIHandlerEnumField : DebugUIHandlerField<DebugUI.EnumField>
	{
		// Token: 0x060008BE RID: 2238 RVA: 0x00029054 File Offset: 0x00027254
		public override void OnIncrement(bool fast)
		{
			if (this.m_Field.enumValues.Length == 0)
			{
				return;
			}
			int[] enumValues = this.m_Field.enumValues;
			int num = this.m_Field.currentIndex;
			if (num == enumValues.Length - 1)
			{
				num = 0;
			}
			else if (fast)
			{
				int[] array = this.m_Field.quickSeparators;
				if (array == null)
				{
					this.m_Field.InitQuickSeparators();
					array = this.m_Field.quickSeparators;
				}
				int num2 = 0;
				while (num2 < array.Length && num + 1 > array[num2])
				{
					num2++;
				}
				if (num2 == array.Length)
				{
					num = 0;
				}
				else
				{
					num = array[num2];
				}
			}
			else
			{
				num++;
			}
			this.m_Field.SetValue(enumValues[num]);
			this.m_Field.currentIndex = num;
			this.UpdateValueLabel();
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00029108 File Offset: 0x00027308
		public override void OnDecrement(bool fast)
		{
			if (this.m_Field.enumValues.Length == 0)
			{
				return;
			}
			int[] enumValues = this.m_Field.enumValues;
			int num = this.m_Field.currentIndex;
			if (num == 0)
			{
				if (fast)
				{
					int[] array = this.m_Field.quickSeparators;
					if (array == null)
					{
						this.m_Field.InitQuickSeparators();
						array = this.m_Field.quickSeparators;
					}
					num = array[array.Length - 1];
				}
				else
				{
					num = enumValues.Length - 1;
				}
			}
			else if (fast)
			{
				int[] array2 = this.m_Field.quickSeparators;
				if (array2 == null)
				{
					this.m_Field.InitQuickSeparators();
					array2 = this.m_Field.quickSeparators;
				}
				int num2 = array2.Length - 1;
				while (num2 > 0 && num <= array2[num2])
				{
					num2--;
				}
				num = array2[num2];
			}
			else
			{
				num--;
			}
			this.m_Field.SetValue(enumValues[num]);
			this.m_Field.currentIndex = num;
			this.UpdateValueLabel();
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x000291EC File Offset: 0x000273EC
		public override void UpdateValueLabel()
		{
			int num = this.m_Field.currentIndex;
			if (num < 0)
			{
				num = 0;
			}
			base.SetLabelText(this.m_Field.enumNames[num].text);
		}
	}
}
