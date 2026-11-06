using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000356 RID: 854
	[RequireComponent(typeof(Volume))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMAutoFocus_URP")]
	public class MMAutoFocus_URP : MonoBehaviour
	{
		// Token: 0x06001775 RID: 6005 RVA: 0x0006354D File Offset: 0x0006174D
		private void Start()
		{
			this._volume = base.GetComponent<Volume>();
			this._profile = this._volume.profile;
			this._profile.TryGet<DepthOfField>(out this._depthOfField);
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x00063580 File Offset: 0x00061780
		private void Update()
		{
			float num = Vector3.Distance(this.CameraTransform.position, this.FocusTargets[Mathf.FloorToInt(this.FocusTargetID)].position);
			this._depthOfField.focusDistance.Override(num);
			this._depthOfField.aperture.Override(this.Aperture);
		}

		// Token: 0x04001AFD RID: 6909
		[Header("Bindings")]
		[Tooltip("the position of the camera")]
		public Transform CameraTransform;

		// Token: 0x04001AFE RID: 6910
		[Tooltip("a list of all possible targets")]
		public Transform[] FocusTargets;

		// Token: 0x04001AFF RID: 6911
		[Header("Setup")]
		[Tooltip("the current target of this auto focus")]
		public float FocusTargetID;

		// Token: 0x04001B00 RID: 6912
		[Header("Desired Aperture")]
		[Tooltip("the aperture to work with")]
		[Range(0.1f, 20f)]
		public float Aperture = 0.1f;

		// Token: 0x04001B01 RID: 6913
		protected Volume _volume;

		// Token: 0x04001B02 RID: 6914
		protected VolumeProfile _profile;

		// Token: 0x04001B03 RID: 6915
		protected DepthOfField _depthOfField;
	}
}
