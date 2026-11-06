using System;
using System.Reflection;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000119 RID: 281
	public class MMProperty
	{
		// Token: 0x0600063A RID: 1594 RVA: 0x0001FBB4 File Offset: 0x0001DDB4
		public MMProperty(Component targetComponent, MMProperty.MemberTypes type, PropertyInfo propertyInfo, FieldInfo fieldInfo, string memberName, ScriptableObject targetScriptable)
		{
			this.TargetComponent = targetComponent;
			this.TargetScriptableObject = targetScriptable;
			this.MemberType = type;
			this.MemberPropertyInfo = propertyInfo;
			this.MemberFieldInfo = fieldInfo;
			this.MemberName = memberName;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0001FBEC File Offset: 0x0001DDEC
		public static MMProperty FindProperty(string propertyName, Component targetComponent, GameObject source, ScriptableObject scriptable)
		{
			FieldInfo fieldInfo = null;
			PropertyInfo propertyInfo = null;
			MMProperty mmproperty = null;
			if (scriptable == null)
			{
				propertyInfo = targetComponent.GetType().GetProperty(propertyName);
				if (propertyInfo == null)
				{
					fieldInfo = targetComponent.GetType().GetField(propertyName);
				}
			}
			else
			{
				fieldInfo = scriptable.GetType().GetField(propertyName);
			}
			if (propertyInfo != null)
			{
				mmproperty = new MMProperty(targetComponent, MMProperty.MemberTypes.Property, propertyInfo, null, propertyName, scriptable);
			}
			if (fieldInfo != null)
			{
				mmproperty = new MMProperty(targetComponent, MMProperty.MemberTypes.Field, null, fieldInfo, propertyName, scriptable);
			}
			if (propertyName == "")
			{
				if (source != null)
				{
					Debug.LogError("The MMProperty on " + source.name + " : you need to pick a property from the Property list");
				}
				return null;
			}
			if (propertyInfo == null && fieldInfo == null)
			{
				if (source != null)
				{
					Debug.LogError(string.Concat(new string[] { "The MMProperty on ", source.name, " couldn't find any property or field named ", propertyName, " on ", targetComponent.name }));
				}
				return null;
			}
			if (scriptable == null)
			{
				if (mmproperty.MemberType == MMProperty.MemberTypes.Property)
				{
					mmproperty.MemberPropertyInfo = targetComponent.GetType().GetProperty(mmproperty.MemberName);
					mmproperty.PropertyType = mmproperty.MemberPropertyInfo.PropertyType;
				}
				else if (mmproperty.MemberType == MMProperty.MemberTypes.Field)
				{
					mmproperty.MemberFieldInfo = targetComponent.GetType().GetField(mmproperty.MemberName);
					mmproperty.PropertyType = mmproperty.MemberFieldInfo.FieldType;
				}
			}
			else
			{
				mmproperty.MemberFieldInfo = scriptable.GetType().GetField(mmproperty.MemberName);
				mmproperty.PropertyType = mmproperty.MemberFieldInfo.FieldType;
			}
			return mmproperty;
		}

		// Token: 0x040007EF RID: 2031
		public Component TargetComponent;

		// Token: 0x040007F0 RID: 2032
		public ScriptableObject TargetScriptableObject;

		// Token: 0x040007F1 RID: 2033
		public MMProperty.MemberTypes MemberType;

		// Token: 0x040007F2 RID: 2034
		public PropertyInfo MemberPropertyInfo;

		// Token: 0x040007F3 RID: 2035
		public FieldInfo MemberFieldInfo;

		// Token: 0x040007F4 RID: 2036
		public Type PropertyType;

		// Token: 0x040007F5 RID: 2037
		public string MemberName;

		// Token: 0x0200043C RID: 1084
		public enum MemberTypes
		{
			// Token: 0x04001FC0 RID: 8128
			Property,
			// Token: 0x04001FC1 RID: 8129
			Field
		}
	}
}
