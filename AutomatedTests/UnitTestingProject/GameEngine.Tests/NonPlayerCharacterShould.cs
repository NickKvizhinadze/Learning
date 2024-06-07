using Xunit;

namespace GameEngine.Tests
{
    [Trait("Category", "NonPlayerCharacter")]
    public class NonPlayerCharacterShould
    {
        
        [Theory]
        [MemberData(nameof(InternalHealthDamageTestData.TestData), MemberType = typeof(InternalHealthDamageTestData))]
        public void TakeDamage(int damage, int expectedHealth)
        {
            var sut = new NonPlayerCharacter();
            sut.TakeDamage(damage);
            Assert.Equal(expectedHealth, sut.Health);
        }
        
        [Theory]
        [MemberData(nameof(ExternalHealthDamageTestData.TestData), MemberType = typeof(ExternalHealthDamageTestData))]
        public void TakeDamageWithExternalData(int damage, int expectedHealth)
        {
            var sut = new NonPlayerCharacter();
            sut.TakeDamage(damage);
            Assert.Equal(expectedHealth, sut.Health);
        }
        
        [Theory]
        [HealthDamageData]
        public void TakeDamageWithDataAttribute(int damage, int expectedHealth)
        {
            var sut = new NonPlayerCharacter();
            sut.TakeDamage(damage);
            Assert.Equal(expectedHealth, sut.Health);
        }
    }
}
