using System;
using JetBrains.Annotations;

namespace UsbInfo.Natives
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal class PnpConfigrationException : Exception
    {
        public PnpConfigrationResult PnpConfigrationResult { get; }

        public PnpConfigrationException(PnpConfigrationResult result)
        {
            PnpConfigrationResult = result;
        }
    }
}