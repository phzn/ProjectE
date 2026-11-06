using System;

namespace Pathfinding
{
	// Token: 0x02000120 RID: 288
	[Serializable]
	public abstract class MonoModifier : VersionedMonoBehaviour, IPathModifier
	{
		// Token: 0x060009C5 RID: 2501 RVA: 0x0003258B File Offset: 0x0003078B
		protected virtual void OnEnable()
		{
			this.seeker = base.GetComponent<Seeker>();
			if (this.seeker != null)
			{
				this.seeker.RegisterModifier(this);
			}
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x000325B3 File Offset: 0x000307B3
		protected virtual void OnDisable()
		{
			if (this.seeker != null)
			{
				this.seeker.DeregisterModifier(this);
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060009C7 RID: 2503
		public abstract int Order { get; }

		// Token: 0x060009C8 RID: 2504 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void PreProcess(Path path)
		{
		}

		// Token: 0x060009C9 RID: 2505
		public abstract void Apply(Path path);

		// Token: 0x04000633 RID: 1587
		[NonSerialized]
		public Seeker seeker;
	}
}
