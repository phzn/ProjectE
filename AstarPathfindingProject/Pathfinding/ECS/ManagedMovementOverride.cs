using System;
using Unity.Entities;
using UnityEngine.Scripting;

namespace Pathfinding.ECS
{
	// Token: 0x020002F8 RID: 760
	public class ManagedMovementOverride<T> : IComponentData, IQueryTypeParameter where T : class, Delegate
	{
		// Token: 0x060012ED RID: 4845 RVA: 0x0007347F File Offset: 0x0007167F
		public void AddCallback(T callback)
		{
			this.callback = (T)((object)Delegate.Combine(this.callback, callback));
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x000734A4 File Offset: 0x000716A4
		public bool RemoveCallback(T callback)
		{
			return (this.callback = (T)((object)Delegate.Remove(this.callback, callback))) != null;
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x0000378A File Offset: 0x0000198A
		[Preserve]
		public ManagedMovementOverride()
		{
		}

		// Token: 0x04000DC7 RID: 3527
		public T callback;
	}
}
