using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000056 RID: 86
	[HelpURL("https://wiki.gurbu.com/index.php?title=GPU_Instancer_Pro:GettingStarted#GPUI_Area_Culler")]
	[DefaultExecutionOrder(1200)]
	[ExecuteInEditMode]
	public class GPUIAreaCuller : MonoBehaviour
	{
		// Token: 0x06000340 RID: 832 RVA: 0x00011AAC File Offset: 0x0000FCAC
		private void OnEnable()
		{
			if (this.useColliders)
			{
				this._colliders = base.GetComponents<Collider>();
				if (Application.isPlaying && (this._colliders == null || this._colliders.Length == 0))
				{
					Debug.LogWarning(GPUIConstants.LOG_PREFIX + "GPUI Area Culler can not find any colliders on its GameObject.", base.gameObject);
					base.enabled = false;
					return;
				}
			}
			GPUIRenderingSystem.OnBufferDataModified = (GPUIRenderingSystem.OnBufferDataModifiedCallback)Delegate.Remove(GPUIRenderingSystem.OnBufferDataModified, new GPUIRenderingSystem.OnBufferDataModifiedCallback(this.CullInstances));
			GPUIRenderingSystem.OnBufferDataModified = (GPUIRenderingSystem.OnBufferDataModifiedCallback)Delegate.Combine(GPUIRenderingSystem.OnBufferDataModified, new GPUIRenderingSystem.OnBufferDataModifiedCallback(this.CullInstances));
			this.OnValuesChanged();
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00011B4D File Offset: 0x0000FD4D
		private void OnDisable()
		{
			GPUIRenderingSystem.OnBufferDataModified = (GPUIRenderingSystem.OnBufferDataModifiedCallback)Delegate.Remove(GPUIRenderingSystem.OnBufferDataModified, new GPUIRenderingSystem.OnBufferDataModifiedCallback(this.CullInstances));
			this.OnValuesChanged();
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00011B75 File Offset: 0x0000FD75
		private void Update()
		{
			if (base.transform.hasChanged)
			{
				this.OnValuesChanged();
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00011B8A File Offset: 0x0000FD8A
		public void OnValuesChanged()
		{
			base.transform.hasChanged = false;
			GPUITransformBufferUtility.ResetAllCulledInstances();
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00011BA0 File Offset: 0x0000FDA0
		private void CullInstances(GPUITransformBufferData transformBufferData)
		{
			if (this.useColliders && (this._colliders == null || this._colliders.Length == 0))
			{
				return;
			}
			int num = 0;
			int bufferSize = transformBufferData.RenderSourceGroup.BufferSize;
			bool flag = this.prototypeIndexFilter != null && this.prototypeIndexFilter.Count > 0;
			if (this.gpuiManagerFilter != null && this.gpuiManagerFilter.Count > 0)
			{
				for (int i = 0; i < this.gpuiManagerFilter.Count; i++)
				{
					GPUIManager gpuimanager = this.gpuiManagerFilter[i];
					if (!(gpuimanager == null))
					{
						int prototypeCount = gpuimanager.GetPrototypeCount();
						for (int j = 0; j < prototypeCount; j++)
						{
							GPUITransformBufferData gpuitransformBufferData;
							if ((!flag || this.prototypeIndexFilter.Contains(j)) && GPUIRenderingSystem.TryGetTransformBufferData(gpuimanager.GetRenderKey(j), out gpuitransformBufferData, out num, out bufferSize, true) && gpuitransformBufferData == transformBufferData)
							{
								if (this.useColliders)
								{
									foreach (Collider collider in this._colliders)
									{
										GPUITransformBufferUtility.CullInstancesInsideCollider(transformBufferData, num, bufferSize, collider, this.offset);
									}
								}
								else
								{
									Bounds bounds = this.bounds;
									bounds.center += base.transform.position;
									GPUITransformBufferUtility.CullInstancesInsideBounds(transformBufferData, num, bufferSize, bounds, this.offset);
								}
							}
						}
					}
				}
				return;
			}
			if (this.useColliders)
			{
				foreach (Collider collider2 in this._colliders)
				{
					GPUITransformBufferUtility.CullInstancesInsideCollider(transformBufferData, num, bufferSize, collider2, this.offset);
				}
				return;
			}
			Bounds bounds2 = this.bounds;
			bounds2.center += base.transform.position;
			GPUITransformBufferUtility.CullInstancesInsideBounds(transformBufferData, num, bufferSize, bounds2, this.offset);
		}

		// Token: 0x04000250 RID: 592
		[SerializeField]
		public bool useColliders = true;

		// Token: 0x04000251 RID: 593
		[SerializeField]
		public Bounds bounds;

		// Token: 0x04000252 RID: 594
		[SerializeField]
		public List<GPUIManager> gpuiManagerFilter;

		// Token: 0x04000253 RID: 595
		[SerializeField]
		public List<int> prototypeIndexFilter;

		// Token: 0x04000254 RID: 596
		[SerializeField]
		public float offset;

		// Token: 0x04000255 RID: 597
		[NonSerialized]
		private Collider[] _colliders;
	}
}
