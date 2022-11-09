using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class Task_2_2 : MonoBehaviour
{
    [SerializeField] private int _count;
    [SerializeField] private int _maxValue;
    private NativeArray<Vector3> _position;
    private NativeArray<Vector3> _velocity;
    private NativeArray<Vector3> _finalyPosition;

    void Start()
    {
        _position = new NativeArray<Vector3>(_count, Allocator.Persistent);
        _velocity = new NativeArray<Vector3>(_count, Allocator.Persistent);
        _finalyPosition = new NativeArray<Vector3>(_count, Allocator.Persistent);

        FillArrays();

        MiddleJob middleJob = new MiddleJob()
        {
            Position = _position,
            Velocity = _velocity,
            FinalyPosition = _finalyPosition
        };

        JobHandle jobHandle = middleJob.Schedule(_count, 0);
        jobHandle.Complete();

        for (int i = 0; i < _count; i++)
        {
            Debug.Log(_position[i] + " + " + _velocity[i] + " = " + _finalyPosition[i]);
        }

        _position.Dispose();
        _velocity.Dispose();
        _finalyPosition.Dispose();
    }

    private void FillArrays()
    {
        for(int i = 0; i < _count; i++)
        {
            _position[i] = new Vector3(Random.Range(0, _maxValue), Random.Range(0, _maxValue), Random.Range(0, _maxValue));
            _velocity[i] = new Vector3(Random.Range(0, _maxValue), Random.Range(0, _maxValue), Random.Range(0, _maxValue));
            _finalyPosition[i] = new Vector3();
        }
    }

    private struct MiddleJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> Position;
        [ReadOnly] public NativeArray<Vector3> Velocity;

        public NativeArray<Vector3> FinalyPosition;

        public void Execute(int index)
        {
            FinalyPosition[index] = Position[index] + Velocity[index];
        }
    }
}
