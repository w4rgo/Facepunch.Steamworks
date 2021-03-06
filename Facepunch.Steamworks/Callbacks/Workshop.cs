﻿using System.Runtime.InteropServices;
using Facepunch.Steamworks.Interop;

namespace Facepunch.Steamworks.Callbacks.Workshop
{

    [StructLayout( LayoutKind.Explicit )]
    internal class ItemInstalled
    {
        [FieldOffset(0)]
        public uint AppId;
        [FieldOffset(4)]
        public ulong FileId;

        public const int CallbackId = Index.UGC + 5;
    };

    [StructLayout( LayoutKind.Explicit )]
    internal class DownloadResult
    {
        [FieldOffset(0)]
        public uint AppId;
        [FieldOffset(4)]
        public ulong FileId;
        [FieldOffset(12)]
        public Result Result;

        public const int CallbackId = Index.UGC + 6;
    };

    internal class QueryCompleted : CallResult<QueryCompleted.Data>
    {
        public override int CallbackId { get { return Index.UGC + 1; } }

        [StructLayout( LayoutKind.Sequential )]
        internal struct Data
        {
            internal ulong Handle;
            internal int Result;
            internal uint m_unNumResultsReturned;
            internal uint m_unTotalMatchingResults;
            [MarshalAs(UnmanagedType.I1)]
            internal bool m_bCachedData; // indicates whether this data was retrieved from the local on-disk cache
        };
    }

    internal class CreateItem : CallResult<CreateItem.Data>
    {
        public override int CallbackId { get { return Index.UGC + 3; } }

        [StructLayout( LayoutKind.Sequential )]
        internal struct Data
        {
            internal Result Result;
            internal ulong FileId;
            [MarshalAs(UnmanagedType.I1)]
            internal bool NeedsLegalAgreement; 
        };
    }

    internal class SubmitItemUpdate : CallResult<SubmitItemUpdate.Data>
    {
        public override int CallbackId { get { return Index.UGC + 4; } }

        [StructLayout( LayoutKind.Sequential )]
        internal struct Data
        {
            internal Result Result;
            [MarshalAs(UnmanagedType.I1)]
            internal bool NeedsLegalAgreement;
        };
    }
}
