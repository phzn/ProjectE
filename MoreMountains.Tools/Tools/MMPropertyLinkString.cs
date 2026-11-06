using System;

namespace MoreMountains.Tools
{
	// Token: 0x02000120 RID: 288
	public class MMPropertyLinkString : MMPropertyLink
	{
		// Token: 0x06000672 RID: 1650 RVA: 0x00020922 File Offset: 0x0001EB22
		public override void Initialization(MMProperty property)
		{
			base.Initialization(property);
			this._initialValue = (string)this.GetPropertyValue(property);
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00020940 File Offset: 0x0001EB40
		public override void CreateGettersAndSetters(MMProperty property)
		{
			base.CreateGettersAndSetters(property);
			if (property.MemberType == MMProperty.MemberTypes.Property)
			{
				object obj = ((property.TargetScriptableObject == null) ? property.TargetComponent : property.TargetScriptableObject);
				if (property.MemberPropertyInfo.GetGetMethod() != null)
				{
					this.GetStringDelegate = (Func<string>)Delegate.CreateDelegate(typeof(Func<string>), obj, property.MemberPropertyInfo.GetGetMethod());
				}
				if (property.MemberPropertyInfo.GetSetMethod() != null)
				{
					this.SetStringDelegate = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), obj, property.MemberPropertyInfo.GetSetMethod());
				}
				this._getterSetterInitialized = true;
			}
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x000209F5 File Offset: 0x0001EBF5
		public override object GetValue(MMPropertyEmitter emitter, MMProperty property)
		{
			return this.GetValueOptimized(property);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x000209FE File Offset: 0x0001EBFE
		public override void SetValue(MMPropertyReceiver receiver, MMProperty property, object newValue)
		{
			this.SetValueOptimized(property, (string)newValue);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00020A0D File Offset: 0x0001EC0D
		public override void SetLevel(MMPropertyReceiver receiver, MMProperty property, float level)
		{
			base.SetLevel(receiver, property, level);
			this._newValue = ((level > receiver.Threshold) ? receiver.StringRemapOne : receiver.StringRemapZero);
			this.SetValueOptimized(property, this._newValue);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00020A42 File Offset: 0x0001EC42
		protected virtual string GetValueOptimized(MMProperty property)
		{
			if (!this._getterSetterInitialized)
			{
				return (string)this.GetPropertyValue(property);
			}
			return this.GetStringDelegate();
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00020A64 File Offset: 0x0001EC64
		protected virtual void SetValueOptimized(MMProperty property, string newValue)
		{
			if (this._getterSetterInitialized)
			{
				this.SetStringDelegate(this._newValue);
				return;
			}
			this.SetPropertyValue(property, this._newValue);
		}

		// Token: 0x0400080C RID: 2060
		public Func<string> GetStringDelegate;

		// Token: 0x0400080D RID: 2061
		public Action<string> SetStringDelegate;

		// Token: 0x0400080E RID: 2062
		protected string _initialValue;

		// Token: 0x0400080F RID: 2063
		protected string _newValue;
	}
}
