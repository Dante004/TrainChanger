using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Train : MonoBehaviour
{
    public GameObject Car;
    public Sprite FirstClass;
    public Sprite SecondClass;
    public Sprite Wars;

    public float Speed;
    private float _currentSpeed;

    private Rigidbody2D _rigidBody2d;
    private bool waitToStart = false;

    public float coolDownToStart = 6f;
    private float _currentCoolDown = 0;

    public int numberOfCars = 0;

    public Goal _goal;

    public Text GoalInformation;

    private float yOffset = 0;

    void Start()
    {
        GoalInformation = GameObject.FindGameObjectWithTag("Goal").GetComponent<Text>();

        _rigidBody2d = GetComponent<Rigidbody2D>();
        _currentSpeed = Speed;
        _currentCoolDown = coolDownToStart;

        _goal = new Goal();

        switch (_goal.goalType)
        {
            case GoalType.ThreeSecondClass:
                SpawnCar(CarClass.FirstCar);
                for (var i = 0; i < 2; i++)
                {
                    SpawnCar();
                    numberOfCars++;
                }
                SortCars();
                break;
            case GoalType.FirstClassWasSecondClass:
                SpawnCar(CarClass.FirstCar);
                SpawnCar(CarClass.FirstClass);
                numberOfCars++;
                SortCars();
                break;
            case GoalType.TwoFirstOneWarsOneSecondClass:
                SpawnCar(CarClass.FirstCar);
                for (var i = 0; i < 2; i++)
                {
                    SpawnCar(CarClass.FirstClass);
                    numberOfCars++;
                }
                SortCars();
                break;
            case GoalType.None:
                SpawnCar(CarClass.FirstCar);
                SpawnCar(CarClass.FirstClass);
                SpawnCar(CarClass.Wars);
                SpawnCar(CarClass.SecondClass);
                SpawnCar(CarClass.SecondClass);
                numberOfCars = 4;
                SortCars();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void Update()
    {
        if (waitToStart)
        {
            _rigidBody2d.velocity = new Vector2(0,0);
            _currentSpeed = 0;
            _currentCoolDown -= Time.deltaTime;
            _rigidBody2d.isKinematic = true;

            if (_currentCoolDown <= 0)
            {
                _currentCoolDown = coolDownToStart;
                _currentSpeed = Speed;
                waitToStart = false;
                _rigidBody2d.isKinematic = false;
            }
        }
        else
        {
            _rigidBody2d.velocity = Vector2.up * _currentSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        CheckRail(coll);
        if (coll.tag == "Car")
        {
            if (waitToStart)
            {
                SpawnCar(coll.transform.gameObject.GetComponent<Car>().CarClass);
                Destroy(coll.transform.gameObject);
                numberOfCars++;
                SortCars();
            }
        }
    }

    void OnMouseDown()
    {
        GoalInformation.text = $"Goal: {_goal.Description}";
    }

    private void SpawnCar(CarClass carClass = CarClass.SecondClass)
    {
        var car = Instantiate(Car);
        car.transform.SetParent(gameObject.transform, true);
        car.transform.position = new Vector2(transform.position.x, transform.position.y + yOffset);
        Destroy(car.GetComponent<DragObject>());
        Destroy(car.GetComponent<Rigidbody2D>());
        Destroy(car.GetComponent<BoxCollider2D>());
        switch (carClass)
        {
            case CarClass.FirstCar:
                car.GetComponent<SpriteRenderer>().sprite = FirstClass;
                car.GetComponent<Car>().CarClass = CarClass.FirstCar;
                break;
            case CarClass.FirstClass:
                car.GetComponent<SpriteRenderer>().sprite = FirstClass;
                car.GetComponent<Car>().CarClass = CarClass.FirstClass;
                break;
            case CarClass.Wars:
                car.GetComponent<SpriteRenderer>().sprite = Wars;
                car.GetComponent<Car>().CarClass = CarClass.Wars;
                break;
            case CarClass.SecondClass:
                car.GetComponent<SpriteRenderer>().sprite = SecondClass;
                car.GetComponent<Car>().CarClass = CarClass.SecondClass;
                break;
        }
        yOffset-=2.5f;
    }

    private void SortCars()
    {
        var children = GetComponentsInChildren<Car>().ToList();
        var sorted = children.OrderBy(x => (int) x.CarClass).ToList();
        yOffset = 0;
        for (var i = 0; i < sorted.Count(); ++i)
        {
            sorted.ElementAt(i).transform.SetSiblingIndex(i);
            sorted.ElementAt(i).transform.position = new Vector2(transform.position.x, transform.position.y + yOffset);
            yOffset -= 2.5f;
        }
    }

    private void CheckRail(Collider2D coll)
    {
        if (coll.tag == "Rail")
        {
            var rail = coll.GetComponent<Rail>();
            if (rail.RailType == RailType.Middle)
            {
                waitToStart = true;
            }
            else if (rail.RailType == RailType.Last)
            {
                _goal.CheckGoal(this);
                Destroy(gameObject);
            }
        }
    }
}
