using System;
using Features.Detection;
using UnityEngine;

namespace Features.WantedPoster
{
	// Token: 0x02000B75 RID: 2933
	internal static class NpcInterestPlayerVisibility
	{
		// Token: 0x06005358 RID: 21336 RVA: 0x00205A80 File Offset: 0x00203C80
		public static bool TryResolveContext(out Camera camera, out Transform playerRoot)
		{
			camera = Camera.main;
			playerRoot = null;
			SensoryManager instance = SensoryManager.Instance;
			StealthActor stealthActor = ((instance != null) ? instance.GetPlayer() : null);
			if (stealthActor != null)
			{
				playerRoot = stealthActor.transform;
			}
			if (playerRoot == null)
			{
				try
				{
					GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
					if (gameObject != null)
					{
						playerRoot = gameObject.transform;
					}
				}
				catch (UnityException)
				{
				}
			}
			return camera != null && camera.isActiveAndEnabled && playerRoot != null;
		}

		// Token: 0x06005359 RID: 21337 RVA: 0x00205B1C File Offset: 0x00203D1C
		public static bool IsVisibleToPlayer(Camera camera, Transform playerRoot, Vector3 worldPoint, Transform targetRoot, float maximumPlayerDistance, float viewportPadding, LayerMask obstructionMask)
		{
			if (camera == null || playerRoot == null || targetRoot == null)
			{
				return false;
			}
			float num = Mathf.Max(1f, maximumPlayerDistance);
			if ((worldPoint - playerRoot.position).sqrMagnitude > num * num)
			{
				return false;
			}
			Vector3 vector = camera.WorldToViewportPoint(worldPoint);
			float num2 = Mathf.Clamp(viewportPadding, 0f, 0.2f);
			return vector.z > camera.nearClipPlane && vector.x >= num2 && vector.x <= 1f - num2 && vector.y >= num2 && vector.y <= 1f - num2 && NpcInterestPlayerVisibility.HasCameraLineOfSight(camera, playerRoot, worldPoint, targetRoot, obstructionMask);
		}

		// Token: 0x0600535A RID: 21338 RVA: 0x00205BD4 File Offset: 0x00203DD4
		private static bool HasCameraLineOfSight(Camera camera, Transform playerRoot, Vector3 worldPoint, Transform targetRoot, LayerMask obstructionMask)
		{
			Vector3 vector = worldPoint - camera.transform.position;
			float magnitude = vector.magnitude;
			if (magnitude <= 0.01f)
			{
				return true;
			}
			RaycastHit[] array = Physics.RaycastAll(camera.transform.position, vector / magnitude, magnitude + 0.15f, obstructionMask, QueryTriggerInteraction.Ignore);
			if (array.Length == 0)
			{
				return true;
			}
			Array.Sort<RaycastHit>(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
			foreach (RaycastHit raycastHit in array)
			{
				Transform transform = raycastHit.transform;
				if (!(transform == null) && !transform.IsChildOf(playerRoot))
				{
					return transform == targetRoot || transform.IsChildOf(targetRoot) || raycastHit.distance >= magnitude - 0.15f;
				}
			}
			return true;
		}

		// Token: 0x040053DE RID: 21470
		private const float SurfaceTolerance = 0.15f;
	}
}
