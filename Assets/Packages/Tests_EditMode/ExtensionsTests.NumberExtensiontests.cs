using System;
using ExtensionMethods;
using NUnit.Framework;

namespace Tests_EditMode
{
    public partial class ExtensionsTests
    {
        public class NumberExtensiontests
        {
            [Flags]
            public enum SampleFlags
            {
                none = 0,
                one = 1 << 0,
                two = 1 << 1,
                three = 1 << 2,
                four = 1 << 3,
                five = 1 << 4,
                six = 1 << 5,
                seven = 1 << 6,
                eight = 1 << 7,
                nine = 1 << 8,
                ten = 1 << 9,
            }
            [Test, Sequential]
            public void RankAs_NonNegativeRank_FlagWithOnlyTheRankOn([Range(0, 10)] int rank, [Values] SampleFlags expected)
            {
                //ACT
                SampleFlags flags = rank.RankAs<SampleFlags>();
                //ASSERT
                Assert.AreEqual(expected, flags);
            }
        }
    }
}
