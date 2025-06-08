using System.Diagnostics;

using H5_GenericDataLogger.Data;

using Microsoft.Extensions.Logging;

namespace H5_GenericDataLogger {
	public static class MauiProgram {
		public static MauiApp CreateMauiApp() {
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts => {
					fonts.AddFont("OpenSans_Condensed-Bold.ttf", "OpenSansCondensedBold");
					fonts.AddFont("OpenSans_Condensed-BoldItalic.ttf", "OpenSansCondensedBoldItalic");
					fonts.AddFont("OpenSans_Condensed-ExtraBold.ttf", "OpenSansCondensedExtraBold");
					fonts.AddFont("OpenSans_Condensed-ExtraBoldItalic.ttf", "OpenSansCondensedExtraBoldItalic");
					fonts.AddFont("OpenSans_Condensed-Italic.ttf", "OpenSansCondensedItalic");
					fonts.AddFont("OpenSans_Condensed-Light.ttf", "OpenSansCondensedLight");
					fonts.AddFont("OpenSans_Condensed-LightItalic.ttf", "OpenSansCondensedLightItalic");
					fonts.AddFont("OpenSans_Condensed-Medium.ttf", "OpenSansCondensedMedium");
					fonts.AddFont("OpenSans_Condensed-MediumItalic.ttf", "OpenSansCondensedMediumItalic");
					fonts.AddFont("OpenSans_Condensed-Regular.ttf", "OpenSansCondensedRegular");
					fonts.AddFont("OpenSans_Condensed-SemiBold.ttf", "OpenSansCondensedSemiBold");
					fonts.AddFont("OpenSans_Condensed-SemiBoldItalic.ttf", "OpenSansCondensedSemiBoldItalic");
					fonts.AddFont("OpenSans_SemiCondensed-Bold.ttf", "OpenSansSemiCondensedBold");
					fonts.AddFont("OpenSans_SemiCondensed-BoldItalic.ttf", "OpenSansSemiCondensedBoldItalic");
					fonts.AddFont("OpenSans_SemiCondensed-ExtraBold.ttf", "OpenSansSemiCondensedExtraBold");
					fonts.AddFont("OpenSans_SemiCondensed-ExtraBoldItalic.ttf", "OpenSansSemiCondensedExtraBoldItalic");
					fonts.AddFont("OpenSans_SemiCondensed-Italic.ttf", "OpenSansSemiCondensedItalic");
					fonts.AddFont("OpenSans_SemiCondensed-Light.ttf", "OpenSansSemiCondensedLight");
					fonts.AddFont("OpenSans_SemiCondensed-LightItalic.ttf", "OpenSansSemiCondensedLightItalic");
					fonts.AddFont("OpenSans_SemiCondensed-Medium.ttf", "OpenSansSemiCondensedMedium");
					fonts.AddFont("OpenSans_SemiCondensed-MediumItalic.ttf", "OpenSansSemiCondensedMediumItalic");
					fonts.AddFont("OpenSans_SemiCondensed-Regular.ttf", "OpenSansSemiCondensedRegular");
					fonts.AddFont("OpenSans_SemiCondensed-SemiBold.ttf", "OpenSansSemiCondensedSemiBold");
					fonts.AddFont("OpenSans_SemiCondensed-SemiBoldItalic.ttf", "OpenSansSemiCondensedSemiBoldItalic");
					fonts.AddFont("OpenSans-Bold.ttf", "OpenSansBold");
					fonts.AddFont("OpenSans-BoldItalic.ttf", "OpenSansBoldItalic");
					fonts.AddFont("OpenSans-ExtraBold.ttf", "OpenSansExtraBold");
					fonts.AddFont("OpenSans-ExtraBoldItalic.ttf", "OpenSansExtraBoldItalic");
					fonts.AddFont("OpenSans-Italic.ttf", "OpenSansItalic");
					fonts.AddFont("OpenSans-Light.ttf", "OpenSansLight");
					fonts.AddFont("OpenSans-LightItalic.ttf", "OpenSansLightItalic");
					fonts.AddFont("OpenSans-Medium.ttf", "OpenSansMedium");
					fonts.AddFont("OpenSans-MediumItalic.ttf", "OpenSansMediumItalic");
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
					fonts.AddFont("OpenSans-SemiBoldItalic.ttf", "OpenSansSemiBoldItalic");
				});

#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
