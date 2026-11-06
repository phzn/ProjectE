using System;
using System.Collections.Generic;

namespace DA_Assets.SVGMeshUnity.Internals
{
	// Token: 0x0200000C RID: 12
	public class WorkBufferPool
	{
		// Token: 0x0600005A RID: 90 RVA: 0x000040A4 File Offset: 0x000022A4
		public WorkBuffer<T> Get<T>()
		{
			WorkBuffer<T> workBuffer = null;
			Stack<object> stack;
			if (this.Pool.TryGetValue(typeof(T), out stack) && stack.Count > 0)
			{
				workBuffer = (WorkBuffer<T>)stack.Pop();
			}
			return workBuffer ?? new WorkBuffer<T>(32);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000040ED File Offset: 0x000022ED
		public void Get<T>(ref WorkBuffer<T> buf)
		{
			buf = this.Get<T>();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000040F8 File Offset: 0x000022F8
		public void Release<T>(ref WorkBuffer<T> buf)
		{
			buf.Clear();
			Stack<object> stack;
			if (!this.Pool.TryGetValue(typeof(T), out stack))
			{
				stack = (this.Pool[typeof(T)] = new Stack<object>());
			}
			stack.Push(buf);
			buf = null;
		}

		// Token: 0x0400002C RID: 44
		private readonly Dictionary<Type, Stack<object>> Pool = new Dictionary<Type, Stack<object>>();
	}
}
