using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GoalType
{
    ThreeSecondClass = 1,
    FirstClassWasSecondClass = 2,
    TwoFirstOneWarsOneSecondClass = 3,
    None = 4
}

public class Goal
{
    public string Description;
    public GoalType goalType;
    private readonly GameController _gameController;

    public Goal()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        goalType = (GoalType)Random.Range(1, 5);
        switch (goalType)
        {
            case GoalType.ThreeSecondClass:
                Description = "Add new car second class";
                break;
            case GoalType.FirstClassWasSecondClass:
                Description = "Add 2 cars with wars and second class";
                break;
            case GoalType.TwoFirstOneWarsOneSecondClass:
                Description = "Add wars and second class";
                break;
            case GoalType.None:
                Description = "Train don't need any cars";
                break;
        }
    }
    
    public void CheckGoal(Train train)
    {
        var children = train.GetComponentsInChildren<Car>().ToList();
        switch (goalType)
        {
            case GoalType.ThreeSecondClass:
                if (train.numberOfCars == 3 && children.All(c => c.CarClass == CarClass.SecondClass))
                {
                    _gameController.Points++;
                }
                else
                {
                    _gameController.Lives--;
                }
                break;
            case GoalType.FirstClassWasSecondClass:
                if (train.numberOfCars == 3 && children.Any(c => c.CarClass == CarClass.SecondClass) &&
                    children.Any(c => c.CarClass == CarClass.FirstClass) &&
                    children.Any(c => c.CarClass == CarClass.Wars))
                {
                    _gameController.Points++;
                }
                else
                {
                    _gameController.Lives--;
                }
                break;
            case GoalType.TwoFirstOneWarsOneSecondClass:
                if (train.numberOfCars == 4 && children.Any(c => c.CarClass == CarClass.SecondClass) &&
                    children.Count(c => c.CarClass == CarClass.FirstClass) == 2 &&
                    children.Any(c => c.CarClass == CarClass.Wars))
                {
                    _gameController.Points++;
                }
                else
                {
                    _gameController.Lives--;
                }
                break;
            case GoalType.None:
                if (train.numberOfCars == 4)
                {
                    _gameController.Points++;
                }
                else
                {
                    _gameController.Lives--;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
