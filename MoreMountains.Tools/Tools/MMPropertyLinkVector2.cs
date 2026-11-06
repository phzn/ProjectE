using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000121 RID: 289
	public class MMPropertyLinkVector2 : MMPropertyLink
	{
		// Token: 0x0600067A RID: 1658 RVA: 0x00020A95 File Offset: 0x0001EC95
		public override void Initialization(MMProperty property)
		{
			base.Initialization(property);
			this._initialValue = (Vector2)this.GetPropertyValue(property);
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00020AB0 File Offset: 0x0001ECB0
		public override void CreateGettersAndSetters(MMProperty property)
		{
			base.CreateGettersAndSetters(property);
			if (property.MemberType == MMProperty.MemberTypes.Property)
			{
				object obj = ((property.TargetScriptableObject == null) ? property.TargetComponent : property.TargetScriptableObject);
				if (property.MemberPropertyInfo.GetGetMethod() != null)
				{
					this.GetVector2Delegate = (Func<Vector2>)Delegate.CreateDelegate(typeof(Func<Vector2>), obj, property.MemberPropertyInfo.GetGetMethod());
				}
				if (property.MemberPropertyInfo.GetSetMethod() != null)
				{
					this.SetVector2Delegate = (Action<Vector2>)Delegate.CreateDelegate(typeof(Action<Vector2>), obj, property.MemberPropertyInfo.GetSetMethod());
				}
				this._getterSetterInitialized = true;
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00020B65 File Offset: 0x0001ED65
		public override object GetValue(MMPropertyEmitter emitter, MMProperty property)
		{
			return this.GetValueOptimized(property);
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00020B73 File Offset: 0x0001ED73
		public override void SetValue(MMPropertyReceiver receiver, MMProperty property, object newValue)
		{
			this.SetValueOptimized(property, (Vector2)newValue);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00020B84 File Offset: 0x0001ED84
		public override float GetLevel(MMPropertyEmitter emitter, MMProperty property)
		{
			this._vector2 = (this._getterSetterInitialized ? this.GetVector2Delegate() : ((Vector2)this.GetPropertyValue(property)));
			float num = 0f;
			MMPropertyEmitter.Vector2Options vector2Option = emitter.Vector2Option;
			if (vector2Option != MMPropertyEmitter.Vector2Options.X)
			{
				if (vector2Option == MMPropertyEmitter.Vector2Options.Y)
				{
					num = this._vector2.y;
				}
			}
			else
			{
				num = this._vector2.x;
			}
			float num2 = num;
			num2 = MMMaths.Clamp(num2, emitter.FloatRemapMinToZero, emitter.FloatRemapMaxToOne, emitter.ClampMin, emitter.ClampMax);
			num2 = MMMaths.Remap(num2, emitter.FloatRemapMinToZero, emitter.FloatRemapMaxToOne, 0f, 1f);
			emitter.Level = num2;
			return num2;
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00020C30 File Offset: 0x0001EE30
		public override void SetLevel(MMPropertyReceiver receiver, MMProperty property, float level)
		{
			base.SetLevel(receiver, property, level);
			this._newValue.x = (receiver.ModifyX ? MMMaths.Remap(level, 0f, 1f, receiver.Vector2RemapZero.x, receiver.Vector2RemapOne.x) : 0f);
			this._newValue.y = (receiver.ModifyY ? MMMaths.Remap(level, 0f, 1f, receiver.Vector2RemapZero.y, receiver.Vector2RemapOne.y) : 0f);
			if (receiver.RelativeValue)
			{
				this._newValue = this._initialValue + this._newValue;
			}
			if (this._getterSetterInitialized)
			{
				this.SetVector2Delegate(this._newValue);
				return;
			}
			this.SetPropertyValue(property, this._newValue);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00020D11 File Offset: 0x0001EF11
		protected virtual Vector2 GetValueOptimized(MMProperty property)
		{
			if (!this._getterSetterInitialized)
			{
				return (Vector2)this.GetPropertyValue(property);
			}
			return this.GetVector2Delegate();
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00020D33 File Offset: 0x0001EF33
		protected virtual void SetValueOptimized(MMProperty property, Vector2 newValue)
		{
			if (this._getterSetterInitialized)
			{
				this.SetVector2Delegate(this._newValue);
				return;
			}
			this.SetPropertyValue(property, this._newValue);
		}

		// Token: 0x04000810 RID: 2064
		public Func<Vector2> GetVector2Delegate;

		// Token: 0x04000811 RID: 2065
		public Action<Vector2> SetVector2Delegate;

		// Token: 0x04000812 RID: 2066
		protected Vector2 _initialValue;

		// Token: 0x04000813 RID: 2067
		protected Vector2 _newValue;

		// Token: 0x04000814 RID: 2068
		protected Vector2 _vector2;
	}
}
