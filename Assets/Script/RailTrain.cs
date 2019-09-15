using UnityEngine;

public class RailTrain : MonoBehaviour
{
    public int numberOfPlatform;
    public int numberOfTrack;

    public bool requestToSendTrain;

    public GameObject FirstRail;
    public GameObject MiddleRail;
    public GameObject LastRail;

    public GameObject Train;

    public float coolDownToSpawn = 5;
    private float _currentCoolDownToSpawn;

    void Start()
    {
        _currentCoolDownToSpawn = 0;
    }

    void Update()
    {
        if (requestToSendTrain)
        {
            if (_currentCoolDownToSpawn <= 0)
            {
                Instantiate(Train, FirstRail.transform.position, Quaternion.identity);
                requestToSendTrain = false;
                _currentCoolDownToSpawn = coolDownToSpawn;
            }
            else
            {
                requestToSendTrain = false;
            }
        }

        _currentCoolDownToSpawn -= Time.deltaTime;
    }
}
