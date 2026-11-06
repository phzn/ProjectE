using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000122 RID: 290
	public class MMPropertyLinkVector3 : MMPropertyLink
	{
		// Token: 0x06000683 RID: 1667 RVA: 0x00020D69 File Offset: 0x0001EF69
		public override void Initialization(MMProperty property)
		{
			base.Initialization(property);
			this._initialValue = (Vector3)this.GetPropertyValue(property);
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00020D84 File Offset: 0x0001EF84
		public override void CreateGettersAndSetters(MMProperty property)
		{
			base.CreateGettersAndSetters(property);
			if (property.MemberType == MMProperty.MemberTypes.Property)
			{
				object obj = ((property.TargetScriptableObject == null) ? property.TargetComponent : property.TargetScriptableObject);
				if (property.MemberPropertyInfo.GetGetMethod() != null)
				{
					this.GetVector3Delegate = (Func<Vector3>)Delegate.CreateDelegate(typeof(Func<Vector3>), obj, property.MemberPropertyInfo.GetGetMethod());
				}
				if (property.MemberPropertyInfo.GetSetMethod() != null)
				{
					this.SetVector3Delegate = (Action<Vector3>)Delegate.CreateDelegate(typeof(Action<Vector3>), obj, property.MemberPropertyInfo.GetSetMethod());
				}
				this._getterSetterInitialized = true;
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00020E39 File Offset: 0x0001F039
		public override object GetValue(MMPropertyEmitter emitter, MMProperty property)
		{
			return this.GetValueOptimized(property);
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00020E47 File Offset: 0x0001F047
		public override void SetValue(MMPropertyReceiver receiver, MMProperty property, object newValue)
		{
			this.SetValueOptimized(property, (Vector3)newValue);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00020E58 File Offset: 0x0001F058
		public override float GetLevel(MMPropertyEmitter emitter, MMProperty property)
		{
			this._vector3 = this.GetValueOptimized(property);
			float num = 0f;
			switch (emitter.Vector3Option)
			{
			case MMPropertyEmitter.Vector3Options.X:
				num = this._vector3.x;
				break;
			case MMPropertyEmitter.Vector3Options.Y:
				num = this._vector3.y;
				break;
			case MMPropertyEmitter.Vector3Options.Z:
				num = this._vector3.z;
				break;
			}
			float num2 = num;
			num2 = MMMaths.Clamp(num2, emitter.FloatRemapMinToZero, emitter.FloatRemapMaxToOne, emitter.ClampMin, emitter.ClampMax);
			num2 = MMMaths.Remap(num2, emitter.FloatRemapMinToZero, emitter.FloatRemapMaxToOne, 0f, 1f);
			emitter.Level = num2;
			return num2;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00020F04 File Offset: 0x0001F104
		public override void SetLevel(MMPropertyReceiver receiver, MMProperty property, float level)
		{
			base.SetLevel(receiver, property, level);
			this._newValue.x = (receiver.ModifyX ? MMMaths.Remap(level, 0f, 1f, receiver.Vector3RemapZero.x, receiver.Vector3RemapOne.x) : 0f);
			this._newValue.y = (receiver.ModifyY ? MMMaths.Remap(level, 0f, 1f, receiver.Vector3RemapZero.y, receiver.Vector3RemapOne.y) : 0f);
			this._newValue.z = (receiver.ModifyZ ? MMMaths.Remap(level, 0f, 1f, receiver.Vector3RemapZero.z, receiver.Vector3RemapOne.z) : 0f);
			if (receiver.RelativeValue)
			{
				this._newValue = this._initialValue + this._newValue;
			}
			this.SetValueOptimized(property, this._newValue);
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00021006 File Offset: 0x0001F206
		protected virtual Vector3 GetValueOptimized(MMProperty property)
		{
			if (!this._getterSetterInitialized)
			{
				return (Vector3)this.GetPropertyValue(property);
			}
			return this.GetVector3Delegate();
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00021028 File Offset: 0x0001F228
		protected virtual void SetValueOptimized(MMProperty property, Vector3 newValue)
		{
			if (this._getterSetterInitialized)
			{
				this.SetVector3Delegate(this._newValue);
				return;
			}
			this.SetPropertyValue(property, this._newValue);
		}

		// Token: 0x04000815 RID: 2069
		public Func<Vector3> GetVector3Delegate;

		// Token: 0x04000816 RID: 2070
		public Action<Vector3> SetVector3Delegate;

		// Token: 0x04000817 RID: 2071
		protected Vector3 _initialValue;

		// Token: 0x04000818 RID: 2072
		protected Vector3 _newValue;

		// Token: 0x04000819 RID: 2073
		protected Vector3 _vector3;
	}
}
