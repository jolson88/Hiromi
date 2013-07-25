using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public class ProcessManager
    {
        private List<Process> _processesToAttach;
        private List<Process> _processesToRemove;
        private List<Process> _currentProcesses;

        public ProcessManager()
        {
            _processesToAttach = new List<Process>();
            _processesToRemove = new List<Process>();
            _currentProcesses = new List<Process>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var process in _processesToRemove)
            {
                _currentProcesses.Remove(process);
            }
            _currentProcesses.AddRange(_processesToAttach);
            _processesToRemove.Clear();
            _processesToAttach.Clear();

            foreach (var process in _currentProcesses)
            {
                if (process.State == ProcessState.Uninitialized)
                {
                    process.Initialize();
                }

                if (process.State == ProcessState.Running)
                {
                    process.Update(gameTime);
                }

                if (process.IsDead)
                {
                    _processesToRemove.Add(process);
                    if(process.State == ProcessState.Succeeded)
                    {
                        foreach (var p in process.Children) { AttachProcess(p); }
                    }
                }
            }
        }

        public void AttachProcess(Process process) { AttachProcess(process, false); }
        public void AttachProcess(Process process, bool replaceExistingProcesses)
        {
            _processesToAttach.Add(process);
            if (replaceExistingProcesses)
            {
                foreach (var p in _currentProcesses)
                {
                    if (p.GetType() == process.GetType())
                    {
                        _processesToRemove.Add(p);
                    }
                }
            }
        }

        public void RemoveProcess(Process process)
        {
            _currentProcesses.Remove(process);
        }

        public void RemoveAllProcesses()
        {
            _currentProcesses.Clear();
        }

        public int GetProcessCount()
        {
            return _currentProcesses.Count;
        }
    }
}
