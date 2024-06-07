using System;
using Xunit;

namespace GameEngine.Tests
{
    [Trait("Category", "Enemy")]
    public class EnemyFactoryShould
    {
        [Fact]
        public void CreateNormalEnemyByDefault()
        {
            var sut = new EnemyFactory();
            var enemy = sut.Create("Zombie");

            Assert.IsType<NormalEnemy>(enemy);
        }

        [Fact(Skip = "Don't need to run this test")]
        public void CreateNormalEnemyByDefault_NotTypeExample()
        {
            var sut = new EnemyFactory();
            var enemy = sut.Create("Zombie");

            Assert.IsNotType<DateTime>(enemy);
        }

        [Fact]
        public void CreateBossEnemy()
        {
            var sut = new EnemyFactory();
            var enemy = sut.Create("Zombie King", true);

            Assert.IsType<BossEnemy>(enemy);
        }

        [Fact]
        public void CreateBossEnemy_CastReturnTypeExample()
        {
            var sut = new EnemyFactory();
            var enemy = sut.Create("Zombie King", true);

            // Assert and get cast result
            var boss = Assert.IsType<BossEnemy>(enemy);

            // Additional assert on typed object
            Assert.Equal("Zombie King", boss.Name);
        }

        [Fact]
        public void CreateBossEnemy_AssertAssignableTypes()
        {
            var sut = new EnemyFactory();
            var enemy = sut.Create("Zombie King", true);

            Assert.IsAssignableFrom<Enemy>(enemy);
        }

        [Fact]
        public void CreateSeparateInstances()
        {
            var sut = new EnemyFactory();
            var enemy1 = sut.Create("Zombie");
            var enemy2 = sut.Create("Zombie");

            Assert.NotSame(enemy1, enemy2);
        }

        [Fact]
        public void NotAllowNullNames()
        {
            var sut = new EnemyFactory();

            Assert.Throws<ArgumentNullException>(() => sut.Create(null));
        }

        [Fact]
        public void OnlyAllowQueenOrKingBossEnemies()
        {
            var sut = new EnemyFactory();


            var ex = Assert.Throws<EnemyCreationException>(() => sut.Create("Zombie", true));

            Assert.Equal("Zombie", ex.RequestedEnemyName);
        }
    }
}
