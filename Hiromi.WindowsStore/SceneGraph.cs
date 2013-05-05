using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi.Components;

namespace Hiromi
{
    public enum RenderPass
    {
        FirstPass = 0,
        BackgroundPass = 1,
        GameObjectPass = 2,
        UserInterfacePass = 3,
        LassPass = 4
    }

    public class SceneGraph
    {
        public bool DebugVisuals { get; set; }
 
        private SpriteBatch _spriteBatch;
        private SpriteBatch _nonTransformedSpriteBatch;
        private MessageManager _messageManager;
        private Dictionary<int, List<IRenderAwareComponent>> _gameObjectLookup;
        private Camera _camera;
        private Dictionary<RenderPass, List<IRenderAwareComponent>> _renderComponents;

        public SceneGraph(MessageManager messageManager)
        {
            this.DebugVisuals = false;
            this._spriteBatch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);
            this._nonTransformedSpriteBatch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);

            _messageManager = messageManager;
            _gameObjectLookup = new Dictionary<int, List<IRenderAwareComponent>>();

            _renderComponents = new Dictionary<RenderPass, List<IRenderAwareComponent>>();
            foreach (var val in Enum.GetValues(typeof(RenderPass)))
            {
                _renderComponents.Add((RenderPass)val, new List<IRenderAwareComponent>());
            }

            _messageManager.AddListener<GameObjectLoadedMessage>(OnGameObjectLoaded);
            _messageManager.AddListener<GameObjectRemovedMessage>(OnGameObjectRemoved);

            _camera = new Camera(_messageManager);
        }

        public void AddComponent(IRenderAwareComponent component)
        {
            _renderComponents[component.RenderPass].Add(component);
            
            if (!_gameObjectLookup.ContainsKey(component.GameObjectId))
            {
                _gameObjectLookup.Add(component.GameObjectId, new List<IRenderAwareComponent>());
            }
            _gameObjectLookup[component.GameObjectId].Add(component);
        }

        public void RemoveComponent(IRenderAwareComponent component)
        {
            System.Diagnostics.Debug.Assert(_gameObjectLookup.Keys.Contains(component.GameObjectId));

            foreach (var val in Enum.GetValues(typeof(RenderPass)))
            {
                _renderComponents[(RenderPass)val].Remove(component);
            }
            _gameObjectLookup[component.GameObjectId].Remove(component);
        }

        public void Draw(GameTime gameTime)
        {
            // When we transform via camera, we need to flip rasterizer (counter clockwise) since we are flipping Y component to Y+ up (instead of down)
            this._spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, RasterizerState.CullCounterClockwise, null, _camera.TransformationMatrix);
            this._nonTransformedSpriteBatch.Begin();

            // Draw the scene in render pass order
            for (int i = 0; i <= (int)RenderPass.LassPass; i++)
            {
                foreach (var component in _renderComponents[(RenderPass)i])
                {
                    var batch = GetSpriteBatchForComponent(component);
                    component.Draw(gameTime, batch);

                    if (DebugVisuals && i > (int)RenderPass.BackgroundPass)
                    {
                        DrawDebugVisuals(batch, component, gameTime);
                    }
                }
            }

            this._nonTransformedSpriteBatch.End();
            this._spriteBatch.End();


            // Draw debug visuals if we need to
            if (DebugVisuals)
            {
                this._spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, RasterizerState.CullCounterClockwise, null, _camera.TransformationMatrix);
                this._nonTransformedSpriteBatch.Begin();

                // Draw the scene in render pass order
                for (int i = (int)RenderPass.GameObjectPass; i <= (int)RenderPass.LassPass; i++)
                {
                    foreach (var component in _renderComponents[(RenderPass)i])
                    {
                        DrawDebugVisuals(GetSpriteBatchForComponent(component), component, gameTime);
                    }
                }

                this._nonTransformedSpriteBatch.End();
                this._spriteBatch.End();
            }
        }

        public bool Pick(Vector2 pointerLocation, ref int? gameObjectId)
        {
            // Account for camera transformation;
            var transformedPointer = Vector2.Transform(pointerLocation, Matrix.Invert(_camera.TransformationMatrix));

            // Reverse drawing order to find top-most game object picked
            for (int i = (int)RenderPass.LassPass; i >= (int)RenderPass.GameObjectPass; i--)
            {
                foreach (var component in _renderComponents[(RenderPass)i])
                {
                    if (PointerOverComponent(component, transformedPointer))
                    {
                        gameObjectId = component.GameObjectId;
                        return true;
                    }
                }
            }
            return false;
        }

        private void DrawDebugVisuals(SpriteBatch batch, IRenderAwareComponent component, GameTime gameTime)
        {
            var topLeft = new Vector2(component.Transform.Bounds.Left, component.Transform.Bounds.Top);
            var bottomLeft = new Vector2(component.Transform.Bounds.Left, component.Transform.Bounds.Bottom);
            var topRight = new Vector2(component.Transform.Bounds.Right, component.Transform.Bounds.Top);
            var bottomRight = new Vector2(component.Transform.Bounds.Right, component.Transform.Bounds.Bottom);

            batch.DrawLine(2f, Color.Fuchsia, topLeft, topRight);
            batch.DrawLine(2f, Color.Fuchsia, topRight, bottomRight);
            batch.DrawLine(2f, Color.Fuchsia, bottomRight, bottomLeft);
            batch.DrawLine(2f, Color.Fuchsia, bottomLeft, topLeft);
        }

        private bool PointerOverComponent(IRenderAwareComponent component, Vector2 pointerLocation)
        {
            // Need to convert pixel coordinates from mouse into screen coordinates
            if (component.Transform != null)
            {
                return component.Transform.Bounds.Contains((float)pointerLocation.X, (float)pointerLocation.Y);
            }
            return false;
        }

        private SpriteBatch GetSpriteBatchForComponent(IRenderAwareComponent component)
        {
            if (component.RenderPass == RenderPass.BackgroundPass)
            {
                return _nonTransformedSpriteBatch;
            }
            else
            {
                return _spriteBatch;
            }
        }

        private void OnGameObjectLoaded(GameObjectLoadedMessage msg)
        {
            var cameraAware = msg.GameObject.GetComponentWithAwareness<ICameraAwareComponent>();
            if (cameraAware != null)
            {
                cameraAware.ActiveCamera = _camera;
            }

            var renderAware = msg.GameObject.GetComponentWithAwareness<IRenderAwareComponent>();
            if (renderAware != null)
            {
                AddComponent(renderAware);
            }
        }

        private void OnGameObjectRemoved(GameObjectRemovedMessage msg)
        {
            if (_gameObjectLookup.ContainsKey(msg.GameObjectId))
            {
                // .ToList() to bring in local copy as we will be removing from the list we are iterating over
                foreach (var node in _gameObjectLookup[msg.GameObjectId].ToList())
                {
                    RemoveComponent(node);
                }
            }
        }
    }
}
