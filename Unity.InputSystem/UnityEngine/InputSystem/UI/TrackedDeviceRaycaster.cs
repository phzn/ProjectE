using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UnityEngine.InputSystem.UI
{
	// Token: 0x02000091 RID: 145
	[AddComponentMenu("Event/Tracked Device Raycaster")]
	[RequireComponent(typeof(Canvas))]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.inputsystem@1.14/manual/TrackedInputDevices.html#tracked-device-raycaster")]
	public class TrackedDeviceRaycaster : BaseRaycaster
	{
		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x0003E394 File Offset: 0x0003C594
		public override Camera eventCamera
		{
			get
			{
				Canvas canvas = this.canvas;
				if (!(canvas != null))
				{
					return null;
				}
				return canvas.worldCamera;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x0003E3B9 File Offset: 0x0003C5B9
		// (set) Token: 0x06000BA5 RID: 2981 RVA: 0x0003E3C1 File Offset: 0x0003C5C1
		public LayerMask blockingMask
		{
			get
			{
				return this.m_BlockingMask;
			}
			set
			{
				this.m_BlockingMask = value;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x0003E3CA File Offset: 0x0003C5CA
		// (set) Token: 0x06000BA7 RID: 2983 RVA: 0x0003E3D2 File Offset: 0x0003C5D2
		public bool checkFor3DOcclusion
		{
			get
			{
				return this.m_CheckFor3DOcclusion;
			}
			set
			{
				this.m_CheckFor3DOcclusion = value;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x0003E3DB File Offset: 0x0003C5DB
		// (set) Token: 0x06000BA9 RID: 2985 RVA: 0x0003E3E3 File Offset: 0x0003C5E3
		public bool checkFor2DOcclusion
		{
			get
			{
				return this.m_CheckFor2DOcclusion;
			}
			set
			{
				this.m_CheckFor2DOcclusion = value;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x0003E3EC File Offset: 0x0003C5EC
		// (set) Token: 0x06000BAB RID: 2987 RVA: 0x0003E3F4 File Offset: 0x0003C5F4
		public bool ignoreReversedGraphics
		{
			get
			{
				return this.m_IgnoreReversedGraphics;
			}
			set
			{
				this.m_IgnoreReversedGraphics = value;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x0003E3FD File Offset: 0x0003C5FD
		// (set) Token: 0x06000BAD RID: 2989 RVA: 0x0003E405 File Offset: 0x0003C605
		public float maxDistance
		{
			get
			{
				return this.m_MaxDistance;
			}
			set
			{
				this.m_MaxDistance = value;
			}
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x0003E40E File Offset: 0x0003C60E
		protected override void OnEnable()
		{
			base.OnEnable();
			TrackedDeviceRaycaster.s_Instances.AppendWithCapacity(this, 10);
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x0003E424 File Offset: 0x0003C624
		protected override void OnDisable()
		{
			int num = TrackedDeviceRaycaster.s_Instances.IndexOfReference(this);
			if (num != -1)
			{
				TrackedDeviceRaycaster.s_Instances.RemoveAtByMovingTailWithCapacity(num);
			}
			base.OnDisable();
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x0003E454 File Offset: 0x0003C654
		public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
		{
			ExtendedPointerEventData extendedPointerEventData = eventData as ExtendedPointerEventData;
			if (extendedPointerEventData != null && extendedPointerEventData.pointerType == UIPointerType.Tracked)
			{
				this.PerformRaycast(extendedPointerEventData, resultAppendList);
			}
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0003E47C File Offset: 0x0003C67C
		internal void PerformRaycast(ExtendedPointerEventData eventData, List<RaycastResult> resultAppendList)
		{
			if (this.canvas == null)
			{
				return;
			}
			if (this.eventCamera == null)
			{
				return;
			}
			Ray ray = new Ray(eventData.trackedDevicePosition, eventData.trackedDeviceOrientation * Vector3.forward);
			float num = this.m_MaxDistance;
			RaycastHit raycastHit;
			if (this.m_CheckFor3DOcclusion && Physics.Raycast(ray, out raycastHit, num, this.m_BlockingMask))
			{
				num = raycastHit.distance;
			}
			if (this.m_CheckFor2DOcclusion)
			{
				float num2 = num;
				RaycastHit2D rayIntersection = Physics2D.GetRayIntersection(ray, num2, this.m_BlockingMask);
				if (rayIntersection.collider != null)
				{
					num = rayIntersection.distance;
				}
			}
			this.m_RaycastResultsCache.Clear();
			this.SortedRaycastGraphics(this.canvas, ray, this.m_RaycastResultsCache);
			for (int i = 0; i < this.m_RaycastResultsCache.Count; i++)
			{
				bool flag = true;
				TrackedDeviceRaycaster.RaycastHitData raycastHitData = this.m_RaycastResultsCache[i];
				GameObject gameObject = raycastHitData.graphic.gameObject;
				if (this.m_IgnoreReversedGraphics)
				{
					Vector3 direction = ray.direction;
					Vector3 vector = gameObject.transform.rotation * Vector3.forward;
					flag = Vector3.Dot(direction, vector) > 0f;
				}
				flag &= raycastHitData.distance < num;
				if (flag)
				{
					RaycastResult raycastResult = new RaycastResult
					{
						gameObject = gameObject,
						module = this,
						distance = raycastHitData.distance,
						index = (float)resultAppendList.Count,
						depth = raycastHitData.graphic.depth,
						worldPosition = raycastHitData.worldHitPosition,
						screenPosition = raycastHitData.screenPosition
					};
					resultAppendList.Add(raycastResult);
				}
			}
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0003E640 File Offset: 0x0003C840
		private void SortedRaycastGraphics(Canvas canvas, Ray ray, List<TrackedDeviceRaycaster.RaycastHitData> results)
		{
			IList<Graphic> graphicsForCanvas = GraphicRegistry.GetGraphicsForCanvas(canvas);
			TrackedDeviceRaycaster.s_SortedGraphics.Clear();
			for (int i = 0; i < graphicsForCanvas.Count; i++)
			{
				Graphic graphic = graphicsForCanvas[i];
				Vector3 vector;
				float num;
				if (graphic.depth != -1 && TrackedDeviceRaycaster.RayIntersectsRectTransform(graphic.rectTransform, ray, out vector, out num))
				{
					Vector2 vector2 = this.eventCamera.WorldToScreenPoint(vector);
					if (graphic.Raycast(vector2, this.eventCamera))
					{
						TrackedDeviceRaycaster.s_SortedGraphics.Add(new TrackedDeviceRaycaster.RaycastHitData(graphic, vector, vector2, num));
					}
				}
			}
			TrackedDeviceRaycaster.s_SortedGraphics.Sort((TrackedDeviceRaycaster.RaycastHitData g1, TrackedDeviceRaycaster.RaycastHitData g2) => g2.graphic.depth.CompareTo(g1.graphic.depth));
			results.AddRange(TrackedDeviceRaycaster.s_SortedGraphics);
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0003E700 File Offset: 0x0003C900
		private static bool RayIntersectsRectTransform(RectTransform transform, Ray ray, out Vector3 worldPosition, out float distance)
		{
			Vector3[] array = new Vector3[4];
			transform.GetWorldCorners(array);
			Plane plane = new Plane(array[0], array[1], array[2]);
			float num;
			if (plane.Raycast(ray, out num))
			{
				Vector3 point = ray.GetPoint(num);
				Vector3 vector = array[3] - array[0];
				Vector3 vector2 = array[1] - array[0];
				float num2 = Vector3.Dot(point - array[0], vector);
				if (Vector3.Dot(point - array[0], vector2) >= 0f && num2 >= 0f)
				{
					Vector3 vector3 = array[1] - array[2];
					Vector3 vector4 = array[3] - array[2];
					float num3 = Vector3.Dot(point - array[2], vector3);
					float num4 = Vector3.Dot(point - array[2], vector4);
					if (num3 >= 0f && num4 >= 0f)
					{
						worldPosition = point;
						distance = num;
						return true;
					}
				}
			}
			worldPosition = Vector3.zero;
			distance = 0f;
			return false;
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x0003E836 File Offset: 0x0003CA36
		private Canvas canvas
		{
			get
			{
				if (this.m_Canvas != null)
				{
					return this.m_Canvas;
				}
				this.m_Canvas = base.GetComponent<Canvas>();
				return this.m_Canvas;
			}
		}

		// Token: 0x04000436 RID: 1078
		[NonSerialized]
		private List<TrackedDeviceRaycaster.RaycastHitData> m_RaycastResultsCache = new List<TrackedDeviceRaycaster.RaycastHitData>();

		// Token: 0x04000437 RID: 1079
		internal static InlinedArray<TrackedDeviceRaycaster> s_Instances;

		// Token: 0x04000438 RID: 1080
		private static readonly List<TrackedDeviceRaycaster.RaycastHitData> s_SortedGraphics = new List<TrackedDeviceRaycaster.RaycastHitData>();

		// Token: 0x04000439 RID: 1081
		[FormerlySerializedAs("ignoreReversedGraphics")]
		[SerializeField]
		private bool m_IgnoreReversedGraphics;

		// Token: 0x0400043A RID: 1082
		[FormerlySerializedAs("checkFor2DOcclusion")]
		[SerializeField]
		private bool m_CheckFor2DOcclusion;

		// Token: 0x0400043B RID: 1083
		[FormerlySerializedAs("checkFor3DOcclusion")]
		[SerializeField]
		private bool m_CheckFor3DOcclusion;

		// Token: 0x0400043C RID: 1084
		[Tooltip("Maximum distance (in 3D world space) that rays are traced to find a hit.")]
		[SerializeField]
		private float m_MaxDistance = 1000f;

		// Token: 0x0400043D RID: 1085
		[SerializeField]
		private LayerMask m_BlockingMask;

		// Token: 0x0400043E RID: 1086
		[NonSerialized]
		private Canvas m_Canvas;

		// Token: 0x020001D9 RID: 473
		private struct RaycastHitData
		{
			// Token: 0x060014A2 RID: 5282 RVA: 0x0005F8EA File Offset: 0x0005DAEA
			public RaycastHitData(Graphic graphic, Vector3 worldHitPosition, Vector2 screenPosition, float distance)
			{
				this.graphic = graphic;
				this.worldHitPosition = worldHitPosition;
				this.screenPosition = screenPosition;
				this.distance = distance;
			}

			// Token: 0x1700058D RID: 1421
			// (get) Token: 0x060014A3 RID: 5283 RVA: 0x0005F909 File Offset: 0x0005DB09
			public readonly Graphic graphic { get; }

			// Token: 0x1700058E RID: 1422
			// (get) Token: 0x060014A4 RID: 5284 RVA: 0x0005F911 File Offset: 0x0005DB11
			public readonly Vector3 worldHitPosition { get; }

			// Token: 0x1700058F RID: 1423
			// (get) Token: 0x060014A5 RID: 5285 RVA: 0x0005F919 File Offset: 0x0005DB19
			public readonly Vector2 screenPosition { get; }

			// Token: 0x17000590 RID: 1424
			// (get) Token: 0x060014A6 RID: 5286 RVA: 0x0005F921 File Offset: 0x0005DB21
			public readonly float distance { get; }
		}
	}
}
