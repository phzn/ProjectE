using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Properties.Internal;

namespace Unity.Properties
{
	// Token: 0x0200002E RID: 46
	internal readonly struct FieldMember : IMemberInfo
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x00004D40 File Offset: 0x00002F40
		public FieldMember(FieldInfo fieldInfo)
		{
			this.m_FieldInfo = fieldInfo;
			this.Name = ReflectionUtilities.SanitizeMemberName(this.m_FieldInfo);
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00004D5B File Offset: 0x00002F5B
		public string Name { get; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00004D63 File Offset: 0x00002F63
		public bool IsReadOnly
		{
			get
			{
				return this.m_FieldInfo.IsInitOnly;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00004D70 File Offset: 0x00002F70
		public Type ValueType
		{
			get
			{
				return this.m_FieldInfo.FieldType;
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004D7D File Offset: 0x00002F7D
		public object GetValue(object obj)
		{
			return this.m_FieldInfo.GetValue(obj);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004D8B File Offset: 0x00002F8B
		public void SetValue(object obj, object value)
		{
			this.m_FieldInfo.SetValue(obj, value);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004D9B File Offset: 0x00002F9B
		public IEnumerable<Attribute> GetCustomAttributes()
		{
			return this.m_FieldInfo.GetCustomAttributes();
		}

		// Token: 0x0400004D RID: 77
		internal readonly FieldInfo m_FieldInfo;
	}
}
