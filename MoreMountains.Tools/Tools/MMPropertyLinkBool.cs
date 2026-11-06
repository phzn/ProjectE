using System;

namespace MoreMountains.Tools
{
	// Token: 0x0200011B RID: 283
	public class MMPropertyLinkBool : MMPropertyLink
	{
		// Token: 0x06000645 RID: 1605 RVA: 0x0001FE72 File Offset: 0x0001E072
		public override void Initialization(MMProperty property)
		{
			base.Initialization(property);
			this._initialValue = (bool)this.GetPropertyValue(property);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0001FE90 File Offset: 0x0001E090
		public override void CreateGettersAndSetters(MMProperty property)
		{
			base.CreateGettersAndSetters(property);
			if (property.MemberType == MMProperty.MemberTypes.Property)
			{
				object obj = ((property.TargetScriptableObject == null) ? property.TargetComponent : property.TargetScriptableObject);
				if (property.MemberPropertyInfo.GetGetMethod() != null)
				{
					this.GetBoolDelegate = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), obj, property.MemberPropertyInfo.GetGetMethod());
				}
				if (property.MemberPropertyInfo.GetSetMethod() != null)
				{
					this.SetBoolDelegate = (Action<bool>)Delegate.CreateDelegate(typeof(Action<bool>), obj, property.MemberPropertyInfo.GetSetMethod());
				}
				this._getterSetterInitialized = true;
			}
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0001FF45 File Offset: 0x0001E145
		public override object GetValue(MMPropertyEmitter emitter, MMProperty property)
		{
			return this.GetValueOptimized(property);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0001FF53 File Offset: 0x0001E153
		public override void SetValue(MMPropertyReceiver receiver, MMProperty property, object newValue)
		{
			this.SetValueOptimized(property, (bool)newValue);
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001FF64 File Offset: 0x0001E164
		public override float GetLevel(MMPropertyEmitter emitter, MMProperty property)
		{
			float num = (this.GetValueOptimized(property) ? emitter.BoolRemapTrue : emitter.BoolRemapFalse);
			emitter.Level = num;
			return num;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0001FF91 File Offset: 0x0001E191
		public override void SetLevel(MMPropertyReceiver receiver, MMProperty property, float level)
		{
			base.SetLevel(receiver, property, level);
			this._newValue = ((level > receiver.Threshold) ? receiver.BoolRemapOne : receiver.BoolRemapZero);
			this.SetValueOptimized(property, this._newValue);
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0001FFC6 File Offset: 0x0001E1C6
		protected virtual bool GetValueOptimized(MMProperty property)
		{
			if (!this._getterSetterInitialized)
			{
				return (bool)this.GetPropertyValue(property);
			}
			return this.GetBoolDelegate();
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0001FFE8 File Offset: 0x0001E1E8
		protected virtual void SetValueOptimized(MMProperty property, bool newValue)
		{
			if (this._getterSetterInitialized)
			{
				this.SetBoolDelegate(this._newValue);
				return;
			}
			this.SetPropertyValue(property, this._newValue);
		}

		// Token: 0x040007F7 RID: 2039
		public Func<bool> GetBoolDelegate;

		// Token: 0x040007F8 RID: 2040
		public Action<bool> SetBoolDelegate;

		// Token: 0x040007F9 RID: 2041
		protected bool _initialValue;

		// Token: 0x040007FA RID: 2042
		protected bool _newValue;
	}
}
