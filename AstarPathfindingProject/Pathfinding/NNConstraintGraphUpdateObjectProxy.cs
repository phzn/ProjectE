using System;

namespace Pathfinding
{
	// Token: 0x02000067 RID: 103
	public class NNConstraintGraphUpdateObjectProxy
	{
		// Token: 0x060003FF RID: 1023 RVA: 0x00012D4E File Offset: 0x00010F4E
		public NNConstraintGraphUpdateObjectProxy(GraphUpdateObject graphUpdateObject)
		{
			this.graphUpdateObject = graphUpdateObject;
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x00012D5D File Offset: 0x00010F5D
		// (set) Token: 0x06000401 RID: 1025 RVA: 0x00012D6A File Offset: 0x00010F6A
		public GraphMask graphMask
		{
			get
			{
				return this.graphUpdateObject.graphMask;
			}
			set
			{
				this.graphUpdateObject.graphMask = value;
			}
		}

		// Token: 0x04000246 RID: 582
		private readonly GraphUpdateObject graphUpdateObject;
	}
}
