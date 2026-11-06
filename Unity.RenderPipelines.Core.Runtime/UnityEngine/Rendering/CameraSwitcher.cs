using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000032 RID: 50
	public class CameraSwitcher : MonoBehaviour
	{
		// Token: 0x060001EB RID: 491 RVA: 0x00009BE4 File Offset: 0x00007DE4
		private void OnEnable()
		{
			this.m_OriginalCamera = base.GetComponent<Camera>();
			this.m_CurrentCamera = this.m_OriginalCamera;
			if (this.m_OriginalCamera == null)
			{
				Debug.LogError("Camera Switcher needs a Camera component attached");
				return;
			}
			this.m_CurrentCameraIndex = this.GetCameraCount() - 1;
			this.m_CameraNames = new GUIContent[this.GetCameraCount()];
			this.m_CameraIndices = new int[this.GetCameraCount()];
			for (int i = 0; i < this.m_Cameras.Length; i++)
			{
				Camera camera = this.m_Cameras[i];
				if (camera != null)
				{
					this.m_CameraNames[i] = new GUIContent(camera.name);
				}
				else
				{
					this.m_CameraNames[i] = new GUIContent("null");
				}
				this.m_CameraIndices[i] = i;
			}
			this.m_CameraNames[this.GetCameraCount() - 1] = new GUIContent("Original Camera");
			this.m_CameraIndices[this.GetCameraCount() - 1] = this.GetCameraCount() - 1;
			this.m_DebugEntry = new DebugUI.EnumField
			{
				displayName = "Camera Switcher",
				getter = () => this.m_CurrentCameraIndex,
				setter = delegate(int value)
				{
					this.SetCameraIndex(value);
				},
				enumNames = this.m_CameraNames,
				enumValues = this.m_CameraIndices,
				getIndex = () => this.m_DebugEntryEnumIndex,
				setIndex = delegate(int value)
				{
					this.m_DebugEntryEnumIndex = value;
				}
			};
			DebugManager.instance.GetPanel("Camera", true, 0, false).children.Add(this.m_DebugEntry);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00009D6F File Offset: 0x00007F6F
		private void OnDisable()
		{
			if (this.m_DebugEntry != null && this.m_DebugEntry.panel != null)
			{
				this.m_DebugEntry.panel.children.Remove(this.m_DebugEntry);
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00009DA2 File Offset: 0x00007FA2
		private int GetCameraCount()
		{
			return this.m_Cameras.Length + 1;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00009DAE File Offset: 0x00007FAE
		private Camera GetNextCamera()
		{
			if (this.m_CurrentCameraIndex == this.m_Cameras.Length)
			{
				return this.m_OriginalCamera;
			}
			return this.m_Cameras[this.m_CurrentCameraIndex];
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00009DD4 File Offset: 0x00007FD4
		private void SetCameraIndex(int index)
		{
			if (index > 0 && index < this.GetCameraCount())
			{
				this.m_CurrentCameraIndex = index;
				if (this.m_CurrentCamera == this.m_OriginalCamera)
				{
					this.m_OriginalCameraPosition = this.m_OriginalCamera.transform.position;
					this.m_OriginalCameraRotation = this.m_OriginalCamera.transform.rotation;
				}
				this.m_CurrentCamera = this.GetNextCamera();
				if (this.m_CurrentCamera != null)
				{
					if (this.m_CurrentCamera == this.m_OriginalCamera)
					{
						this.m_OriginalCamera.transform.position = this.m_OriginalCameraPosition;
						this.m_OriginalCamera.transform.rotation = this.m_OriginalCameraRotation;
					}
					base.transform.position = this.m_CurrentCamera.transform.position;
					base.transform.rotation = this.m_CurrentCamera.transform.rotation;
				}
			}
		}

		// Token: 0x04000116 RID: 278
		public Camera[] m_Cameras;

		// Token: 0x04000117 RID: 279
		private int m_CurrentCameraIndex = -1;

		// Token: 0x04000118 RID: 280
		private Camera m_OriginalCamera;

		// Token: 0x04000119 RID: 281
		private Vector3 m_OriginalCameraPosition;

		// Token: 0x0400011A RID: 282
		private Quaternion m_OriginalCameraRotation;

		// Token: 0x0400011B RID: 283
		private Camera m_CurrentCamera;

		// Token: 0x0400011C RID: 284
		private GUIContent[] m_CameraNames;

		// Token: 0x0400011D RID: 285
		private int[] m_CameraIndices;

		// Token: 0x0400011E RID: 286
		private DebugUI.EnumField m_DebugEntry;

		// Token: 0x0400011F RID: 287
		private int m_DebugEntryEnumIndex;
	}
}
