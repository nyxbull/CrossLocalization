using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Nyxbull.Plugins.CrossLocalization
{
	public static partial class CrossLocalization
	{
		/// <summary>
		/// Current dictionary
		/// </summary>
		static Dictionary<string, string> dictionary;

		/// <summary>
		/// Default dictionary
		/// </summary>
		static Dictionary<string, string> dictionaryDefault;

		/// <summary>
		/// Default namespace
		/// </summary>
		static string runNamespace;

		/// <summary>
		/// Default language code
		/// </summary>
		static string defaultLangCode;

		/// <summary>
		/// Path to the languages folder
		/// </summary>
		static string pathToLanguagesFolder;

		/// <summary>
		/// Application assembly
		/// </summary>
		static Assembly assembly;

		/// <summary>
		/// Loads default dictionary
		/// </summary>
		static void loadDefault()
		{
			if (string.IsNullOrEmpty(runNamespace)) {
				throw new LocalizationException(Consts.ExceptionMessageNamespaceNotSet);
			}

			var stream = assembly.GetManifestResourceStream(runNamespace + "." + pathToLanguagesFolder + "." + defaultLangCode + ".json");

			if (stream == null) {
				throw new LocalizationException(Consts.ExceptionProcessingPathToJson);
			}

			string enJSONfile = "";
			using (var reader = new StreamReader(stream)) {
				enJSONfile = reader.ReadToEnd();
			}

			dictionaryDefault = JsonConvert.DeserializeObject<Dictionary<string, string>>(enJSONfile);
		}

		/// <summary>
		/// Checks if the run namespace is null or empty
		/// </summary>
		/// <param name="runNamespaceToCheck">Run namespace to check</param>
		static void checkRunNamespace(string runNamespaceToCheck)
		{
			if (string.IsNullOrEmpty(runNamespaceToCheck)) {
				throw new LocalizationException(Consts.ExceptionMessageNamespaceNotSet);
			}
		}

		/// <summary>
		/// Checks if the language code contains errors or is empty
		/// </summary>
		/// <param name="langCode">Language code</param>
		static void checkLangCode(string langCode)
		{
			if (string.IsNullOrEmpty(langCode)) {
				throw new LocalizationException(Consts.ExceptionMessageNullLangCode);
			}

			if (langCode.Length != 2) {
				throw new LocalizationException(Consts.ExceptionMessageTwoLetterLangCode);
			}
		}
	}
}