using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeShellsV3.Events;
using Unity;
using SeeShellsV3.Repositories;
using SeeShellsV3.Services;

namespace SeeShellsV3.UI
{
    public class InspectorViewVM : ViewModel, IInspectorViewVM
    {
        [Dependency] public ISelected Selected { get; set; }

    }
}
