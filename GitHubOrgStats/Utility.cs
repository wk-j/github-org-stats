using System;
namespace GitHubOrgStats {

	public class GithubUtility<T> {
		public GithubUtility() {
		}

		public static T Default(Func<T> action) {
			try {
				return action();
			} catch (Exception ex) {
				return default(T);
			}
		}
	}
}
