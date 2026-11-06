using System;
using System.Reflection;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000113 RID: 275
	public class MonoAttribute
	{
		// Token: 0x060005FF RID: 1535 RVA: 0x0001D761 File Offset: 0x0001B961
		public MonoAttribute(MonoBehaviour targetObject, MonoAttribute.MemberTypes type, PropertyInfo propertyInfo, FieldInfo fieldInfo, string memberName)
		{
			this.TargetObject = targetObject;
			this.MemberType = type;
			this.MemberPropertyInfo = propertyInfo;
			this.MemberFieldInfo = fieldInfo;
			this.MemberName = memberName;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001D790 File Offset: 0x0001B990
		public virtual float GetValue()
		{
			if (this.MemberType == MonoAttribute.MemberTypes.Property)
			{
				return (float)this.MemberPropertyInfo.GetValue(this.TargetObject);
			}
			if (this.MemberType == MonoAttribute.MemberTypes.Field)
			{
				return (float)this.MemberFieldInfo.GetValue(this.TargetObject);
			}
			return 0f;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001D7E4 File Offset: 0x0001B9E4
		public virtual void SetValue(float newValue)
		{
			if (this.MemberType == MonoAttribute.MemberTypes.Property)
			{
				this.MemberPropertyInfo.SetValue(this.TargetObject, newValue);
				return;
			}
			if (this.MemberType == MonoAttribute.MemberTypes.Field)
			{
				this.MemberFieldInfo.SetValue(this.TargetObject, newValue);
			}
		}

		// Token: 0x04000726 RID: 1830
		public MonoBehaviour TargetObject;

		// Token: 0x04000727 RID: 1831
		public MonoAttribute.MemberTypes MemberType;

		// Token: 0x04000728 RID: 1832
		public PropertyInfo MemberPropertyInfo;

		// Token: 0x04000729 RID: 1833
		public FieldInfo MemberFieldInfo;

		// Token: 0x0400072A RID: 1834
		public string MemberName;

		// Token: 0x02000434 RID: 1076
		public enum MemberTypes
		{
			// Token: 0x04001F9C RID: 8092
			Property,
			// Token: 0x04001F9D RID: 8093
			Field
		}
	}
}
