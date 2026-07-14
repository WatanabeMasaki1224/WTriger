using UnityEngine;

public class Bailout : MonoBehaviour
{
    [SerializeField] private float _upSpeed = 1f;
    [SerializeField] private float _escapeSpeed = 10f;
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private float _upTime = 1f;

    private float _timer;
    private bool _escape;
    private Vector3 _escapeDirection;

    private void Start()
    {
        // ランダムな方向へ飛ぶ
        Vector3 randomDir = new Vector3(
            Random.Range(-1f, 1f),
            0,
            Random.Range(-1f, 1f)
        );

        _escapeDirection = randomDir.normalized;

        Destroy(gameObject, _lifeTime);
    }


    private void Update()
    {
        _timer += Time.deltaTime;


        // 最初は上へ上昇
        if (!_escape)
        {
            transform.position += Vector3.up * _upSpeed * Time.deltaTime;


            if (_timer >= _upTime)
            {
                _escape = true;
            }
        }
        // 高度到達後、遠くへ飛ぶ
        else
        {
            transform.position +=
                _escapeDirection * _escapeSpeed * Time.deltaTime;
        }
    }
}
