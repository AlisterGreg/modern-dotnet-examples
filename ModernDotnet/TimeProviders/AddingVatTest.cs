using Microsoft.Extensions.Time.Testing;

namespace TimeProviders
{
	public sealed class AddingVatTest
	{
		[Test]
		public void ThePriceIsReturnedIncreasedByVat()
		{
			var calculator = new Calculator();

			var priceWithVat = calculator.AddVat(10000m);

			Assert.That(priceWithVat, Is.EqualTo(12000m));
		}

		[Test]
		public void ThePriceIsReturnedIncreasedByVatWithTimeProvider()
		{
			var calculator = new Calculator();

			var fakeTimeProvider = new FakeTimeProvider();
			fakeTimeProvider.SetUtcNow(DateTimeOffset.Parse("2025/6/14"));

			var priceWithVat = calculator.AddVat(10000m, fakeTimeProvider);

			Assert.That(priceWithVat, Is.EqualTo(12000m));
		}

		[Test]
		public void ThePriceIsReturnedIncreasedByVatWithTimeProviderAutoAdvance()
		{
			var calculator = new Calculator();

			var fakeTimeProvider = new FakeTimeProvider()
			{
				AutoAdvanceAmount = TimeSpan.FromDays(2)
			};
			fakeTimeProvider.SetUtcNow(DateTimeOffset.Parse("2025/6/14"));

			var priceWithVatBeforeTurnOn = calculator.AddVat(10000m, fakeTimeProvider);
			Assert.That(priceWithVatBeforeTurnOn, Is.EqualTo(12000m));

			var priceWithVatAfterTurnOn = calculator.AddVat(10000m, fakeTimeProvider);
			Assert.That(priceWithVatAfterTurnOn, Is.EqualTo(12500m));
		}
	}

	public sealed class Calculator
	{
		public decimal AddVat(decimal price)
		{
			var vatRate = 1.2m;

			if (DateTime.UtcNow > new DateTime(2025, 6, 15))
				vatRate = 1.25m;

			return price * vatRate;
		}

		public decimal AddVat(decimal price, TimeProvider timeProvider)
		{
			var vatRate = 1.2m;

			if (timeProvider.GetUtcNow() > new DateTime(2025, 6, 15))
				vatRate = 1.25m;

			return price * vatRate;
		}
	}
}