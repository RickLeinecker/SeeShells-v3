using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV3.UI
{
    public class CategoryLabels : OxyPlot.Axes.CategoryAxis
    {
        // Allows the labels to be updated for the Heat Map.
        public void update(int numOfCategories)
        {
            this.UpdateLabels(numOfCategories);
        }
    }
}
