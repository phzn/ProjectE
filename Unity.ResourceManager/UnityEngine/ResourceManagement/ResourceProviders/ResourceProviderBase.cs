using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x02000042 RID: 66
	public abstract class ResourceProviderBase : IResourceProvider, IInitializableObject
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00006D4D File Offset: 0x00004F4D
		public virtual string ProviderId
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_ProviderId))
				{
					this.m_ProviderId = base.GetType().FullName;
				}
				return this.m_ProviderId;
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00006D73 File Offset: 0x00004F73
		public virtual bool Initialize(string id, string data)
		{
			this.m_ProviderId = id;
			return !string.IsNullOrEmpty(this.m_ProviderId);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00006D8A File Offset: 0x00004F8A
		public virtual bool CanProvide(Type t, IResourceLocation location)
		{
			return this.GetDefaultType(location).IsAssignableFrom(t);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00006D99 File Offset: 0x00004F99
		public override string ToString()
		{
			return this.ProviderId;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00006DA1 File Offset: 0x00004FA1
		public virtual void Release(IResourceLocation location, object obj)
		{
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00006DA3 File Offset: 0x00004FA3
		public virtual Type GetDefaultType(IResourceLocation location)
		{
			return typeof(object);
		}

		// Token: 0x060001A5 RID: 421
		public abstract void Provide(ProvideHandle provideHandle);

		// Token: 0x060001A6 RID: 422 RVA: 0x00006DB0 File Offset: 0x00004FB0
		public virtual AsyncOperationHandle<bool> InitializeAsync(ResourceManager rm, string id, string data)
		{
			ResourceProviderBase.BaseInitAsyncOp baseInitAsyncOp = new ResourceProviderBase.BaseInitAsyncOp();
			baseInitAsyncOp.Init(() => this.Initialize(id, data));
			return rm.StartOperation<bool>(baseInitAsyncOp, default(AsyncOperationHandle));
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00006E00 File Offset: 0x00005000
		ProviderBehaviourFlags IResourceProvider.BehaviourFlags
		{
			get
			{
				return this.m_BehaviourFlags;
			}
		}

		// Token: 0x0400009F RID: 159
		protected string m_ProviderId;

		// Token: 0x040000A0 RID: 160
		protected ProviderBehaviourFlags m_BehaviourFlags;

		// Token: 0x02000078 RID: 120
		private class BaseInitAsyncOp : AsyncOperationBase<bool>
		{
			// Token: 0x0600036E RID: 878 RVA: 0x0000C537 File Offset: 0x0000A737
			public void Init(Func<bool> callback)
			{
				this.m_CallBack = callback;
			}

			// Token: 0x0600036F RID: 879 RVA: 0x0000C540 File Offset: 0x0000A740
			protected override bool InvokeWaitForCompletion()
			{
				ResourceManager rm = this.m_RM;
				if (rm != null)
				{
					rm.Update(Time.unscaledDeltaTime);
				}
				if (!this.HasExecuted)
				{
					base.InvokeExecute();
				}
				return true;
			}

			// Token: 0x06000370 RID: 880 RVA: 0x0000C567 File Offset: 0x0000A767
			protected override void Execute()
			{
				if (this.m_CallBack != null)
				{
					base.Complete(this.m_CallBack(), true, "");
					return;
				}
				base.Complete(true, true, "");
			}

			// Token: 0x0400016A RID: 362
			private Func<bool> m_CallBack;
		}
	}
}
