using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Hiromi.Components;
using Hiromi.Processing;

namespace Hiromi.Systems
{
    public class ScreenWrappingSystem : GameSystem
    {
        private List<int> _reflectedObjects;

        public ScreenWrappingSystem ()
	    {
            _reflectedObjects = new List<int>();
	    }

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (var obj in this.GameObjects.Values)
            {
                if (!_reflectedObjects.Contains(obj.Id))
                {
                    var newPosition = Vector2.Zero;
                    var pos = obj.GetComponent<PositionComponent>();

                    // Reflect X-axis
                    if (pos.Bounds.Right < 0.0f)
                    {
                        newPosition = new Vector2(1.0f, pos.Position.Y);
                    }
                    else if (pos.Bounds.Left > 1.0f)
                    {
                        newPosition = new Vector2(-pos.Bounds.Width, pos.Position.Y);
                    }

                    // Reflect Y-axis
                    if (pos.Bounds.Bottom < 0.0f)
                    {
                        newPosition = new Vector2(pos.Position.X, 1.0f);
                    }
                    else if (pos.Bounds.Top > 1.0f)
                    {
                        newPosition = new Vector2(pos.Position.X, -pos.Bounds.Height);
                    }

                    if (newPosition != Vector2.Zero)
                    {
                        pos.Position = newPosition;
                        _reflectedObjects.Add(obj.Id);

                        // Make sure we don't keep resetting ourselves by introducing a delay
                        this.ProcessManager.AttachProcess(new DelayProcess(TimeSpan.FromSeconds(1),
                            new ActionProcess(() => { _reflectedObjects.Remove(obj.Id); })));
                    }
                }
            }
        }

        protected override bool IsGameObjectForSystem(GameObject obj)
        {
            return obj.HasComponent<PositionComponent>() && obj.HasComponent<ScreenWrappingComponent>();
        }
    }
}
