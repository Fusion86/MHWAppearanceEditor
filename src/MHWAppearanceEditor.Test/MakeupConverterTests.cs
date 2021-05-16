using MHWAppearanceEditor.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MHWAppearanceEditor.Test
{
    [TestClass]
    public class MakeupConverterTests
    {
        private static double delta = 0.00001;

        [TestMethod]
        [DataRow(1, 0)]
        [DataRow(0.6, 20)]
        [DataRow(0.2, 40)]
        [DataRow(0.1, 45)]
        [DataRow(0, 50)]
        [DataRow(-0.035, 55)]
        [DataRow(-0.06999999, 60)]
        [DataRow(-0.21, 80)]
        [DataRow(-0.245, 85)]
        [DataRow(-0.28, 90)]
        [DataRow(-0.35, 100)]
        public void MakeupPosToRaw(double expected, int value)
        {
            Assert.AreEqual(expected, Utility.MakeupPosToRaw(value), delta);
        }

        [TestMethod]
        [DataRow(00, 1)]
        [DataRow(20, 0.6)]
        [DataRow(40, 0.2)]
        [DataRow(45, 0.1)]
        [DataRow(50, 0)]
        [DataRow(55, -0.035)]
        [DataRow(60, -0.06999999)]
        [DataRow(80, -0.21)]
        [DataRow(85, -0.245)]
        [DataRow(90, -0.28)]
        [DataRow(100, -0.35)]
        public void MakeupPosFromRaw(int expected, double value)
        {
            Assert.AreEqual(expected, Utility.MakeupPosFromRaw(value), delta);
        }

        [TestMethod]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method)]
        public void MakeupPosBiDirectional(int value)
        {
            Assert.AreEqual(value, Utility.MakeupPosFromRaw(Utility.MakeupPosToRaw(value)), delta);
        }

        public static IEnumerable<object[]> GetData()
        {
            for (int i = 0; i <= 100; i++)
                yield return new object[] { i };
        }
    }
}
