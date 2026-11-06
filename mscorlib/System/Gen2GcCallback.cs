using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200011D RID: 285
	internal sealed class Gen2GcCallback : CriticalFinalizerObject
	{
		// Token: 0x06000AEF RID: 2799 RVA: 0x0002891B File Offset: 0x00026B1B
		private Gen2GcCallback()
		{
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00028923 File Offset: 0x00026B23
		public static void Register(Func<object, bool> callback, object targetObj)
		{
			new Gen2GcCallback().Setup(callback, targetObj);
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x00028931 File Offset: 0x00026B31
		private void Setup(Func<object, bool> callback, object targetObj)
		{
			this._callback = callback;
			this._weakTargetObj = GCHandle.Alloc(targetObj, GCHandleType.Weak);
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x00028948 File Offset: 0x00026B48
		protected override void Finalize()
		{
			try
			{
				object target = this._weakTargetObj.Target;
				if (target == null)
				{
					this._weakTargetObj.Free();
				}
				else
				{
					try
					{
						if (!this._callback(target))
						{
							return;
						}
					}
					catch
					{
					}
					if (!Environment.HasShutdownStarted)
					{
						GC.ReRegisterForFinalize(this);
					}
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x040010E3 RID: 4323
		private Func<object, bool> _callback;

		// Token: 0x040010E4 RID: 4324
		private GCHandle _weakTargetObj;
	}
}
