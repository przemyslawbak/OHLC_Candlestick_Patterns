using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGraphMaker
{
    public interface IPatternsDictionary
    {
        Dictionary<string, string> GetCategory();
    }

    public class PatternsDictionary : IPatternsDictionary
    {
        public Dictionary<string, string> GetCategory()
        {
            return new Dictionary<string, string>()
            {
                { "BullishGartley", "fibonacci" },
                { "BullishButterfly", "fibonacci" },
                { "BullishABCD", "fibonacci" },
                { "Bullish3Extension", "fibonacci" },
                { "Bullish3Retracement", "fibonacci" },
                { "Bullish3Drive", "fibonacci" },
                { "BearishGartley", "fibonacci" },
                { "BearishButterfly", "fibonacci" },
                { "BearishABCD", "fibonacci" },
                { "Bearish3Extension", "fibonacci" },
                { "Bearish3Retracement", "fibonacci" },
                { "Bearish3Drive", "fibonacci" },
                
                { "BearishDoubleTops", "formations" },
                { "BearishTripleTops", "formations" },
                { "BearishHeadAndShoulders", "formations" },
                { "BearishInverseCupAndHandle", "formations" },
                { "BearishDescendingTriangle", "formations" },
                { "BearishRisingWedge", "formations" },
                { "BearishBearFlagsPennants", "formations" },
                { "BearishDescendingPriceChannel", "formations" },
                { "BearishRoundingTopPattern", "formations" },

                { "BullishDoubleBottoms", "formations" },
                { "BullishTripleBottoms", "formations" },
                { "BullishCupAndHandle", "formations" },
                { "BullishInverseHeadAndShoulders", "formations" },
                { "BullishAscendingTriangle", "formations" },
                { "BullishFallingWedge", "formations" },
                { "BullishBullFlagsPennants", "formations" },
                { "BullishAscendingPriceChannel", "formations" },
                { "BullishRoundingBottomPattern", "formations" },

                { "ContinuationSymmetricTriangle", "formations" },
                { "ContinuationDiamondFormation", "formations" },
            };
        }
    }
}
