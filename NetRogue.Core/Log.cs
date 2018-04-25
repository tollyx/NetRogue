using System;
using System.Collections.Generic;
using System.Linq;

namespace NetRogue.Core {
    public class Log : ILogger {
        private static ILogger singleton;

        private List<string> messages;

        public List<string> Messages { get => messages; }

        public Log() {
            if (singleton == null) {
                singleton = this;
            }
            messages = new List<string>();
        }

        public void Add(string msg) {
            messages.Add(msg);
        }

        public IEnumerable<string> GetLast(int amount = 1) {
            return messages.Skip(Math.Max(0, messages.Count - amount));
        }

        public static void SetLogger(ILogger logger) {
            singleton = logger;
        } 

        public static void Message(string msg) {
            if (singleton != null) {
                singleton.Add(msg);
            }
        }
    }
}