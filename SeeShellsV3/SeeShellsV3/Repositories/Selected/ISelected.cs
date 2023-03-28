using System;
using System.Collections.Generic;
using System.ComponentModel;

using SeeShellsV3.Data;

namespace SeeShellsV3.Repositories
{
    /// <summary>
    /// A repository that stores selected objects. Selected objects can be set or displayed by any UI element.
    /// </summary>
    public interface ISelected : INotifyPropertyChanged
    {
        object CurrentInspector { get; set; }

        object CurrentData { get; set; }

        Boolean regView { get; set; }
    }
}
