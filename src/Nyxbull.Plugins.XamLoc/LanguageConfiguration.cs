using System.Collections.Generic;

namespace Nyxbull.Plugins.XamLoc
{
	/// <summary>
	/// Provides methods for configuring and getting languages
	/// </summary>
	internal static class LanguageConfiguration
	{
		/// <summary>
		/// The languages list
		/// </summary>
		static List<Language> languagesList;

		/// <summary>
		/// Adds the language support
		/// </summary>
		/// <param name="langCode">Two-letter language code (ISO 639-1)</param>
		/// <param name="languageEnglish">Language name in English</param>
		/// <param name="languageLocal">Language name in local language (optional)</param>
		public static void AddLanguageSupport(string langCode, string languageEnglish, string languageLocal = null)
		{
			if (languagesList == null) {
				languagesList = new List<Language>();
			}

			languagesList.Add(new Language(langCode, languageEnglish, languageLocal));
		}

		/// <summary>
		/// Gets supported languages list
		/// </summary>
		/// <returns>Supported languages list</returns>
		public static List<Language> GetSupportedLanguages()
		{
			return languagesList;
		}

		/// <summary>
		/// Checks if the language is supported
		/// </summary>
		/// <returns><c>true</c>, if the language is supported, <c>false</c> otherwise</returns>
		/// <param name="langCode">Two-letter language code (ISO 639-1)</param>
		public static bool CheckLanguageSupported(string langCode)
		{
			if (langCode == Consts.SystemLangCode) return true;
			foreach (var language in languagesList) {
				if (langCode == language.LangCode) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Gets the language by code.
		/// </summary>
		/// <returns>The language by two-letter code.</returns>
		/// <param name="langCode">Two-letter language code (ISO 639-1)</param>
		public static Language GetLanguageByCode(string langCode)
		{
			Language currentLang = null;
			foreach (var language in languagesList) {
				if (langCode == language.LangCode) {
					currentLang = language;
				}
			}
			return currentLang;
		}
	}
}