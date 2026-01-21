using NUnit.Framework;
using HeroesGame.Implementation.Hero;

namespace HeroesGame.Tests
{
    [TestFixture]
    public class HerosShould
    {
        // Тази променлива ще се ползва във всички тестове
        private Warrior _hero;

        // [SetUp] се изпълнява ПРЕДИ всеки един тест.
        // Така винаги имаме "чист" нов герой за всяка проверка.
        [SetUp]
        public void Setup()
        {
            _hero = new Warrior();
        }

        [Test]
        public void HaveCorrectInitialValues_WhenCreated()
        {
            // Тук вече ползваме _hero, създаден в Setup-а
            Assert.That(_hero.Level, Is.EqualTo(1), "Initial level should be 1.");
            Assert.That(_hero.Experience, Is.EqualTo(0), "Initial experience should be 0.");
            Assert.That(_hero.Health, Is.EqualTo(_hero.MaxHealth), "Hero should start with full health.");
        }

        [Test]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        public void LoseHealth_WhenTakeHit(int damage)
        {
            // Arrange (Подготовка)
            // Тук вече 'damage' идва от скобите на TestCase горе
            var expectedHealth = _hero.MaxHealth - damage;

            // Act (Действие)
            _hero.TakeHit(damage);

            // Assert (Проверка)
            Assert.That(_hero.Health, Is.EqualTo(expectedHealth), $"Health should decrease by {damage}.");
        }

        [Test]
        public void ThrowException_WhenTakeHitWithNegativeDamage()
        {
            // Arrange
            int damage = -10;

            // Act & Assert
            // Assert.Throws проверява дали кодът в скобите хвърля конкретна грешка (ArgumentException).
            // Ако методът НЕ хвърли грешка, тестът ще се оцвети в червено (Fail).
            Assert.Throws<ArgumentException>(() => _hero.TakeHit(damage));
        }
    }
}