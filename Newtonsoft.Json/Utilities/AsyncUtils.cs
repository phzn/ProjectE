using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000041 RID: 65
	[NullableContext(1)]
	[Nullable(0)]
	internal static class AsyncUtils
	{
		// Token: 0x06000424 RID: 1060 RVA: 0x0001037E File Offset: 0x0000E57E
		internal static Task<bool> ToAsync(this bool value)
		{
			if (!value)
			{
				return AsyncUtils.False;
			}
			return AsyncUtils.True;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001038E File Offset: 0x0000E58E
		[NullableContext(2)]
		public static Task CancelIfRequestedAsync(this CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return null;
			}
			return cancellationToken.FromCanceled();
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x000103A1 File Offset: 0x0000E5A1
		[NullableContext(2)]
		[return: Nullable(new byte[] { 2, 1 })]
		public static Task<T> CancelIfRequestedAsync<T>(this CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return null;
			}
			return cancellationToken.FromCanceled<T>();
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x000103B4 File Offset: 0x0000E5B4
		public static Task FromCanceled(this CancellationToken cancellationToken)
		{
			return new Task(delegate
			{
			}, cancellationToken);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x000103DC File Offset: 0x0000E5DC
		public static Task<T> FromCanceled<[Nullable(2)] T>(this CancellationToken cancellationToken)
		{
			Func<T> func;
			if ((func = AsyncUtils.<>c__6<T>.<>9__6_0) == null)
			{
				Func<T> func2 = (AsyncUtils.<>c__6<T>.<>9__6_0 = () => default(T));
				func = func2;
			}
			return new Task<T>(func, cancellationToken);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00010410 File Offset: 0x0000E610
		public static Task WriteAsync(this TextWriter writer, char value, CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return writer.WriteAsync(value);
			}
			return cancellationToken.FromCanceled();
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00010429 File Offset: 0x0000E629
		public static Task WriteAsync(this TextWriter writer, [Nullable(2)] string value, CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return writer.WriteAsync(value);
			}
			return cancellationToken.FromCanceled();
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00010442 File Offset: 0x0000E642
		public static Task WriteAsync(this TextWriter writer, char[] value, int start, int count, CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return writer.WriteAsync(value, start, count);
			}
			return cancellationToken.FromCanceled();
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001045E File Offset: 0x0000E65E
		public static Task<int> ReadAsync(this TextReader reader, char[] buffer, int index, int count, CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return reader.ReadAsync(buffer, index, count);
			}
			return cancellationToken.FromCanceled<int>();
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0001047A File Offset: 0x0000E67A
		public static bool IsCompletedSuccessfully(this Task task)
		{
			return task.Status == TaskStatus.RanToCompletion;
		}

		// Token: 0x0400014C RID: 332
		public static readonly Task<bool> False = Task.FromResult<bool>(false);

		// Token: 0x0400014D RID: 333
		public static readonly Task<bool> True = Task.FromResult<bool>(true);

		// Token: 0x0400014E RID: 334
		internal static readonly Task CompletedTask = Task.Delay(0);
	}
}
