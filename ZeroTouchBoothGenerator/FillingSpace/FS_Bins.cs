using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroTouchBoothGenerator.FillingSpace
{
    public static class FS_Bins
    {
        private static bool FitsIn(double bin, double size) => bin > size;
        private static double RemainingSpace(double bin, double size) => bin - size;

        /// <summary>
        /// Tries to fill a bin of size binSize with the given sizes.
        /// Thereby as less instances of sizes as possible are used.
        /// If it is not possible to fill the bin with the given parts, the 
        /// last size item will be of custom size with at least the given minSize.
        /// </summary>
        /// <param name="sizes">Sorted list of element sizes to fill the bin with. Has to begin with the largest item.</param>
        /// <param name="binSize"></param>
        /// <param name="minSize"></param>
        /// <returns></returns>
        public static IEnumerable<double> FillBinWith(List<double> sizes, double binSize, double minSize)
        {
            while (binSize > 0)
            {
                bool foundSize = false;
                foreach (double size in sizes)
                {
                    if (!FitsIn(binSize, size))
                        continue;

                    if (RemainingSpace(binSize, size) < minSize)
                        continue;

                    yield return size;
                    binSize -= size;
                    foundSize = true;
                    break;
                }

                if (!foundSize)
                {
                    yield return binSize;
                    binSize = 0;
                }
            }
        }

        /// <summary>
        /// This method generates a bin filling wehre the first and the last
        /// part have the same length. All other parts have the given standard
        /// length.
        /// 
        /// Example:
        /// | |    |    |    | |
        /// </summary>
        /// <param name="binSize">Size of the bin.</param>
        /// <param name="standardSize">Standard part size</param>
        /// <param name="minSize">Minimum size of a part.</param>
        /// <returns></returns>
        public static IEnumerable<int> FillBinWithExtraSides(int binSize, int standardSize, int minSize)
        {
            double numStandardParts = (int)((binSize - 2 * minSize) / standardSize);
            double edgePartWidth = ((binSize - (numStandardParts * standardSize)) / 2);

            yield return (int)edgePartWidth;

            for (int i = 0; i < numStandardParts; i++)
            {
                yield return standardSize;
            }

            yield return (int)edgePartWidth;
        }
    }
}
