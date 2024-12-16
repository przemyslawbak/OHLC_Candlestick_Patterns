using Candlestick_Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGraphMaker
{
    public interface IMethodsDictionary
    {
        Dictionary<string, string> GetCategory();
    }

    public class MethodsDictionary : IMethodsDictionary
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

                {"Bearish2Crows","patterns" },
                {"Bearish3BlackCrows", "patterns" },
                {"Bearish3InsideDown", "patterns" },
                {"Bearish3OutsideDown","patterns" },
                {"Bearish3LineStrike","patterns" },
                {"BearishAdvanceBlock","patterns" },
                {"BearishBeltHold","patterns" },
                {"BearishBlackClosingMarubozu","patterns" },
                {"BearishBlackMarubozu","patterns" },
                {"BearishBlackOpeningMarubozu","patterns" },
                {"BearishBreakaway","patterns" },
                {"BearishDeliberation","patterns" },
                {"BearishDarkCloudCover","patterns" },
                {"BearishDojiStar","patterns" },
                {"BearishDownsideGap3Methods","patterns" },
                {"BearishDownsideTasukiGap","patterns" },
                {"BearishDragonflyDoji","patterns" },
                {"BearishEngulfing","patterns" },
                {"BearishEveningDojiStar","patterns" },
                {"BearishEveningStar","patterns" },
                {"BearishFalling3Methods","patterns" },
                {"BearishGravestoneDoji","patterns" },
                {"BearishHarami","patterns" },
                {"BearishIdentical3Crows","patterns" },
                {"BearishHaramiCross","patterns" },
                {"BearishInNeck","patterns" },
                {"BearishKicking","patterns" },
                {"BearishLongBlackCandelstick","patterns" },
                {"BearishMeetingLines","patterns" },
                {"BearishOnNeck","patterns" },
                {"BearishSeparatingLines","patterns" },
                {"BearishShootingStar","patterns" },
                {"BearishSideBySideWhiteLines","patterns" },
                {"BearishThrusting","patterns" },
                {"BearishTriStar","patterns" },
                {"BearishTweezerTop","patterns" },
                {"BearishUpsideGap2Crows","patterns" },
                {"Bullish3InsideUp","patterns" },
                {"Bullish3OutsideUp","patterns" },
                {"Bullish3StarsintheSouth","patterns" },
                {"Bullish3WhiteSoldiers","patterns" },
                {"Bullish3LineStrike","patterns" },
                {"BullishBeltHold","patterns" },
                {"BullishBreakaway","patterns" },
                {"BullishConcealingBabySwallow","patterns" },
                {"BullishDojiStar","patterns" },
                {"BullishDragonflyDoji","patterns" },
                {"BullishEngulfing","patterns" },
                {"BullishGravestoneDoji","patterns" },
                {"BullishHarami","patterns" },
                {"BullishHaramiCross","patterns" },
                {"BullishHomingPigeon","patterns" },
                {"BullishInvertedHammer","patterns" },
                {"BullishKicking","patterns" },
                {"BullishLadderBottom","patterns" },
                {"BullishLongWhiteCandlestick","patterns" },
                {"BullishMatHold","patterns" },
                {"BullishMatchingLow","patterns" },
                {"BullishMeetingLines","patterns" },
                {"BullishMorningDojiStar","patterns" },
                {"BullishMorningStar","patterns" },
                {"BullishPiercingLine","patterns" },
                {"BullishRising3Methods","patterns" },
                {"BullishSeparatingLines","patterns" },
                {"BullishSideBySideWhiteLines","patterns" },
                {"BullishStickSandwich","patterns" },
                {"BullishTriStar","patterns" },
                {"BullishTweezerBottom","patterns" },
                {"BullishUnique3RiverBottom","patterns" },
                {"BullishUpsideGap3Methods","patterns" },
                {"BullishUpsideTasukiGap","patterns" },
                {"BullishWhiteClosingMarubozu","patterns" },
                {"BullishWhiteMarubozu","patterns" },
                {"BullishWhiteOpeningMarubozu","patterns" },
            };
        }
    }
}
