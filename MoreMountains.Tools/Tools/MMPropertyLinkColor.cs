using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200011C RID: 284
	public class MMPropertyLinkColor : MMPropertyLink
	{
		// Token: 0x0600064E RID: 1614 RVA: 0x0002001E File Offset: 0x0001E21E
		public override void Initialization(MMProperty property)
		{
			base.Initialization(property);
			this._initialValue = (Color)this.GetPropertyValue(property);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0002003C File Offset: 0x0001E23C
		public override void CreateGettersAndSetters(MMProperty property)
		{
			base.CreateGettersAndSetters(property);
			if (property.MemberType == MMProperty.MemberTypes.Property)
			{
				object obj = ((property.TargetScriptableObject == null) ? property.TargetComponent : property.TargetScriptableObject);
				if (property.MemberPropertyInfo.GetGetMethod() != null)
				{
					this.GetColorDelegate = (Func<Color>)Delegate.CreateDelegate(typeof(Func<Color>), obj, property.MemberPropertyInfo.GetGetMethod());
				}
				if (property.MemberPropertyInfo.GetSetMethod() != null)
				{
					this.SetColorDelegate = (Action<Color>)Delegate.CreateDelegate(typeof(Action<Color>), obj, property.MemberPropertyInfo.GetSetMethod());
				}
				this._getterSetterInitialized = true;
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x000200F1 File Offset: 0x0001E2F1
		public override object GetValue(MMPropertyEmitter emitter, MMProperty property)
		{
			return this.GetValueOptimized(property);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x000200FF File Offset: 0x0001E2FF
		public override void SetValue(MMPropertyReceiver receiver, MMProperty property, object newValue)
		{
			this.SetValueOptimized(property, (Color)newValue);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0002010E File Offset: 0x0001E30E
		public override float GetLevel(MMPropertyEmitter emitter, MMProperty property)
		{
			this._color = (this._getterSetterInitialized ? this.GetColorDelegate() : ((Color)this.GetPropertyValue(property)));
			return this._color.MeanRGB();
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00020144 File Offset: 0x0001E344
		public override void SetLevel(MMPropertyReceiver receiver, MMProperty property, float level)
		{
			base.SetLevel(receiver, property, level);
			this._newValue = Color.LerpUnclamped(receiver.ColorRemapZero, receiver.ColorRemapOne, level);
			if (receiver.RelativeValue)
			{
				this._newValue = this._initialValue + this._newValue;
			}
			this.SetValueOptimized(property, this._newValue);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0002019E File Offset: 0x0001E39E
		protected virtual Color GetValueOptimized(MMProperty property)
		{
			if (!this._getterSetterInitialized)
			{
				return (Color)this.GetPropertyValue(property);
			}
			return this.GetColorDelegate();
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x000201C0 File Offset: 0x0001E3C0
		protected virtual void SetValueOptimized(MMProperty property, Color newValue)
		{
			if (this._getterSetterInitialized)
			{
				this.SetColorDelegate(this._newValue);
				return;
			}
			this.SetPropertyValue(property, this._newValue);
		}

		// Token: 0x040007FB RID: 2043
		public Func<Color> GetColorDelegate;

		// Token: 0x040007FC RID: 2044
		public Action<Color> SetColorDelegate;

		// Token: 0x040007FD RID: 2045
		protected Color _initialValue;

		// Token: 0x040007FE RID: 2046
		protected Color _newValue;

		// Token: 0x040007FF RID: 2047
		protected Color _color;
	}
}
