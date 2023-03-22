using System.ComponentModel;
using SeeShellsV3.Data;

namespace SeeShellsV3.Repositories
{
    /// <summary>
    /// A collection that holds ShellEvents to be used in specific PDF modules.
    /// </summary>
    public interface IReportEventCollection
    {
        /// <summary>
        /// Collection that stores the selected ShellEvents.
        /// </summary>
        IShellEventCollection SelectedEvents { get; }

        /// <summary>
        /// Boolean that signifies whether the collection is empty or not.
        /// </summary>
        bool HasEvents { get; set; }

        /// <summary>
        /// Adds an event to the collection.
        /// </summary>
        /// <param name="shellEvent">Event to be added to the collection.</param>
        /// <returns>Boolean to signify whether the operation was successful.</returns>
        bool Add(IShellEvent shellEvent);

        /// <summary>
        /// Removes an event from the collection.
        /// </summary>
        /// <param name="shellEvent">Event to be removed from the collection.</param>
        /// <returns>Boolean to signify whether the operation was successful.</returns>
        bool Remove(IShellEvent shellEvent);

        /// <summary>
        /// Checks to see if a given Shell Event is already in the collection.
        /// </summary>
        /// <param name="shellEvent">Event to check if it is in the collection.</param>
        /// <returns>Boolean to signify whether the event is in the collection or not.</returns>
        bool Contains(IShellEvent shellEvent);

        /// <summary>
        /// Given a Shell Event, inserts it into the collection if it is not present, or removes it from
        /// the collection if it is present.
        /// </summary>
        /// <param name="shellEvent">Event to potentially add or remove.</param>
        /// <returns>True to signify that the event was added or removed, false if the event could not be added or removed. </returns>
        bool Decide(IShellEvent shellEvent);
    }
}