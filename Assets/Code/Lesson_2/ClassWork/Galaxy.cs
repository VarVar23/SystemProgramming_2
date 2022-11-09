using UnityEngine;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;

public class Galaxy : MonoBehaviour
{
    [SerializeField] private int _numberOfEntites;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _maxMasses;
    [SerializeField] private float _gravitation;

    private NativeArray<Vector3> _position;
    private NativeArray<Vector3> _velocity;
    private NativeArray<Vector3> _acceliration;
    private NativeArray<float> _masses;

    private TransformAccessArray _transformAccessArray;

    void Start()
    {
        _position = new NativeArray<Vector3>(_numberOfEntites, Allocator.Persistent);
        _velocity = new NativeArray<Vector3>(_numberOfEntites, Allocator.Persistent);
        _acceliration = new NativeArray<Vector3>(_numberOfEntites, Allocator.Persistent);
        _masses = new NativeArray<float>(_numberOfEntites, Allocator.Persistent);

        Transform[] transfoms = new Transform[_numberOfEntites];

        for(int i = 0; i < _numberOfEntites; i++)
        {
            _position[i] = Random.insideUnitSphere * Random.Range(0, _maxDistance);
            _velocity[i] = Random.insideUnitSphere * Random.Range(0, _maxVelocity);
            _acceliration[i] = new Vector3();
            _masses[i] = Random.Range(1, _maxMasses);

            transfoms[i] = Instantiate(_prefab).transform;
            transfoms[i].position = _position[i];
        }

        _transformAccessArray = new TransformAccessArray(transfoms);
    }

    private void Update()
    {
        GravitationJob gravitationJob = new GravitationJob()
        {
            Position = _position,
            Velocity = _velocity,
            Masses = _masses,
            Gravitation = _gravitation,
            DeltaTime = Time.deltaTime,
            NumberOfEntites = _numberOfEntites,
            Acceliration = _acceliration
        };

        MoveJob moveJob = new MoveJob()
        {
            DeltaTime = Time.deltaTime,
            Positions = _position,
            Velocities = _velocity,
            Accelerations = _acceliration
        };

        JobHandle gravitationHandle = gravitationJob.Schedule(_numberOfEntites, 0);
        JobHandle moveHandle = moveJob.Schedule(_transformAccessArray, gravitationHandle); // ѕон€тно, что мы ждем выполнени€ задачи, но мы не прокидываем index... ќткуда он беретс€?
        moveHandle.Complete();
    }

    private struct GravitationJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> Position;
        [ReadOnly] public NativeArray<Vector3> Velocity;
        [ReadOnly] public NativeArray<float> Masses;
        [ReadOnly] public float Gravitation;
        [ReadOnly] public float DeltaTime;
        [ReadOnly] public float NumberOfEntites;

        public NativeArray<Vector3> Acceliration;

        private float _distance;
        private Vector3 _direction;
        private Vector3 _gravitation;
        public void Execute(int index)
        {
            for (int i = 0; i < NumberOfEntites; i++)
            {
                if (i == index) continue;

                _distance = Vector3.Distance(Position[i], Position[index]);
                _direction = Position[i] - Position[index];
                _gravitation = (_direction * Masses[i] * Gravitation) / (Masses[index] * Mathf.Pow(_distance, 2));

                Acceliration[index] += _gravitation * DeltaTime;
            }
        }
    }

    private struct MoveJob : IJobParallelForTransform
    {
        [ReadOnly] public float DeltaTime;

        public NativeArray<Vector3> Positions;
        public NativeArray<Vector3> Velocities;
        public NativeArray<Vector3> Accelerations;

        private Vector3 _velocity;

        public void Execute(int index, TransformAccess transform)
        {
            _velocity = Velocities[index] + Accelerations[index];
            transform.position += _velocity * DeltaTime;

            Positions[index] = transform.position;
            Velocities[index] = _velocity;
            Accelerations[index] = Vector3.zero;
        }
    }

    private void OnDestroy()
    {
        if (!gameObject.GetComponent<Galaxy>().enabled) return;

        _masses.Dispose();
        _position.Dispose();
        _velocity.Dispose();
        _acceliration.Dispose();
        _transformAccessArray.Dispose();
    }
}
