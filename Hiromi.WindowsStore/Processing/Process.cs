﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi.Processing
{
    public class Process
    {
        private ProcessState _state;
        private List<Process> _children;

        public ProcessState State { get { return _state; } }
        public bool IsAlive
        {
            get
            {
                return _state == ProcessState.Running || _state == ProcessState.Paused;
            }
        }
        public bool IsDead
        {
            get
            {
                return _state == ProcessState.Succeeded || _state == ProcessState.Failed || _state == ProcessState.Aborted;
            }
        }
        public bool IsRemoved { get { return _state == ProcessState.Removed; } }
        public bool IsPaused { get { return _state == ProcessState.Paused; } }
        public List<Process> Children { get { return _children; } }

        public Process()
        {
            _children = new List<Process>();
            _state = ProcessState.Uninitialized;
        }

        public void Initialize()
        {
            OnInitialize();
            _state = ProcessState.Running;
        }

        public void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }

        public void Succeed()
        {
            _state = ProcessState.Succeeded;
        }

        public void Fail()
        {
            _state = ProcessState.Failed;
        }

        public void Pause()
        {
            _state = ProcessState.Paused;
        }

        public void Resume()
        {
            _state = ProcessState.Running;
        }

        public void AttachChild(Process child)
        {
            _children.Add(child);
        }

        public void RemoveChildren()
        {
            _children.Clear();
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
    }
}
