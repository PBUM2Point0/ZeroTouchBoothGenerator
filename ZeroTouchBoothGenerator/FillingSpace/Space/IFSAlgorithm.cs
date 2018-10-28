using Autodesk.DesignScript.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroTouchBoothGenerator.FillingSpace.Space
{
    [IsVisibleInDynamoLibrary(false)]
    public interface IFSAlgorithm
    {
        IEnumerable<IEnumerable<FS_Rectangle>> Fill(FS_LineFilledSpace space);
    }
}
