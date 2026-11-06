using System;
using Pathfinding.Util;
using Unity.Entities;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000010 RID: 16
	[UniqueComponent(tag = "ai.destination")]
	[AddComponentMenu("Pathfinding/AI/Behaviors/AIDestinationSetter")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/aidestinationsetter.html")]
	public class AIDestinationSetter : VersionedMonoBehaviour, IRuntimeBaker
	{
		// Token: 0x0600007C RID: 124 RVA: 0x0000420C File Offset: 0x0000240C
		private void OnEnable()
		{
			this.ai = base.GetComponent<IAstarAI>();
			FollowerEntity followerEntity = this.ai as FollowerEntity;
			if (followerEntity != null)
			{
				followerEntity.RegisterRuntimeBaker(this);
				return;
			}
			if (this.ai != null)
			{
				IAstarAI astarAI = this.ai;
				astarAI.onSearchPath = (Action)Delegate.Combine(astarAI.onSearchPath, new Action(this.UpdateDestination));
			}
			BatchedEvents.Add<AIDestinationSetter>(this, BatchedEvents.Event.Update, new Action<AIDestinationSetter[], int>(AIDestinationSetter.OnUpdate), 0);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004280 File Offset: 0x00002480
		private void OnDisable()
		{
			if (this.world != null && this.world.IsCreated && this.world.EntityManager.Exists(this.entity))
			{
				this.world.EntityManager.RemoveComponent<AIDestinationSetter>(this.entity);
			}
			if (this.ai != null && !(this.ai is FollowerEntity))
			{
				IAstarAI astarAI = this.ai;
				astarAI.onSearchPath = (Action)Delegate.Remove(astarAI.onSearchPath, new Action(this.UpdateDestination));
			}
			BatchedEvents.Remove<AIDestinationSetter>(this);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000431C File Offset: 0x0000251C
		void IRuntimeBaker.OnCreatedEntity(World world, Entity entity)
		{
			this.entity = entity;
			this.world = world;
			world.EntityManager.AddComponentObject(entity, this);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004348 File Offset: 0x00002548
		private static void OnUpdate(AIDestinationSetter[] components, int count)
		{
			for (int i = 0; i < count; i++)
			{
				components[i].UpdateDestination();
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004369 File Offset: 0x00002569
		private void UpdateDestination()
		{
			if (this.target != null && this.ai != null)
			{
				this.ai.destination = this.target.position;
			}
		}

		// Token: 0x0400006D RID: 109
		public Transform target;

		// Token: 0x0400006E RID: 110
		public bool useRotation;

		// Token: 0x0400006F RID: 111
		private IAstarAI ai;

		// Token: 0x04000070 RID: 112
		private Entity entity;

		// Token: 0x04000071 RID: 113
		private World world;
	}
}
