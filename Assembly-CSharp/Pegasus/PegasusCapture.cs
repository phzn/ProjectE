using System;
using UnityEngine;
using UnityEngine.UI;

namespace Pegasus
{
	// Token: 0x020001A2 RID: 418
	public class PegasusCapture : MonoBehaviour
	{
		// Token: 0x06000CD4 RID: 3284 RVA: 0x00057D20 File Offset: 0x00055F20
		private void Start()
		{
			if (this.m_mainCamera == null)
			{
				this.m_mainCamera = Camera.main;
			}
			if (this.m_path == null)
			{
				this.m_path = PegasusPath.CreatePegasusPath();
			}
			if (this.m_reticuleGO == null)
			{
				this.m_reticuleGO = GameObject.Find("Pegasus Capture Reticule");
			}
			if (this.m_reticuleGO != null)
			{
				this.m_reticuleGO.SetActive(this.m_showReticule && this.m_enableOnStart);
				this.UpdateReticuleText();
			}
			if (this.m_enableOnStart)
			{
				if (this.m_clearOnStart)
				{
					this.m_path.ClearPath();
				}
				foreach (PegasusPath.PegasusPoint pegasusPoint in this.m_path.m_path)
				{
					GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					gameObject.transform.position = pegasusPoint.m_location;
					gameObject.transform.localScale = Vector3.one * 0.25f;
				}
			}
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00057E3C File Offset: 0x0005603C
		private void Update()
		{
			if (Application.isPlaying)
			{
				if (!this.m_enableOnStart)
				{
					return;
				}
				if (this.m_path == null || this.m_mainCamera == null)
				{
					return;
				}
				if (Input.GetKeyDown(this.m_keyCodeCapture))
				{
					Debug.Log("Pegasus POI Location Captured!");
					this.ProcessCaptureEvent();
				}
			}
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00057E94 File Offset: 0x00056094
		private void ProcessCaptureEvent()
		{
			this.m_path.AddPoint(this.m_mainCamera.gameObject);
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.transform.position = this.m_mainCamera.gameObject.transform.position;
			gameObject.transform.localScale = Vector3.one;
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x00057EEC File Offset: 0x000560EC
		public void UpdateReticuleVisibility()
		{
			if (this.m_reticuleGO == null)
			{
				this.m_reticuleGO = GameObject.Find("Pegasus Capture Reticule");
			}
			if (this.m_reticuleGO != null)
			{
				this.m_reticuleGO.SetActive(this.m_showReticule && this.m_enableOnStart);
			}
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x00057F44 File Offset: 0x00056144
		public void UpdateReticuleText()
		{
			if (this.m_reticuleGO == null)
			{
				this.m_reticuleGO = GameObject.Find("Pegasus Capture Reticule");
			}
			if (this.m_reticuleGO != null)
			{
				Text[] componentsInChildren = this.m_reticuleGO.GetComponentsInChildren<Text>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].text = string.Format("Play your game and then press {0} to create a POI at the current location.", this.m_keyCodeCapture.ToString());
				}
				this.m_reticuleGO.SetActive(this.m_showReticule && this.m_enableOnStart);
			}
		}

		// Token: 0x04000CC2 RID: 3266
		public KeyCode m_keyCodeCapture = KeyCode.P;

		// Token: 0x04000CC3 RID: 3267
		public Camera m_mainCamera;

		// Token: 0x04000CC4 RID: 3268
		public PegasusPath m_path;

		// Token: 0x04000CC5 RID: 3269
		public bool m_enableOnStart = true;

		// Token: 0x04000CC6 RID: 3270
		public bool m_clearOnStart = true;

		// Token: 0x04000CC7 RID: 3271
		public bool m_showReticule = true;

		// Token: 0x04000CC8 RID: 3272
		public GameObject m_reticuleGO;
	}
}
