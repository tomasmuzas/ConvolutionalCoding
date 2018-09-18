using System;
using System.Collections.Generic;
using System.Linq;

namespace ConvolutionalCodes.Tests.UnitTests.TestHelpers
{
    public class CollectionTest
    {
        internal const bool T = true;
        internal const bool F = false;

        internal bool CollectionsAreEqual<T>(IEnumerable<T> collection1, IEnumerable<T> collection2)
        {
            return collection1
                .SequenceEqual(collection2);
        }
    }
}
