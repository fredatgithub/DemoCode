﻿// Copyright 2019 Jon Skeet. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Windows.Controls;

namespace VDrumExplorer.Wpf
{
    internal class TextBlockLogger : ILogger
    {
        private readonly TextBlock block;

        internal TextBlockLogger(TextBlock block) =>
            this.block = block;

        public IDisposable BeginScope<TState>(TState state) => NoOpDisposable.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        private void Log(string text) =>
            block.Text += $"{DateTime.Now:HH:mm:ss.fff} {text}\r\n";

        private void Log(string message, Exception e)
        {
            // TODO: Aggregate exception etc.
            Log($"{message}: {e}");
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);
            if (exception is null)
            {
                Log(message);
            }
            else
            {
                Log(message, exception);
            }
        }

        public void SaveLog(string file) => File.WriteAllText(file, block.Text);

        private class NoOpDisposable : IDisposable
        {
            internal static NoOpDisposable Instance { get; } = new NoOpDisposable();

            public void Dispose()
            {
            }
        }
    }
}
