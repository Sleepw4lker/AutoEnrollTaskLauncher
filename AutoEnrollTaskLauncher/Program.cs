using System;
using System.Runtime.InteropServices;
using TaskScheduler;

namespace AutoEnrollTaskLauncher
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            var taskName = "UserTask";
            var taskFlags = (int) _TASK_RUN_FLAGS.TASK_RUN_AS_SELF;

            if (args.Length == 1)
                if (args[0] == "-System")
                {
                    taskName = "SystemTask";
                    taskFlags = (int) _TASK_RUN_FLAGS.TASK_RUN_NO_FLAGS;
                }

            // https://docs.microsoft.com/en-us/windows/win32/api/taskschd/
            var taskScheduler = new TaskScheduler.TaskScheduler();

            try
            {
                // https://docs.microsoft.com/en-us/windows/win32/api/taskschd/nf-taskschd-itaskservice-connect
                taskScheduler.Connect();

                // https://docs.microsoft.com/en-us/windows/win32/api/taskschd/nf-taskschd-itaskservice-getfolder
                var task = taskScheduler.GetFolder("Microsoft\\Windows\\CertificateServicesClient")
                    .GetTask(taskName);

                // Only run the task if it is not already running
                // It might have been triggered at logon and may still be running
                if (task.State == _TASK_STATE.TASK_STATE_READY)
                    // https://docs.microsoft.com/en-us/windows/win32/api/taskschd/nf-taskschd-iregisteredtask-runex
                    task.RunEx(
                        null,
                        taskFlags,
                        0,
                        null
                    );
            }
            finally
            {
                // Clean up the COM Object
                Marshal.ReleaseComObject(taskScheduler);
                GC.Collect();
            }
        }
    }
}