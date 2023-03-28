
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using SeeShellsV3.Data;
using SeeShellsV3.Utilities;
using System.Windows.Data;
using System.ComponentModel;

namespace SeeShellsV3.Repositories
{
    public class ShellItemCollection : ObservableSortedList<IShellItem>, IShellItemCollection
    {
        public event FilterEventHandler Filter;

        public ICollectionView FilteredView => collectionViewSource.View;

        public ShellItemCollection()
        {
            collectionViewSource.Source = this;
            collectionViewSource.Filter += (o, e) =>
            {
                foreach (var callback in Filter?.GetInvocationList())
                {
                    callback.DynamicInvoke(o, e);

                    if (!e.Accepted) break;
                }
            };
        }

        private readonly CollectionViewSource collectionViewSource = new CollectionViewSource();

    }
}
