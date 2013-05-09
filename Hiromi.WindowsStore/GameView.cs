using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public enum GameViewKind
    {
        Human,
        Remote,
        AI,
        Recorder,
        Other
    }

    public interface IGameView
    {
        GameViewKind GetKind();
        void Draw(GameTime gameTime);
        void Update(GameTime gameTime);
        void Initialize(GameObjectManager gameObjectManager, MessageManager messageManager);
        void OnLoaded();
    }
}
