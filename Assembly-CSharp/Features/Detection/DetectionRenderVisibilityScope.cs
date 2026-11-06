using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.Detection
{
	// Token: 0x02001248 RID: 4680
	public sealed class DetectionRenderVisibilityScope : IDisposable
	{
		// Token: 0x06009189 RID: 37257 RVA: 0x003B9538 File Offset: 0x003B7738
		public DetectionRenderVisibilityScope(Camera outputCamera)
		{
			if (outputCamera == null)
			{
				throw new ArgumentNullException("outputCamera");
			}
			this._outputCamera = outputCamera;
			DetectionRenderVisibilityScope.LiveScopes.Add(this);
			RenderPipelineManager.beginCameraRendering += this.OnBeginCameraRendering;
			RenderPipelineManager.endCameraRendering += this.OnEndCameraRendering;
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(this.OnPreCull));
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(this.OnPostRender));
		}

		// Token: 0x0600918A RID: 37258 RVA: 0x003B9608 File Offset: 0x003B7808
		public void SetHiddenRenderers(IList<Renderer> renderers)
		{
			if (this._disposed)
			{
				return;
			}
			this.RestoreCurrentRender();
			this._renderers.Clear();
			this._uniqueRenderers.Clear();
			if (renderers != null)
			{
				for (int i = 0; i < renderers.Count; i++)
				{
					Renderer renderer = renderers[i];
					if (renderer != null && this._uniqueRenderers.Add(renderer))
					{
						this._renderers.Add(renderer);
					}
				}
			}
			this.ApplyForCurrentCamera();
		}

		// Token: 0x0600918B RID: 37259 RVA: 0x003B967F File Offset: 0x003B787F
		public void Restore()
		{
			this.RestoreCurrentRender();
			this._renderers.Clear();
			this._uniqueRenderers.Clear();
			this._cameraStack.Clear();
		}

		// Token: 0x0600918C RID: 37260 RVA: 0x003B96A8 File Offset: 0x003B78A8
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			RenderPipelineManager.beginCameraRendering -= this.OnBeginCameraRendering;
			RenderPipelineManager.endCameraRendering -= this.OnEndCameraRendering;
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(this.OnPreCull));
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(this.OnPostRender));
			DetectionRenderVisibilityScope.LiveScopes.Remove(this);
			this.Restore();
		}

		// Token: 0x0600918D RID: 37261 RVA: 0x003B973C File Offset: 0x003B793C
		public static IDisposable AllowAuxiliaryRendering()
		{
			DetectionRenderVisibilityScope._auxiliaryRenderingDepth++;
			if (DetectionRenderVisibilityScope._auxiliaryRenderingDepth == 1)
			{
				foreach (DetectionRenderVisibilityScope detectionRenderVisibilityScope in DetectionRenderVisibilityScope.LiveScopes)
				{
					detectionRenderVisibilityScope.RestoreCurrentRender();
				}
			}
			return new DetectionRenderVisibilityScope.AuxiliaryRenderingLease();
		}

		// Token: 0x0600918E RID: 37262 RVA: 0x003B97A4 File Offset: 0x003B79A4
		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (GraphicsSettings.currentRenderPipeline != null)
			{
				this.BeginCamera(camera);
			}
		}

		// Token: 0x0600918F RID: 37263 RVA: 0x003B97BA File Offset: 0x003B79BA
		private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (GraphicsSettings.currentRenderPipeline != null)
			{
				this.EndCamera(camera);
			}
		}

		// Token: 0x06009190 RID: 37264 RVA: 0x003B97D0 File Offset: 0x003B79D0
		private void OnPreCull(Camera camera)
		{
			if (GraphicsSettings.currentRenderPipeline == null)
			{
				this.BeginCamera(camera);
			}
		}

		// Token: 0x06009191 RID: 37265 RVA: 0x003B97E6 File Offset: 0x003B79E6
		private void OnPostRender(Camera camera)
		{
			if (GraphicsSettings.currentRenderPipeline == null)
			{
				this.EndCamera(camera);
			}
		}

		// Token: 0x06009192 RID: 37266 RVA: 0x003B97FC File Offset: 0x003B79FC
		private void BeginCamera(Camera camera)
		{
			if (this._disposed || camera == null)
			{
				return;
			}
			this.RestoreCurrentRender();
			this._cameraStack.Add(camera);
			this.ApplyForCurrentCamera();
		}

		// Token: 0x06009193 RID: 37267 RVA: 0x003B9828 File Offset: 0x003B7A28
		private void EndCamera(Camera camera)
		{
			if (this._disposed)
			{
				return;
			}
			int num = this._cameraStack.Count - 1;
			while (num >= 0 && this._cameraStack[num] != camera)
			{
				num--;
			}
			if (num < 0)
			{
				return;
			}
			this.RestoreCurrentRender();
			this._cameraStack.RemoveRange(num, this._cameraStack.Count - num);
			this.ApplyForCurrentCamera();
		}

		// Token: 0x06009194 RID: 37268 RVA: 0x003B9894 File Offset: 0x003B7A94
		private void ApplyForCurrentCamera()
		{
			if (this._disposed || DetectionRenderVisibilityScope._auxiliaryRenderingDepth != 0 || this._outputCamera == null || this._cameraStack.Count == 0 || this._cameraStack[this._cameraStack.Count - 1] != this._outputCamera || this._savedStates.Count != 0)
			{
				return;
			}
			for (int i = 0; i < this._renderers.Count; i++)
			{
				Renderer renderer = this._renderers[i];
				if (!(renderer == null))
				{
					this._savedStates.Add(new DetectionRenderVisibilityScope.RendererState
					{
						renderer = renderer,
						forceRenderingOff = renderer.forceRenderingOff
					});
					renderer.forceRenderingOff = true;
				}
			}
		}

		// Token: 0x06009195 RID: 37269 RVA: 0x003B995C File Offset: 0x003B7B5C
		private void RestoreCurrentRender()
		{
			for (int i = 0; i < this._savedStates.Count; i++)
			{
				DetectionRenderVisibilityScope.RendererState rendererState = this._savedStates[i];
				if (rendererState.renderer != null)
				{
					rendererState.renderer.forceRenderingOff = rendererState.forceRenderingOff;
				}
			}
			this._savedStates.Clear();
		}

		// Token: 0x040095D0 RID: 38352
		private static readonly HashSet<DetectionRenderVisibilityScope> LiveScopes = new HashSet<DetectionRenderVisibilityScope>();

		// Token: 0x040095D1 RID: 38353
		private static int _auxiliaryRenderingDepth;

		// Token: 0x040095D2 RID: 38354
		private readonly Camera _outputCamera;

		// Token: 0x040095D3 RID: 38355
		private readonly List<Renderer> _renderers = new List<Renderer>(32);

		// Token: 0x040095D4 RID: 38356
		private readonly HashSet<Renderer> _uniqueRenderers = new HashSet<Renderer>();

		// Token: 0x040095D5 RID: 38357
		private readonly List<DetectionRenderVisibilityScope.RendererState> _savedStates = new List<DetectionRenderVisibilityScope.RendererState>(32);

		// Token: 0x040095D6 RID: 38358
		private readonly List<Camera> _cameraStack = new List<Camera>(4);

		// Token: 0x040095D7 RID: 38359
		private bool _disposed;

		// Token: 0x02001249 RID: 4681
		private struct RendererState
		{
			// Token: 0x040095D8 RID: 38360
			public Renderer renderer;

			// Token: 0x040095D9 RID: 38361
			public bool forceRenderingOff;
		}

		// Token: 0x0200124A RID: 4682
		private sealed class AuxiliaryRenderingLease : IDisposable
		{
			// Token: 0x06009197 RID: 37271 RVA: 0x003B99C4 File Offset: 0x003B7BC4
			public void Dispose()
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				DetectionRenderVisibilityScope._auxiliaryRenderingDepth--;
				if (DetectionRenderVisibilityScope._auxiliaryRenderingDepth != 0)
				{
					return;
				}
				foreach (DetectionRenderVisibilityScope detectionRenderVisibilityScope in DetectionRenderVisibilityScope.LiveScopes)
				{
					detectionRenderVisibilityScope.ApplyForCurrentCamera();
				}
			}

			// Token: 0x040095DA RID: 38362
			private bool _disposed;
		}
	}
}
