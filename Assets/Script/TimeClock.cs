using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public struct HorizontalLine
{
    public int numberOfPlatform;
    public int numberOfTrack;
    public float minute;
    public float hours;
}

public class TimeClock : MonoBehaviour
{
    public Text clock;
    public float minute = 0;
    public float hour = 8;
    public int lastNumberOfPlatform;
    public int lastNumberOfTrack;

    public List<RailTrain> Rails = new List<RailTrain>();
    public List<HorizontalLine> TimeTable = new List<HorizontalLine>();

    private GameController _gameController;

    void Start()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        GenerateDrives();

        var rails = FindObjectsOfType<RailTrain>();

        foreach (var railTrain in rails)
        {
            Rails.Add(railTrain);
        }
    }

    void Update()
    {
        minute += Time.deltaTime * 4;

        if (minute >= 60)
        {
            hour++;
            minute = 0;
        }

        if (hour >= 24)
        {
            hour = 0;
        }

        // clock.text = $"{hour}:{(int) minute}";
        clock.text = $"{formatDate((int)hour, (int)minute)}";


        if (TimeTable.Any(t => (int)t.hours == (int)hour && (int)t.minute == (int)minute))
        {
            var horizontalLine = TimeTable.FirstOrDefault(t => (int)t.hours == (int)hour && (int)t.minute == (int)minute);
            var rail = Rails.FirstOrDefault(r =>
                r.numberOfPlatform == horizontalLine.numberOfPlatform &&
                r.numberOfTrack == horizontalLine.numberOfTrack);
            rail.requestToSendTrain = true;
        }
    }

    string formatDate(int hours, int minutes)
    {
        string hour = hours.ToString();
        string minute = minutes.ToString();
        if (hours < 10)
        {
            hour = '0' + hours.ToString();
        }
        if (minutes < 10)
        {
            minute = '0' + minutes.ToString();
        }

        return ($"{hour}:{minute}");
    }


    void GenerateDrives()
    {
        for (float i = hour; i <= 22; i++)
        {
            var firstDrive = new HorizontalLine()
            {
                numberOfPlatform = GeneratePlatform(),
                numberOfTrack = GenerateTracks(),
                hours = i,
                minute = Random.Range(0, 60)
            };

            var secondDrive = new HorizontalLine()
            {
                numberOfPlatform = GeneratePlatform(),
                numberOfTrack = GenerateTracks(),
                hours = i,
                minute = Random.Range(0, 60)
            };

            TimeTable.Add(firstDrive);
            TimeTable.Add(secondDrive);
        }
    }

    private int GenerateTracks()
    {
        int numberOfTrack = Random.Range(lastNumberOfPlatform * 2 - 1, lastNumberOfPlatform * 2 + 1);
        if (lastNumberOfTrack == numberOfTrack)
        {
            if (numberOfTrack % 2 == 0)
            {
                numberOfTrack = numberOfTrack - 1;
                lastNumberOfTrack = numberOfTrack;
                return numberOfTrack;
            }
            else
            {
                numberOfTrack = numberOfTrack + 1;
                lastNumberOfTrack = numberOfTrack;
                return numberOfTrack;
            }
        }
        else
        {
            lastNumberOfTrack = numberOfTrack;
            return numberOfTrack;
        }


    }
    private int GeneratePlatform()
    {
        int numberOfPlatform = Random.Range(1, _gameController.maxPlatforms + 1);
        lastNumberOfPlatform = numberOfPlatform;
        return numberOfPlatform;
    }

    void findObjectsOfType()
    {
        int trains = FindObjectsOfType<Train>().Length;
    }
}
