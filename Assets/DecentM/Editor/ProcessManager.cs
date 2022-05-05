﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace DecentM.EditorTools
{
    public struct ProcessResult
    {
        public string stdout;
        public string stderr;
    }

    public enum BlockingBehaviour
    {
        Blocking,
        NonBlocking,
    }

    public static class ProcessManager
    {
        private static IEnumerator ProcessCoroutine(Process process, BlockingBehaviour blocking, Action<Process> callback)
        {
            StreamReader read = process.StandardOutput;

            if (blocking == BlockingBehaviour.Blocking)
            {
                while (read.Peek() == 0)
                    yield return new WaitForSeconds(0.1f);
            }
            else
            {
                while (!process.HasExited)
                    yield return new WaitForSeconds(0.1f);
            }

            callback(process);
            read.Close();
        }

        private static IEnumerator WaitForTask(Task task)
        {
            while (!task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                yield return new WaitForSeconds(.15f);

            yield return task;
        }

        private static Process CreateProcess(string filename, string arguments, string workdir)
        {
            Process process = new Process
            {
                StartInfo =
                {
                    FileName = filename,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    WorkingDirectory = workdir,
                },

                EnableRaisingEvents = true
            };

            return process;
        }

        public static IEnumerator CreateProcessCoroutine(string filename, string arguments, string workdir, BlockingBehaviour blocking, Action<Process> callback)
        {
            Process process = CreateProcess(filename, arguments, workdir);
            process.Start();
            return ProcessCoroutine(process, blocking, callback);
        }

        public static EditorCoroutines.EditorCoroutine RunProcessCoroutine(string filename, string arguments, string workdir, BlockingBehaviour blocking, Action<Process> callback)
        {
            IEnumerator coroutine = CreateProcessCoroutine(filename, arguments, workdir, blocking, callback);
            return EditorCoroutines.StartCoroutine(coroutine, typeof(ProcessManager));
        }

        public static void RunProcessAsync(string filename, string arguments, string workdir, Action<ProcessResult> callback)
        {
            RunProcessAsync(filename, arguments, workdir, 15000, callback);
        }

        public static void RunProcessAsync(string filename, string arguments, Action<ProcessResult> callback)
        {
            RunProcessAsync(filename, arguments, ".", 15000, callback);
        }

        public static void RunProcessAsync(string filename, string arguments, int timeout, Action<ProcessResult> callback)
        {
            RunProcessAsync(filename, arguments, ".", timeout, callback);
        }

        public static Task<ProcessResult> RunProcessAsync(string filename, string arguments, string workdir)
        {
            var tcs = new TaskCompletionSource<ProcessResult>();
            Process process = CreateProcess(filename, arguments, workdir);
            ProcessResult result = new ProcessResult();

            StringBuilder output = new StringBuilder();
            StringBuilder error = new StringBuilder();

            process.Exited += (sender, args) =>
            {
                result.stdout = output.ToString();
                result.stderr = error.ToString();

                tcs.SetResult(result);
                process.Dispose();
            };

            process.Start();

            using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
            using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
            {
                process.OutputDataReceived += (sender, e) => {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        output.AppendLine(e.Data);
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        errorWaitHandle.Set();
                    }
                    else
                    {
                        error.AppendLine(e.Data);
                    }
                };

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }

            return tcs.Task;
        }

        public static void RunProcessAsync(string filename, string arguments, string workdir, int timeout, Action<ProcessResult> callback)
        {
            Task.Run(() =>
            {
                ProcessResult result = RunProcessSync(filename, arguments, workdir, timeout);
                callback(result);
            });
        }

        public static ProcessResult RunProcessSync(string filename, string arguments, string workdir)
        {
            return RunProcessSync(filename, arguments, workdir, 15000);
        }

        public static ProcessResult RunProcessSync(string filename, string arguments, string workdir, int timeout)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = filename;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WorkingDirectory = workdir;

                StringBuilder output = new StringBuilder();
                StringBuilder error = new StringBuilder();

                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, e) => {
                        if (e.Data == null)
                        {
                            outputWaitHandle.Set();
                        }
                        else
                        {
                            output.AppendLine(e.Data);
                        }
                    };
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            errorWaitHandle.Set();
                        }
                        else
                        {
                            error.AppendLine(e.Data);
                        }
                    };

                    process.Start();

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    ProcessResult result = new ProcessResult();

                    if (process.WaitForExit(timeout) &&
                        outputWaitHandle.WaitOne(timeout) &&
                        errorWaitHandle.WaitOne(timeout))
                    {
                        result.stdout = output.ToString();
                        result.stderr = error.ToString();

                        return result;
                    }
                    else
                    {
                        result.stdout = output.ToString();
                        result.stderr = error.ToString();

                        return result;
                    }
                }
            }
        }
    }
}
