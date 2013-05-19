using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Hiromi
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
            foreach (var process in m_processesToRemove)
            {
                m_processes.Remove(process);
            }
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

            foreach (var process in m_processes.ToList())
            {
                if (process.IsDead)
                {
                    m_processes.Remove(process);
                }
            }
        }

        public void AttachProcess(Process process) { AttachProcess(process, false); }
        public void AttachProcess(Process process, bool replaceExistingProcesses)
        {
            //System.Diagnostics.Debug.WriteLine("[{0}] Attaching '{1}' process", process.GetType().Name, process.ToString());

            m_processesToAttach.Add(process);
            if (replaceExistingProcesses)
            {
                foreach (var p in m_processes)
                {
                    if (p.GetType() == process.GetType())
                    {
                        m_processesToRemove.Add(p);
                    }
                }
            }
        }

        public void RemoveProcess(Process process)
        {
            m_processes.Remove(process);
        }

        public void RemoveAllProcesses()
        {
            m_processes.Clear();
        }

        public int GetProcessCount()
        {
            return m_processes.Count;
        }
    }
}
