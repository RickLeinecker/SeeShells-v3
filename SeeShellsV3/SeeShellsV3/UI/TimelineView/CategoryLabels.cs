using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV3.UI
{
    public class CategoryLabels : OxyPlot.Axes.CategoryAxis
    {
        public void update(int numOfCategories)
        {
            this.UpdateLabels(numOfCategories);
        }
    }
}
