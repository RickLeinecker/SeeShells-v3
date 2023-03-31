using System;
using System.ComponentModel;
using System.Windows.Data;
using SeeShellsV3.Data;
using SeeShellsV3.Utilities;

namespace SeeShellsV3.Repositories;

public class TimezoneCollection: ObservableSortedList<ITimezone>, ITimezoneCollection
{
    public event FilterEventHandler Filter;
    public Boolean updating { get; set; }
    public ICollectionView FilteredView => collectionViewSource.View;

    public TimezoneCollection()
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

        Filter += (object o, FilterEventArgs args) => args.Accepted = args.Accepted;
    }

    private readonly CollectionViewSource collectionViewSource = new CollectionViewSource();
}