using System.Collections.Generic;
using UnityEngine;

public static class MyLibrary
{
    public static Queue<int> CreateRandomQueue(int size)
    {
        Queue<int> resultQueue = new Queue<int>();
        int[] array = new int[size];
        for (int i = 0; i < size; i++) array[i] = i;

        for (int i = 0; i < size; i++)
        {
            int randomPosition = Random.Range(0, size);

            int temp = array[randomPosition];
            array[randomPosition] = array[i];
            array[i] = temp;
        }
        for (int i = 0; i < size; i++) resultQueue.Enqueue(array[i]);

        return resultQueue;
    }


    
}
