using System;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
	// Token: 0x02000020 RID: 32
	public class MMViewportEdgeTeleporter : MonoBehaviour
	{
		// Token: 0x060000AD RID: 173 RVA: 0x00003E3E File Offset: 0x0000203E
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003E46 File Offset: 0x00002046
		protected virtual void Initialization()
		{
			if (this.AutoGrabMainCamera)
			{
				this.MainCamera = Camera.main;
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003E5B File Offset: 0x0000205B
		public virtual void SetCamera(Camera newCamera)
		{
			this.MainCamera = newCamera;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003E64 File Offset: 0x00002064
		protected virtual void Update()
		{
			this.DetectEdges();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003E6C File Offset: 0x0000206C
		protected virtual void DetectEdges()
		{
			this._viewportPosition = this.MainCamera.WorldToViewportPoint(base.transform.position);
			bool flag = false;
			if (this._viewportPosition.x < this.ViewportOrigin.x)
			{
				this._newViewportPosition.x = this.TeleportDimensions.x;
				this._newViewportPosition.y = this._viewportPosition.y;
				this._newViewportPosition.z = this._viewportPosition.z;
				flag = true;
			}
			else if (this._viewportPosition.x >= this.ViewportDimensions.x)
			{
				this._newViewportPosition.x = this.TeleportOrigin.x;
				this._newViewportPosition.y = this._viewportPosition.y;
				this._newViewportPosition.z = this._viewportPosition.z;
				flag = true;
			}
			if (this._viewportPosition.y < this.ViewportOrigin.y)
			{
				this._newViewportPosition.x = this._viewportPosition.x;
				this._newViewportPosition.y = this.TeleportDimensions.y;
				this._newViewportPosition.z = this._viewportPosition.z;
				flag = true;
			}
			else if (this._viewportPosition.y >= this.ViewportDimensions.y)
			{
				this._newViewportPosition.x = this._viewportPosition.x;
				this._newViewportPosition.y = this.TeleportOrigin.y;
				this._newViewportPosition.z = this._viewportPosition.z;
				flag = true;
			}
			if (flag)
			{
				UnityEvent onTeleport = this.OnTeleport;
				if (onTeleport != null)
				{
					onTeleport.Invoke();
				}
				base.transform.position = this.MainCamera.ViewportToWorldPoint(this._newViewportPosition);
			}
		}

		// Token: 0x040000C0 RID: 192
		[Header("Camera")]
		public bool AutoGrabMainCamera;

		// Token: 0x040000C1 RID: 193
		public Camera MainCamera;

		// Token: 0x040000C2 RID: 194
		[Header("Viewport Bounds")]
		[MMVector(new string[] { "X", "Y" })]
		public Vector2 ViewportOrigin = new Vector2(0f, 0f);

		// Token: 0x040000C3 RID: 195
		[MMVector(new string[] { "W", "H" })]
		public Vector2 ViewportDimensions = new Vector2(1f, 1f);

		// Token: 0x040000C4 RID: 196
		[Header("Teleport Bounds")]
		[MMVector(new string[] { "X", "Y" })]
		public Vector2 TeleportOrigin = new Vector2(0f, 0f);

		// Token: 0x040000C5 RID: 197
		[MMVector(new string[] { "W", "H" })]
		public Vector2 TeleportDimensions = new Vector2(1f, 1f);

		// Token: 0x040000C6 RID: 198
		[Header("Events")]
		public UnityEvent OnTeleport;

		// Token: 0x040000C7 RID: 199
		protected Vector3 _viewportPosition;

		// Token: 0x040000C8 RID: 200
		protected Vector3 _newViewportPosition;
	}
}
