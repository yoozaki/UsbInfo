using System;
using JetBrains.Annotations;

namespace UsbInfo.Natives
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal class CrException : Exception
    {
        public CrResult CrResult { get; }

        public CrException(CrResult result)
        {
            CrResult = result;
        }
    }
}