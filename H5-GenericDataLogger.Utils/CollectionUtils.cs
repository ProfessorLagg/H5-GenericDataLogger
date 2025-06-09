namespace H5_GenericDataLogger;

public static class CollectionUtils {
	public static void PushRange<T>(this Stack<T> @this, IEnumerable<T> items) {
		foreach (T item in items) {
			@this.Push(item);
		}
	}

}
