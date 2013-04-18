using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi.Processing
{
    public class ProcessManager
    {
        private List<Process> m_processesToAttach;
        private List<Process> m_processesToRemove;
        private List<Process> m_processes;

        public ProcessManager()
        {
            m_processesToAttach = new List<Process>();
            m_processesToRemove = new List<Process>();
            m_processes = new List<Process>();
        }

        public void Update(GameTime gameTime)
        {
            m_processes.RemoveAll(p => m_processesToRemove.Contains(p));
            m_processes.AddRange(m_processesToAttach);

            m_processesToRemove.Clear();
            m_processesToAttach.Clear();
            foreach (var process in m_processes)
            {
                if (process.State == ProcessState.Uninitialized)
                {
                    process.Initialize();
                }

                if (process.State == ProcessState.Running)
                {
                    process.Update(gameTime);
                }

                if (process.IsDead && process.State == ProcessState.Succeeded)
                {
                    foreach (var p in process.Children) { AttachProcess(p); }
                }
            }
            m_processes.RemoveAll(p => p.IsDead);
        }

        public void AttachProcess(Process process, bool replaceExistingProcesses = false)
        {
            m_processesToAttach.Add(process);
            if (replaceExistingProcesses)
            {
                m_processesToRemove.AddRange(m_processes.FindAll(p => p.GetType() == process.GetType()));
            }
        }

        public int GetProcessCount()
        {
            return m_processes.Count;
        }
    }
}
