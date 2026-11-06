using System;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.Operations
{
	// Token: 0x020000C2 RID: 194
	internal class LocalizationGroupOperation : GroupOperation
	{
		// Token: 0x06000583 RID: 1411 RVA: 0x00013ACC File Offset: 0x00011CCC
		protected override bool InvokeWaitForCompletion()
		{
			if (base.IsDone || base.Result == null)
			{
				return true;
			}
			for (int i = 0; i < base.Result.Count; i++)
			{
				base.Result[i].WaitForCompletion();
				if (base.Result == null)
				{
					return true;
				}
			}
			ResourceManager rm = this.m_RM;
			if (rm != null)
			{
				rm.Update(Time.unscaledDeltaTime);
			}
			if (!base.IsDone && base.Result != null)
			{
				this.Execute();
			}
			ResourceManager rm2 = this.m_RM;
			if (rm2 != null)
			{
				rm2.Update(Time.unscaledDeltaTime);
			}
			return base.IsDone;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00013B68 File Offset: 0x00011D68
		protected override void Destroy()
		{
			LocalizationGroupOperation.Pool.Release(this);
			base.Destroy();
		}

		// Token: 0x040001D0 RID: 464
		public static readonly ObjectPool<LocalizationGroupOperation> Pool = new ObjectPool<LocalizationGroupOperation>(() => new LocalizationGroupOperation(), null, null, null, false, 10, 10000);
	}
}
