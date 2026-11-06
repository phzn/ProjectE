using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x0200011D RID: 285
	public class DebugUIHandlerBitField : DebugUIHandlerWidget
	{
		// Token: 0x06000881 RID: 2177 RVA: 0x00027E44 File Offset: 0x00026044
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Field = base.CastWidget<DebugUI.BitField>();
			this.m_Container = base.GetComponent<DebugUIHandlerContainer>();
			this.nameLabel.text = this.m_Field.displayName;
			int i = 0;
			foreach (GUIContent guicontent in this.m_Field.enumNames)
			{
				if (i < this.toggles.Count)
				{
					DebugUIHandlerIndirectToggle debugUIHandlerIndirectToggle = this.toggles[i];
					debugUIHandlerIndirectToggle.getter = new Func<int, bool>(this.GetValue);
					debugUIHandlerIndirectToggle.setter = new Action<int, bool>(this.SetValue);
					debugUIHandlerIndirectToggle.nextUIHandler = ((i < this.m_Field.enumNames.Length - 1) ? this.toggles[i + 1] : null);
					debugUIHandlerIndirectToggle.previousUIHandler = ((i > 0) ? this.toggles[i - 1] : null);
					debugUIHandlerIndirectToggle.parentUIHandler = this;
					debugUIHandlerIndirectToggle.index = i;
					debugUIHandlerIndirectToggle.nameLabel.text = guicontent.text;
					debugUIHandlerIndirectToggle.Init();
					i++;
				}
			}
			while (i < this.toggles.Count)
			{
				CoreUtils.Destroy(this.toggles[i].gameObject);
				this.toggles[i] = null;
				i++;
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00027F90 File Offset: 0x00026190
		private bool GetValue(int index)
		{
			if (index == 0)
			{
				return false;
			}
			index--;
			return (Convert.ToInt32(this.m_Field.GetValue()) & (1 << index)) != 0;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00027FB8 File Offset: 0x000261B8
		private void SetValue(int index, bool value)
		{
			if (index == 0)
			{
				this.m_Field.SetValue(Enum.ToObject(this.m_Field.enumType, 0));
				using (List<DebugUIHandlerIndirectToggle>.Enumerator enumerator = this.toggles.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DebugUIHandlerIndirectToggle debugUIHandlerIndirectToggle = enumerator.Current;
						if (debugUIHandlerIndirectToggle != null && debugUIHandlerIndirectToggle.getter != null)
						{
							debugUIHandlerIndirectToggle.UpdateValueLabel();
						}
					}
					return;
				}
			}
			int num = Convert.ToInt32(this.m_Field.GetValue());
			if (value)
			{
				num |= this.m_Field.enumValues[index];
			}
			else
			{
				num &= ~this.m_Field.enumValues[index];
			}
			this.m_Field.SetValue(Enum.ToObject(this.m_Field.enumType, num));
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00028088 File Offset: 0x00026288
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

		// Token: 0x06000885 RID: 2181 RVA: 0x000280FF File Offset: 0x000262FF
		public override void OnDeselection()
		{
			this.nameLabel.color = this.colorDefault;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00028112 File Offset: 0x00026312
		public override void OnIncrement(bool fast)
		{
			this.valueToggle.isOn = true;
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00028120 File Offset: 0x00026320
		public override void OnDecrement(bool fast)
		{
			this.valueToggle.isOn = false;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0002812E File Offset: 0x0002632E
		public override void OnAction()
		{
			this.valueToggle.isOn = !this.valueToggle.isOn;
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0002814C File Offset: 0x0002634C
		public override DebugUIHandlerWidget Next()
		{
			if (!this.valueToggle.isOn || this.m_Container == null)
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

		// Token: 0x04000500 RID: 1280
		public Text nameLabel;

		// Token: 0x04000501 RID: 1281
		public UIFoldout valueToggle;

		// Token: 0x04000502 RID: 1282
		public List<DebugUIHandlerIndirectToggle> toggles;

		// Token: 0x04000503 RID: 1283
		private DebugUI.BitField m_Field;

		// Token: 0x04000504 RID: 1284
		private DebugUIHandlerContainer m_Container;
	}
}
