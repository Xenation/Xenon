using System.Collections.Generic;

namespace Xenon.Processes {
	public class CompositeProcess : Process {

		private ProcessManager manager;
		private LinkedList<Process> processes;

		public CompositeProcess() {
			manager = new ProcessManager();
			processes = new LinkedList<Process>();
		}

		public override void OnBegin() {
			if (processes.Count != 0) {
				manager.LaunchProcess(processes.First.Value);
			} else {
				Terminate();
			}
		}

		public override void Update(float dt) {
			manager.UpdateProcesses(dt);
		}

		public override void OnTerminate() {
			
		}

		public void AddLast(Process process) {
			if (processes.Count != 0) {
				processes.Last.Value.Attach(process);
				processes.Last.Value.TerminateCallback -= OnLastProcessEnded;
			}
			processes.AddLast(process);
			process.TerminateCallback += OnLastProcessEnded;
		}

		public void AddFirst(Process process) {
			if (State != ProcessState.CREATED) return;
			if (processes.Count == 0) {
				process.TerminateCallback += OnLastProcessEnded;
			} else {
				process.Attach(processes.First.Value);
			}
			processes.AddFirst(process);
		}

		protected virtual void OnLastProcessEnded() {
			Terminate();
		}

	}
}
