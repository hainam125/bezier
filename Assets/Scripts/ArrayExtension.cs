
public static class ArrayExtension {
    public static void Clear(this int[] arr, int defaultValue = 0)
    {
        for (int i = 0; i < arr.Length; i++) arr[i] = defaultValue;
    }

    public static void Clear<T>(this T[] arr)
    {
        for (int i = 0; i < arr.Length; i++) arr[i] = default(T);
    }
}
