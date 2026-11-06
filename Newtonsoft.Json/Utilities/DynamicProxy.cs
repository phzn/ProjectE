using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000051 RID: 81
	[NullableContext(1)]
	[Nullable(0)]
	internal class DynamicProxy<[Nullable(2)] T>
	{
		// Token: 0x060004D7 RID: 1239 RVA: 0x0001440E File Offset: 0x0001260E
		public virtual IEnumerable<string> GetDynamicMemberNames(T instance)
		{
			return CollectionUtils.ArrayEmpty<string>();
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00014415 File Offset: 0x00012615
		public virtual bool TryBinaryOperation(T instance, BinaryOperationBinder binder, object arg, [Nullable(2)] out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0001441C File Offset: 0x0001261C
		public virtual bool TryConvert(T instance, ConvertBinder binder, [Nullable(2)] out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00014422 File Offset: 0x00012622
		public virtual bool TryCreateInstance(T instance, CreateInstanceBinder binder, object[] args, [Nullable(2)] out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00014429 File Offset: 0x00012629
		public virtual bool TryDeleteIndex(T instance, DeleteIndexBinder binder, object[] indexes)
		{
			return false;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001442C File Offset: 0x0001262C
		public virtual bool TryDeleteMember(T instance, DeleteMemberBinder binder)
		{
			return false;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001442F File Offset: 0x0001262F
		public virtual bool TryGetIndex(T instance, GetIndexBinder binder, object[] indexes, [Nullable(2)] out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00014436 File Offset: 0x00012636
		public virtual bool TryGetMember(T instance, GetMemberBinder binder, [Nullable(2)] out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001443C File Offset: 0x0001263C
		public virtual bool TryInvoke(T instance, InvokeBinder binder, object[] args, [Nullable(2)] out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00014443 File Offset: 0x00012643
		public virtual bool TryInvokeMember(T instance, InvokeMemberBinder binder, object[] args, [Nullable(2)] out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001444A File Offset: 0x0001264A
		public virtual bool TrySetIndex(T instance, SetIndexBinder binder, object[] indexes, object value)
		{
			return false;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001444D File Offset: 0x0001264D
		public virtual bool TrySetMember(T instance, SetMemberBinder binder, object value)
		{
			return false;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00014450 File Offset: 0x00012650
		public virtual bool TryUnaryOperation(T instance, UnaryOperationBinder binder, [Nullable(2)] out object result)
		{
			result = null;
			return false;
		}
	}
}
