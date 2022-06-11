using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CoffeeMachineProject;

namespace CoffeeMachineProject.UnitTests
{   
    [TestFixture]
    class MachineTests
    {
        private Machine _machine;
        private Drink _drink;
        private Coffee _coffee;

        [SetUp]
        public void SetUp()
        {
            _machine = new Machine();
            _drink = new Drink(001, "Carbonated Water", "A nice cold glass of carbonated water.", 0, 02.00m, 1, true, 0, 0);
            _coffee = new Coffee(013, "House Coffee", "A steaming cup of coffee with added sugar and milk.", 1, 20.00m, 2, false, 1, 1, 1);
        }
        [Test]
        public void WaterConnected_WhenCalled_ShouldBeFalseByDefault()
        {
            var result = _machine.WaterConnected;
            Assert.That(result, Is.False);
        }

        [Test]
        [TestCase(10)]
        [TestCase(12.5)]
        public void checkPaymentSuccessfullyReceived_WhenCalled_ReturnsTrue(decimal payment)
        {
            var result = _machine.checkPaymentSuccessfullyReceived(payment);
            Assert.That(result, Is.True);
        }
        [Test]
        [TestCase(10)]
        [TestCase(12.5)]
        public void checkPaymentSuccessfullyReceived_WhenCalled_PaymentReceivedIsAddedToMachine(decimal payment)
        {
            _machine.checkPaymentSuccessfullyReceived(payment);
            var result = _machine.PaymentReceived;
            Assert.That(result, Is.EqualTo(payment));
        }

        [Test]
        [TestCase(-10)]
        [TestCase(-13.5)]
        public void checkPaymentSuccessfullyReceived_WhenCalledWithNegativeAmount_ThrowsArgumentOutOfRangeException(decimal payment)
        {
            Assert.That(() => _machine.checkPaymentSuccessfullyReceived(payment), 
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(10)]
        [TestCase(2)]
        public void isPaidFor_WhenPaymentIsEnough_ReturnsTrue(decimal payment)
        {
            _machine.checkPaymentSuccessfullyReceived(payment);
            var result = _machine.isPaidFor(_drink);
            Assert.That(result, Is.True);
        }
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void isPaidFor_WhenPaymentIsNotEnough_ReturnsFalse(decimal payment)
        {
            _machine.checkPaymentSuccessfullyReceived(payment);
            var result = _machine.isPaidFor(_drink);
            Assert.That(result, Is.False);
        }

        [Test]
        public void brewDrink_WhenCalled_ReturnsWaterNotConnected()
        {
            var result = _machine.brewDrink(_drink);
            Assert.That(result, Is.EqualTo("Water not connected. Please contact the service helpdesk."));
        }

        [Test]
        public void brewDrink_WhenCalled_MakesDrink()
        {
            _machine.WaterConnected = true;
            _machine.checkPaymentSuccessfullyReceived(10);
            var result = _machine.brewDrink(_drink);
            Assert.That(result, Is.EqualTo("Success!"));
        }

        [Test]
        public void brewDrink_AfterMakingDrink_ResetsPaymentReceived()
        {
            _machine.WaterConnected = true;
            _machine.checkPaymentSuccessfullyReceived(10);
            _machine.brewDrink(_drink);
            var result = _machine.PaymentReceived;
            Assert.That(result, Is.EqualTo(00.00m));
        }
        [Test]
        public void brewDrink_AfterMakingCoffee_RemovesAmoutOfCoffee()
        {
            _machine.WaterConnected = true;
            _machine.checkPaymentSuccessfullyReceived(10);
            _machine.brewDrink(_coffee);
            var result = _machine.CoffeeLevel;
            Assert.That(result, Is.EqualTo(9));
        }

        [Test]
        public void brewDrink_WhenCoffeeNotSuficient_ReturnsMessage()
        {
            _machine.WaterConnected = true;
            _machine.CoffeeLevel = 0;
            _machine.checkPaymentSuccessfullyReceived(10);
            var result = _machine.brewDrink(_coffee);
            Assert.That(result, Is.EqualTo("Not enough coffee. Please contact administration to refill this ingredient."));
        }
        [Test]
        [TestCase(10)]
        [TestCase(22)]
        [TestCase(1)]
        public void abort_WhenCalled_ReturnsPaymentToZero(decimal payment)
        {
            _machine.checkPaymentSuccessfullyReceived(payment);
            _machine.abort();
            var result = _machine.PaymentReceived;
            Assert.That(result, Is.EqualTo(00.00m));
            
        }

        [Test]
        public void brewDrink_WhenMakingDrink_ReturnsString()
        {
            _machine.WaterConnected = true;
            _machine.checkPaymentSuccessfullyReceived(10);
            var result = _machine.brewDrink(_drink);
            Assert.That(result, Is.TypeOf<String>());
        }
    }
}
