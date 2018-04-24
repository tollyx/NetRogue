using System.Collections.Generic;

namespace NetRogue.Core {
    public interface ILogger {
        List<string> Messages { get; }

        void Add(string msg);
    }
}