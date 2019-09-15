using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public GameObject Car;
    public float coolDownToSpawn = 1.5f;
    public float _currentCoolDownToSpawn;

    void Start()
    {
        _currentCoolDownToSpawn = coolDownToSpawn;
    }

    void Update()
    {
        _currentCoolDownToSpawn -= Time.deltaTime;
    }

    public void SpawnCar()
    {
        if (_currentCoolDownToSpawn <= 0)
        {
            var car = Instantiate(Car);
            var mouseInput = Input.mousePosition;
            car.transform.position = Camera.main.ScreenToWorldPoint(mouseInput);
            car.transform.position = new Vector3(car.transform.position.x, car.transform.position.y, 0);
            car.GetComponent<BoxCollider2D>().isTrigger = true;
            _currentCoolDownToSpawn = coolDownToSpawn;
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }
}
