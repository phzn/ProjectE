using System;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x0200013D RID: 317
	public class DebugUIHandlerVector4 : DebugUIHandlerWidget
	{
		// Token: 0x06000978 RID: 2424 RVA: 0x0002B744 File Offset: 0x00029944
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Field = base.CastWidget<DebugUI.Vector4Field>();
			this.m_Container = base.GetComponent<DebugUIHandlerContainer>();
			this.nameLabel.text = this.m_Field.displayName;
			this.fieldX.getter = () => this.m_Field.GetValue().x;
			this.fieldX.setter = delegate(float x)
			{
				this.SetValue(x, true, false, false, false);
			};
			this.fieldX.nextUIHandler = this.fieldY;
			this.SetupSettings(this.fieldX);
			this.fieldY.getter = () => this.m_Field.GetValue().y;
			this.fieldY.setter = delegate(float x)
			{
				this.SetValue(x, false, true, false, false);
			};
			this.fieldY.previousUIHandler = this.fieldX;
			this.fieldY.nextUIHandler = this.fieldZ;
			this.SetupSettings(this.fieldY);
			this.fieldZ.getter = () => this.m_Field.GetValue().z;
			this.fieldZ.setter = delegate(float x)
			{
				this.SetValue(x, false, false, true, false);
			};
			this.fieldZ.previousUIHandler = this.fieldY;
			this.fieldZ.nextUIHandler = this.fieldW;
			this.SetupSettings(this.fieldZ);
			this.fieldW.getter = () => this.m_Field.GetValue().w;
			this.fieldW.setter = delegate(float x)
			{
				this.SetValue(x, false, false, false, true);
			};
			this.fieldW.previousUIHandler = this.fieldZ;
			this.SetupSettings(this.fieldW);
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0002B8D4 File Offset: 0x00029AD4
		private void SetValue(float v, bool x = false, bool y = false, bool z = false, bool w = false)
		{
			Vector4 value = this.m_Field.GetValue();
			if (x)
			{
				value.x = v;
			}
			if (y)
			{
				value.y = v;
			}
			if (z)
			{
				value.z = v;
			}
			if (w)
			{
				value.w = v;
			}
			this.m_Field.SetValue(value);
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0002B928 File Offset: 0x00029B28
		private void SetupSettings(DebugUIHandlerIndirectFloatField field)
		{
			field.parentUIHandler = this;
			field.incStepGetter = () => this.m_Field.incStep;
			field.incStepMultGetter = () => this.m_Field.incStepMult;
			field.decimalsGetter = () => (float)this.m_Field.decimals;
			field.Init();
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0002B978 File Offset: 0x00029B78
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

		// Token: 0x0600097C RID: 2428 RVA: 0x0002B9EF File Offset: 0x00029BEF
		public override void OnDeselection()
		{
			this.nameLabel.color = this.colorDefault;
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0002BA02 File Offset: 0x00029C02
		public override void OnIncrement(bool fast)
		{
			this.valueToggle.isOn = true;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0002BA10 File Offset: 0x00029C10
		public override void OnDecrement(bool fast)
		{
			this.valueToggle.isOn = false;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0002BA1E File Offset: 0x00029C1E
		public override void OnAction()
		{
			this.valueToggle.isOn = !this.valueToggle.isOn;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0002BA3C File Offset: 0x00029C3C
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

		// Token: 0x04000580 RID: 1408
		public Text nameLabel;

		// Token: 0x04000581 RID: 1409
		public UIFoldout valueToggle;

		// Token: 0x04000582 RID: 1410
		public DebugUIHandlerIndirectFloatField fieldX;

		// Token: 0x04000583 RID: 1411
		public DebugUIHandlerIndirectFloatField fieldY;

		// Token: 0x04000584 RID: 1412
		public DebugUIHandlerIndirectFloatField fieldZ;

		// Token: 0x04000585 RID: 1413
		public DebugUIHandlerIndirectFloatField fieldW;

		// Token: 0x04000586 RID: 1414
		private DebugUI.Vector4Field m_Field;

		// Token: 0x04000587 RID: 1415
		private DebugUIHandlerContainer m_Container;
	}
}
