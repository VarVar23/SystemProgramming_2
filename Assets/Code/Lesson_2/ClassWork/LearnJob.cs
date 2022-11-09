using System.Collections;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;

public class LearnJob : MonoBehaviour
{
    private NativeArray<Vector3> _array;
    private JobHandle _jobHandle;

    void Start()
    {
        
        _array = new NativeArray<Vector3>(100, Allocator.TempJob);

        for(int i = 0; i < _array.Length; i++)
        {
            _array[i] = new Vector3(Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100));
        }

        MyJob job = new MyJob()
        {
            array = _array,
            a = _array[0]
        };

        _jobHandle = job.Schedule(100, 0);
        StartCoroutine(JobCorutine());
        _jobHandle.Complete();


    }

    private IEnumerator JobCorutine()
    {
        while (!_jobHandle.IsCompleted)
        {
            Debug.Log(_jobHandle.IsCompleted);
            yield return null;
        }

        foreach(var o in _array)
        {
            Debug.Log(o);
        }

        _array.Dispose();
    }

    struct MyJob : IJobParallelFor
    {
        public NativeArray<Vector3> array;
        public Vector3 a;

        public void Execute(int index)
        {
            array[index] = a;
        }
    }
}
