using System;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.ExposeLingerie;
using Features.Flash;
using UnityEngine;

namespace Features.BodyPhysics
{
	// Token: 0x02001505 RID: 5381
	[DisallowMultipleComponent]
	public sealed class OpenGarmentBodyPhysics : MonoBehaviour
	{
		// Token: 0x0600A62D RID: 42541 RVA: 0x004571F8 File Offset: 0x004553F8
		public static OpenGarmentBodyPhysics Ensure(GameObject host)
		{
			if (!Application.isPlaying || host == null)
			{
				return null;
			}
			OpenGarmentBodyPhysics component = host.GetComponent<OpenGarmentBodyPhysics>();
			if (!(component != null))
			{
				return host.AddComponent<OpenGarmentBodyPhysics>();
			}
			return component;
		}

		// Token: 0x0600A62E RID: 42542 RVA: 0x0045722F File Offset: 0x0045542F
		private void Awake()
		{
			this._wardrobe = base.GetComponent<WardrobeBodyPhysicsController>();
			this._armsFlashes = base.GetComponentsInChildren<ArmsFlashController>(true);
			this._fullFlashes = base.GetComponentsInChildren<ClothFullFlashController>(true);
			this._lingerie = base.GetComponentsInChildren<ExposeLingerieController>(true);
		}

		// Token: 0x0600A62F RID: 42543 RVA: 0x00457264 File Offset: 0x00455464
		private void Update()
		{
			if (this._wardrobe == null)
			{
				return;
			}
			for (int i = 0; i < OpenGarmentBodyPhysics.ArmsFlashes.Length; i++)
			{
				this.Report(OpenGarmentBodyPhysics.ArmsFlashes[i], this.ArmsFlashOpen(OpenGarmentBodyPhysics.ArmsFlashes[i].Flash));
			}
			this.Report(OpenGarmentBodyPhysics.TowelOpen, this.FullFlashOpen());
			this.Report(OpenGarmentBodyPhysics.ExposeBraOpen, this.ExposeBraOpened());
		}

		// Token: 0x0600A630 RID: 42544 RVA: 0x004572DB File Offset: 0x004554DB
		private void Report(OpenGarmentBodyPhysics.Opening opening, bool open)
		{
			this._wardrobe.SetGarmentOpening(opening.Source, opening.PresetId, open && opening.Breasts, open && opening.Butt);
		}

		// Token: 0x0600A631 RID: 42545 RVA: 0x0045730C File Offset: 0x0045550C
		private bool ArmsFlashOpen(string flash)
		{
			for (int i = 0; i < this._armsFlashes.Length; i++)
			{
				ArmsFlashController armsFlashController = this._armsFlashes[i];
				if (armsFlashController != null && armsFlashController.isActiveAndEnabled && armsFlashController.IsOpen && armsFlashController.Config != null && armsFlashController.Config.gateAsFlash && armsFlashController.Config.flashLayerName == flash)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A632 RID: 42546 RVA: 0x0045737C File Offset: 0x0045557C
		private bool FullFlashOpen()
		{
			for (int i = 0; i < this._fullFlashes.Length; i++)
			{
				if (this._fullFlashes[i] != null && this._fullFlashes[i].isActiveAndEnabled && this._fullFlashes[i].IsOpen)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A633 RID: 42547 RVA: 0x004573D0 File Offset: 0x004555D0
		private bool ExposeBraOpened()
		{
			for (int i = 0; i < this._lingerie.Length; i++)
			{
				if (this._lingerie[i] != null && this._lingerie[i].IsBraOpen)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400AAF8 RID: 43768
		private static readonly OpenGarmentBodyPhysics.Opening[] ArmsFlashes = new OpenGarmentBodyPhysics.Opening[]
		{
			new OpenGarmentBodyPhysics.Opening("SwimsuitFlash", "swimsuit_school", true, false),
			new OpenGarmentBodyPhysics.Opening("BikiniBraFlash", "bikini_bra", true, false),
			new OpenGarmentBodyPhysics.Opening("BikiniPantiesFlash", "bikini_panties", false, true),
			new OpenGarmentBodyPhysics.Opening("RainCoatFlash", "raincoat", true, false)
		};

		// Token: 0x0400AAF9 RID: 43769
		private static readonly OpenGarmentBodyPhysics.Opening TowelOpen = new OpenGarmentBodyPhysics.Opening("TowelFlash", "towel_bath", true, true);

		// Token: 0x0400AAFA RID: 43770
		private static readonly OpenGarmentBodyPhysics.Opening ExposeBraOpen = new OpenGarmentBodyPhysics.Opening("ExposeBra", "expose_bra", true, false);

		// Token: 0x0400AAFB RID: 43771
		private WardrobeBodyPhysicsController _wardrobe;

		// Token: 0x0400AAFC RID: 43772
		private ArmsFlashController[] _armsFlashes;

		// Token: 0x0400AAFD RID: 43773
		private ClothFullFlashController[] _fullFlashes;

		// Token: 0x0400AAFE RID: 43774
		private ExposeLingerieController[] _lingerie;

		// Token: 0x02001506 RID: 5382
		private readonly struct Opening
		{
			// Token: 0x0600A636 RID: 42550 RVA: 0x004574B8 File Offset: 0x004556B8
			public Opening(string flash, string presetId, bool breasts, bool butt)
			{
				this.Source = "OpenGarment." + flash;
				this.Flash = flash;
				this.PresetId = presetId;
				this.Breasts = breasts;
				this.Butt = butt;
			}

			// Token: 0x0400AAFF RID: 43775
			public readonly string Source;

			// Token: 0x0400AB00 RID: 43776
			public readonly string Flash;

			// Token: 0x0400AB01 RID: 43777
			public readonly string PresetId;

			// Token: 0x0400AB02 RID: 43778
			public readonly bool Breasts;

			// Token: 0x0400AB03 RID: 43779
			public readonly bool Butt;
		}
	}
}
