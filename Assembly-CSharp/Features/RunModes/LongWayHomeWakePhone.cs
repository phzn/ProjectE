using System;
using Features.PhoneCompanion;
using UnityEngine;
using UnityEngine.UI;

namespace Features.RunModes
{
	// Token: 0x02000D3F RID: 3391
	public sealed class LongWayHomeWakePhone : MonoBehaviour
	{
		// Token: 0x06006361 RID: 25441 RVA: 0x0027BEAC File Offset: 0x0027A0AC
		public static LongWayHomeWakePhone CreateInactive(SmartphoneController smartphone, Transform player, Transform parent, Vector3 localOffset)
		{
			if (smartphone == null || player == null)
			{
				return null;
			}
			GameObject gameObject = ((smartphone.phoneInstance != null) ? smartphone.phoneInstance : smartphone.phonePrefab);
			if (gameObject == null)
			{
				return null;
			}
			GameObject gameObject2 = new GameObject("[LongWayHome Ground Phone]");
			gameObject2.SetActive(false);
			gameObject2.transform.SetParent(parent, false);
			LongWayHomeWakePhone longWayHomeWakePhone = gameObject2.AddComponent<LongWayHomeWakePhone>();
			GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject, gameObject2.transform, false);
			gameObject3.name = "Ciel's Phone (visual only)";
			gameObject3.transform.localPosition = Vector3.zero;
			gameObject3.transform.localRotation = Quaternion.identity;
			gameObject3.transform.localScale = smartphone.attachLocalScale;
			LongWayHomeWakePhone.KeepHomeScreen(gameObject3.transform);
			LongWayHomeWakePhone.StripInteractiveComponents(gameObject3);
			gameObject3.SetActive(true);
			Quaternion quaternion = Quaternion.Euler(0f, player.eulerAngles.y, 0f);
			Vector3 vector = player.position + quaternion * localOffset;
			Vector3 vector2 = Vector3.up;
			float num = float.PositiveInfinity;
			foreach (RaycastHit raycastHit in Physics.RaycastAll(vector + Vector3.up, Vector3.down, 2f, -1, QueryTriggerInteraction.Ignore))
			{
				if (!raycastHit.collider.transform.IsChildOf(player) && raycastHit.normal.y >= 0.6f && raycastHit.distance < num)
				{
					num = raycastHit.distance;
					vector = raycastHit.point;
					vector2 = raycastHit.normal;
				}
			}
			Vector3 normalized = Vector3.ProjectOnPlane(quaternion * (Quaternion.Euler(0f, 24f, 0f) * Vector3.forward), vector2).normalized;
			gameObject2.transform.SetPositionAndRotation(vector, Quaternion.LookRotation(vector2, normalized));
			LongWayHomeWakePhone.RestMeshOnGround(gameObject3, gameObject2.transform, vector, vector2);
			return longWayHomeWakePhone;
		}

		// Token: 0x06006362 RID: 25442 RVA: 0x0027C0A4 File Offset: 0x0027A2A4
		public void Reveal()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06006363 RID: 25443 RVA: 0x0027C0B4 File Offset: 0x0027A2B4
		private static void KeepHomeScreen(Transform phone)
		{
			Transform transform = phone.Find("Canvas/Panel");
			if (transform == null)
			{
				return;
			}
			foreach (object obj in transform)
			{
				Transform transform2 = (Transform)obj;
				bool flag = transform2.name == "Home" || transform2.name == "Wallpaper" || transform2.name == "TopBar";
				transform2.gameObject.SetActive(flag);
			}
			transform.gameObject.SetActive(true);
			transform.parent.gameObject.SetActive(true);
			foreach (CanvasGroup canvasGroup in transform.GetComponentsInChildren<CanvasGroup>(true))
			{
				canvasGroup.alpha = 1f;
				canvasGroup.interactable = false;
				canvasGroup.blocksRaycasts = false;
			}
		}

		// Token: 0x06006364 RID: 25444 RVA: 0x0027C1B4 File Offset: 0x0027A3B4
		private static void StripInteractiveComponents(GameObject view)
		{
			Transform[] componentsInChildren = view.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 0;
			}
			Component[] componentsInChildren2 = view.GetComponentsInChildren<Component>(true);
			for (int j = componentsInChildren2.Length - 1; j >= 0; j--)
			{
				Component component = componentsInChildren2[j];
				if (!(component == null))
				{
					Canvas canvas = component as Canvas;
					if (canvas != null)
					{
						canvas.renderMode = RenderMode.WorldSpace;
						canvas.overrideSorting = false;
						canvas.sortingOrder = 0;
						canvas.enabled = true;
					}
					else
					{
						Graphic graphic = component as Graphic;
						if (graphic != null)
						{
							graphic.raycastTarget = false;
						}
						else if (!(component is Transform) && !(component is MeshFilter) && !(component is MeshRenderer) && !(component is SkinnedMeshRenderer) && !(component is CanvasRenderer) && !(component is CanvasGroup) && !(component is BaseMeshEffect) && !(component is Mask) && !(component is RectMask2D) && !(component is PhoneStableUiMaterials) && !(component is PhoneStableUiGraphic) && !(component is LayoutGroup) && !(component is LayoutElement) && !(component is ContentSizeFitter) && !(component is AspectRatioFitter))
						{
							Behaviour behaviour = component as Behaviour;
							if (behaviour != null)
							{
								behaviour.enabled = false;
							}
							if (Application.isPlaying)
							{
								Object.Destroy(component);
							}
							else
							{
								Object.DestroyImmediate(component);
							}
						}
					}
				}
			}
		}

		// Token: 0x06006365 RID: 25445 RVA: 0x0027C324 File Offset: 0x0027A524
		private static void RestMeshOnGround(GameObject view, Transform host, Vector3 ground, Vector3 normal)
		{
			float num = float.PositiveInfinity;
			foreach (MeshFilter meshFilter in view.GetComponentsInChildren<MeshFilter>(true))
			{
				if (!(meshFilter.sharedMesh == null) && !(meshFilter.GetComponentInParent<Canvas>() != null))
				{
					Bounds bounds = meshFilter.sharedMesh.bounds;
					for (int j = 0; j < 8; j++)
					{
						Vector3 vector = bounds.center + Vector3.Scale(bounds.extents, new Vector3(((j & 1) == 0) ? (-1f) : 1f, ((j & 2) == 0) ? (-1f) : 1f, ((j & 4) == 0) ? (-1f) : 1f));
						num = Mathf.Min(num, Vector3.Dot(meshFilter.transform.TransformPoint(vector) - ground, normal));
					}
				}
			}
			host.position += normal * (float.IsPositiveInfinity(num) ? 0.008f : (0.003f - num));
		}
	}
}
