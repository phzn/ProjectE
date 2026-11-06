using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
	// Token: 0x02000021 RID: 33
	[AddComponentMenu("")]
	[ExecuteAlways]
	public abstract class MMCinemachineZone : MonoBehaviour
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x000040A3 File Offset: 0x000022A3
		protected virtual void Awake()
		{
			this.AlwaysInitialization();
			if (!Application.isPlaying)
			{
				return;
			}
			this.Initialization();
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000040B9 File Offset: 0x000022B9
		protected virtual void AlwaysInitialization()
		{
			this.InitializeCollider();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000040C4 File Offset: 0x000022C4
		protected virtual void Initialization()
		{
			if (this.VirtualCamera == null)
			{
				this.VirtualCamera = base.GetComponentInChildren<CinemachineVirtualCamera>();
			}
			if (this.VirtualCamera == null)
			{
				Debug.LogWarning("[MMCinemachineZone2D] " + base.name + " : no virtual camera is attached to this zone. Set one in its inspector.");
			}
			if (this.SetupConfinerOnStart)
			{
				this.SetupConfinerGameObject();
			}
			foreach (GameObject gameObject in this.ActivationList)
			{
				gameObject.SetActive(false);
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004168 File Offset: 0x00002368
		protected virtual void Start()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.SetupConfinerOnStart)
			{
				this.SetupConfiner();
			}
			base.StartCoroutine(this.EnableCamera(this.CameraStartsActive, 1));
		}

		// Token: 0x060000B7 RID: 183
		protected abstract void InitializeCollider();

		// Token: 0x060000B8 RID: 184
		protected abstract void SetupConfiner();

		// Token: 0x060000B9 RID: 185 RVA: 0x00004194 File Offset: 0x00002394
		protected virtual void ManualSetupConfiner()
		{
			this.Initialization();
			this.SetupConfiner();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000041A4 File Offset: 0x000023A4
		protected virtual void SetupConfinerGameObject()
		{
			Transform transform = base.transform.Find("Confiner");
			if (transform != null)
			{
				Object.DestroyImmediate(transform.gameObject);
			}
			this._confinerGameObject = new GameObject();
			this._confinerGameObject.transform.localPosition = Vector3.zero;
			this._confinerGameObject.transform.SetParent(base.transform);
			this._confinerGameObject.name = "Confiner";
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000421C File Offset: 0x0000241C
		protected virtual bool TestCollidingGameObject(GameObject collider)
		{
			return true;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000421F File Offset: 0x0000241F
		protected virtual IEnumerator EnableCamera(bool state, int frames)
		{
			if (this.VirtualCamera == null)
			{
				yield break;
			}
			if (frames > 0)
			{
				yield return MMCoroutine.WaitForFrames(frames);
			}
			if (this.Mode == MMCinemachineZone.Modes.Enable)
			{
				this.VirtualCamera.enabled = state;
			}
			else if (this.Mode == MMCinemachineZone.Modes.Priority)
			{
				this.VirtualCamera.Priority = (state ? this.EnabledPriority : this.DisabledPriority);
			}
			yield break;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000423C File Offset: 0x0000243C
		protected virtual void EnterZone()
		{
			if (!this.RoomVisited)
			{
				this.OnEnterZoneForTheFirstTimeEvent.Invoke();
			}
			this.CurrentRoom = true;
			this.RoomVisited = true;
			this.OnEnterZoneEvent.Invoke();
			base.StartCoroutine(this.EnableCamera(true, 0));
			foreach (GameObject gameObject in this.ActivationList)
			{
				gameObject.SetActive(true);
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000042C8 File Offset: 0x000024C8
		protected virtual void ExitZone()
		{
			this.CurrentRoom = false;
			this.OnExitZoneEvent.Invoke();
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.EnableCamera(false, 0));
			}
			foreach (GameObject gameObject in this.ActivationList)
			{
				gameObject.SetActive(false);
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004348 File Offset: 0x00002548
		protected virtual void Reset()
		{
			this.GizmosColor = MMColors.RandomColor();
			this.GizmosColor.a = 0.2f;
		}

		// Token: 0x040000C9 RID: 201
		[Header("Virtual Camera")]
		[Tooltip("whether to enable/disable virtual cameras, or to play on their priority for transitions")]
		public MMCinemachineZone.Modes Mode = MMCinemachineZone.Modes.Priority;

		// Token: 0x040000CA RID: 202
		[Tooltip("whether or not the camera in this zone should start active")]
		public bool CameraStartsActive;

		// Token: 0x040000CB RID: 203
		[Tooltip("the virtual camera associated to this zone (will try to grab one in children if none is set)")]
		public CinemachineVirtualCamera VirtualCamera;

		// Token: 0x040000CC RID: 204
		[Tooltip("when in priority mode, the priority this camera should have when the zone is active")]
		[MMEnumCondition("Mode", new int[] { 1 })]
		public int EnabledPriority = 10;

		// Token: 0x040000CD RID: 205
		[Tooltip("when in priority mode, the priority this camera should have when the zone is inactive")]
		[MMEnumCondition("Mode", new int[] { 1 })]
		public int DisabledPriority;

		// Token: 0x040000CE RID: 206
		[Header("Collisions")]
		[Tooltip("a layermask containing all the layers that should activate this zone")]
		public LayerMask TriggerMask;

		// Token: 0x040000CF RID: 207
		[Header("Confiner Setup")]
		[Tooltip("whether or not the zone should auto setup its camera's confiner on start - alternative is to manually click the ManualSetupConfiner, or do your own setup")]
		public bool SetupConfinerOnStart;

		// Token: 0x040000D0 RID: 208
		[MMInspectorButton("ManualSetupConfiner")]
		public bool GenerateConfinerSetup;

		// Token: 0x040000D1 RID: 209
		[Header("State")]
		[Tooltip("whether this room is the current room or not")]
		[MMReadOnly]
		public bool CurrentRoom;

		// Token: 0x040000D2 RID: 210
		[Tooltip("whether this room has already been visited or not")]
		public bool RoomVisited;

		// Token: 0x040000D3 RID: 211
		[Header("Events")]
		[Tooltip("a UnityEvent to trigger when entering the zone for the first time")]
		public UnityEvent OnEnterZoneForTheFirstTimeEvent;

		// Token: 0x040000D4 RID: 212
		[Tooltip("a UnityEvent to trigger when entering the zone")]
		public UnityEvent OnEnterZoneEvent;

		// Token: 0x040000D5 RID: 213
		[Tooltip("a UnityEvent to trigger when exiting the zone")]
		public UnityEvent OnExitZoneEvent;

		// Token: 0x040000D6 RID: 214
		[Header("Activation")]
		[Tooltip("a list of gameobjects to enable when entering the zone, and disable when exiting it")]
		public List<GameObject> ActivationList;

		// Token: 0x040000D7 RID: 215
		[Header("Debug")]
		[Tooltip("whether or not to draw shape gizmos to help visualize the zone's bounds")]
		public bool DrawGizmos = true;

		// Token: 0x040000D8 RID: 216
		[Tooltip("the color of the gizmos to draw in edit mode")]
		public Color GizmosColor;

		// Token: 0x040000D9 RID: 217
		protected GameObject _confinerGameObject;

		// Token: 0x040000DA RID: 218
		protected Vector3 _gizmoSize;

		// Token: 0x020003A6 RID: 934
		public enum Modes
		{
			// Token: 0x04001D6A RID: 7530
			Enable,
			// Token: 0x04001D6B RID: 7531
			Priority
		}
	}
}
