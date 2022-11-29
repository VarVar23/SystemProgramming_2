using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class FireAction4 : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _startAmmunition = 20;

    protected string countBullet = string.Empty;
    protected Queue<GameObject> bullets = new Queue<GameObject>();
    protected Queue<GameObject> ammunitions = new Queue<GameObject>();
    protected bool reloading = false;

    protected virtual void Start()
    {
        for(int i = 0; i < _startAmmunition; i++)
        {
            GameObject bullet;

            if(_bulletPrefab == null)
            {
                bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                bullet.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            else
            {
                bullet = Instantiate(_bulletPrefab);
            }

            bullet.SetActive(false);
            ammunitions.Enqueue(bullet);
        }
    }

    public virtual async void Reloading()
    {
        bullets = await Reload();
    }

    protected virtual void Shooting()
    {
        if(bullets.Count == 0)
        {
            Reloading();
        }
    }

    private async Task<Queue<GameObject>> Reload()
    {
        if (reloading) return bullets;

        reloading = true;
        StartCoroutine(ReloadingAnim());
        return await Task.Run(delegate {

            var cage = 10;

            if(bullets.Count < cage)
            {
                Thread.Sleep(3000);
                var bullets = this.bullets;

                while(bullets.Count > 0)
                {
                    ammunitions.Enqueue(bullets.Dequeue());
                }

                cage = Mathf.Min(cage, ammunitions.Count);

                if(cage > 0)
                {
                    for(int i = 0; i < cage; i++)
                    {
                        bullets.Enqueue(ammunitions.Dequeue());
                    }
                }
            }
            reloading = false;
            return bullets;
        });

    }

    private IEnumerator ReloadingAnim()
    {
        while (reloading)
        {
            countBullet = " | ";
            yield return new WaitForSeconds(0.01f);
            countBullet = @" \ ";
            yield return new WaitForSeconds(0.01f);
            countBullet = "---";
            yield return new WaitForSeconds(0.01f);
            countBullet = " / ";
            yield return new WaitForSeconds(0.01f);
        }
        countBullet = bullets.Count.ToString();
        yield return null;
    }
}
