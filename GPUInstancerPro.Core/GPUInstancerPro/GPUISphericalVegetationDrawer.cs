using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200004F RID: 79
	[ExecuteInEditMode]
	public class GPUISphericalVegetationDrawer : MonoBehaviour
	{
		// Token: 0x060002A0 RID: 672 RVA: 0x0000EECE File Offset: 0x0000D0CE
		private void OnEnable()
		{
			if (this.vegetationSettings == null || this.vegetationSettings.Count == 0 || this.sphereTransform == null)
			{
				return;
			}
			this.RegisterRenderers();
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000EEFA File Offset: 0x0000D0FA
		private void OnDisable()
		{
			this.DisposeRenderers();
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000EF02 File Offset: 0x0000D102
		private void OnValidate()
		{
			if (GPUIRenderingSystem.IsActive && this._isInitialized)
			{
				this.RegisterRenderers();
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000EF1C File Offset: 0x0000D11C
		public void RegisterRenderers()
		{
			this.DisposeRenderers();
			this.CreateMatrices();
			this._renderKeys = new int[this.vegetationSettings.Count];
			for (int i = 0; i < this.vegetationSettings.Count; i++)
			{
				if (this.vegetationSettings[i].matrices != null && GPUICoreAPI.RegisterRenderer(this, this.vegetationSettings[i].prefab, this.vegetationSettings[i].profile, out this._renderKeys[i]))
				{
					GPUICoreAPI.SetTransformBufferData(this._renderKeys[i], this.vegetationSettings[i].matrices, 0, 0, 0, true);
				}
			}
			this._previousMatrix.SetTRS(this.sphereTransform.position, Quaternion.identity, this.sphereTransform.lossyScale);
			GPUICoreAPI.AddCameraEventOnPreCull(new Action<GPUICameraData>(this.HandleFloatingOrigin));
			this._isInitialized = true;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000F00C File Offset: 0x0000D20C
		public void DisposeRenderers()
		{
			this._isInitialized = false;
			GPUICoreAPI.RemoveCameraEventOnPreCull(new Action<GPUICameraData>(this.HandleFloatingOrigin));
			if (this._renderKeys == null)
			{
				return;
			}
			for (int i = 0; i < this._renderKeys.Length; i++)
			{
				if (this._renderKeys[i] != 0)
				{
					GPUICoreAPI.DisposeRenderer(this._renderKeys[i]);
				}
			}
			this._renderKeys = null;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000F06C File Offset: 0x0000D26C
		public void CreateMatrices()
		{
			for (int i = 0; i < this.vegetationSettings.Count; i++)
			{
				if (this.seed != 0)
				{
					Random.InitState(this.seed + i);
				}
				GPUISphericalVegetationDrawer.VegetationSetting vegetationSetting = this.vegetationSettings[i];
				Matrix4x4[] array = new Matrix4x4[this.vegetationSettings[i].spawnAmount];
				Vector3 position = this.sphereTransform.position;
				Vector3 vector = this.sphereTransform.localScale * this.scaleMultiplier;
				for (int j = 0; j < array.Length; j++)
				{
					Vector3 vector2 = Vector3.Normalize(Random.insideUnitSphere) * vector.x / 2f + position;
					Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, vector2 - position) * Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
					array[j] = Matrix4x4.TRS(vector2, quaternion, Vector3.one * Random.Range(this.vegetationSettings[i].minMaxScale.x, this.vegetationSettings[i].minMaxScale.y));
				}
				vegetationSetting.matrices = array;
				this.vegetationSettings[i] = vegetationSetting;
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000F1CC File Offset: 0x0000D3CC
		private void HandleFloatingOrigin(GPUICameraData cameraData)
		{
			Matrix4x4 localToWorldMatrix = this.sphereTransform.localToWorldMatrix;
			if (localToWorldMatrix == this._previousMatrix)
			{
				return;
			}
			Matrix4x4 matrix4x = localToWorldMatrix * this._previousMatrix.inverse;
			for (int i = 0; i < this._renderKeys.Length; i++)
			{
				GPUITransformBufferUtility.ApplyMatrixOffsetToTransforms(this._renderKeys[i], matrix4x);
			}
			this._previousMatrix = localToWorldMatrix;
		}

		// Token: 0x0400020B RID: 523
		[SerializeField]
		public Transform sphereTransform;

		// Token: 0x0400020C RID: 524
		[SerializeField]
		public float scaleMultiplier = 1f;

		// Token: 0x0400020D RID: 525
		[SerializeField]
		public int seed;

		// Token: 0x0400020E RID: 526
		[SerializeField]
		public List<GPUISphericalVegetationDrawer.VegetationSetting> vegetationSettings;

		// Token: 0x0400020F RID: 527
		[NonSerialized]
		private int[] _renderKeys;

		// Token: 0x04000210 RID: 528
		[NonSerialized]
		private Matrix4x4 _previousMatrix;

		// Token: 0x04000211 RID: 529
		[NonSerialized]
		private bool _isInitialized;

		// Token: 0x02000050 RID: 80
		[Serializable]
		public struct VegetationSetting
		{
			// Token: 0x04000212 RID: 530
			public GameObject prefab;

			// Token: 0x04000213 RID: 531
			public GPUIProfile profile;

			// Token: 0x04000214 RID: 532
			[Range(0f, 10000f)]
			public int spawnAmount;

			// Token: 0x04000215 RID: 533
			public Vector2 minMaxScale;

			// Token: 0x04000216 RID: 534
			[NonSerialized]
			public Matrix4x4[] matrices;
		}
	}
}
