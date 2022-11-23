using System.Threading;
using System.Threading.Tasks;

public static class Task_3 
{
    public static async Task<bool> WhatTaskFasterAsync(CancellationToken cancellationToken, Task task1, Task task2)
    {
        await Task.WhenAny(task1, task2);

        if (cancellationToken.IsCancellationRequested) return false;
        if (task1.IsCompleted) return true;
        if (task2.IsCompleted) return false;

        return false;
    }

}
