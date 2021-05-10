﻿using ISTraining_Part.Properties;
using MaterialDesignThemes.Wpf;
using System.Windows.Media;

namespace ISTraining_Part.UI
{

    static class SetThemeColor
    {

        public static void SetTheme(bool isDark)
        {
            PaletteHelper paletteHelper = new PaletteHelper();

            ITheme theme = paletteHelper.GetTheme();
            IBaseTheme baseTheme = isDark ? (IBaseTheme)new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            paletteHelper.SetTheme(theme);

            Settings.Default.isDarkTheme = isDark;
        }

        internal static void SetColor(Color value)
        {
            PaletteHelper paletteHelper = new PaletteHelper();

            ITheme theme = paletteHelper.GetTheme();
            theme.SetPrimaryColor(value);
            paletteHelper.SetTheme(theme);

            Settings.Default.themeColor = value;
        }
    }
}
