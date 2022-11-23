using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class Task_2_1 : MonoBehaviour
{
    [SerializeField] private int _count;
    private NativeArray<int> _array;

    void Start()
    {
        _array = new NativeArray<int>(_count, Allocator.TempJob);

        Debug.Log("Start value: ");

        for(int i = 0; i < _count; i++)
        {
            _array[i] = Random.Range(0, 20);
            Debug.Log(_array[i]);
        }

        SimpleJob job = new SimpleJob();
        job.Array = _array;

        JobHandle jobHandle = job.Schedule();
        jobHandle.Complete();

        Debug.Log("End value: ");

        for (int i = 0; i < _count; i++)
        {
            Debug.Log(_array[i]);
        }

        _array.Dispose();
    }

    private struct SimpleJob : IJob
    {
        public NativeArray<int> Array;
        public void Execute()
        {
            for(int i = 0; i < Array.Length; i++)
            {
                if(Array[i] > 10) Array[i] = 0;
            }
        }
    }
}
