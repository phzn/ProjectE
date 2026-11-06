using System;
using System.Collections;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x02000124 RID: 292
	public class DebugUIHandlerEnumHistory : DebugUIHandlerEnumField
	{
		// Token: 0x060008C2 RID: 2242 RVA: 0x0002922C File Offset: 0x0002742C
		internal override void SetWidget(DebugUI.Widget widget)
		{
			DebugUI.HistoryEnumField historyEnumField = widget as DebugUI.HistoryEnumField;
			int num = ((historyEnumField != null) ? historyEnumField.historyDepth : 0);
			this.historyValues = new Text[num];
			float num2 = ((num > 0) ? (230f / (float)num) : 0f);
			for (int i = 0; i < num; i++)
			{
				Text text = Object.Instantiate<Text>(this.valueLabel, base.transform);
				Vector3 position = text.transform.position;
				position.x += (float)(i + 1) * num2;
				text.transform.position = position;
				Text component = text.GetComponent<Text>();
				component.color = new Color32(110, 110, 110, byte.MaxValue);
				this.historyValues[i] = component;
			}
			base.SetWidget(widget);
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x000292E8 File Offset: 0x000274E8
		public override void UpdateValueLabel()
		{
			int num = this.m_Field.currentIndex;
			if (num < 0)
			{
				num = 0;
			}
			this.valueLabel.text = this.m_Field.enumNames[num].text;
			DebugUI.HistoryEnumField historyEnumField = this.m_Field as DebugUI.HistoryEnumField;
			int num2 = ((historyEnumField != null) ? historyEnumField.historyDepth : 0);
			for (int i = 0; i < num2; i++)
			{
				if (i < this.historyValues.Length && this.historyValues[i] != null)
				{
					this.historyValues[i].text = historyEnumField.enumNames[historyEnumField.GetHistoryValue(i)].text;
				}
			}
			if (base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.RefreshAfterSanitization());
			}
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00029399 File Offset: 0x00027599
		private IEnumerator RefreshAfterSanitization()
		{
			yield return null;
			this.m_Field.currentIndex = this.m_Field.getIndex();
			this.valueLabel.text = this.m_Field.enumNames[this.m_Field.currentIndex].text;
			yield break;
		}

		// Token: 0x0400051B RID: 1307
		private Text[] historyValues;

		// Token: 0x0400051C RID: 1308
		private const float k_XOffset = 230f;
	}
}
