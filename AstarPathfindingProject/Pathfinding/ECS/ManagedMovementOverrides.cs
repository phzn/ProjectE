using System;
using Unity.Entities;

namespace Pathfinding.ECS
{
	// Token: 0x020002F7 RID: 759
	public struct ManagedMovementOverrides
	{
		// Token: 0x060012E4 RID: 4836 RVA: 0x000732E8 File Offset: 0x000714E8
		public ManagedMovementOverrides(Entity entity, World world)
		{
			this.entity = entity;
			this.world = world;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x000732F8 File Offset: 0x000714F8
		public void AddBeforeControlCallback(BeforeControlDelegate value)
		{
			this.AddCallback<ManagedMovementOverrideBeforeControl, BeforeControlDelegate>(value);
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x00073301 File Offset: 0x00071501
		public void RemoveBeforeControlCallback(BeforeControlDelegate value)
		{
			this.RemoveCallback<ManagedMovementOverrideBeforeControl, BeforeControlDelegate>(value);
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x0007330A File Offset: 0x0007150A
		public void AddAfterControlCallback(AfterControlDelegate value)
		{
			this.AddCallback<ManagedMovementOverrideAfterControl, AfterControlDelegate>(value);
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x00073313 File Offset: 0x00071513
		public void RemoveAfterControlCallback(AfterControlDelegate value)
		{
			this.RemoveCallback<ManagedMovementOverrideAfterControl, AfterControlDelegate>(value);
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0007331C File Offset: 0x0007151C
		public void AddBeforeMovementCallback(BeforeMovementDelegate value)
		{
			this.AddCallback<ManagedMovementOverrideBeforeMovement, BeforeMovementDelegate>(value);
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x00073325 File Offset: 0x00071525
		public void RemoveBeforeMovementCallback(BeforeMovementDelegate value)
		{
			this.RemoveCallback<ManagedMovementOverrideBeforeMovement, BeforeMovementDelegate>(value);
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x00073330 File Offset: 0x00071530
		private void AddCallback<C, T>(T callback) where C : ManagedMovementOverride<T>, IComponentData, new() where T : Delegate
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			if (this.world == null || !this.world.EntityManager.Exists(this.entity))
			{
				throw new InvalidOperationException("The entity does not exist. You can only set a callback when the FollowerEntity is active and has been enabled. If you are trying to set this during Awake or OnEnable, try setting it during Start instead.");
			}
			if (!this.world.EntityManager.HasComponent<C>(this.entity))
			{
				this.world.EntityManager.AddComponentData(this.entity, new C());
			}
			this.world.EntityManager.GetComponentData<C>(this.entity).AddCallback(callback);
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x000733DC File Offset: 0x000715DC
		private void RemoveCallback<C, T>(T callback) where C : ManagedMovementOverride<T>, IComponentData, new() where T : Delegate
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			if (this.world == null || !this.world.EntityManager.Exists(this.entity))
			{
				return;
			}
			if (!this.world.EntityManager.HasComponent<C>(this.entity))
			{
				return;
			}
			if (!this.world.EntityManager.GetComponentData<C>(this.entity).RemoveCallback(callback))
			{
				this.world.EntityManager.RemoveComponent<C>(this.entity);
			}
		}

		// Token: 0x04000DC5 RID: 3525
		private Entity entity;

		// Token: 0x04000DC6 RID: 3526
		private World world;
	}
}
