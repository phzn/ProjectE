using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Properties.Internal;

namespace Unity.Properties
{
	// Token: 0x0200002F RID: 47
	internal readonly struct PropertyMember : IMemberInfo
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00004DA8 File Offset: 0x00002FA8
		public string Name { get; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00004DB0 File Offset: 0x00002FB0
		public bool IsReadOnly
		{
			get
			{
				return !this.m_PropertyInfo.CanWrite;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00004DC0 File Offset: 0x00002FC0
		public Type ValueType
		{
			get
			{
				return this.m_PropertyInfo.PropertyType;
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004DCD File Offset: 0x00002FCD
		public PropertyMember(PropertyInfo propertyInfo)
		{
			this.m_PropertyInfo = propertyInfo;
			this.Name = ReflectionUtilities.SanitizeMemberName(this.m_PropertyInfo);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004DE8 File Offset: 0x00002FE8
		public object GetValue(object obj)
		{
			return this.m_PropertyInfo.GetValue(obj);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004DF6 File Offset: 0x00002FF6
		public void SetValue(object obj, object value)
		{
			this.m_PropertyInfo.SetValue(obj, value);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004E06 File Offset: 0x00003006
		public IEnumerable<Attribute> GetCustomAttributes()
		{
			return this.m_PropertyInfo.GetCustomAttributes();
		}

		// Token: 0x0400004F RID: 79
		internal readonly PropertyInfo m_PropertyInfo;
	}
}
