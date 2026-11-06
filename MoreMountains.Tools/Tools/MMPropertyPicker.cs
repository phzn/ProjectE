using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000125 RID: 293
	[Serializable]
	public class MMPropertyPicker
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x000213F8 File Offset: 0x0001F5F8
		// (set) Token: 0x06000698 RID: 1688 RVA: 0x00021400 File Offset: 0x0001F600
		public virtual bool PropertyFound { get; protected set; }

		// Token: 0x06000699 RID: 1689 RVA: 0x0002140C File Offset: 0x0001F60C
		public virtual void Initialization(GameObject source)
		{
			if (this.TargetComponent == null && this.TargetScriptableObject == null)
			{
				this.PropertyFound = false;
				return;
			}
			this._targetMMProperty = MMProperty.FindProperty(this.TargetPropertyName, this.TargetComponent, source, this.TargetScriptableObject);
			if (this._targetMMProperty == null)
			{
				this.PropertyFound = false;
				return;
			}
			if (this._targetMMProperty.TargetComponent == null && this._targetMMProperty.TargetScriptableObject == null)
			{
				this.PropertyFound = false;
				return;
			}
			if (this._targetMMProperty.MemberPropertyInfo == null && this._targetMMProperty.MemberFieldInfo == null)
			{
				this.PropertyFound = false;
				return;
			}
			this.PropertyFound = true;
			this._initialized = true;
			if (this._targetMMProperty.PropertyType == typeof(string))
			{
				this._propertySetter = new MMPropertyLinkString();
				this._propertySetter.Initialization(this._targetMMProperty);
				return;
			}
			if (this._targetMMProperty.PropertyType == typeof(float))
			{
				this._propertySetter = new MMPropertyLinkFloat();
				this._propertySetter.Initialization(this._targetMMProperty);
				return;
			}
			if (this._targetMMProperty.PropertyType == typeof(Vector2))
			{
				this._propertySetter = new MMPropertyLinkVector2();
				this._propertySetter.Initialization(this._targetMMProperty);
				return;
			}
			if (this._targetMMProperty.PropertyType == typeof(Vector3))
			{
				this._propertySetter = new MMPropertyLinkVector3();
				this._propertySetter.Initialization(this._targetMMProperty);
				return;
			}
			if (this._targetMMProperty.PropertyType == typeof(Vector4))
			{
				this._propertySetter = new MMPropertyLinkVector4();
				this._propertySetter.Initialization(this._targetMMProperty);
				return;
			}
			if (this._targetMMProperty.PropertyType == typeof(Quaternion))
			{
				this._propertySetter = new MMPropertyLinkQuaternion();
				this._propertySetter.Initialization(this._targetMMProperty);
				return;
			}
			if (this._targetMMProperty.PropertyType == typeof(int))
			{
				this._propertySetter = new MMPropertyLinkInt();
				this._propertySetter.Initialization(this._targetMMProperty);
				return;
			}
			if (this._targetMMProperty.PropertyType == typeof(bool))
			{
				this._propertySetter = new MMPropertyLinkBool();
				this._propertySetter.Initialization(this._targetMMProperty);
				return;
			}
			if (this._targetMMProperty.PropertyType == typeof(Color))
			{
				this._propertySetter = new MMPropertyLinkColor();
				this._propertySetter.Initialization(this._targetMMProperty);
				return;
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x000216D6 File Offset: 0x0001F8D6
		public virtual object GetRawValue()
		{
			return this._propertySetter.GetPropertyValue(this._targetMMProperty);
		}

		// Token: 0x0400082D RID: 2093
		public Object TargetObject;

		// Token: 0x0400082E RID: 2094
		public Component TargetComponent;

		// Token: 0x0400082F RID: 2095
		public ScriptableObject TargetScriptableObject;

		// Token: 0x04000830 RID: 2096
		public string TargetPropertyName;

		// Token: 0x04000832 RID: 2098
		protected MMProperty _targetMMProperty;

		// Token: 0x04000833 RID: 2099
		protected bool _initialized;

		// Token: 0x04000834 RID: 2100
		protected MMPropertyLink _propertySetter;
	}
}
