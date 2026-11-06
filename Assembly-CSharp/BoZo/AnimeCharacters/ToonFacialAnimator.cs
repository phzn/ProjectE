using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008CD RID: 2253
	public class ToonFacialAnimator : MonoBehaviour
	{
		// Token: 0x06003CA0 RID: 15520 RVA: 0x001689EC File Offset: 0x00166BEC
		private void Start()
		{
			this.GenerateFace();
		}

		// Token: 0x06003CA1 RID: 15521 RVA: 0x001689F4 File Offset: 0x00166BF4
		[ContextMenu("test")]
		public void test()
		{
			this.SetFace(0);
		}

		// Token: 0x06003CA2 RID: 15522 RVA: 0x00168A00 File Offset: 0x00166C00
		private void GenerateFace()
		{
			OutfitSystem outfitSystem = base.GetComponentInParent<OutfitSystem>();
			if (!outfitSystem)
			{
				outfitSystem = base.GetComponentInChildren<OutfitSystem>();
			}
			if (outfitSystem == null)
			{
				Debug.LogError("[ToonFacialAnimator] OutfitSystem not found on " + base.name + " or its parents/children.");
				return;
			}
			if (!outfitSystem.initalized)
			{
				outfitSystem.Init();
			}
			Dictionary<string, Transform> bones = outfitSystem.GetBones();
			if (bones == null || !bones.ContainsKey("head") || !bones.ContainsKey("eyeRoot_l") || !bones.ContainsKey("eyeRoot_r"))
			{
				Debug.LogError("[ToonFacialAnimator] Required bones not found in OutfitSystem on " + base.name + ". Ensure OutfitSystem is initialized and has 'head', 'eyeRoot_l', and 'eyeRoot_r' bones.");
				return;
			}
			this.leftEye = new GameObject("ToonLeftEye", new Type[] { typeof(SpriteRenderer) }).GetComponent<SpriteRenderer>();
			this.rightEye = new GameObject("ToonRightEye", new Type[] { typeof(SpriteRenderer) }).GetComponent<SpriteRenderer>();
			this.mouth = new GameObject("ToonMouth", new Type[] { typeof(SpriteRenderer) }).GetComponent<SpriteRenderer>();
			Transform transform = bones["head"];
			Transform transform2 = bones["eyeRoot_l"];
			Transform transform3 = bones["eyeRoot_r"];
			this.leftEye.transform.parent = transform2;
			this.rightEye.transform.parent = transform3;
			this.mouth.transform.parent = transform;
			this.leftEye.transform.localPosition = this.commonLeftEyeOffset;
			this.leftEye.transform.localScale = this.spriteScale;
			this.leftEye.transform.localRotation = Quaternion.Euler(0f, this.eyeRotation, 0f);
			this.rightEye.transform.localPosition = this.commonRightEyeOffset;
			this.rightEye.transform.localScale = this.spriteScale;
			this.rightEye.transform.localRotation = Quaternion.Euler(0f, -this.eyeRotation, 0f);
			this.mouth.transform.localPosition = this.commonMouthOffset;
			this.mouth.transform.localScale = this.spriteScale;
			this.mouth.transform.localRotation = Quaternion.Euler(this.mouthRotation, 0f, 0f);
		}

		// Token: 0x06003CA3 RID: 15523 RVA: 0x00168C68 File Offset: 0x00166E68
		public void SetFace(int preset)
		{
			if (this.leftEye == null || this.rightEye == null || this.mouth == null)
			{
				this.GenerateFace();
			}
			if (this.leftEye == null || this.rightEye == null || this.mouth == null)
			{
				return;
			}
			if (preset < 0 || preset >= this.facePresets.Length)
			{
				Debug.LogWarning(string.Format("[ToonFacialAnimator] Invalid preset index {0}. Max is {1}", preset, this.facePresets.Length - 1));
				return;
			}
			ToonFacialAnimator.ToonFacePreset toonFacePreset = this.facePresets[preset];
			this.leftEye.sprite = toonFacePreset.leftEyeSprite;
			this.leftEye.flipX = toonFacePreset.leftEyeFlip;
			this.leftEye.transform.localPosition = toonFacePreset.leftEyeOffset + this.commonLeftEyeOffset;
			this.leftEye.transform.localScale = toonFacePreset.leftEyeScale + this.spriteScale;
			this.rightEye.sprite = toonFacePreset.rightEyeSprite;
			this.rightEye.flipX = toonFacePreset.rightEyeFlip;
			this.rightEye.transform.localPosition = toonFacePreset.rightEyeOffset + this.commonRightEyeOffset;
			this.rightEye.transform.localScale = toonFacePreset.rightEyeScale + this.spriteScale;
			this.mouth.sprite = toonFacePreset.mouthSprite;
			this.mouth.flipX = toonFacePreset.mouthFlip;
			this.mouth.transform.localPosition = toonFacePreset.mouthOffset + this.commonMouthOffset;
			this.mouth.transform.localScale = toonFacePreset.mouthScale + this.spriteScale;
		}

		// Token: 0x04003C9C RID: 15516
		public Vector3 commonLeftEyeOffset;

		// Token: 0x04003C9D RID: 15517
		public Vector3 commonRightEyeOffset;

		// Token: 0x04003C9E RID: 15518
		public Vector3 commonMouthOffset;

		// Token: 0x04003C9F RID: 15519
		public float eyeRotation;

		// Token: 0x04003CA0 RID: 15520
		public float mouthRotation;

		// Token: 0x04003CA1 RID: 15521
		public Vector3 spriteScale;

		// Token: 0x04003CA2 RID: 15522
		private SpriteRenderer leftEye;

		// Token: 0x04003CA3 RID: 15523
		private SpriteRenderer rightEye;

		// Token: 0x04003CA4 RID: 15524
		private SpriteRenderer mouth;

		// Token: 0x04003CA5 RID: 15525
		[SerializeField]
		private ToonFacialAnimator.ToonFacePreset[] facePresets;

		// Token: 0x020008CE RID: 2254
		[Serializable]
		public class ToonFacePreset
		{
			// Token: 0x04003CA6 RID: 15526
			public string presetName;

			// Token: 0x04003CA7 RID: 15527
			public Sprite leftEyeSprite;

			// Token: 0x04003CA8 RID: 15528
			public bool leftEyeFlip = true;

			// Token: 0x04003CA9 RID: 15529
			public Vector3 leftEyeOffset;

			// Token: 0x04003CAA RID: 15530
			public Vector3 leftEyeScale = Vector3.zero;

			// Token: 0x04003CAB RID: 15531
			public Sprite rightEyeSprite;

			// Token: 0x04003CAC RID: 15532
			public bool rightEyeFlip;

			// Token: 0x04003CAD RID: 15533
			public Vector3 rightEyeOffset;

			// Token: 0x04003CAE RID: 15534
			public Vector3 rightEyeScale = Vector3.zero;

			// Token: 0x04003CAF RID: 15535
			public Sprite mouthSprite;

			// Token: 0x04003CB0 RID: 15536
			public bool mouthFlip = true;

			// Token: 0x04003CB1 RID: 15537
			public Vector3 mouthOffset;

			// Token: 0x04003CB2 RID: 15538
			public Vector3 mouthScale = Vector3.zero;
		}
	}
}
