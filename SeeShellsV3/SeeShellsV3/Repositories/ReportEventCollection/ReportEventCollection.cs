using System;
using System.ComponentModel;
using SeeShellsV3.Data;

namespace SeeShellsV3.Repositories
{
    public class ReportEventCollection: IReportEventCollection
    {
        public IShellEventCollection SelectedEvents { get; } = new ShellEventCollection();
        public bool HasEvents { get; set; }
        public bool Add(IShellEvent shellEvent)
        {
            if (shellEvent == null)
            {
                return false;
            }

            try
            {
                SelectedEvents.Add(shellEvent);
                HasEvents = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        public bool Remove(IShellEvent shellEvent)
        {
            if (shellEvent == null || !HasEvents)
            {
                return false;
            }

            try
            {
                bool success = SelectedEvents.Remove(shellEvent);

                if (SelectedEvents.Count == 0)
                {
                    HasEvents = false;
                }

                return success;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool Contains(IShellEvent shellEvent)
        {
            return SelectedEvents.Contains(shellEvent);
        }

        public bool Decide(IShellEvent shellEvent)
        {
            if (Contains(shellEvent))
            {
                 return Remove(shellEvent);
            }
            else
            {
                return Add(shellEvent);
            }
        }
    }
}