static class ArrayUtils
{
    public static void Iterate<T>(T[,] array, Action<int, int> action)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                action(i, j);
            }
        }
    }
}