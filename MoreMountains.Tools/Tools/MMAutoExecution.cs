using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000009 RID: 9
	public class MMAutoExecution : MonoBehaviour
	{
		// Token: 0x06000021 RID: 33 RVA: 0x0000268C File Offset: 0x0000088C
		protected virtual void Awake()
		{
			foreach (MMAutoExecutionItem mmautoExecutionItem in this.Events)
			{
				if (mmautoExecutionItem.AutoExecuteOnAwake && mmautoExecutionItem.Event != null)
				{
					mmautoExecutionItem.Event.Invoke();
				}
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000026F4 File Offset: 0x000008F4
		protected virtual void Start()
		{
			foreach (MMAutoExecutionItem mmautoExecutionItem in this.Events)
			{
				if (mmautoExecutionItem.AutoExecuteOnStart && mmautoExecutionItem.Event != null)
				{
					mmautoExecutionItem.Event.Invoke();
				}
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000275C File Offset: 0x0000095C
		protected virtual void OnEnable()
		{
			foreach (MMAutoExecutionItem mmautoExecutionItem in this.Events)
			{
				if (mmautoExecutionItem.AutoExecuteOnEnable && mmautoExecutionItem.Event != null)
				{
					mmautoExecutionItem.Event.Invoke();
				}
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000027C4 File Offset: 0x000009C4
		protected virtual void OnDisable()
		{
			foreach (MMAutoExecutionItem mmautoExecutionItem in this.Events)
			{
				if (mmautoExecutionItem.AutoExecuteOnDisable && mmautoExecutionItem.Event != null)
				{
					mmautoExecutionItem.Event.Invoke();
				}
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000282C File Offset: 0x00000A2C
		protected virtual void OnInstantiate()
		{
			foreach (MMAutoExecutionItem mmautoExecutionItem in this.Events)
			{
				if (mmautoExecutionItem.AutoExecuteOnInstantiate && mmautoExecutionItem.Event != null)
				{
					mmautoExecutionItem.Event.Invoke();
				}
			}
		}

		// Token: 0x04000020 RID: 32
		public List<MMAutoExecutionItem> Events;
	}
}
