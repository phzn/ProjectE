using System;

namespace MoreMountains.Tools
{
	// Token: 0x0200011D RID: 285
	public class MMPropertyLinkFloat : MMPropertyLink
	{
		// Token: 0x06000657 RID: 1623 RVA: 0x000201F6 File Offset: 0x0001E3F6
		public override void Initialization(MMProperty property)
		{
			base.Initialization(property);
			this._initialValue = (float)this.GetPropertyValue(property);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00020214 File Offset: 0x0001E414
		public override void CreateGettersAndSetters(MMProperty property)
		{
			base.CreateGettersAndSetters(property);
			if (property.MemberType == MMProperty.MemberTypes.Property)
			{
				object obj = ((property.TargetScriptableObject == null) ? property.TargetComponent : property.TargetScriptableObject);
				if (property.MemberPropertyInfo.GetGetMethod() != null)
				{
					this.GetFloatDelegate = (Func<float>)Delegate.CreateDelegate(typeof(Func<float>), obj, property.MemberPropertyInfo.GetGetMethod());
				}
				if (property.MemberPropertyInfo.GetSetMethod() != null)
				{
					this.SetFloatDelegate = (Action<float>)Delegate.CreateDelegate(typeof(Action<float>), obj, property.MemberPropertyInfo.GetSetMethod());
				}
				this._getterSetterInitialized = true;
			}
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x000202C9 File Offset: 0x0001E4C9
		public override object GetValue(MMPropertyEmitter emitter, MMProperty property)
		{
			return this.GetValueOptimized(property);
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x000202D7 File Offset: 0x0001E4D7
		public override void SetValue(MMPropertyReceiver receiver, MMProperty property, object newValue)
		{
			this.SetValueOptimized(property, (float)newValue);
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x000202E8 File Offset: 0x0001E4E8
		public override float GetLevel(MMPropertyEmitter emitter, MMProperty property)
		{
			float num = this.GetValueOptimized(property);
			num = MMMaths.Clamp(num, emitter.FloatRemapMinToZero, emitter.FloatRemapMaxToOne, emitter.ClampMin, emitter.ClampMax);
			num = MMMaths.Remap(num, emitter.FloatRemapMinToZero, emitter.FloatRemapMaxToOne, 0f, 1f);
			emitter.Level = num;
			return num;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00020344 File Offset: 0x0001E544
		public override void SetLevel(MMPropertyReceiver receiver, MMProperty property, float level)
		{
			base.SetLevel(receiver, property, level);
			this._newValue = MMMaths.Remap(level, 0f, 1f, receiver.FloatRemapZero, receiver.FloatRemapOne);
			if (receiver.RelativeValue)
			{
				this._newValue = this._initialValue + this._newValue;
			}
			this.SetValueOptimized(property, this._newValue);
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x000203A4 File Offset: 0x0001E5A4
		protected virtual float GetValueOptimized(MMProperty property)
		{
			if (!this._getterSetterInitialized)
			{
				return (float)this.GetPropertyValue(property);
			}
			return this.GetFloatDelegate();
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x000203C6 File Offset: 0x0001E5C6
		protected virtual void SetValueOptimized(MMProperty property, float newValue)
		{
			if (this._getterSetterInitialized)
			{
				this.SetFloatDelegate(this._newValue);
				return;
			}
			this.SetPropertyValue(property, this._newValue);
		}

		// Token: 0x04000800 RID: 2048
		public Func<float> GetFloatDelegate;

		// Token: 0x04000801 RID: 2049
		public Action<float> SetFloatDelegate;

		// Token: 0x04000802 RID: 2050
		protected float _initialValue;

		// Token: 0x04000803 RID: 2051
		protected float _newValue;
	}
}
