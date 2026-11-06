using System;
using Unity.Collections;

namespace UnityEngine.Splines
{
	// Token: 0x02000039 RID: 57
	public struct SplineComputeBufferScope<T> : IDisposable where T : ISpline
	{
		// Token: 0x060002AE RID: 686 RVA: 0x0000C888 File Offset: 0x0000AA88
		public SplineComputeBufferScope(T spline)
		{
			this.m_Spline = spline;
			this.m_KnotCount = 0;
			this.m_CurveBuffer = (this.m_LengthBuffer = null);
			this.m_Shader = null;
			this.m_Info = (this.m_Curves = (this.m_CurveLengths = null));
			this.m_Kernel = 0;
			this.Upload();
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000C8E0 File Offset: 0x0000AAE0
		public void Bind(ComputeShader shader, int kernel, string info, string curves, string lengths)
		{
			if (shader == null)
			{
				throw new ArgumentNullException("shader");
			}
			if (string.IsNullOrEmpty(info))
			{
				throw new ArgumentNullException("info");
			}
			if (string.IsNullOrEmpty(curves))
			{
				throw new ArgumentNullException("curves");
			}
			if (string.IsNullOrEmpty(lengths))
			{
				throw new ArgumentNullException("lengths");
			}
			this.m_Shader = shader;
			this.m_Info = info;
			this.m_Curves = curves;
			this.m_CurveLengths = lengths;
			this.m_Kernel = kernel;
			this.m_Shader.SetVector(this.m_Info, this.Info);
			this.m_Shader.SetBuffer(this.m_Kernel, this.m_Curves, this.Curves);
			this.m_Shader.SetBuffer(this.m_Kernel, this.m_CurveLengths, this.CurveLengths);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000C9B2 File Offset: 0x0000ABB2
		public void Dispose()
		{
			ComputeBuffer curveBuffer = this.m_CurveBuffer;
			if (curveBuffer != null)
			{
				curveBuffer.Dispose();
			}
			ComputeBuffer lengthBuffer = this.m_LengthBuffer;
			if (lengthBuffer == null)
			{
				return;
			}
			lengthBuffer.Dispose();
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000C9D8 File Offset: 0x0000ABD8
		public void Upload()
		{
			int count = this.m_Spline.Count;
			if (this.m_KnotCount != count)
			{
				this.m_KnotCount = this.m_Spline.Count;
				ComputeBuffer curveBuffer = this.m_CurveBuffer;
				if (curveBuffer != null)
				{
					curveBuffer.Dispose();
				}
				ComputeBuffer lengthBuffer = this.m_LengthBuffer;
				if (lengthBuffer != null)
				{
					lengthBuffer.Dispose();
				}
				this.m_CurveBuffer = new ComputeBuffer(this.m_KnotCount, 48);
				this.m_LengthBuffer = new ComputeBuffer(this.m_KnotCount, 4);
			}
			NativeArray<BezierCurve> nativeArray = new NativeArray<BezierCurve>(this.m_KnotCount, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<float> nativeArray2 = new NativeArray<float>(this.m_KnotCount, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < this.m_KnotCount; i++)
			{
				nativeArray[i] = this.m_Spline.GetCurve(i);
				nativeArray2[i] = this.m_Spline.GetCurveLength(i);
			}
			if (!string.IsNullOrEmpty(this.m_Info))
			{
				this.m_Shader.SetVector(this.m_Info, this.Info);
			}
			this.m_CurveBuffer.SetData<BezierCurve>(nativeArray);
			this.m_LengthBuffer.SetData<float>(nativeArray2);
			nativeArray.Dispose();
			nativeArray2.Dispose();
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000CB0C File Offset: 0x0000AD0C
		public Vector4 Info
		{
			get
			{
				return new Vector4((float)this.m_Spline.Count, (float)(this.m_Spline.Closed ? 1 : 0), this.m_Spline.GetLength(), 0f);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000CB5E File Offset: 0x0000AD5E
		public ComputeBuffer Curves
		{
			get
			{
				return this.m_CurveBuffer;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000CB66 File Offset: 0x0000AD66
		public ComputeBuffer CurveLengths
		{
			get
			{
				return this.m_LengthBuffer;
			}
		}

		// Token: 0x040000E0 RID: 224
		private T m_Spline;

		// Token: 0x040000E1 RID: 225
		private int m_KnotCount;

		// Token: 0x040000E2 RID: 226
		private ComputeBuffer m_CurveBuffer;

		// Token: 0x040000E3 RID: 227
		private ComputeBuffer m_LengthBuffer;

		// Token: 0x040000E4 RID: 228
		private ComputeShader m_Shader;

		// Token: 0x040000E5 RID: 229
		private string m_Info;

		// Token: 0x040000E6 RID: 230
		private string m_Curves;

		// Token: 0x040000E7 RID: 231
		private string m_CurveLengths;

		// Token: 0x040000E8 RID: 232
		private int m_Kernel;
	}
}
