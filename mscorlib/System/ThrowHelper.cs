using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x020001C6 RID: 454
	[StackTraceHidden]
	internal static class ThrowHelper
	{
		// Token: 0x06001350 RID: 4944 RVA: 0x0004DBB8 File Offset: 0x0004BDB8
		internal static void ThrowArgumentNullException(ExceptionArgument argument)
		{
			throw ThrowHelper.CreateArgumentNullException(argument);
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0004DBC0 File Offset: 0x0004BDC0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentNullException(ExceptionArgument argument)
		{
			return new ArgumentNullException(argument.ToString());
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0004DBD4 File Offset: 0x0004BDD4
		internal static void ThrowArrayTypeMismatchException()
		{
			throw ThrowHelper.CreateArrayTypeMismatchException();
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0004DBDB File Offset: 0x0004BDDB
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArrayTypeMismatchException()
		{
			return new ArrayTypeMismatchException();
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x0004DBE2 File Offset: 0x0004BDE2
		internal static void ThrowArgumentException_InvalidTypeWithPointersNotSupported(Type type)
		{
			throw ThrowHelper.CreateArgumentException_InvalidTypeWithPointersNotSupported(type);
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0004DBEA File Offset: 0x0004BDEA
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentException_InvalidTypeWithPointersNotSupported(Type type)
		{
			return new ArgumentException(SR.Format("Cannot use type '{0}'. Only value types without pointers or references are supported.", type));
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x0004DBFC File Offset: 0x0004BDFC
		internal static void ThrowArgumentException_DestinationTooShort()
		{
			throw ThrowHelper.CreateArgumentException_DestinationTooShort();
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x0004DC03 File Offset: 0x0004BE03
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentException_DestinationTooShort()
		{
			return new ArgumentException("Destination is too short.");
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0004DC0F File Offset: 0x0004BE0F
		internal static void ThrowIndexOutOfRangeException()
		{
			throw ThrowHelper.CreateIndexOutOfRangeException();
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0004DC16 File Offset: 0x0004BE16
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateIndexOutOfRangeException()
		{
			return new IndexOutOfRangeException();
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0004DC1D File Offset: 0x0004BE1D
		internal static void ThrowArgumentOutOfRangeException()
		{
			throw ThrowHelper.CreateArgumentOutOfRangeException();
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x0004DC24 File Offset: 0x0004BE24
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentOutOfRangeException()
		{
			return new ArgumentOutOfRangeException();
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0004DC2B File Offset: 0x0004BE2B
		internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
		{
			throw ThrowHelper.CreateArgumentOutOfRangeException(argument);
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x0004DC33 File Offset: 0x0004BE33
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentOutOfRangeException(ExceptionArgument argument)
		{
			return new ArgumentOutOfRangeException(argument.ToString());
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x0004DC47 File Offset: 0x0004BE47
		internal static void ThrowArgumentOutOfRangeException_PrecisionTooLarge()
		{
			throw ThrowHelper.CreateArgumentOutOfRangeException_PrecisionTooLarge();
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x0004DC4E File Offset: 0x0004BE4E
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentOutOfRangeException_PrecisionTooLarge()
		{
			return new ArgumentOutOfRangeException("precision", SR.Format("Precision cannot be larger than {0}.", 99));
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x0004DC6B File Offset: 0x0004BE6B
		internal static void ThrowArgumentOutOfRangeException_SymbolDoesNotFit()
		{
			throw ThrowHelper.CreateArgumentOutOfRangeException_SymbolDoesNotFit();
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x0004DC72 File Offset: 0x0004BE72
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentOutOfRangeException_SymbolDoesNotFit()
		{
			return new ArgumentOutOfRangeException("symbol", "Format specifier was invalid.");
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x0004DC83 File Offset: 0x0004BE83
		internal static void ThrowInvalidOperationException()
		{
			throw ThrowHelper.CreateInvalidOperationException();
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x0004DC8A File Offset: 0x0004BE8A
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateInvalidOperationException()
		{
			return new InvalidOperationException();
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x0004DC91 File Offset: 0x0004BE91
		internal static void ThrowInvalidOperationException_OutstandingReferences()
		{
			throw ThrowHelper.CreateInvalidOperationException_OutstandingReferences();
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x0004DC98 File Offset: 0x0004BE98
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateInvalidOperationException_OutstandingReferences()
		{
			return new InvalidOperationException("Release all references before disposing this instance.");
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x0004DCA4 File Offset: 0x0004BEA4
		internal static void ThrowInvalidOperationException_UnexpectedSegmentType()
		{
			throw ThrowHelper.CreateInvalidOperationException_UnexpectedSegmentType();
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x0004DCAB File Offset: 0x0004BEAB
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateInvalidOperationException_UnexpectedSegmentType()
		{
			return new InvalidOperationException("Unexpected segment type.");
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x0004DCB7 File Offset: 0x0004BEB7
		internal static void ThrowInvalidOperationException_EndPositionNotReached()
		{
			throw ThrowHelper.CreateInvalidOperationException_EndPositionNotReached();
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x0004DCBE File Offset: 0x0004BEBE
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateInvalidOperationException_EndPositionNotReached()
		{
			return new InvalidOperationException("End position was not reached during enumeration.");
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x0004DCCA File Offset: 0x0004BECA
		internal static void ThrowArgumentOutOfRangeException_PositionOutOfRange()
		{
			throw ThrowHelper.CreateArgumentOutOfRangeException_PositionOutOfRange();
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x0004DCD1 File Offset: 0x0004BED1
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentOutOfRangeException_PositionOutOfRange()
		{
			return new ArgumentOutOfRangeException("position");
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x0004DCDD File Offset: 0x0004BEDD
		internal static void ThrowArgumentOutOfRangeException_OffsetOutOfRange()
		{
			throw ThrowHelper.CreateArgumentOutOfRangeException_OffsetOutOfRange();
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x0004DCE4 File Offset: 0x0004BEE4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentOutOfRangeException_OffsetOutOfRange()
		{
			return new ArgumentOutOfRangeException("offset");
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x0004DCF0 File Offset: 0x0004BEF0
		internal static void ThrowObjectDisposedException_ArrayMemoryPoolBuffer()
		{
			throw ThrowHelper.CreateObjectDisposedException_ArrayMemoryPoolBuffer();
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x0004DCF7 File Offset: 0x0004BEF7
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateObjectDisposedException_ArrayMemoryPoolBuffer()
		{
			return new ObjectDisposedException("ArrayMemoryPoolBuffer");
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x0004DD03 File Offset: 0x0004BF03
		internal static void ThrowFormatException_BadFormatSpecifier()
		{
			throw ThrowHelper.CreateFormatException_BadFormatSpecifier();
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x0004DD0A File Offset: 0x0004BF0A
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateFormatException_BadFormatSpecifier()
		{
			return new FormatException("Format specifier was invalid.");
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x0004DD16 File Offset: 0x0004BF16
		internal static void ThrowArgumentException_OverlapAlignmentMismatch()
		{
			throw ThrowHelper.CreateArgumentException_OverlapAlignmentMismatch();
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x0004DD1D File Offset: 0x0004BF1D
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentException_OverlapAlignmentMismatch()
		{
			return new ArgumentException("Overlapping spans have mismatching alignment.");
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x0004DD29 File Offset: 0x0004BF29
		internal static void ThrowNotSupportedException()
		{
			throw ThrowHelper.CreateThrowNotSupportedException();
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x0004DD30 File Offset: 0x0004BF30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateThrowNotSupportedException()
		{
			return new NotSupportedException();
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x0004DD37 File Offset: 0x0004BF37
		public static bool TryFormatThrowFormatException(out int bytesWritten)
		{
			bytesWritten = 0;
			ThrowHelper.ThrowFormatException_BadFormatSpecifier();
			return false;
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x0004DD42 File Offset: 0x0004BF42
		public static bool TryParseThrowFormatException<T>(out T value, out int bytesConsumed)
		{
			value = default(T);
			bytesConsumed = 0;
			ThrowHelper.ThrowFormatException_BadFormatSpecifier();
			return false;
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x0004DD54 File Offset: 0x0004BF54
		public static void ThrowArgumentValidationException<T>(ReadOnlySequenceSegment<T> startSegment, int startIndex, ReadOnlySequenceSegment<T> endSegment)
		{
			throw ThrowHelper.CreateArgumentValidationException<T>(startSegment, startIndex, endSegment);
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0004DD60 File Offset: 0x0004BF60
		private static Exception CreateArgumentValidationException<T>(ReadOnlySequenceSegment<T> startSegment, int startIndex, ReadOnlySequenceSegment<T> endSegment)
		{
			if (startSegment == null)
			{
				return ThrowHelper.CreateArgumentNullException(ExceptionArgument.startSegment);
			}
			if (endSegment == null)
			{
				return ThrowHelper.CreateArgumentNullException(ExceptionArgument.endSegment);
			}
			if (startSegment != endSegment && startSegment.RunningIndex > endSegment.RunningIndex)
			{
				return ThrowHelper.CreateArgumentOutOfRangeException(ExceptionArgument.endSegment);
			}
			if (startSegment.Memory.Length < startIndex)
			{
				return ThrowHelper.CreateArgumentOutOfRangeException(ExceptionArgument.startIndex);
			}
			return ThrowHelper.CreateArgumentOutOfRangeException(ExceptionArgument.endIndex);
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0004DDBD File Offset: 0x0004BFBD
		public static void ThrowArgumentValidationException(Array array, int start)
		{
			throw ThrowHelper.CreateArgumentValidationException(array, start);
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0004DDC6 File Offset: 0x0004BFC6
		private static Exception CreateArgumentValidationException(Array array, int start)
		{
			if (array == null)
			{
				return ThrowHelper.CreateArgumentNullException(ExceptionArgument.array);
			}
			if (start > array.Length)
			{
				return ThrowHelper.CreateArgumentOutOfRangeException(ExceptionArgument.start);
			}
			return ThrowHelper.CreateArgumentOutOfRangeException(ExceptionArgument.length);
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x0004DDEA File Offset: 0x0004BFEA
		public static void ThrowStartOrEndArgumentValidationException(long start)
		{
			throw ThrowHelper.CreateStartOrEndArgumentValidationException(start);
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x0004DDF2 File Offset: 0x0004BFF2
		private static Exception CreateStartOrEndArgumentValidationException(long start)
		{
			if (start < 0L)
			{
				return ThrowHelper.CreateArgumentOutOfRangeException(ExceptionArgument.start);
			}
			return ThrowHelper.CreateArgumentOutOfRangeException(ExceptionArgument.length);
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x0004DE08 File Offset: 0x0004C008
		internal static void ThrowWrongKeyTypeArgumentException(object key, Type targetType)
		{
			throw new ArgumentException(Environment.GetResourceString("The value \"{0}\" is not of type \"{1}\" and cannot be used in this generic collection.", new object[] { key, targetType }), "key");
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x0004DE2C File Offset: 0x0004C02C
		internal static void ThrowWrongValueTypeArgumentException(object value, Type targetType)
		{
			throw new ArgumentException(Environment.GetResourceString("The value \"{0}\" is not of type \"{1}\" and cannot be used in this generic collection.", new object[] { value, targetType }), "value");
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x0004DE50 File Offset: 0x0004C050
		internal static void ThrowKeyNotFoundException()
		{
			throw new KeyNotFoundException();
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x0004DE57 File Offset: 0x0004C057
		internal static void ThrowArgumentException(ExceptionResource resource)
		{
			throw new ArgumentException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0004DE69 File Offset: 0x0004C069
		internal static void ThrowArgumentException(ExceptionResource resource, ExceptionArgument argument)
		{
			throw new ArgumentException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)), ThrowHelper.GetArgumentName(argument));
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x0004DE81 File Offset: 0x0004C081
		internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource)
		{
			if (CompatibilitySwitches.IsAppEarlierThanWindowsPhone8)
			{
				throw new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument), string.Empty);
			}
			throw new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument), Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x0004DEB1 File Offset: 0x0004C0B1
		internal static void ThrowInvalidOperationException(ExceptionResource resource)
		{
			throw new InvalidOperationException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x0004DEC3 File Offset: 0x0004C0C3
		internal static void ThrowSerializationException(ExceptionResource resource)
		{
			throw new SerializationException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x0004DED5 File Offset: 0x0004C0D5
		internal static void ThrowSecurityException(ExceptionResource resource)
		{
			throw new SecurityException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x0004DEE7 File Offset: 0x0004C0E7
		internal static void ThrowNotSupportedException(ExceptionResource resource)
		{
			throw new NotSupportedException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x0004DEF9 File Offset: 0x0004C0F9
		internal static void ThrowUnauthorizedAccessException(ExceptionResource resource)
		{
			throw new UnauthorizedAccessException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0004DF0B File Offset: 0x0004C10B
		internal static void ThrowObjectDisposedException(string objectName, ExceptionResource resource)
		{
			throw new ObjectDisposedException(objectName, Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x0004DF1E File Offset: 0x0004C11E
		internal static void ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion()
		{
			throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0004DF2A File Offset: 0x0004C12A
		internal static void ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen()
		{
			throw new InvalidOperationException("Enumeration has either not started or has already finished.");
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x0004DF36 File Offset: 0x0004C136
		internal static void ThrowInvalidOperationException_InvalidOperation_EnumNotStarted()
		{
			throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x0004DF42 File Offset: 0x0004C142
		internal static void ThrowInvalidOperationException_InvalidOperation_EnumEnded()
		{
			throw new InvalidOperationException("Enumeration already finished.");
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x0004DF4E File Offset: 0x0004C14E
		internal static void ThrowInvalidOperationException_InvalidOperation_NoValue()
		{
			throw new InvalidOperationException("Nullable object must have a value.");
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0004DF5A File Offset: 0x0004C15A
		private static ArgumentOutOfRangeException GetArgumentOutOfRangeException(ExceptionArgument argument, string resource)
		{
			return new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument), resource);
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x0004DF68 File Offset: 0x0004C168
		internal static void ThrowArgumentOutOfRange_IndexException()
		{
			throw ThrowHelper.GetArgumentOutOfRangeException(ExceptionArgument.index, "Index was out of range. Must be non-negative and less than the size of the collection.");
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x0004DF76 File Offset: 0x0004C176
		internal static void ThrowIndexArgumentOutOfRange_NeedNonNegNumException()
		{
			throw ThrowHelper.GetArgumentOutOfRangeException(ExceptionArgument.index, "Non-negative number required.");
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x0004DF84 File Offset: 0x0004C184
		internal static void ThrowArgumentException_Argument_InvalidArrayType()
		{
			throw new ArgumentException("Target array type is not compatible with the type of items in the collection.");
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x0004DF90 File Offset: 0x0004C190
		private static ArgumentException GetAddingDuplicateWithKeyArgumentException(object key)
		{
			return new ArgumentException(SR.Format("An item with the same key has already been added. Key: {0}", key));
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x0004DFA2 File Offset: 0x0004C1A2
		internal static void ThrowAddingDuplicateWithKeyArgumentException(object key)
		{
			throw ThrowHelper.GetAddingDuplicateWithKeyArgumentException(key);
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x0004DFAA File Offset: 0x0004C1AA
		private static KeyNotFoundException GetKeyNotFoundException(object key)
		{
			throw new KeyNotFoundException(SR.Format("The given key '{0}' was not present in the dictionary.", key.ToString()));
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x0004DFC1 File Offset: 0x0004C1C1
		internal static void ThrowKeyNotFoundException(object key)
		{
			throw ThrowHelper.GetKeyNotFoundException(key);
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x0004DFC9 File Offset: 0x0004C1C9
		internal static void ThrowInvalidTypeWithPointersNotSupported(Type targetType)
		{
			throw new ArgumentException(SR.Format("Cannot use type '{0}'. Only value types without pointers or references are supported.", targetType));
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x0004DFDB File Offset: 0x0004C1DB
		internal static void ThrowInvalidOperationException_ConcurrentOperationsNotSupported()
		{
			throw ThrowHelper.GetInvalidOperationException("Operations that change non-concurrent collections must have exclusive access. A concurrent update was performed on this collection and corrupted its state. The collection's state is no longer correct.");
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x0004DFE7 File Offset: 0x0004C1E7
		internal static InvalidOperationException GetInvalidOperationException(string str)
		{
			return new InvalidOperationException(str);
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x0004DFEF File Offset: 0x0004C1EF
		internal static void ThrowArraySegmentCtorValidationFailedExceptions(Array array, int offset, int count)
		{
			throw ThrowHelper.GetArraySegmentCtorValidationFailedException(array, offset, count);
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0004DFF9 File Offset: 0x0004C1F9
		private static Exception GetArraySegmentCtorValidationFailedException(Array array, int offset, int count)
		{
			if (array == null)
			{
				return ThrowHelper.GetArgumentNullException(ExceptionArgument.array);
			}
			if (offset < 0)
			{
				return ThrowHelper.GetArgumentOutOfRangeException(ExceptionArgument.offset, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (count < 0)
			{
				return ThrowHelper.GetArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			return ThrowHelper.GetArgumentException(ExceptionResource.Argument_InvalidOffLen);
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x0004E026 File Offset: 0x0004C226
		private static ArgumentException GetArgumentException(ExceptionResource resource)
		{
			return new ArgumentException(resource.ToString());
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x0004E03A File Offset: 0x0004C23A
		private static ArgumentNullException GetArgumentNullException(ExceptionArgument argument)
		{
			return new ArgumentNullException(ThrowHelper.GetArgumentName(argument));
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x0004E048 File Offset: 0x0004C248
		internal static void IfNullAndNullsAreIllegalThenThrow<T>(object value, ExceptionArgument argName)
		{
			if (value == null && default(T) != null)
			{
				ThrowHelper.ThrowArgumentNullException(argName);
			}
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x0004E070 File Offset: 0x0004C270
		internal static string GetArgumentName(ExceptionArgument argument)
		{
			string text;
			switch (argument)
			{
			case ExceptionArgument.obj:
				text = "obj";
				break;
			case ExceptionArgument.dictionary:
				text = "dictionary";
				break;
			case ExceptionArgument.dictionaryCreationThreshold:
				text = "dictionaryCreationThreshold";
				break;
			case ExceptionArgument.array:
				text = "array";
				break;
			case ExceptionArgument.info:
				text = "info";
				break;
			case ExceptionArgument.key:
				text = "key";
				break;
			case ExceptionArgument.collection:
				text = "collection";
				break;
			case ExceptionArgument.list:
				text = "list";
				break;
			case ExceptionArgument.match:
				text = "match";
				break;
			case ExceptionArgument.converter:
				text = "converter";
				break;
			case ExceptionArgument.queue:
				text = "queue";
				break;
			case ExceptionArgument.stack:
				text = "stack";
				break;
			case ExceptionArgument.capacity:
				text = "capacity";
				break;
			case ExceptionArgument.index:
				text = "index";
				break;
			case ExceptionArgument.startIndex:
				text = "startIndex";
				break;
			case ExceptionArgument.value:
				text = "value";
				break;
			case ExceptionArgument.count:
				text = "count";
				break;
			case ExceptionArgument.arrayIndex:
				text = "arrayIndex";
				break;
			case ExceptionArgument.name:
				text = "name";
				break;
			case ExceptionArgument.mode:
				text = "mode";
				break;
			case ExceptionArgument.item:
				text = "item";
				break;
			case ExceptionArgument.options:
				text = "options";
				break;
			case ExceptionArgument.view:
				text = "view";
				break;
			case ExceptionArgument.sourceBytesToCopy:
				text = "sourceBytesToCopy";
				break;
			default:
				return string.Empty;
			}
			return text;
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x0004E1C9 File Offset: 0x0004C3C9
		private static ArgumentOutOfRangeException GetArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource)
		{
			return new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument), resource.ToString());
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x0004E1E3 File Offset: 0x0004C3E3
		internal static void ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_Index()
		{
			throw ThrowHelper.GetArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x0004E1EE File Offset: 0x0004C3EE
		internal static void ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count()
		{
			throw ThrowHelper.GetArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_Count);
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x0004E1FC File Offset: 0x0004C3FC
		internal static string GetResourceName(ExceptionResource resource)
		{
			string text;
			switch (resource)
			{
			case ExceptionResource.Argument_ImplementIComparable:
				text = "At least one object must implement IComparable.";
				break;
			case ExceptionResource.Argument_InvalidType:
				text = "The type of arguments passed into generic comparer methods is invalid.";
				break;
			case ExceptionResource.Argument_InvalidArgumentForComparison:
				text = "Type of argument is not compatible with the generic comparer.";
				break;
			case ExceptionResource.Argument_InvalidRegistryKeyPermissionCheck:
				text = "The specified RegistryKeyPermissionCheck value is invalid.";
				break;
			case ExceptionResource.ArgumentOutOfRange_NeedNonNegNum:
				text = "Non-negative number required.";
				break;
			case ExceptionResource.Arg_ArrayPlusOffTooSmall:
				text = "Destination array is not long enough to copy all the items in the collection. Check array index and length.";
				break;
			case ExceptionResource.Arg_NonZeroLowerBound:
				text = "The lower bound of target array must be zero.";
				break;
			case ExceptionResource.Arg_RankMultiDimNotSupported:
				text = "Only single dimensional arrays are supported for the requested action.";
				break;
			case ExceptionResource.Arg_RegKeyDelHive:
				text = "Cannot delete a registry hive's subtree.";
				break;
			case ExceptionResource.Arg_RegKeyStrLenBug:
				text = "Registry key names should not be greater than 255 characters.";
				break;
			case ExceptionResource.Arg_RegSetStrArrNull:
				text = "RegistryKey.SetValue does not allow a String[] that contains a null String reference.";
				break;
			case ExceptionResource.Arg_RegSetMismatchedKind:
				text = "The type of the value object did not match the specified RegistryValueKind or the object could not be properly converted.";
				break;
			case ExceptionResource.Arg_RegSubKeyAbsent:
				text = "Cannot delete a subkey tree because the subkey does not exist.";
				break;
			case ExceptionResource.Arg_RegSubKeyValueAbsent:
				text = "No value exists with that name.";
				break;
			case ExceptionResource.Argument_AddingDuplicate:
				text = "An item with the same key has already been added.";
				break;
			case ExceptionResource.Serialization_InvalidOnDeser:
				text = "OnDeserialization method was called while the object was not being deserialized.";
				break;
			case ExceptionResource.Serialization_MissingKeys:
				text = "The Keys for this Hashtable are missing.";
				break;
			case ExceptionResource.Serialization_NullKey:
				text = "One of the serialized keys is null.";
				break;
			case ExceptionResource.Argument_InvalidArrayType:
				text = "Target array type is not compatible with the type of items in the collection.";
				break;
			case ExceptionResource.NotSupported_KeyCollectionSet:
				text = "Mutating a key collection derived from a dictionary is not allowed.";
				break;
			case ExceptionResource.NotSupported_ValueCollectionSet:
				text = "Mutating a value collection derived from a dictionary is not allowed.";
				break;
			case ExceptionResource.ArgumentOutOfRange_SmallCapacity:
				text = "capacity was less than the current size.";
				break;
			case ExceptionResource.ArgumentOutOfRange_Index:
				text = "Index was out of range. Must be non-negative and less than the size of the collection.";
				break;
			case ExceptionResource.Argument_InvalidOffLen:
				text = "Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.";
				break;
			case ExceptionResource.Argument_ItemNotExist:
				text = "The specified item does not exist in this KeyedCollection.";
				break;
			case ExceptionResource.ArgumentOutOfRange_Count:
				text = "Count must be positive and count must refer to a location within the string/array/collection.";
				break;
			case ExceptionResource.ArgumentOutOfRange_InvalidThreshold:
				text = "The specified threshold for creating dictionary is out of range.";
				break;
			case ExceptionResource.ArgumentOutOfRange_ListInsert:
				text = "Index must be within the bounds of the List.";
				break;
			case ExceptionResource.NotSupported_ReadOnlyCollection:
				text = "Collection is read-only.";
				break;
			case ExceptionResource.InvalidOperation_CannotRemoveFromStackOrQueue:
				text = "Removal is an invalid operation for Stack or Queue.";
				break;
			case ExceptionResource.InvalidOperation_EmptyQueue:
				text = "Queue empty.";
				break;
			case ExceptionResource.InvalidOperation_EnumOpCantHappen:
				text = "Enumeration has either not started or has already finished.";
				break;
			case ExceptionResource.InvalidOperation_EnumFailedVersion:
				text = "Collection was modified; enumeration operation may not execute.";
				break;
			case ExceptionResource.InvalidOperation_EmptyStack:
				text = "Stack empty.";
				break;
			case ExceptionResource.ArgumentOutOfRange_BiggerThanCollection:
				text = "Larger than collection size.";
				break;
			case ExceptionResource.InvalidOperation_EnumNotStarted:
				text = "Enumeration has not started. Call MoveNext.";
				break;
			case ExceptionResource.InvalidOperation_EnumEnded:
				text = "Enumeration already finished.";
				break;
			case ExceptionResource.NotSupported_SortedListNestedWrite:
				text = "This operation is not supported on SortedList nested types because they require modifying the original SortedList.";
				break;
			case ExceptionResource.InvalidOperation_NoValue:
				text = "Nullable object must have a value.";
				break;
			case ExceptionResource.InvalidOperation_RegRemoveSubKey:
				text = "Registry key has subkeys and recursive removes are not supported by this method.";
				break;
			case ExceptionResource.Security_RegistryPermission:
				text = "Requested registry access is not allowed.";
				break;
			case ExceptionResource.UnauthorizedAccess_RegistryNoWrite:
				text = "Cannot write to the registry key.";
				break;
			case ExceptionResource.ObjectDisposed_RegKeyClosed:
				text = "Cannot access a closed registry key.";
				break;
			case ExceptionResource.NotSupported_InComparableType:
				text = "A type must implement IComparable<T> or IComparable to support comparison.";
				break;
			case ExceptionResource.Argument_InvalidRegistryOptionsCheck:
				text = "The specified RegistryOptions value is invalid.";
				break;
			case ExceptionResource.Argument_InvalidRegistryViewCheck:
				text = "The specified RegistryView value is invalid.";
				break;
			default:
				return string.Empty;
			}
			return text;
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x0004E49F File Offset: 0x0004C69F
		internal static void ThrowValueArgumentOutOfRange_NeedNonNegNumException()
		{
			throw ThrowHelper.GetArgumentOutOfRangeException(ExceptionArgument.value, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
		}
	}
}
