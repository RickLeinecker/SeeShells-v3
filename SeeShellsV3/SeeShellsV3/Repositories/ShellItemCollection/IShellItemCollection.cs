using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeShellsV3.Data;
using System.Windows.Data;

namespace SeeShellsV3.Repositories
{
    /// <summary>
    /// A collection of IShellItem objects. Services and ViewModels can get a reference to this collection
    /// by declaring an appropriate constructor or property dependency.
    /// </summary>
    public interface IShellItemCollection : IDataRepository<IShellItem>
    {
        event FilterEventHandler Filter;
        ICollectionView FilteredView { get; }
    }
}