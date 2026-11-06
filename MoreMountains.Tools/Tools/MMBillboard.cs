using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x0200001B RID: 27
	[AddComponentMenu("More Mountains/Tools/Camera/MMBillboard")]
	public class MMBillboard : MonoBehaviour
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000094 RID: 148 RVA: 0x0000334D File Offset: 0x0000154D
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00003355 File Offset: 0x00001555
		public virtual Camera MainCamera { get; set; }

		// Token: 0x06000096 RID: 150 RVA: 0x0000335E File Offset: 0x0000155E
		protected virtual void Awake()
		{
			this._transform = base.transform;
			if (this.GrabMainCameraOnStart)
			{
				this.GrabMainCamera();
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000337A File Offset: 0x0000157A
		private void Start()
		{
			if (this.NestObject)
			{
				this.NestThisObject();
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000338C File Offset: 0x0000158C
		protected virtual void NestThisObject()
		{
			this._parentContainer = new GameObject();
			SceneManager.MoveGameObjectToScene(this._parentContainer, base.gameObject.scene);
			this._parentContainer.name = "Parent" + base.transform.gameObject.name;
			this._parentContainer.transform.position = base.transform.position;
			base.transform.SetParent(this._parentContainer.transform);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003410 File Offset: 0x00001610
		protected virtual void GrabMainCamera()
		{
			this.MainCamera = Camera.main;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003420 File Offset: 0x00001620
		protected virtual void Update()
		{
			if (this.NestObject)
			{
				this._parentContainer.transform.LookAt(this._parentContainer.transform.position + this.MainCamera.transform.rotation * this.OffsetDirection, this.MainCamera.transform.rotation * this.Up);
				return;
			}
			this._transform.LookAt(this._transform.position + this.MainCamera.transform.rotation * this.OffsetDirection, this.MainCamera.transform.rotation * this.Up);
		}

		// Token: 0x04000092 RID: 146
		[Tooltip("whether or not this object should automatically grab a camera on start")]
		public bool GrabMainCameraOnStart = true;

		// Token: 0x04000093 RID: 147
		[Tooltip("whether or not to nest this object below a parent container")]
		public bool NestObject = true;

		// Token: 0x04000094 RID: 148
		[Tooltip("the Vector3 to offset the look at direction by")]
		public Vector3 OffsetDirection = Vector3.back;

		// Token: 0x04000095 RID: 149
		[Tooltip("the Vector3 to consider as 'world up'")]
		public Vector3 Up = Vector3.up;

		// Token: 0x04000096 RID: 150
		protected GameObject _parentContainer;

		// Token: 0x04000097 RID: 151
		private Transform _transform;
	}
}
