using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008CF RID: 2255
	[DefaultExecutionOrder(30000)]
	public class BlendShapeSynchronizer : MonoBehaviour
	{
		// Token: 0x06003CA6 RID: 15526 RVA: 0x00168E70 File Offset: 0x00167070
		public bool Synchronizes(SkinnedMeshRenderer renderer)
		{
			return base.enabled && this._isInitialized && this.sourceRenderer != null && this.sourceRenderer != renderer && this.sourceRenderer.sharedMesh != null && this.targetRenderer == renderer && renderer != null && renderer.sharedMesh != null && this.sourceRenderer.sharedMesh == renderer.sharedMesh && this._blendShapeMapping.Count == renderer.sharedMesh.blendShapeCount;
		}

		// Token: 0x06003CA7 RID: 15527 RVA: 0x00168F19 File Offset: 0x00167119
		private void Awake()
		{
			if (this.targetRenderer == null)
			{
				this.targetRenderer = base.GetComponent<SkinnedMeshRenderer>();
			}
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x00168F35 File Offset: 0x00167135
		private void Start()
		{
			if (!this._isInitialized)
			{
				this.InitializeMapping();
			}
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x00168F45 File Offset: 0x00167145
		private void LateUpdate()
		{
			this.SyncBlendShapes();
		}

		// Token: 0x06003CAA RID: 15530 RVA: 0x00168F4D File Offset: 0x0016714D
		public void SynchronizeNow()
		{
			if (this.targetRenderer == null)
			{
				this.targetRenderer = base.GetComponent<SkinnedMeshRenderer>();
			}
			if (!this._isInitialized)
			{
				this.InitializeMapping();
			}
			this.SyncBlendShapes();
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x00168F7D File Offset: 0x0016717D
		public void SetSourceRenderer(SkinnedMeshRenderer source)
		{
			this.sourceRenderer = source;
			this.InitializeMapping();
		}

		// Token: 0x06003CAC RID: 15532 RVA: 0x00168F8C File Offset: 0x0016718C
		[ContextMenu("Refresh Mapping")]
		public void InitializeMapping()
		{
			if (this.sourceRenderer == null || this.targetRenderer == null)
			{
				this._isInitialized = false;
				return;
			}
			this._blendShapeMapping.Clear();
			Mesh sharedMesh = this.sourceRenderer.sharedMesh;
			Mesh sharedMesh2 = this.targetRenderer.sharedMesh;
			if (sharedMesh == null || sharedMesh2 == null)
			{
				this._isInitialized = false;
				return;
			}
			for (int i = 0; i < sharedMesh2.blendShapeCount; i++)
			{
				if (sharedMesh == sharedMesh2)
				{
					this._blendShapeMapping[i] = i;
				}
				else
				{
					string blendShapeName = sharedMesh2.GetBlendShapeName(i);
					int num = sharedMesh.GetBlendShapeIndex(blendShapeName);
					if (num == -1 && this.usePartialNameMatching)
					{
						string shortName = this.GetShortName(blendShapeName);
						for (int j = 0; j < sharedMesh.blendShapeCount; j++)
						{
							string blendShapeName2 = sharedMesh.GetBlendShapeName(j);
							if (this.GetShortName(blendShapeName2) == shortName)
							{
								num = j;
								break;
							}
						}
					}
					if (num != -1)
					{
						this._blendShapeMapping[i] = num;
					}
				}
			}
			this._isInitialized = true;
			this.SyncBlendShapes();
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x001690AC File Offset: 0x001672AC
		private void SyncBlendShapes()
		{
			if (!this._isInitialized || this.sourceRenderer == null || this.targetRenderer == null)
			{
				return;
			}
			foreach (KeyValuePair<int, int> keyValuePair in this._blendShapeMapping)
			{
				float blendShapeWeight = this.sourceRenderer.GetBlendShapeWeight(keyValuePair.Value);
				if (this.targetRenderer.GetBlendShapeWeight(keyValuePair.Key) != blendShapeWeight)
				{
					this.targetRenderer.SetBlendShapeWeight(keyValuePair.Key, blendShapeWeight);
				}
			}
		}

		// Token: 0x06003CAE RID: 15534 RVA: 0x00169158 File Offset: 0x00167358
		private string GetShortName(string fullName)
		{
			int num = fullName.LastIndexOf('.');
			if (num >= 0 && num < fullName.Length - 1)
			{
				return fullName.Substring(num + 1);
			}
			return fullName;
		}

		// Token: 0x04003CB3 RID: 15539
		[Tooltip("The SkinnedMeshRenderer to copy blendshape values from")]
		[SerializeField]
		private SkinnedMeshRenderer sourceRenderer;

		// Token: 0x04003CB4 RID: 15540
		[Tooltip("The SkinnedMeshRenderer to apply blendshape values to. If null, uses the component on this GameObject.")]
		[SerializeField]
		private SkinnedMeshRenderer targetRenderer;

		// Token: 0x04003CB5 RID: 15541
		[Tooltip("If true, attempts to match blendshapes even if they have different prefixes (e.g. 'Head.Blink' matches 'Accessory.Blink')")]
		[SerializeField]
		private bool usePartialNameMatching;

		// Token: 0x04003CB6 RID: 15542
		private Dictionary<int, int> _blendShapeMapping = new Dictionary<int, int>();

		// Token: 0x04003CB7 RID: 15543
		private bool _isInitialized;
	}
}
