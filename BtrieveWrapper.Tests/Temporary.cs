using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Tests
{
    public class Temporary : IDisposable
    {
        string _directory;

        public Temporary(string name) {
            _directory=Path.Combine(Settings.TemporaryDirectory,name);
            try {
                System.IO.Directory.Delete(_directory, true);
            } catch { }
            if (!System.IO.Directory.Exists(_directory)) {
                System.IO.Directory.CreateDirectory(_directory);
            }
        }

        public void Dispose() {
            try {
                System.IO.Directory.Delete(_directory, true);
            } catch { }
        }

        public string Directory { get { return _directory; } }
        public string GetPath(string fileName) {
            return Path.Combine(_directory, fileName);
        }
    }
}
