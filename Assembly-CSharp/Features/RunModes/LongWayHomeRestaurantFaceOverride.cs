using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CE3 RID: 3299
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(20002)]
	public sealed class LongWayHomeRestaurantFaceOverride : MonoBehaviour
	{
		// Token: 0x06005FC4 RID: 24516 RVA: 0x0025EC88 File Offset: 0x0025CE88
		public void Begin(Transform player, float blendSeconds = 0.8f)
		{
			if (this._player != null && this._player == player)
			{
				return;
			}
			this.Release();
			if (player == null)
			{
				return;
			}
			this._player = player;
			this._blendDuration = Mathf.Max(0f, blendSeconds);
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in player.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer.sharedMesh == null))
				{
					this.Capture(skinnedMeshRenderer, LongWayHomeRestaurantFaceOverride.HeldShapes, 100f);
					this.Capture(skinnedMeshRenderer, LongWayHomeRestaurantFaceOverride.ClearedShapes, 0f);
				}
			}
			this.ApplyBlend();
		}

		// Token: 0x06005FC5 RID: 24517 RVA: 0x0025ED2C File Offset: 0x0025CF2C
		private void Capture(SkinnedMeshRenderer renderer, string[] names, float weight)
		{
			Mesh sharedMesh = renderer.sharedMesh;
			foreach (string text in names)
			{
				int blendShapeIndex = sharedMesh.GetBlendShapeIndex(text);
				if (blendShapeIndex >= 0)
				{
					this._shapes.Add(new LongWayHomeRestaurantFaceOverride.ShapeState
					{
						Renderer = renderer,
						Mesh = sharedMesh,
						Index = blendShapeIndex,
						OriginalWeight = renderer.GetBlendShapeWeight(blendShapeIndex),
						HeldWeight = weight
					});
				}
			}
		}

		// Token: 0x06005FC6 RID: 24518 RVA: 0x0025EDA7 File Offset: 0x0025CFA7
		private void LateUpdate()
		{
			this.AdvanceBlend(Time.deltaTime);
		}

		// Token: 0x06005FC7 RID: 24519 RVA: 0x0025EDB4 File Offset: 0x0025CFB4
		private void AdvanceBlend(float deltaTime)
		{
			if (this._shapes.Count == 0)
			{
				return;
			}
			this._blendElapsed = Mathf.Min(this._blendDuration, this._blendElapsed + Mathf.Max(0f, deltaTime));
			this.ApplyBlend();
		}

		// Token: 0x06005FC8 RID: 24520 RVA: 0x0025EDF0 File Offset: 0x0025CFF0
		private void ApplyBlend()
		{
			float num = ((this._blendDuration > 0f) ? Mathf.Clamp01(this._blendElapsed / this._blendDuration) : 1f);
			float num2 = Mathf.SmoothStep(0f, 1f, num);
			foreach (LongWayHomeRestaurantFaceOverride.ShapeState shapeState in this._shapes)
			{
				if (shapeState.Renderer != null && shapeState.Mesh != null && shapeState.Renderer.sharedMesh == shapeState.Mesh)
				{
					shapeState.Renderer.SetBlendShapeWeight(shapeState.Index, Mathf.Lerp(shapeState.OriginalWeight, shapeState.HeldWeight, num2));
				}
			}
		}

		// Token: 0x06005FC9 RID: 24521 RVA: 0x0025EECC File Offset: 0x0025D0CC
		public void Release()
		{
			foreach (LongWayHomeRestaurantFaceOverride.ShapeState shapeState in this._shapes)
			{
				if (shapeState.Renderer != null && shapeState.Mesh != null && shapeState.Renderer.sharedMesh == shapeState.Mesh)
				{
					shapeState.Renderer.SetBlendShapeWeight(shapeState.Index, shapeState.OriginalWeight);
				}
			}
			this._shapes.Clear();
			this._player = null;
			this._blendDuration = (this._blendElapsed = 0f);
		}

		// Token: 0x06005FCA RID: 24522 RVA: 0x0025EF8C File Offset: 0x0025D18C
		private void OnDisable()
		{
			this.Release();
		}

		// Token: 0x06005FCB RID: 24523 RVA: 0x0025EF8C File Offset: 0x0025D18C
		private void OnDestroy()
		{
			this.Release();
		}

		// Token: 0x04006131 RID: 24881
		private static readonly string[] HeldShapes = new string[] { "fx_/////2", "eye_close_sleep_L", "eye_close_sleep_R" };

		// Token: 0x04006132 RID: 24882
		private static readonly string[] ClearedShapes = new string[]
		{
			"fx_/////1", "fx_/////3", "BLUSH 40%", "BLUSH 60%", "BLUSH 80%", "BLUSH 100%", "eye_zito_L", "eye_zito_R", "eye_zito2_L", "eye_zito2_R",
			"eye_pleasure_L", "eye_pleasure_R", "eye_close_smile_L", "eye_close_smile_R", "eye_close_nagomi_L", "eye_close_nagomi_R", "eye_close_nagomi2_L", "eye_close_nagomi2_R", "eye_surprise_L", "eye_surprise_R",
			"eye_sad_L", "eye_sad_R", "eye_angry_L", "eye_angry_R", "eye_tare_L", "eye_tare_R", "eye_><", "eye_under_down_L", "eye_under_down_R", "eye_under_down2_L",
			"eye_under_down2_R", "eye_under_up_L", "eye_under_up_R", "eye_under_up2_L", "eye_under_up2_R", "eye_under_up3_L", "eye_under_up3_R", "vrc.blink", "blink"
		};

		// Token: 0x04006133 RID: 24883
		private readonly List<LongWayHomeRestaurantFaceOverride.ShapeState> _shapes = new List<LongWayHomeRestaurantFaceOverride.ShapeState>();

		// Token: 0x04006134 RID: 24884
		private Transform _player;

		// Token: 0x04006135 RID: 24885
		private float _blendDuration;

		// Token: 0x04006136 RID: 24886
		private float _blendElapsed;

		// Token: 0x02000CE4 RID: 3300
		private struct ShapeState
		{
			// Token: 0x04006137 RID: 24887
			public SkinnedMeshRenderer Renderer;

			// Token: 0x04006138 RID: 24888
			public Mesh Mesh;

			// Token: 0x04006139 RID: 24889
			public int Index;

			// Token: 0x0400613A RID: 24890
			public float OriginalWeight;

			// Token: 0x0400613B RID: 24891
			public float HeldWeight;
		}
	}
}
