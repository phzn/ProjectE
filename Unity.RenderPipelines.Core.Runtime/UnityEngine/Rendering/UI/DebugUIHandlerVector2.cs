using System;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x0200013B RID: 315
	public class DebugUIHandlerVector2 : DebugUIHandlerWidget
	{
		// Token: 0x06000954 RID: 2388 RVA: 0x0002B0FC File Offset: 0x000292FC
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Field = base.CastWidget<DebugUI.Vector2Field>();
			this.m_Container = base.GetComponent<DebugUIHandlerContainer>();
			this.nameLabel.text = this.m_Field.displayName;
			this.fieldX.getter = () => this.m_Field.GetValue().x;
			this.fieldX.setter = delegate(float x)
			{
				this.SetValue(x, true, false);
			};
			this.fieldX.nextUIHandler = this.fieldY;
			this.SetupSettings(this.fieldX);
			this.fieldY.getter = () => this.m_Field.GetValue().y;
			this.fieldY.setter = delegate(float x)
			{
				this.SetValue(x, false, true);
			};
			this.fieldY.previousUIHandler = this.fieldX;
			this.SetupSettings(this.fieldY);
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0002B1D4 File Offset: 0x000293D4
		private void SetValue(float v, bool x = false, bool y = false)
		{
			Vector2 value = this.m_Field.GetValue();
			if (x)
			{
				value.x = v;
			}
			if (y)
			{
				value.y = v;
			}
			this.m_Field.SetValue(value);
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0002B210 File Offset: 0x00029410
		private void SetupSettings(DebugUIHandlerIndirectFloatField field)
		{
			field.parentUIHandler = this;
			field.incStepGetter = () => this.m_Field.incStep;
			field.incStepMultGetter = () => this.m_Field.incStepMult;
			field.decimalsGetter = () => (float)this.m_Field.decimals;
			field.Init();
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0002B260 File Offset: 0x00029460
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

		// Token: 0x06000958 RID: 2392 RVA: 0x0002B2D7 File Offset: 0x000294D7
		public override void OnDeselection()
		{
			this.nameLabel.color = this.colorDefault;
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0002B2EA File Offset: 0x000294EA
		public override void OnIncrement(bool fast)
		{
			this.valueToggle.isOn = true;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0002B2F8 File Offset: 0x000294F8
		public override void OnDecrement(bool fast)
		{
			this.valueToggle.isOn = false;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0002B306 File Offset: 0x00029506
		public override void OnAction()
		{
			this.valueToggle.isOn = !this.valueToggle.isOn;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0002B324 File Offset: 0x00029524
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

		// Token: 0x04000573 RID: 1395
		public Text nameLabel;

		// Token: 0x04000574 RID: 1396
		public UIFoldout valueToggle;

		// Token: 0x04000575 RID: 1397
		public DebugUIHandlerIndirectFloatField fieldX;

		// Token: 0x04000576 RID: 1398
		public DebugUIHandlerIndirectFloatField fieldY;

		// Token: 0x04000577 RID: 1399
		private DebugUI.Vector2Field m_Field;

		// Token: 0x04000578 RID: 1400
		private DebugUIHandlerContainer m_Container;
	}
}
