using NUnit.Framework;
using Moq;
using HeroesGame.Contract;
using HeroesGame.Implementation.Hero;
using HeroesGame.Implementation;

namespace HeroesGame.Tests
{
    [TestFixture]
    public class CombatProcessorShould
    {
        [Test]
        public void CallHitMethod_WhenFightIsStarted()
        {
            // Arrange (Подготовка)

            // 1. Трябва да създадем фалшиво оръжие, защото BaseHero го изисква в конструктора си
            var weaponMock = new Mock<IWeapon>();

            // 2. Създаваме Mock на BaseHero, като му подаваме оръжието
            var heroMock = new Mock<BaseHero>(weaponMock.Object);

            // 3. Създаваме Mock на чудовището
            var monsterMock = new Mock<IMonster>();

            // ВАЖНО: Настройваме чудовището да е "мъртво" веднага (IsDead връща true).
            // Иначе методът Fight() влиза в безкраен цикъл и тестът забива!
            monsterMock.Setup(m => m.IsDead()).Returns(true);

            // 4. Създаваме процесора, подавайки САМО героя (както е в твоя код)
            var processor = new CombatProcessor(heroMock.Object);

            // Act (Действие)
            // Подаваме чудовището тук, в метода Fight
            processor.Fight(monsterMock.Object);

            // Assert (Проверка)
            // Проверяваме дали чудовището е получило удар.
            // (Тъй като методът Hit в BaseHero не е virtual, не можем да го mock-нем директно, 
            // затова проверяваме крайния резултат -> дали чудовището е изяло бой).
            monsterMock.Verify(m => m.TakeHit(It.IsAny<IWeapon>()), Times.Once);
        }

        [Test]
        public void HeroGainExperience_WhenMonsterDies()
        {
            // Arrange
            var weaponMock = new Mock<IWeapon>();

            // ВАЖНО: CallBase = true казва на мок-а: "Изпълнявай истинския код на методите, ако не са мокнати".
            // Така методът GainExperience ще работи наистина и ще вдигне опита.
            var heroMock = new Mock<BaseHero>(weaponMock.Object);
            heroMock.CallBase = true;

            var monsterMock = new Mock<IMonster>();
            monsterMock.Setup(m => m.IsDead()).Returns(true); // Чудовището умира веднага
            monsterMock.Setup(m => m.Experience()).Returns(20); // Дава 20 XP

            var processor = new CombatProcessor(heroMock.Object);

            // Act
            processor.Fight(monsterMock.Object);

            // Assert
            // Проверяваме дали опитът на героя е станал 20 (ако почва от 0)
            Assert.That(heroMock.Object.Experience, Is.EqualTo(20));
        }
    }
}