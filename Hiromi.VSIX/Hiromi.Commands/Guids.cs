// Guids.cs
// MUST match guids.h
using System;

namespace CodingCoda.Hiromi_Commands
{
    static class GuidList
    {
        public const string guidHiromi_CommandsPkgString = "39711f3e-366f-463a-8f46-79b5e57fe1a8";
        public const string guidHiromi_CommandsCmdSetString = "87429ad7-ba98-4b92-a95a-b9a165bbf70a";

        public static readonly Guid guidHiromi_CommandsCmdSet = new Guid(guidHiromi_CommandsCmdSetString);
    };
}