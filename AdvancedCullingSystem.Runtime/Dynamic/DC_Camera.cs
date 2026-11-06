using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x02000031 RID: 49
	[RequireComponent(typeof(Camera))]
	public class DC_Camera : MonoBehaviour
	{
		// Token: 0x06000122 RID: 290 RVA: 0x0000690D File Offset: 0x00004B0D
		private void Awake()
		{
			this._camera = base.GetComponent<Camera>();
			this._newRaysCount = this._raysCount;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00006928 File Offset: 0x00004B28
		private void OnEnable()
		{
			if (this._camera == null)
			{
				this._camera = base.GetComponent<Camera>();
			}
			this._hitablesDic = DC_Controller.GetHitables();
			this._layerMask = LayerMask.GetMask(new string[] { DC_Controller.GetCullingLayerName() });
			this._newRaysCount = Mathf.Max(1, this._raysCount);
			this._updateRaysCount = true;
			this._updateSettings = true;
			this._cameraEnabled = false;
			this._jobScheduled = false;
			this._scheduledRayCount = 0;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000069A8 File Offset: 0x00004BA8
		private void Start()
		{
			this._hitablesDic = DC_Controller.GetHitables();
			this._layerMask = LayerMask.GetMask(new string[] { DC_Controller.GetCullingLayerName() });
			this._updateRaysCount = true;
			this._updateSettings = true;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000069DC File Offset: 0x00004BDC
		private void Update()
		{
			if (this._camera == null)
			{
				this._camera = base.GetComponent<Camera>();
			}
			if (this._camera == null)
			{
				return;
			}
			this._cameraEnabled = this._camera.enabled && base.gameObject.activeInHierarchy;
			if (!this._cameraEnabled)
			{
				return;
			}
			if (this._hitablesDic == null)
			{
				this._hitablesDic = DC_Controller.GetHitables();
			}
			if (this._hitablesDic == null || this._hitablesDic.Count == 0)
			{
				return;
			}
			this.UpdateIfNeeded();
			if (this._rayDirs == null || this._rayDirs.Length == 0 || !this._rayCommands.IsCreated || !this._rayHits.IsCreated)
			{
				return;
			}
			int num = this._rayDirs.Length;
			if (this._currentRay < 0 || this._currentRay >= num)
			{
				this._currentRay = 0;
			}
			float farPlane = this._settings.farPlane;
			Vector3 position = this._camera.transform.position;
			Matrix4x4 localToWorldMatrix = this._camera.transform.localToWorldMatrix;
			int num2 = Mathf.Min(Mathf.Max(1, this._raysCount), Mathf.Min(this._rayCommands.Length, this._rayHits.Length));
			if (num2 == 0)
			{
				return;
			}
			for (int i = 0; i < num2; i++)
			{
				this._rayCommands[i] = UnityAPI.NewRaycastCommand(position, localToWorldMatrix.MultiplyVector(this._rayDirs[this._currentRay]), farPlane, this._layerMask);
				this._currentRay++;
				if (this._currentRay >= num)
				{
					this._currentRay = 0;
				}
			}
			this._jobHandle = RaycastCommand.ScheduleBatch(this._rayCommands.GetSubArray(0, num2), this._rayHits.GetSubArray(0, num2), 1, default(JobHandle));
			this._scheduledRayCount = num2;
			this._jobScheduled = true;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006BBC File Offset: 0x00004DBC
		private void LateUpdate()
		{
			if (!this._jobScheduled)
			{
				return;
			}
			this.CompletePendingJob();
			int num = this._scheduledRayCount;
			this._scheduledRayCount = 0;
			if (!this._cameraEnabled || !this._rayHits.IsCreated || !this._rayCommands.IsCreated)
			{
				return;
			}
			num = Mathf.Min(num, Mathf.Min(this._rayHits.Length, this._rayCommands.Length));
			for (int i = 0; i < num; i++)
			{
				Collider collider = this._rayHits[i].collider;
				IHitable hitable;
				if (collider != null && this._hitablesDic != null && this._hitablesDic.TryGetValue(collider, out hitable))
				{
					hitable.OnHit();
				}
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006C74 File Offset: 0x00004E74
		private void OnDisable()
		{
			this.ReleaseRayBuffers();
			this._cameraEnabled = false;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006C83 File Offset: 0x00004E83
		private void CompletePendingJob()
		{
			if (this._jobScheduled)
			{
				this._jobHandle.Complete();
				this._jobScheduled = false;
			}
			this._jobHandle = default(JobHandle);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006CAB File Offset: 0x00004EAB
		private void OnDestroy()
		{
			this.ReleaseRayBuffers();
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006CB4 File Offset: 0x00004EB4
		private void ReleaseRayBuffers()
		{
			this.CompletePendingJob();
			if (this._rayCommands.IsCreated)
			{
				this._rayCommands.Dispose();
			}
			if (this._rayHits.IsCreated)
			{
				this._rayHits.Dispose();
			}
			this._rayCommands = default(NativeArray<RaycastCommand>);
			this._rayHits = default(NativeArray<RaycastHit>);
			this._scheduledRayCount = 0;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006D16 File Offset: 0x00004F16
		public void CameraSettingsChanged()
		{
			this._updateSettings = true;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00006D1F File Offset: 0x00004F1F
		public void SetRaysCount(int count)
		{
			this._updateRaysCount = true;
			this._newRaysCount = Mathf.Max(1, count);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006D38 File Offset: 0x00004F38
		private bool IsCameraSettingsChanged()
		{
			return this._settings.width != this._camera.pixelWidth || this._settings.height != this._camera.pixelHeight || this._settings.fov != this._camera.fieldOfView || this._settings.farPlane != this._camera.farClipPlane;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006DB0 File Offset: 0x00004FB0
		private void UpdateIfNeeded()
		{
			if (!this._updateSettings && this._autoCheckChanges && this.IsCameraSettingsChanged())
			{
				this._updateSettings = true;
			}
			if (this._updateSettings)
			{
				this.UpdateCameraSettings();
				this._updateSettings = false;
			}
			if (this._updateRaysCount)
			{
				this.UpdateRaysCount(this._newRaysCount);
				this._updateRaysCount = false;
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006E0C File Offset: 0x0000500C
		private void UpdateRaysCount(int count)
		{
			this.ReleaseRayBuffers();
			count = Mathf.Max(1, count);
			this._rayCommands = new NativeArray<RaycastCommand>(count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this._rayHits = new NativeArray<RaycastHit>(count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this._raysCount = count;
			this._newRaysCount = count;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006E47 File Offset: 0x00005047
		private void UpdateCameraSettings()
		{
			this._rayDirs = DC_Camera.DC_CameraUtil.GetRaysDirections(this._camera, this._raysDistribution, this._fovAddition);
			this._currentRay = 0;
			this._settings = new DC_CameraSettings(this._camera);
		}

		// Token: 0x040000D8 RID: 216
		[SerializeField]
		private int _raysCount = 1500;

		// Token: 0x040000D9 RID: 217
		[SerializeField]
		private DC_Camera.DistributionMethod _raysDistribution = DC_Camera.DistributionMethod.R2;

		// Token: 0x040000DA RID: 218
		[Space]
		[Range(0f, 90f)]
		[SerializeField]
		private int _fovAddition = 5;

		// Token: 0x040000DB RID: 219
		[SerializeField]
		private bool _autoCheckChanges;

		// Token: 0x040000DC RID: 220
		private IReadOnlyDictionary<Collider, IHitable> _hitablesDic;

		// Token: 0x040000DD RID: 221
		private Camera _camera;

		// Token: 0x040000DE RID: 222
		private DC_CameraSettings _settings;

		// Token: 0x040000DF RID: 223
		private bool _updateSettings;

		// Token: 0x040000E0 RID: 224
		private bool _updateRaysCount;

		// Token: 0x040000E1 RID: 225
		private int _newRaysCount;

		// Token: 0x040000E2 RID: 226
		private Vector3[] _rayDirs;

		// Token: 0x040000E3 RID: 227
		private NativeArray<RaycastCommand> _rayCommands;

		// Token: 0x040000E4 RID: 228
		private NativeArray<RaycastHit> _rayHits;

		// Token: 0x040000E5 RID: 229
		private JobHandle _jobHandle;

		// Token: 0x040000E6 RID: 230
		private int _layerMask;

		// Token: 0x040000E7 RID: 231
		private int _currentRay;

		// Token: 0x040000E8 RID: 232
		private bool _cameraEnabled;

		// Token: 0x040000E9 RID: 233
		private bool _jobScheduled;

		// Token: 0x040000EA RID: 234
		private int _scheduledRayCount;

		// Token: 0x02000032 RID: 50
		public enum DistributionMethod
		{
			// Token: 0x040000EC RID: 236
			Halton,
			// Token: 0x040000ED RID: 237
			R2
		}

		// Token: 0x02000033 RID: 51
		private static class DC_CameraUtil
		{
			// Token: 0x06000132 RID: 306 RVA: 0x00006E9F File Offset: 0x0000509F
			[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
			private static void ReloadDomain()
			{
				Dictionary<DC_CameraSettings, Vector3[]> rayDirsTable = DC_Camera.DC_CameraUtil._rayDirsTable;
				if (rayDirsTable == null)
				{
					return;
				}
				rayDirsTable.Clear();
			}

			// Token: 0x06000133 RID: 307 RVA: 0x00006EB0 File Offset: 0x000050B0
			static DC_CameraUtil()
			{
				double num = 1.324717957244746;
				DC_Camera.DC_CameraUtil._r2a1 = 1.0 / num;
				DC_Camera.DC_CameraUtil._r2a2 = 1.0 / (num * num);
			}

			// Token: 0x06000134 RID: 308 RVA: 0x00006EF4 File Offset: 0x000050F4
			public static Vector3[] GetRaysDirections(Camera camera, DC_Camera.DistributionMethod distribution, int fovAddition)
			{
				DC_CameraSettings dc_CameraSettings = new DC_CameraSettings(camera);
				Vector3[] array;
				if (DC_Camera.DC_CameraUtil._rayDirsTable.TryGetValue(dc_CameraSettings, out array))
				{
					return array;
				}
				float fieldOfView = camera.fieldOfView;
				Matrix4x4 inverse = camera.transform.localToWorldMatrix.inverse;
				int num = Screen.width * Screen.height / 8;
				Vector3[] array2 = new Vector3[num];
				camera.fieldOfView = fieldOfView + (float)fovAddition;
				for (int i = 0; i < num; i++)
				{
					Vector2 vector;
					if (distribution == DC_Camera.DistributionMethod.Halton)
					{
						vector = new Vector2(DC_Camera.DC_CameraUtil.HaltonSequence(i, 2), DC_Camera.DC_CameraUtil.HaltonSequence(i, 3));
					}
					else
					{
						vector = DC_Camera.DC_CameraUtil.R2Distribution(i);
					}
					array2[i] = inverse.MultiplyVector(camera.ViewportPointToRay(vector).direction);
				}
				camera.fieldOfView = fieldOfView;
				DC_Camera.DC_CameraUtil._rayDirsTable.Add(dc_CameraSettings, array2);
				return array2;
			}

			// Token: 0x06000135 RID: 309 RVA: 0x00006FCC File Offset: 0x000051CC
			private static float HaltonSequence(int index, int b)
			{
				float num = 0f;
				float num2 = 1f / (float)b;
				int i = index;
				while (i > 0)
				{
					num += num2 * (float)(i % b);
					i = Mathf.FloorToInt((float)(i / b));
					num2 /= (float)b;
				}
				return num;
			}

			// Token: 0x06000136 RID: 310 RVA: 0x0000700C File Offset: 0x0000520C
			private static Vector2 R2Distribution(int index)
			{
				float num = (float)((0.5 + DC_Camera.DC_CameraUtil._r2a1 * (double)index) % 1.0);
				float num2 = (float)((0.5 + DC_Camera.DC_CameraUtil._r2a2 * (double)index) % 1.0);
				return new Vector2(num, num2);
			}

			// Token: 0x040000EE RID: 238
			private static Dictionary<DC_CameraSettings, Vector3[]> _rayDirsTable = new Dictionary<DC_CameraSettings, Vector3[]>();

			// Token: 0x040000EF RID: 239
			private static double _r2a1;

			// Token: 0x040000F0 RID: 240
			private static double _r2a2;
		}
	}
}
