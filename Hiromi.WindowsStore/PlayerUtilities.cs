using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiromi
{
    public static class PlayerUtilities
    {
        public static string GetRandomPlayerName()
        {
            var playerName = "Player";

            var guidBytes = Guid.NewGuid().ToByteArray();
            for (int i = 0; i < 5; i++)
            {
                playerName += guidBytes[i].ToString();
            }

            return playerName;
        }
    }
}
