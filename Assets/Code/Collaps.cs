using UnityEngine;
using System.Threading.Tasks;
using System.Threading;


public class Collaps : MonoBehaviour
{
    async void Start()
    {
        Task t1 = Task.Run(() => Collaps1());
        Task t2 = Task.Run(() => Collaps2());
        await Task.WhenAll(t1, t2);
        Debug.Log("1");
    }

    private async Task Collaps1()
    {
        Debug.Log("2");
        await Task.Delay(1000);
        Debug.Log("3");
    }

    private async Task Collaps2()
    {
        Debug.Log("4");
        await Task.Delay(2000);
        Debug.Log("5");
    }
}
//24351