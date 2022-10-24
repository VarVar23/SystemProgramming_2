using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class Task_2 : MonoBehaviour
{
    private CancellationTokenSource _cancellationTokenSource;

    private void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = _cancellationTokenSource.Token;

        Task task1 = Task.Run(() => Task1(cancellationToken));
        Task task2 = Task.Run(() => Task2(cancellationToken));

        Task3(cancellationToken, task1, task2);
    }

    private async void Task3(CancellationToken cancellationToken, Task task1, Task task2)
    {
        await Task_3.WhatTaskFasterAsync(cancellationToken, task1, task2);
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    private async Task Task1(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested) return;

        await Task.Delay(2000);

        if (cancellationToken.IsCancellationRequested) return;

        Debug.Log("1я задача была выполнена через 1 секунду, после запуска");
    }

    private async Task Task2(CancellationToken cancellationToken)
    {
        for(int countFrames = 0; countFrames < 60; countFrames++)
        {
            if (cancellationToken.IsCancellationRequested) return;
            await Task.Yield();
        }

        Debug.Log("2я задача была выполнена через 60 кадров, после запуска");
    }
}
