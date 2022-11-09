using UnityEngine;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;

public class Task_2_3 : MonoBehaviour
{
    [SerializeField] private Transform[] _objects;
    [SerializeField] private float _speed;

    private TransformAccessArray _transforms;

    void Start()
    {
        _transforms = new TransformAccessArray(_objects);
    }


    void Update()
    {
        HardJob hardJob = new HardJob()
        {
            Speed = _speed,
            DeltaTime = Time.deltaTime
        };

        JobHandle jobHandle = hardJob.Schedule(_transforms);
        jobHandle.Complete();
    }

    private struct HardJob : IJobParallelForTransform
    {
        [ReadOnly] public float Speed;
        [ReadOnly] public float DeltaTime;
        
        public void Execute(int index, TransformAccess transform)
        {
            transform.rotation *= Quaternion.Euler(Vector3.right * Speed * DeltaTime);
        }
    }

    private void OnDestroy()
    {
        if (!gameObject.GetComponent<Task_2_3>().enabled) return;
        _transforms.Dispose();
    }
}
