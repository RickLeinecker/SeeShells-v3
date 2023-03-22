using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Unity;

using SeeShellsV3.Data;
using SeeShellsV3.Factories;
using SeeShellsV3.Repositories;
using System.Diagnostics;

namespace SeeShellsV3.Services
{
    public class ShellEventManager : IShellEventManager
    {
        public event EventHandler ShellEventGenerateBegin;
        public event EventHandler ShellEventGenerateEnd;

        private IShellEventFactory ShellEventFactory { get; set; }
        private IShellEventCollection ShellEvents { get; set; }

        public ShellEventManager(
            [Dependency] IShellEventFactory shellEventFactory,
            [Dependency] IShellEventCollection shellEvents
            )
        {
            ShellEventFactory = shellEventFactory;
            ShellEvents = shellEvents;
        }

        public IEnumerable<IShellEvent> GenerateEvents(IEnumerable<IShellItem> shellItems)
        {
            if (ShellEvents.Count != 0)
            {
                ShellEvents.updating = true;
                foreach (ShellEvent e in ShellEvents.ToList())
                {
                    ShellEvents.Remove(e);
                }
            }
            ShellEventGenerateBegin?.Invoke(this, EventArgs.Empty);
            IList<IIntermediateShellEvent> intermediateShellEvents = new List<IIntermediateShellEvent>();
            IList<IShellEvent> generatedEvents = new List<IShellEvent>();

            // extract timestamps and generate intermediate events
            foreach (var shellItem in shellItems)
            {
                var i = ShellEventFactory.CreateIntermediateEvents(shellItem);
                foreach (var shellEvent in i)
                {
                    intermediateShellEvents.Add(shellEvent);
                }
            }

            // sort intermediate events by time stamp
            intermediateShellEvents = intermediateShellEvents.OrderBy(e => e.TimeStamp).ToList();

            // generate full events from intermediate events
            var f = ShellEventFactory.CreateEvents(intermediateShellEvents);
            foreach (var shellEvent in f)
            {
                ShellEvents.Add(shellEvent);
                generatedEvents.Add(shellEvent);
            }

            // display any intermediate events that were not aggregated
            // into one or more full events
            foreach (var shellEvent in intermediateShellEvents)
            {
                if (!shellEvent.Consumed)
                {
                    ShellEvents.Add(shellEvent);
                    generatedEvents.Add(shellEvent);
                }
            }

            ShellEventGenerateEnd?.Invoke(this, EventArgs.Empty);
            ShellEvents.updating = false;
            return generatedEvents;
        }
    }
}
