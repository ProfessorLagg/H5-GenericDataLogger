using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data {
	internal static class GlobalEnvironment {
		private static DBConnector? _Connector = null;
		public static DBConnector Connector {
			get {
				if (_Connector is null) {
					_Connector = new DBConnector();
					_Connector.EnsureRequiredSchema();
				}
				return _Connector;
			}
		}

		private static AppNavPage? _NavPage = null;
		public static AppNavPage NavPage {
			get {
				if (_NavPage is null) {
					_NavPage = new AppNavPage(new MainPage());
				}
				return _NavPage;
			}
		}

		public static void SetDebugBackgrounds(this Page page) {
			Color debugColor = Color.FromRgba(255, 0, 255, 255 / 10);
			page.BackgroundColor = debugColor;
			foreach (IVisualTreeElement child in page.GetVisualTreeDescendants()) {
				if (child is not View) continue;
				View child_view = (View)child;
				child_view.BackgroundColor = debugColor;
			}

		}
	}
}
