using System;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x02000127 RID: 295
	public class DebugUIHandlerFoldout : DebugUIHandlerWidget
	{
		// Token: 0x060008D5 RID: 2261 RVA: 0x00029640 File Offset: 0x00027840
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Field = base.CastWidget<DebugUI.Foldout>();
			this.m_Container = base.GetComponent<DebugUIHandlerContainer>();
			this.nameLabel.text = this.m_Field.displayName;
			string[] columnLabels = this.m_Field.columnLabels;
			int num = ((columnLabels != null) ? columnLabels.Length : 0);
			float num2 = ((num > 0) ? (230f / (float)num) : 0f);
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.nameLabel.gameObject, base.GetComponent<DebugUIHandlerContainer>().contentHolder);
				gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
				RectTransform rectTransform = gameObject.transform as RectTransform;
				RectTransform rectTransform2 = this.nameLabel.transform as RectTransform;
				Vector2 vector = new Vector2(0f, 1f);
				rectTransform.anchorMin = vector;
				rectTransform.anchorMax = vector;
				rectTransform.sizeDelta = new Vector2(100f, 26f);
				Vector3 vector2 = rectTransform2.anchoredPosition;
				vector2.x += (float)(i + 1) * num2 + 215f;
				rectTransform.anchoredPosition = vector2;
				rectTransform.pivot = new Vector2(0f, 0.5f);
				rectTransform.eulerAngles = new Vector3(0f, 0f, 13f);
				Text component = gameObject.GetComponent<Text>();
				component.fontSize = 15;
				component.text = this.m_Field.columnLabels[i];
			}
			this.UpdateValue();
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x000297BC File Offset: 0x000279BC
		public override bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
		{
			if (fromNext || !this.valueToggle.isOn)
			{
				this.nameLabel.color = this.colorSelected;
			}
			else if (this.valueToggle.isOn)
			{
				if (this.m_Container.IsDirectChild(previous))
				{
					this.nameLabel.color = this.colorSelected;
				}
				else
				{
					DebugUIHandlerWidget lastItem = this.m_Container.GetLastItem();
					DebugManager.instance.ChangeSelection(lastItem, false);
				}
			}
			return true;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00029833 File Offset: 0x00027A33
		public override void OnDeselection()
		{
			this.nameLabel.color = this.colorDefault;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00029846 File Offset: 0x00027A46
		public override void OnIncrement(bool fast)
		{
			this.m_Field.SetValue(true);
			this.UpdateValue();
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0002985A File Offset: 0x00027A5A
		public override void OnDecrement(bool fast)
		{
			this.m_Field.SetValue(false);
			this.UpdateValue();
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x00029870 File Offset: 0x00027A70
		public override void OnAction()
		{
			bool flag = !this.m_Field.GetValue();
			this.m_Field.SetValue(flag);
			this.UpdateValue();
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0002989E File Offset: 0x00027A9E
		private void UpdateValue()
		{
			this.valueToggle.isOn = this.m_Field.GetValue();
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x000298B8 File Offset: 0x00027AB8
		public override DebugUIHandlerWidget Next()
		{
			if (!this.m_Field.GetValue() || this.m_Container == null)
			{
				return base.Next();
			}
			DebugUIHandlerWidget firstItem = this.m_Container.GetFirstItem();
			if (firstItem == null)
			{
				return base.Next();
			}
			return firstItem;
		}

		// Token: 0x04000525 RID: 1317
		public Text nameLabel;

		// Token: 0x04000526 RID: 1318
		public UIFoldout valueToggle;

		// Token: 0x04000527 RID: 1319
		private DebugUI.Foldout m_Field;

		// Token: 0x04000528 RID: 1320
		private DebugUIHandlerContainer m_Container;

		// Token: 0x04000529 RID: 1321
		private const float k_FoldoutXOffset = 215f;

		// Token: 0x0400052A RID: 1322
		private const float k_XOffset = 230f;
	}
}
