﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using SeeShellsV3.Data;
using SeeShellsV3.Utilities;

namespace SeeShellsV3.Repositories
{
    public class ShellEventCollection : ObservableSortedList<IShellEvent>, IShellEventCollection
    {
        public event FilterEventHandler Filter;

        public Boolean updating { get; set; }
        public ICollectionView FilteredView => collectionViewSource.View;

        public ShellEventCollection()
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

            Filter += (object o, FilterEventArgs args) => args.Accepted = !(args.Item is IIntermediateShellEvent e) || !e.Consumed;
        }

        private readonly CollectionViewSource collectionViewSource = new CollectionViewSource();
    }
}
