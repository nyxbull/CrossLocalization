namespace Nyxbull.Plugins.CrossLocalization
{
	/// <summary>
	/// Package constants.
	/// </summary>{
	internal static class Consts
	{
		/// <summary>
		/// The default path to languages folder
		/// </summary>
		public const string DefaultPathToLanguagesFolder = "Localization.Languages";

		/// <summary>
		/// The system language code.
		/// </summary>
		public const string SystemLangCode = "--";

		/// <summary>
		/// The exception message for null or empty namespace situations
		/// </summary>
		public const string ExceptionMessageNamespaceNotSet = "Namespace is not set";

		/// <summary>
		/// The exception message for empty language code situations
		/// </summary>
		public const string ExceptionMessageNullLangCode = "Language code cannot be null or empty";

		/// <summary>
		/// The exception message for two-letter language code.
		/// </summary>
		public const string ExceptionMessageTwoLetterLangCode = "Package supports only two-letter language codes (ISO 639-1)";

		/// <summary>
		/// The exception message for two-letter language code.
		/// </summary>
		public const string ExceptionMessageNullEnglishLang = "English language name cannot be null or empty";

		/// <summary>
		/// The exception message for empty language code situations
		/// </summary>
		public const string ExceptionMessageNullSystemCultureInfo = "Could not get system culture info";

		/// <summary>
		/// The exception message for empty language code situations
		/// </summary>
		public const string ExceptionLangCodeNotSupported = "This language code is not supported";

		/// <summary>
		/// The exception message for language file processing error
		/// </summary>
		public const string ExceptionProcessingPathToJson = "Error occured during processing the path to JSON language file";

		/// <summary>
		/// The exception message for getting <c>CultureInfo</c> error.
		/// </summary>
		public const string ExceptionGettingCultureInfo = "Error occured during getting CultureInfo. The full message: ";
	}
}