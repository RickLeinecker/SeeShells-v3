using System;
using System.ComponentModel;
using System.Windows.Data;
using SeeShellsV3.Data;

namespace SeeShellsV3.Repositories
{
    public interface ITimezoneCollection: IDataRepository<ITimezone>
    {
        /// <summary>
        /// An event that is fired for each item in this collection when viewed using <see cref="FilteredView"/>.
        /// The handlers of this event decide whether a particular ITimezone is visible or not.
        /// </summary>
        event FilterEventHandler Filter;

        /// <summary>
        /// A view of this collection that has been filtered by the <see cref="Filter"/> event handlers.
        /// </summary>
        ICollectionView FilteredView { get; }

        Boolean updating { get; set; }
    }
}