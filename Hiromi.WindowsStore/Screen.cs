using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Hiromi.Processing;
using Hiromi.Systems;

namespace Hiromi
{
    public abstract class Screen
    {
        private List<GameSystem> _systems;

        public void Load()
        {
            _systems = new List<GameSystem>();
            _systems.AddRange(LoadGameSystems());

            foreach (var obj in LoadGameObjects())
            {
                GameObjectService.Instance.AddGameObject(obj);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var sys in _systems)
            {
                sys.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var sys in _systems)
            {
                sys.Draw(gameTime);
            }
        }

        protected abstract List<GameSystem> LoadGameSystems();
        protected abstract List<GameObject> LoadGameObjects();
    }
}
