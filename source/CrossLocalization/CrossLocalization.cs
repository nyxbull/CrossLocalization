using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Nyxbull.Plugins.CrossLocalization
{
	/// <summary>
	/// Provides methods for localization
	/// </summary>
	public static partial class CrossLocalization
	{
		/// <summary>
		/// Gets the current two-letter language code (ISO 639-1)
		/// </summary>
		public static string CurrentLanguageCode { get; private set; }

		/// <summary>
		/// Gets the current language name in local language
		/// </summary>
		public static string CurrentLanguageLocal {
			get {
				return LanguageConfiguration.GetLanguageByCode(CurrentLanguageCode).LanguageLocal;
			}
		}

		/// <summary>
		/// Gets the current language name in English
		/// </summary>
		public static string CurrentLanguageEnglish {
			get {
				return LanguageConfiguration.GetLanguageByCode(CurrentLanguageCode).LanguageEnglish;
			}
		}

		/// <summary>
		/// Gets the current combined language name from local and English language names
		/// </summary>
		public static string CurrentCombinedLanguage {
			get {
				return LanguageConfiguration.GetLanguageByCode(CurrentLanguageCode).LanguageCombined;
			}
		}

		/// <summary>
		/// Initializes XamLoc package
		/// </summary>
		/// <param name="assembly">Application assembly</param>
		/// <param name="runNamespace">Run namespace (default namespace)</param>
		/// <param name="pathToLanguagesFolder">Relative path to the languages folder (default is "Localization.Languages")</param>
		public static void Initialize(Assembly assembly, string runNamespace, string pathToLanguagesFolder = null)
		{
			checkRunNamespace(runNamespace);

			CrossLocalization.assembly = assembly;
			CrossLocalization.runNamespace = runNamespace;

			dictionary = new Dictionary<string, string>();
			dictionaryDefault = new Dictionary<string, string>();

			if (pathToLanguagesFolder == null) {
				CrossLocalization.pathToLanguagesFolder = Consts.DefaultPathToLanguagesFolder;
			} else {
				CrossLocalization.pathToLanguagesFolder = pathToLanguagesFolder;
			}
		}

		/// <summary>
		/// Sets the default language
		/// </summary>
		/// <param name="langCode">Two-letter language code (ISO 639-1)</param>
		public static void SetDefaultLanguage(string langCode)
		{
			checkLangCode(langCode);

			if (defaultLangCode == null) {
				SetLanguage(langCode);
			}

			defaultLangCode = langCode;
			loadDefault();
		}

		/// <summary>
		/// Adds the language support
		/// </summary>
		/// <param name="language"><c>Language</c> object</param>
		public static void AddLanguageSupport(Language language)
		{
			checkLangCode(language.LangCode);

			if (string.IsNullOrEmpty(language.LanguageEnglish)) {
				throw new LocalizationException(Consts.ExceptionMessageNullEnglishLang);
			}

			LanguageConfiguration.AddLanguageSupport(
				language.LangCode,
				language.LanguageEnglish,
				language.LanguageLocal
			);
		}

		/// <summary>
		/// Checks if the language is supported
		/// </summary>
		/// <returns><c>true</c>, if the language is supported, <c>false</c> otherwise</returns>
		/// <param name="langCode">Two-letter language code (ISO 639-1)</param>
		public static bool CheckLanguageSupported(string langCode)
		{
			return LanguageConfiguration.CheckLanguageSupported(langCode);
		}

		/// <summary>
		/// Gets supported languages list
		/// </summary>
		/// <returns>Supported languages list</returns>
		public static List<Language> GetSupportedLanguages()
		{
			return LanguageConfiguration.GetSupportedLanguages();
		}

		/// <summary>
		/// Gets the language by two-letter code.
		/// </summary>
		/// <returns>The language by code.</returns>
		/// <param name="langCode">Two-letter language code (ISO 639-1)</param>
		public static Language GetLanguageByCode(string langCode)
		{
			return LanguageConfiguration.GetLanguageByCode(langCode);
		}

		/// <summary>
		/// Gets the current <c>CultureInfo</c>
		/// </summary>
		/// <returns>The current <c>CultureInfo</c> object</returns>
		/// /// <param name="regionLangCode">Two-letter region language code (ISO 3166-2) (optional)</param>
		public static CultureInfo GetCurrentCultureInfo(string regionLangCode = null)
		{
			if (regionLangCode != null) {
				checkLangCode(regionLangCode);
			}

			return GetCultureInfo(CurrentLanguageCode, regionLangCode);
		}

		/// <summary>
		/// Gets the culture info by two-letter language code
		/// </summary>
		/// <returns><c>CultureInfo object</c></returns>
		/// <param name="langCode">Two-letter language code (ISO 639-1)</param>
		/// <param name="regionLangCode">Two-letter region language code (ISO 3166-2) (optional)</param>
		public static CultureInfo GetCultureInfo(string langCode, string regionLangCode = null)
		{
			checkLangCode(langCode);

			if (regionLangCode != null) {
				checkLangCode(regionLangCode);
			}

			CultureInfo culture = null;

			try {
				if (regionLangCode == null) {
					culture = new CultureInfo(langCode);
				} else {
					try {
						culture = new CultureInfo(langCode + "-" + regionLangCode);
					} catch {
						culture = new CultureInfo(langCode);
					}
				}
			} catch (Exception ex) {
				throw new LocalizationException(Consts.ExceptionGettingCultureInfo + ex.Message);
			}

			return culture;
		}

		/// <summary>
		/// Gets the system <c>CultureInfo</c>
		/// </summary>
		/// <returns>The system  <c>CultureInfo</c></returns>
		public static CultureInfo GetSystemCultureInfo()
		{
			return CultureInfo.CurrentUICulture;
		}

		/// <summary>
		/// Gets the system language code.
		/// </summary>
		/// <returns>Two-letter language code (ISO 639-1)</returns>
		public static string GetSystemLanguageCode()
		{
			var systemCI = GetSystemCultureInfo();
			if (systemCI == null) throw new LocalizationException(Consts.ExceptionMessageNullSystemCultureInfo);
			return systemCI.TwoLetterISOLanguageName;
		}

		/// <summary>
		/// Sets the current language.
		/// </summary>
		/// <param name="langCode">Two-letter language code (ISO 639-1)</param>
		public static void SetLanguage(string langCode)
		{
			checkRunNamespace(runNamespace);

			if (string.IsNullOrEmpty(defaultLangCode)) {
				defaultLangCode = langCode;
			}

			if (!CheckLanguageSupported(langCode)) {
				checkLangCode(langCode);

				if (defaultLangCode == langCode) {
					throw new LocalizationException(Consts.ExceptionLangCodeNotSupported);
				}

				SetLanguage(defaultLangCode);
				return;
			}

			if (langCode == Consts.SystemLangCode) {
				langCode = GetSystemLanguageCode();
			}

			CurrentLanguageCode = langCode;

			dictionary.Clear();

			var stream = assembly.GetManifestResourceStream(runNamespace + "." + pathToLanguagesFolder + "." + CurrentLanguageCode + ".json");

			if (stream == null) {
				throw new LocalizationException(Consts.ExceptionProcessingPathToJson);
			}

			string JSONfile = "";
			using (var reader = new StreamReader(stream)) {
				JSONfile = reader.ReadToEnd();
			}

			dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(JSONfile);
		}

		/// <summary>
		/// Translates string int the default language
		/// </summary>
		/// <returns>String translation</returns>
		/// <param name="tag">String tag</param>
		public static string TranslateDefault(string tag)
		{
			checkRunNamespace(runNamespace);

			string translation = string.Empty;

			try {
				translation = dictionaryDefault[tag];
			} catch (KeyNotFoundException) {
				translation = tag;
			}

			return translation;
		}

		/// <summary>
		/// Translates string in the current language
		/// </summary>
		/// <returns>String translation</returns>
		/// <param name="tag">String tag</param>
		public static string Translate(string tag)
		{
			checkRunNamespace(runNamespace);

			string translation;

			try {
				translation = dictionary[tag];
			} catch (KeyNotFoundException) {
				try {
					translation = dictionaryDefault[tag];
				} catch (KeyNotFoundException) {
					translation = tag;
				}
			}
			return translation;
		}
	}
}