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
            var name = "Player";

            var guidBytes = Guid.NewGuid().ToByteArray();
            for (int i = 0; i < 5; i++)
            {
                name += guidBytes[i].ToString();
            }

            return name;
        }
    }
}
