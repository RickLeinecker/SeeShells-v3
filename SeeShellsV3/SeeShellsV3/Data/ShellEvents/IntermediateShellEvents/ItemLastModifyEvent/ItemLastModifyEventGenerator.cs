using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV3.Data
{
    public class ItemLastModifyEventGenerator : IIntermediateShellEventGenerator
    {
        public bool CanGenerate(IShellItem item)
        {
            return item is IModifiedTimestamp modified && modified.ModifiedDate != DateTime.MinValue;
        }

        public IEnumerable<IIntermediateShellEvent> Generate(IShellItem item)
        {
            if (!CanGenerate(item))
                yield break;


            IModifiedTimestamp modified = item as IModifiedTimestamp;

            yield return new ItemLastModifyEvent()
            {
                TypeName = "Item Last Modify",
                Description = string.Format("{0} Last Modified", modified.Place.Name),
                TimeStamp = modified.ModifiedDate,
                User = modified.RegistryHive.User,
                Place = modified.Place,
                Evidence = new ObservableCollection<IShellItem>() { modified }
            };
        }
    }
}
