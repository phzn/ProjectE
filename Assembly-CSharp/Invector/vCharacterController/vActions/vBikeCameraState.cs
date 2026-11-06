using System;
using System.Collections.Generic;
using Invector.vCamera;
using UnityEngine;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000702 RID: 1794
	public class vBikeCameraState : MonoBehaviour
	{
		// Token: 0x06002DF3 RID: 11763 RVA: 0x00102185 File Offset: 0x00100385
		private void Start()
		{
			this.ValidateCameraState();
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x00102185 File Offset: 0x00100385
		private void OnValidate()
		{
			this.ValidateCameraState();
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x00102190 File Offset: 0x00100390
		private void ValidateCameraState()
		{
			this.tpCamera = base.GetComponent<vThirdPersonCamera>();
			if (this.tpCamera == null)
			{
				this.tpCamera = Object.FindObjectOfType<vThirdPersonCamera>();
			}
			if (this.tpCamera == null || this.tpCamera.CameraStateList == null)
			{
				this.stateExists = false;
				return;
			}
			this.stateExists = false;
			using (List<vThirdPersonCameraState>.Enumerator enumerator = this.tpCamera.CameraStateList.tpCameraStates.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Name == this.stateName)
					{
						this.stateExists = true;
						break;
					}
				}
			}
			if (!this.stateExists)
			{
				Debug.LogWarning("[vBikeCameraState] Camera state '" + this.stateName + "' not found in CameraStateList. Please add it manually in the vThirdPersonCamera's CameraStateList ScriptableObject, or the camera will use 'Default' state when riding the bike.");
			}
		}

		// Token: 0x04002E34 RID: 11828
		[Header("Bike Camera Settings")]
		[Tooltip("Name of the camera state to use when riding (must exist in CameraStateList)")]
		public string stateName = "Bike";

		// Token: 0x04002E35 RID: 11829
		[Header("Info")]
		[Tooltip("Check this to see if the state exists")]
		[SerializeField]
		private bool stateExists;

		// Token: 0x04002E36 RID: 11830
		private vThirdPersonCamera tpCamera;
	}
}
