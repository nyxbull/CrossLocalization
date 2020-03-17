using System.Reflection;
using NUnit.Framework;
using Nyxbull.Plugins.CrossLocalization;

namespace CrossLocalization.UnitTests
{
	[TestFixture]
	public class LocalizationFixture
	{
		const string _defaultLanguage = "en";

		const string _enText = "Hello world";
		const string _ruText = "Привет мир";

		[SetUp]
		public void Setup()
		{
			var assembly = typeof(LocalizationFixture).GetTypeInfo().Assembly;
			Nyxbull.Plugins.CrossLocalization.CrossLocalization.Initialize(
				assembly, "CrossLocalization.UnitTests", "Resources");

			Nyxbull.Plugins.CrossLocalization.CrossLocalization.AddLanguageSupport(Languages.EN);
			Nyxbull.Plugins.CrossLocalization.CrossLocalization.AddLanguageSupport(Languages.RU);
			Nyxbull.Plugins.CrossLocalization.CrossLocalization.SetDefaultLanguage(_defaultLanguage);
			Nyxbull.Plugins.CrossLocalization.CrossLocalization.SetLanguage(_defaultLanguage);
		}

		[Test]
		public void TestPositiveTranslateEnglish()
		{
			var value = Nyxbull.Plugins.CrossLocalization.CrossLocalization.Translate("hello_world");
			Assert.AreEqual(_enText, value);
		}

		[Test]
		public void TestPositiveTranslateRussian()
		{
			Nyxbull.Plugins.CrossLocalization.CrossLocalization.SetLanguage(Languages.RU.LangCode);
			var value = Nyxbull.Plugins.CrossLocalization.CrossLocalization.Translate("hello_world");
			Assert.AreEqual(_ruText, value);
		}

		[Test]
		public void TestNegativeTranslateNotExistingLanguage()
		{
			Nyxbull.Plugins.CrossLocalization.CrossLocalization.SetLanguage(Languages.DE.LangCode);
			var value = Nyxbull.Plugins.CrossLocalization.CrossLocalization.Translate("hello_world");
			Assert.AreEqual(_enText, value);
		}

		[Test]
		public void TestPositiveSetSystemLanguage()
		{
			Nyxbull.Plugins.CrossLocalization.CrossLocalization.SetLanguage(Languages.SYSTEM.LangCode);
			var value = Nyxbull.Plugins.CrossLocalization.CrossLocalization.Translate("hello_world");
			Assert.AreEqual(_enText, value);
		}
	}
}
