using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Features.RenderingPrecision
{
	// Token: 0x02000D81 RID: 3457
	public static class WorldOriginCameraBridge
	{
		// Token: 0x170010D7 RID: 4311
		// (get) Token: 0x060065D4 RID: 26068 RVA: 0x0028A1DC File Offset: 0x002883DC
		public static bool CanShift
		{
			get
			{
				CinemachineCore instance = CinemachineCore.Instance;
				for (int i = 0; i < instance.BrainCount; i++)
				{
					if (instance.GetActiveBrain(i).IsBlending)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x060065D5 RID: 26069 RVA: 0x0028A214 File Offset: 0x00288414
		public static void ApplyShift(Vector3 delta)
		{
			if (delta == Vector3.zero)
			{
				return;
			}
			CinemachineCore instance = CinemachineCore.Instance;
			WorldOriginCameraBridge.RegisteredRoots.Clear();
			WorldOriginCameraBridge.RegisteredCameras.Clear();
			WorldOriginCameraBridge.Collected.Clear();
			for (int i = 0; i < instance.VirtualCameraCount; i++)
			{
				WorldOriginCameraBridge.RegisteredRoots.Add(instance.GetVirtualCamera(i));
			}
			for (int j = 0; j < instance.BrainCount; j++)
			{
				CinemachineVirtualCameraBase cinemachineVirtualCameraBase = instance.GetActiveBrain(j).ActiveVirtualCamera as CinemachineVirtualCameraBase;
				if (cinemachineVirtualCameraBase != null)
				{
					WorldOriginCameraBridge.RegisteredRoots.Add(cinemachineVirtualCameraBase);
				}
			}
			CinemachineVirtualCameraBase cinemachineVirtualCameraBase2 = CinemachineBrain.SoloCamera as CinemachineVirtualCameraBase;
			if (cinemachineVirtualCameraBase2 != null)
			{
				WorldOriginCameraBridge.RegisteredRoots.Add(cinemachineVirtualCameraBase2);
			}
			foreach (CinemachineVirtualCameraBase cinemachineVirtualCameraBase3 in WorldOriginCameraBridge.RegisteredRoots)
			{
				WorldOriginCameraBridge.CollectCameraTree(cinemachineVirtualCameraBase3);
			}
			try
			{
				WorldOriginCameraBridge.ApplyShift(delta, WorldOriginCameraBridge.RegisteredCameras);
			}
			finally
			{
				WorldOriginCameraBridge.RegisteredRoots.Clear();
				WorldOriginCameraBridge.RegisteredCameras.Clear();
				WorldOriginCameraBridge.Collected.Clear();
			}
		}

		// Token: 0x060065D6 RID: 26070 RVA: 0x0028A340 File Offset: 0x00288540
		public static void ApplyShift(Vector3 delta, IReadOnlyList<CinemachineVirtualCameraBase> cameras)
		{
			if (delta == Vector3.zero || cameras == null || cameras.Count == 0)
			{
				return;
			}
			HashSet<CinemachineVirtualCameraBase> hashSet = new HashSet<CinemachineVirtualCameraBase>();
			Dictionary<CinemachineVirtualCameraBase, Vector3> dictionary = new Dictionary<CinemachineVirtualCameraBase, Vector3>();
			Dictionary<CinemachineVirtualCameraBase, Quaternion> dictionary2 = new Dictionary<CinemachineVirtualCameraBase, Quaternion>();
			foreach (CinemachineVirtualCameraBase cinemachineVirtualCameraBase in cameras)
			{
				if (!(cinemachineVirtualCameraBase == null) && hashSet.Add(cinemachineVirtualCameraBase))
				{
					dictionary.Add(cinemachineVirtualCameraBase, cinemachineVirtualCameraBase.transform.localPosition);
					dictionary2.Add(cinemachineVirtualCameraBase, cinemachineVirtualCameraBase.transform.localRotation);
				}
			}
			Dictionary<CinemachineVirtualCameraBase, HashSet<Transform>> dictionary3 = new Dictionary<CinemachineVirtualCameraBase, HashSet<Transform>>();
			foreach (CinemachineVirtualCameraBase cinemachineVirtualCameraBase2 in hashSet)
			{
				CinemachineVirtualCameraBase cinemachineVirtualCameraBase3 = cinemachineVirtualCameraBase2;
				CinemachineVirtualCameraBase cinemachineVirtualCameraBase4 = cinemachineVirtualCameraBase3.ParentCamera as CinemachineVirtualCameraBase;
				while (cinemachineVirtualCameraBase4 != null && hashSet.Contains(cinemachineVirtualCameraBase4) && WorldOriginCameraBridge.ForwardsToChildren(cinemachineVirtualCameraBase4))
				{
					cinemachineVirtualCameraBase3 = cinemachineVirtualCameraBase4;
					cinemachineVirtualCameraBase4 = cinemachineVirtualCameraBase3.ParentCamera as CinemachineVirtualCameraBase;
				}
				HashSet<Transform> hashSet2;
				if (!dictionary3.TryGetValue(cinemachineVirtualCameraBase3, out hashSet2))
				{
					hashSet2 = new HashSet<Transform> { null };
					dictionary3.Add(cinemachineVirtualCameraBase3, hashSet2);
				}
				hashSet2.Add(cinemachineVirtualCameraBase2.Follow);
				hashSet2.Add(cinemachineVirtualCameraBase2.LookAt);
			}
			try
			{
				foreach (KeyValuePair<CinemachineVirtualCameraBase, HashSet<Transform>> keyValuePair in dictionary3)
				{
					foreach (Transform transform in keyValuePair.Value)
					{
						keyValuePair.Key.OnTargetObjectWarped(transform, delta);
					}
				}
			}
			finally
			{
				foreach (CinemachineVirtualCameraBase cinemachineVirtualCameraBase5 in hashSet)
				{
					if (cinemachineVirtualCameraBase5 != null)
					{
						cinemachineVirtualCameraBase5.transform.SetLocalPositionAndRotation(dictionary[cinemachineVirtualCameraBase5], dictionary2[cinemachineVirtualCameraBase5]);
					}
				}
			}
		}

		// Token: 0x060065D7 RID: 26071 RVA: 0x0028A5AC File Offset: 0x002887AC
		private static void CollectCameraTree(CinemachineVirtualCameraBase camera)
		{
			if (camera == null || !WorldOriginCameraBridge.Collected.Add(camera))
			{
				return;
			}
			WorldOriginCameraBridge.RegisteredCameras.Add(camera);
			CinemachineFreeLook cinemachineFreeLook = camera as CinemachineFreeLook;
			if (cinemachineFreeLook != null)
			{
				for (int i = 0; i < 3; i++)
				{
					WorldOriginCameraBridge.CollectCameraTree(cinemachineFreeLook.GetRig(i));
				}
				return;
			}
			CinemachineClearShot cinemachineClearShot = camera as CinemachineClearShot;
			if (cinemachineClearShot != null)
			{
				WorldOriginCameraBridge.CollectChildren(cinemachineClearShot.ChildCameras);
				return;
			}
			CinemachineStateDrivenCamera cinemachineStateDrivenCamera = camera as CinemachineStateDrivenCamera;
			if (cinemachineStateDrivenCamera != null)
			{
				WorldOriginCameraBridge.CollectChildren(cinemachineStateDrivenCamera.ChildCameras);
				return;
			}
			CinemachineBlendListCamera cinemachineBlendListCamera = camera as CinemachineBlendListCamera;
			if (cinemachineBlendListCamera != null)
			{
				WorldOriginCameraBridge.CollectChildren(cinemachineBlendListCamera.ChildCameras);
				return;
			}
			CinemachineMixingCamera cinemachineMixingCamera = camera as CinemachineMixingCamera;
			if (cinemachineMixingCamera == null)
			{
				return;
			}
			WorldOriginCameraBridge.CollectChildren(cinemachineMixingCamera.ChildCameras);
		}

		// Token: 0x060065D8 RID: 26072 RVA: 0x0028A660 File Offset: 0x00288860
		private static void CollectChildren(CinemachineVirtualCameraBase[] children)
		{
			if (children == null)
			{
				return;
			}
			for (int i = 0; i < children.Length; i++)
			{
				WorldOriginCameraBridge.CollectCameraTree(children[i]);
			}
		}

		// Token: 0x060065D9 RID: 26073 RVA: 0x0028A689 File Offset: 0x00288889
		private static bool ForwardsToChildren(CinemachineVirtualCameraBase camera)
		{
			return camera is CinemachineFreeLook || camera is CinemachineClearShot || camera is CinemachineStateDrivenCamera || camera is CinemachineBlendListCamera || camera is CinemachineMixingCamera;
		}

		// Token: 0x04006863 RID: 26723
		private static readonly List<CinemachineVirtualCameraBase> RegisteredRoots = new List<CinemachineVirtualCameraBase>();

		// Token: 0x04006864 RID: 26724
		private static readonly List<CinemachineVirtualCameraBase> RegisteredCameras = new List<CinemachineVirtualCameraBase>();

		// Token: 0x04006865 RID: 26725
		private static readonly HashSet<CinemachineVirtualCameraBase> Collected = new HashSet<CinemachineVirtualCameraBase>();
	}
}
