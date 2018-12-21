using System;
namespace GitHubOrgStats {

	public class Utility<T> {
		public static T Default(Func<T> action) {
			try {
				return action();
			} catch (Exception ex) {
				return default(T);
			}
		}
	}
}
