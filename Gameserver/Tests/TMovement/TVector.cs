using Gameserver.Movement;

namespace TVector
{
    public class TVectorCustom
    {
        [Fact]
        public void TestNotEmptyVector()
        {
            Assert.Equal("Enormous vector input", Assert.Throws<ArgumentException>(() => new Vector(new int[] { })).Message);
        }

        [Fact]
        public void TestEqualsTrue()
        {
            var A = new Vector(new int[] { 1, 2, 3 });
            var B = new Vector(new int[] { 1, 2, 3 });
            Assert.Equal(A, B);
        }

        [Fact]
        public void TestEqualsFalse()
        {
            var A = new Vector(new int[] { 1, 2, 3 });
            var B = new Vector(new int[] { 1, 2 });
            var C = 'a';
            Assert.NotEqual(A, B);
            Assert.False(A.Equals(C));
        }

        [Fact]
        public void TestGetHashCodeTrue()
        {
            var A = new Vector(new int[] { 322, 666, 123123, -945353, 1231231 });
            var B = new Vector(new int[] { 322, 666, 123123, -945353, 1231231 });
            Assert.Equal(A.GetHashCode(), B.GetHashCode());
        }

        [Fact]
        public void TestGetHashCodeFalse()
        {
            var A = new Vector(new int[] { 322, 666, 123123, -945353, 1231231 });
            var B = new Vector(new int[] { -322, -666, 123123, -945353, 1231231 });
            Assert.NotEqual(A.GetHashCode(), B.GetHashCode());
        }

        [Fact]
        public void TestSumTrue()
        {
            var A = new Vector(new int[] { 1, 2, 3 });
            var B = new Vector(new int[] { -1, -2, -3 });
            var C = new Vector(new int[] { 0, 0, 0 });
            Assert.Equal(A + B, C);
        }

        [Fact]
        public void TestSumThrowsExceptionDim()
        {
            var A = new Vector(new int[] { 1, 2, 3 });
            var B = new Vector(new int[] { -1, -2 });

            Assert.Equal("Dimensions doesn't match", Assert.Throws<ArgumentException>(() => A + B).Message);
        }
    }
}