using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200011F RID: 287
	public class MMPropertyLinkQuaternion : MMPropertyLink
	{
		// Token: 0x06000669 RID: 1641 RVA: 0x00020607 File Offset: 0x0001E807
		public override void Initialization(MMProperty property)
		{
			base.Initialization(property);
			this._initialValue = (Quaternion)this.GetPropertyValue(property);
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00020624 File Offset: 0x0001E824
		public override void CreateGettersAndSetters(MMProperty property)
		{
			base.CreateGettersAndSetters(property);
			if (property.MemberType == MMProperty.MemberTypes.Property)
			{
				object obj = ((property.TargetScriptableObject == null) ? property.TargetComponent : property.TargetScriptableObject);
				if (property.MemberPropertyInfo.GetGetMethod() != null)
				{
					this.GetQuaternionDelegate = (Func<Quaternion>)Delegate.CreateDelegate(typeof(Func<Quaternion>), obj, property.MemberPropertyInfo.GetGetMethod());
				}
				if (property.MemberPropertyInfo.GetSetMethod() != null)
				{
					this.SetQuaternionDelegate = (Action<Quaternion>)Delegate.CreateDelegate(typeof(Action<Quaternion>), obj, property.MemberPropertyInfo.GetSetMethod());
				}
				this._getterSetterInitialized = true;
			}
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x000206D9 File Offset: 0x0001E8D9
		public override object GetValue(MMPropertyEmitter emitter, MMProperty property)
		{
			return this.GetValueOptimized(property);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x000206E7 File Offset: 0x0001E8E7
		public override void SetValue(MMPropertyReceiver receiver, MMProperty property, object newValue)
		{
			this.SetValueOptimized(property, (Quaternion)newValue);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x000206F8 File Offset: 0x0001E8F8
		public override float GetLevel(MMPropertyEmitter emitter, MMProperty property)
		{
			float num = 0f;
			Quaternion valueOptimized = this.GetValueOptimized(property);
			switch (emitter.Vector3Option)
			{
			case MMPropertyEmitter.Vector3Options.X:
				num = valueOptimized.eulerAngles.x;
				break;
			case MMPropertyEmitter.Vector3Options.Y:
				num = valueOptimized.eulerAngles.y;
				break;
			case MMPropertyEmitter.Vector3Options.Z:
				num = valueOptimized.eulerAngles.z;
				break;
			}
			num = MMMaths.Clamp(num, emitter.QuaternionRemapMinToZero, emitter.QuaternionRemapMaxToOne, emitter.ClampMin, emitter.ClampMax);
			float num2 = MMMaths.Remap(num, emitter.QuaternionRemapMinToZero, emitter.QuaternionRemapMaxToOne, 0f, 1f);
			emitter.Level = num2;
			return num2;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x000207A0 File Offset: 0x0001E9A0
		public override void SetLevel(MMPropertyReceiver receiver, MMProperty property, float level)
		{
			base.SetLevel(receiver, property, level);
			this._newValue = (receiver.RelativeValue ? this._initialValue : Quaternion.identity);
			if (receiver.ModifyX)
			{
				float num = MMMaths.Remap(level, 0f, 1f, receiver.QuaternionRemapZero.x, receiver.QuaternionRemapOne.x);
				this._newValue *= Quaternion.AngleAxis(num, Vector3.right);
			}
			if (receiver.ModifyY)
			{
				float num2 = MMMaths.Remap(level, 0f, 1f, receiver.QuaternionRemapZero.y, receiver.QuaternionRemapOne.y);
				this._newValue *= Quaternion.AngleAxis(num2, Vector3.up);
			}
			if (receiver.ModifyZ)
			{
				float num3 = MMMaths.Remap(level, 0f, 1f, receiver.QuaternionRemapZero.z, receiver.QuaternionRemapOne.z);
				this._newValue *= Quaternion.AngleAxis(num3, Vector3.forward);
			}
			this.SetValueOptimized(property, this._newValue);
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x000208BF File Offset: 0x0001EABF
		protected virtual Quaternion GetValueOptimized(MMProperty property)
		{
			if (!this._getterSetterInitialized)
			{
				return (Quaternion)this.GetPropertyValue(property);
			}
			return this.GetQuaternionDelegate();
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x000208E1 File Offset: 0x0001EAE1
		protected virtual void SetValueOptimized(MMProperty property, Quaternion newValue)
		{
			if (this._getterSetterInitialized)
			{
				this.SetQuaternionDelegate(this._newValue);
				return;
			}
			this.SetPropertyValue(property, this._newValue);
		}

		// Token: 0x04000808 RID: 2056
		public Func<Quaternion> GetQuaternionDelegate;

		// Token: 0x04000809 RID: 2057
		public Action<Quaternion> SetQuaternionDelegate;

		// Token: 0x0400080A RID: 2058
		protected Quaternion _initialValue = Quaternion.identity;

		// Token: 0x0400080B RID: 2059
		protected Quaternion _newValue;
	}
}
