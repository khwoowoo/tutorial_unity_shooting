using System.Collections;

public static class Utiliy
{
    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        System.Random random = new System.Random(seed);

        for(int i = 0; i < array.Length -1; i++)
        {
            int tempItem = random.Next(i, array.Length);
            T temp = array[tempItem];
            array[tempItem] = array[i];
            array[i] = temp;
        }

        return array;
    }
   
}
