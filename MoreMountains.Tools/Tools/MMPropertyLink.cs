using System;

namespace MoreMountains.Tools
{
	// Token: 0x0200011A RID: 282
	public abstract class MMPropertyLink
	{
		// Token: 0x0600063C RID: 1596 RVA: 0x0001FD88 File Offset: 0x0001DF88
		public virtual void Initialization(MMProperty property)
		{
			this.CreateGettersAndSetters(property);
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0001FD91 File Offset: 0x0001DF91
		public virtual void CreateGettersAndSetters(MMProperty property)
		{
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0001FD93 File Offset: 0x0001DF93
		public virtual float GetLevel(MMPropertyEmitter emitter, MMProperty property)
		{
			return 0f;
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0001FD9A File Offset: 0x0001DF9A
		public virtual void SetLevel(MMPropertyReceiver receiver, MMProperty property, float level)
		{
			receiver.Level = level;
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0001FDA3 File Offset: 0x0001DFA3
		public virtual object GetValue(MMPropertyEmitter emitter, MMProperty property)
		{
			return 0f;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0001FDAF File Offset: 0x0001DFAF
		public virtual void SetValue(MMPropertyReceiver receiver, MMProperty property, object newValue)
		{
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0001FDB4 File Offset: 0x0001DFB4
		public virtual object GetPropertyValue(MMProperty property)
		{
			object obj = ((property.TargetScriptableObject == null) ? property.TargetComponent : property.TargetScriptableObject);
			if (property.MemberType == MMProperty.MemberTypes.Property)
			{
				return property.MemberPropertyInfo.GetValue(obj);
			}
			if (property.MemberType == MMProperty.MemberTypes.Field)
			{
				return property.MemberFieldInfo.GetValue(obj);
			}
			return 0f;
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0001FE14 File Offset: 0x0001E014
		protected virtual void SetPropertyValue(MMProperty property, object newValue)
		{
			object obj = ((property.TargetScriptableObject == null) ? property.TargetComponent : property.TargetScriptableObject);
			if (property.MemberType == MMProperty.MemberTypes.Property)
			{
				property.MemberPropertyInfo.SetValue(obj, newValue);
				return;
			}
			if (property.MemberType == MMProperty.MemberTypes.Field)
			{
				property.MemberFieldInfo.SetValue(obj, newValue);
			}
		}

		// Token: 0x040007F6 RID: 2038
		protected bool _getterSetterInitialized;
	}
}
