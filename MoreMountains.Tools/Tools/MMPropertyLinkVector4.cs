using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000123 RID: 291
	public class MMPropertyLinkVector4 : MMPropertyLink
	{
		// Token: 0x0600068C RID: 1676 RVA: 0x0002105E File Offset: 0x0001F25E
		public override void Initialization(MMProperty property)
		{
			base.Initialization(property);
			this._initialValue = (Vector4)this.GetPropertyValue(property);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0002107C File Offset: 0x0001F27C
		public override void CreateGettersAndSetters(MMProperty property)
		{
			base.CreateGettersAndSetters(property);
			if (property.MemberType == MMProperty.MemberTypes.Property)
			{
				object obj = ((property.TargetScriptableObject == null) ? property.TargetComponent : property.TargetScriptableObject);
				if (property.MemberPropertyInfo.GetGetMethod() != null)
				{
					this.GetVector4Delegate = (Func<Vector4>)Delegate.CreateDelegate(typeof(Func<Vector4>), obj, property.MemberPropertyInfo.GetGetMethod());
				}
				if (property.MemberPropertyInfo.GetSetMethod() != null)
				{
					this.SetVector4Delegate = (Action<Vector4>)Delegate.CreateDelegate(typeof(Action<Vector4>), obj, property.MemberPropertyInfo.GetSetMethod());
				}
				this._getterSetterInitialized = true;
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00021131 File Offset: 0x0001F331
		public override object GetValue(MMPropertyEmitter emitter, MMProperty property)
		{
			return this.GetValueOptimized(property);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0002113F File Offset: 0x0001F33F
		public override void SetValue(MMPropertyReceiver receiver, MMProperty property, object newValue)
		{
			this.SetValueOptimized(property, (Vector4)newValue);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00021150 File Offset: 0x0001F350
		public override float GetLevel(MMPropertyEmitter emitter, MMProperty property)
		{
			this._vector4 = this.GetValueOptimized(property);
			float num = 0f;
			switch (emitter.Vector4Option)
			{
			case MMPropertyEmitter.Vector4Options.X:
				num = this._vector4.x;
				break;
			case MMPropertyEmitter.Vector4Options.Y:
				num = this._vector4.y;
				break;
			case MMPropertyEmitter.Vector4Options.Z:
				num = this._vector4.z;
				break;
			case MMPropertyEmitter.Vector4Options.W:
				num = this._vector4.w;
				break;
			}
			float num2 = num;
			num2 = MMMaths.Clamp(num2, emitter.FloatRemapMinToZero, emitter.FloatRemapMaxToOne, emitter.ClampMin, emitter.ClampMax);
			num2 = MMMaths.Remap(num2, emitter.FloatRemapMinToZero, emitter.FloatRemapMaxToOne, 0f, 1f);
			emitter.Level = num2;
			return num2;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0002120C File Offset: 0x0001F40C
		public override void SetLevel(MMPropertyReceiver receiver, MMProperty property, float level)
		{
			base.SetLevel(receiver, property, level);
			this._newValue.x = (receiver.ModifyX ? MMMaths.Remap(level, 0f, 1f, receiver.Vector4RemapZero.x, receiver.Vector4RemapOne.x) : 0f);
			this._newValue.y = (receiver.ModifyY ? MMMaths.Remap(level, 0f, 1f, receiver.Vector4RemapZero.y, receiver.Vector4RemapOne.y) : 0f);
			this._newValue.z = (receiver.ModifyZ ? MMMaths.Remap(level, 0f, 1f, receiver.Vector4RemapZero.z, receiver.Vector4RemapOne.z) : 0f);
			this._newValue.w = (receiver.ModifyW ? MMMaths.Remap(level, 0f, 1f, receiver.Vector4RemapZero.w, receiver.Vector4RemapOne.w) : 0f);
			if (receiver.RelativeValue)
			{
				this._newValue = this._initialValue + this._newValue;
			}
			this.SetValueOptimized(property, this._newValue);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0002134E File Offset: 0x0001F54E
		protected virtual Vector4 GetValueOptimized(MMProperty property)
		{
			if (!this._getterSetterInitialized)
			{
				return (Vector4)this.GetPropertyValue(property);
			}
			return this.GetVector4Delegate();
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00021370 File Offset: 0x0001F570
		protected virtual void SetValueOptimized(MMProperty property, Vector4 newValue)
		{
			if (this._getterSetterInitialized)
			{
				this.SetVector4Delegate(this._newValue);
				return;
			}
			this.SetPropertyValue(property, this._newValue);
		}

		// Token: 0x0400081A RID: 2074
		public Func<Vector4> GetVector4Delegate;

		// Token: 0x0400081B RID: 2075
		public Action<Vector4> SetVector4Delegate;

		// Token: 0x0400081C RID: 2076
		protected Vector4 _initialValue;

		// Token: 0x0400081D RID: 2077
		protected Vector4 _newValue;

		// Token: 0x0400081E RID: 2078
		protected Vector4 _vector4;
	}
}
