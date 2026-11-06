using System;

namespace MoreMountains.Tools
{
	// Token: 0x0200011E RID: 286
	public class MMPropertyLinkInt : MMPropertyLink
	{
		// Token: 0x06000660 RID: 1632 RVA: 0x000203FC File Offset: 0x0001E5FC
		public override void Initialization(MMProperty property)
		{
			base.Initialization(property);
			this._initialValue = (int)this.GetPropertyValue(property);
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00020418 File Offset: 0x0001E618
		public override void CreateGettersAndSetters(MMProperty property)
		{
			base.CreateGettersAndSetters(property);
			if (property.MemberType == MMProperty.MemberTypes.Property)
			{
				object obj = ((property.TargetScriptableObject == null) ? property.TargetComponent : property.TargetScriptableObject);
				if (property.MemberPropertyInfo.GetGetMethod() != null)
				{
					this.GetIntDelegate = (Func<int>)Delegate.CreateDelegate(typeof(Func<int>), obj, property.MemberPropertyInfo.GetGetMethod());
				}
				if (property.MemberPropertyInfo.GetSetMethod() != null)
				{
					this.SetIntDelegate = (Action<int>)Delegate.CreateDelegate(typeof(Action<int>), obj, property.MemberPropertyInfo.GetSetMethod());
				}
				this._getterSetterInitialized = true;
			}
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x000204CD File Offset: 0x0001E6CD
		public override object GetValue(MMPropertyEmitter emitter, MMProperty property)
		{
			return this.GetValueOptimized(property);
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x000204DB File Offset: 0x0001E6DB
		public override void SetValue(MMPropertyReceiver receiver, MMProperty property, object newValue)
		{
			this.SetValueOptimized(property, (int)newValue);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x000204EC File Offset: 0x0001E6EC
		public override float GetLevel(MMPropertyEmitter emitter, MMProperty property)
		{
			float num = (float)this.GetValueOptimized(property);
			num = MMMaths.Clamp(num, (float)emitter.IntRemapMinToZero, (float)emitter.IntRemapMaxToOne, emitter.ClampMin, emitter.ClampMax);
			num = MMMaths.Remap(num, (float)emitter.IntRemapMinToZero, (float)emitter.IntRemapMaxToOne, 0f, 1f);
			emitter.Level = num;
			return num;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0002054C File Offset: 0x0001E74C
		public override void SetLevel(MMPropertyReceiver receiver, MMProperty property, float level)
		{
			base.SetLevel(receiver, property, level);
			this._newValue = (int)MMMaths.Remap(level, 0f, 1f, (float)receiver.IntRemapZero, (float)receiver.IntRemapOne);
			if (receiver.RelativeValue)
			{
				this._newValue = this._initialValue + this._newValue;
			}
			this.SetValueOptimized(property, this._newValue);
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x000205AF File Offset: 0x0001E7AF
		protected virtual int GetValueOptimized(MMProperty property)
		{
			if (!this._getterSetterInitialized)
			{
				return (int)this.GetPropertyValue(property);
			}
			return this.GetIntDelegate();
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x000205D1 File Offset: 0x0001E7D1
		protected virtual void SetValueOptimized(MMProperty property, int newValue)
		{
			if (this._getterSetterInitialized)
			{
				this.SetIntDelegate(this._newValue);
				return;
			}
			this.SetPropertyValue(property, this._newValue);
		}

		// Token: 0x04000804 RID: 2052
		public Func<int> GetIntDelegate;

		// Token: 0x04000805 RID: 2053
		public Action<int> SetIntDelegate;

		// Token: 0x04000806 RID: 2054
		protected int _initialValue;

		// Token: 0x04000807 RID: 2055
		protected int _newValue;
	}
}
