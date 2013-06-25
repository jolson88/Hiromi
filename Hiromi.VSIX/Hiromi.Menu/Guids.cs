// Guids.cs
// MUST match guids.h
using System;

namespace CodingCoda.Hiromi_Menu
{
    static class GuidList
    {
        public const string guidHiromi_MenuPkgString = "4263d0e4-b41b-4924-aa23-9d1f6227c9bc";
        public const string guidHiromi_MenuCmdSetString = "5cd31d00-f4f6-4a78-afc6-f4eb13b8a215";

        public static readonly Guid guidHiromi_MenuCmdSet = new Guid(guidHiromi_MenuCmdSetString);
    };
}