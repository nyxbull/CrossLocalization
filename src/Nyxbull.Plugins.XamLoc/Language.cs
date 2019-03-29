namespace Nyxbull.Plugins.XamLoc
{
	/// <summary>
	/// Provides information about the language
	/// </summary>
	public class Language
	{
		/// <summary>
		/// Two-letter language code (ISO 639-1)
		/// </summary>
		public string LangCode { get; set; }

		/// <summary>
		/// Language name in English
		/// </summary>
		public string LanguageEnglish { get; set; }

		/// <summary>
		/// Language name in local language
		/// </summary>
		public string LanguageLocal { get; set; }

		/// <summary>
		/// Combined language name from local and English language names
		/// </summary>
		public string LanguageCombined { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Nyxbull.Plugins.XamLoc.Language"/> class
		/// </summary>
		/// <param name="langCode">Two-letter language code (ISO 639-1)</param>
		/// <param name="languageEnglish">Language name in English</param>
		/// <param name="languageLocal">Language name in local language (optional)</param>
		public Language(string langCode, string languageEnglish, string languageLocal = null)
		{
			LangCode = langCode;
			LanguageEnglish = languageEnglish;
			LanguageLocal = languageLocal;

			if (string.IsNullOrEmpty(languageLocal)) {
				LanguageCombined = LanguageEnglish;
			} else {
				if (string.Compare(LanguageLocal, LanguageEnglish) != 0) {
					LanguageCombined = LanguageLocal + " (" + LanguageEnglish + ")";
				} else {
					LanguageCombined = LanguageEnglish;
				}
			}

			if (string.IsNullOrEmpty(LanguageLocal)) {
				LanguageLocal = LanguageEnglish;
			}
		}
	}
}