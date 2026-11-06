using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x02000013 RID: 19
	[DefaultExecutionOrder(-200)]
	public class GPUIRigidbodySimulator : GPUIColliderHelper<GPUIRigidbodyReplacer>
	{
		// Token: 0x06000046 RID: 70 RVA: 0x000035C0 File Offset: 0x000017C0
		protected override void OnEnable()
		{
			base.OnEnable();
			if (GPUIRigidbodySimulator._Instances == null)
			{
				GPUIRigidbodySimulator._Instances = new List<GPUIRigidbodySimulator>();
			}
			if (!GPUIRigidbodySimulator._Instances.Contains(this))
			{
				GPUIRigidbodySimulator._Instances.Add(this);
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000035F4 File Offset: 0x000017F4
		public static void InitializeInstance(GPUIRigidbodyReplacer instance)
		{
			if (GPUIRigidbodySimulator._Instances != null)
			{
				foreach (GPUIRigidbodySimulator gpuirigidbodySimulator in GPUIRigidbodySimulator._Instances)
				{
					Rigidbody rigidbody;
					if (gpuirigidbodySimulator.IsInsideCollider(instance))
					{
						gpuirigidbodySimulator._enteredInstances.Add(instance);
						gpuirigidbodySimulator.OnEnteredCollider(instance);
					}
					else if (instance.TryGetComponent<Rigidbody>(out rigidbody))
					{
						instance.ReplaceRigidbody(rigidbody);
					}
				}
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003678 File Offset: 0x00001878
		protected override void OnEnteredCollider(GPUIRigidbodyReplacer instance)
		{
			instance.AddRigidbody();
			instance.gpuiPrefab.UpdateTransformData();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000368C File Offset: 0x0000188C
		protected override bool OnExitedCollider(GPUIRigidbodyReplacer instance)
		{
			Rigidbody rigidbody;
			if (instance.TryGetComponent<Rigidbody>(out rigidbody))
			{
				if (!rigidbody.IsSleeping())
				{
					return false;
				}
				instance.ReplaceRigidbody(rigidbody);
			}
			instance.gpuiPrefab.UpdateTransformData();
			return true;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000036C0 File Offset: 0x000018C0
		protected override void OnUpdate(GPUIRigidbodyReplacer instance)
		{
			instance.gpuiPrefab.UpdateTransformData();
		}

		// Token: 0x04000062 RID: 98
		private static List<GPUIRigidbodySimulator> _Instances;
	}
}
