using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Entities.CodeGeneratedJobForEach;
using Unity.Jobs;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x020002CC RID: 716
	[RequireDerived]
	public abstract class SystemBase : ComponentSystemBase
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06001324 RID: 4900 RVA: 0x0004B35A File Offset: 0x0004955A
		// (set) Token: 0x06001325 RID: 4901 RVA: 0x0004B367 File Offset: 0x00049567
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected unsafe JobHandle Dependency
		{
			get
			{
				return base.CheckedState()->Dependency;
			}
			set
			{
				base.CheckedState()->Dependency = value;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06001326 RID: 4902 RVA: 0x0004B375 File Offset: 0x00049575
		public unsafe ref SystemState CheckedStateRef
		{
			get
			{
				return ref *base.CheckedState();
			}
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x0004B37D File Offset: 0x0004957D
		protected unsafe void CompleteDependency()
		{
			base.CheckedState()->CompleteDependency();
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06001328 RID: 4904 RVA: 0x0004B38C File Offset: 0x0004958C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected internal ForEachLambdaJobDescription Entities
		{
			get
			{
				return default(ForEachLambdaJobDescription);
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06001329 RID: 4905 RVA: 0x0004B3A4 File Offset: 0x000495A4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected internal LambdaSingleJobDescription Job
		{
			get
			{
				return default(LambdaSingleJobDescription);
			}
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x0004B3BC File Offset: 0x000495BC
		public unsafe sealed override void Update()
		{
			SystemState* ptr = base.CheckedState();
			if (base.Enabled && base.ShouldRunSystem())
			{
				ref WorldUnmanagedImpl impl = ref base.World.Unmanaged.GetImpl();
				WorldUnmanagedImpl.PreviousSystemGlobalState previousSystemGlobalState = new WorldUnmanagedImpl.PreviousSystemGlobalState(ref impl, ptr);
				ptr->BeforeOnUpdate();
				try
				{
					if (!ptr->PreviouslyEnabled)
					{
						ptr->PreviouslyEnabled = true;
						this.OnStartRunning();
					}
					this.OnUpdate();
					return;
				}
				finally
				{
					ptr->AfterOnUpdate();
					previousSystemGlobalState.Restore(ref impl, ptr);
				}
			}
			if (ptr->PreviouslyEnabled)
			{
				ref WorldUnmanagedImpl impl2 = ref base.World.Unmanaged.GetImpl();
				WorldUnmanagedImpl.PreviousSystemGlobalState previousSystemGlobalState2 = new WorldUnmanagedImpl.PreviousSystemGlobalState(ref impl2, ptr);
				ptr->PreviouslyEnabled = false;
				ptr->BeforeOnUpdate();
				try
				{
					this.OnStopRunningInternal();
				}
				finally
				{
					ptr->AfterOnUpdate();
					previousSystemGlobalState2.Restore(ref impl2, ptr);
				}
			}
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x0004B498 File Offset: 0x00049698
		internal sealed override void OnBeforeCreateInternal(World world)
		{
			base.OnBeforeCreateInternal(world);
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x0004B4A1 File Offset: 0x000496A1
		internal unsafe sealed override void OnBeforeDestroyInternal()
		{
			base.OnBeforeDestroyInternal();
			base.CheckedState()->m_JobHandle.Complete();
		}

		// Token: 0x0600132D RID: 4909
		[RequiredMember]
		[Preserve]
		protected abstract void OnUpdate();

		// Token: 0x0600132E RID: 4910 RVA: 0x0004B4BC File Offset: 0x000496BC
		[Obsolete("Use SystemAPI.GetComponent instead (RemovedAfter Entities 1.0)")]
		protected internal T GetComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IComponentData
		{
			return base.EntityManager.GetComponentData<T>(entity);
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x0004B4D8 File Offset: 0x000496D8
		[Obsolete("Use SystemAPI.SetComponent instead (RemovedAfter Entities 1.0)")]
		protected internal void SetComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, T component) where T : struct, ValueType, IComponentData
		{
			base.EntityManager.SetComponentData<T>(entity, component);
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0004B4F8 File Offset: 0x000496F8
		[Obsolete("Use SystemAPI.HasComponent instead (RemovedAfter Entities 1.0)")]
		protected internal bool HasComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IComponentData
		{
			return base.EntityManager.HasComponent<T>(entity);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0004B514 File Offset: 0x00049714
		protected internal bool HasBuffer<T>(Entity entity) where T : struct, IBufferElementData
		{
			return base.EntityManager.HasBuffer<T>(entity);
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0004B530 File Offset: 0x00049730
		public new ComponentLookup<T> GetComponentLookup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IComponentData
		{
			return base.GetComponentLookup<T>(isReadOnly);
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0004B530 File Offset: 0x00049730
		[Obsolete("This method has been renamed to GetComponentLookup. (RemovedAFter Entities 1.0)", true)]
		public new ComponentLookup<T> GetComponentDataFromEntity<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IComponentData
		{
			return base.GetComponentLookup<T>(isReadOnly);
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x0004B539 File Offset: 0x00049739
		[Obsolete("Use SystemAPI.GetBuffer instead (RemovedAfter Entities 1.0)")]
		public unsafe DynamicBuffer<T> GetBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			return base.CheckedState()->GetBuffer<T>(entity, isReadOnly);
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x0004B548 File Offset: 0x00049748
		public new BufferLookup<T> GetBufferLookup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			return base.GetBufferLookup<T>(isReadOnly);
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x0004B548 File Offset: 0x00049748
		[Obsolete("This method has been renamed to GetBufferLookup. (RemovedAFter Entities 1.0)", true)]
		public new BufferLookup<T> GetBufferFromEntity<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			return base.GetBufferLookup<T>(isReadOnly);
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x0004B551 File Offset: 0x00049751
		public new EntityStorageInfoLookup GetEntityStorageInfoLookup()
		{
			return base.GetEntityStorageInfoLookup();
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x0004B551 File Offset: 0x00049751
		[Obsolete("This method has been renamed to GetEntityStorageInfoLookup. (RemovedAFter Entities 1.0)", true)]
		public new EntityStorageInfoLookup GetStorageInfoFromEntity()
		{
			return base.GetEntityStorageInfoLookup();
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x0004B55C File Offset: 0x0004975C
		[Obsolete("Use SystemAPI.Exists instead (RemovedAfter Entities 1.0)")]
		public bool Exists(Entity entity)
		{
			return base.EntityManager.Exists(entity);
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0004B578 File Offset: 0x00049778
		[Preserve]
		protected SystemBase()
		{
		}
	}
}
