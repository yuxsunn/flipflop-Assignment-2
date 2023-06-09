using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialCommunication : MonoBehaviour
{
    public string scenario;

    public GameObject hand;
    public GameObject dop;
    //public GameObject motorcycle;
    public GameObject Handlebar;
    public GameObject PepperMill;
    public GameObject SpinnyThingy;
    public ParticleSystem pepper;
    public GameObject SpinnyThingy_end;
    public GameObject ShoulderR;
    public GameObject ShoulderL;

    AudioSource audioData;

    string analog_data;
    float prev_analog;
    float start_data;
    
    public SerialPort data_stream = new SerialPort("COM3", 9600);

    void Awake() // Awake() is called before Start()
    {
        analog_data = "0.0";

        //Open the serial stream
        data_stream.Open();
        //data_stream.WriteLine("1"); //Makes serail available on the Arduino
        data_stream.DtrEnable = true;
        data_stream.RtsEnable = true;
        data_stream.WriteTimeout = 300;
        data_stream.ReadTimeout = 5000;

        Thread thread = new Thread(Run);
        thread.Start();
    }

    void Start()
    {

    }
    
    void Run()
    {
        try
        {
            analog_data = data_stream.ReadLine();
            //Debug.Log("Received data from Arduino " + analog_data);
            Debug.Log(scenario);
        }
        catch (TimeoutException t)
        {
            Debug.Log("Arduino Read timeout" + t);
        }

        while (true)
        {
            data_stream.WriteLine(scenario.ToString());
            //reading incoming string from Arduino to Unity
            analog_data = data_stream.ReadLine();
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (scenario == "none")
        {
            Handlebar.GetComponent<AudioSource>().Pause();
            PepperMill.GetComponent<AudioSource>().Pause();
            SpinnyThingy.GetComponent<AudioSource>().Pause();
            SpinnyThingy_end.GetComponent<AudioSource>().Pause();

            start_data = float.Parse(analog_data);
            //Debug.Log(start_data);                            
        }

        // Detect if hand has overlapped an object
        if (Handlebar.GetComponent<CollisionDetector>().detected == true)
        {
            scenario = "handlebar";
            Debug.Log(scenario);
        } else if (PepperMill.GetComponent<CollisionDetector>().detected == true)
        {
            scenario = "peppermill";
            Debug.Log(scenario);
        } else if (SpinnyThingy.GetComponent<CollisionDetector>().detected == true)
        {
            scenario = "spinnythingy";
            Debug.Log(scenario);
        } else
        {
            scenario = "none";
        }

        if (scenario != "none")
        {
            float analog = float.Parse(analog_data);
            Debug.Log("Received data from Arduino " + analog_data);

            float rotational_change = analog - prev_analog;
            Debug.Log(start_data);

            /*//Get rid of noise from the trimpots
            if (Mathf.Abs(rotational_change) < 10)
            {
                rotational_change = 0;
            }*/


            //Update object rotation of unity object
            if (scenario == "handlebar")
            {
                Handlebar.transform.Rotate(new Vector3(0, -rotational_change, 0));

                if ((analog > 20) || (analog < -20))
                {
                    if (!Handlebar.GetComponent<AudioSource>().isPlaying)
                    {
                        Handlebar.GetComponent<AudioSource>().Play();
                    }
                } else
                {
                    Handlebar.GetComponent<AudioSource>().Pause();
                }
            } else if (scenario == "peppermill")
            {
                if ((analog > 20) || (analog < -20))
                {
                    if (!PepperMill.GetComponent<AudioSource>().isPlaying)
                    {
                        PepperMill.GetComponent<AudioSource>().Play();
                    }
                    if (!pepper.GetComponent<ParticleSystem>().isPlaying)
                    {
                        pepper.GetComponent<ParticleSystem>().Play();
                    }
                }
                else
                {
                    pepper.GetComponent<ParticleSystem>().Pause();
                    pepper.GetComponent<ParticleSystem>().Clear();
                }
            } else if (scenario == "spinnythingy")
            {
                SpinnyThingy.transform.Rotate(new Vector3(0, rotational_change, 0));
                //AudioSource data = SpinnyThingy.GetComponent<AudioSource>();
                //data.Play();

                if ((analog > 20 && analog > prev_analog) || (analog < -20 && analog < prev_analog))
                {
                    if (SpinnyThingy_end.GetComponent<AudioSource>().isPlaying)
                    {
                        SpinnyThingy_end.GetComponent<AudioSource>().Pause();
                    }
                    if (!SpinnyThingy.GetComponent<AudioSource>().isPlaying)
                    {
                        SpinnyThingy.GetComponent<AudioSource>().Play();
                    }
                }
                else if ((analog > 20 && analog < prev_analog) || (analog < -20 && analog > prev_analog))
                {
                    ShoulderL.transform.Rotate(new Vector3(10, 0, 0));
                    ShoulderR.transform.Rotate(new Vector3(10, 0, 0));
                    if (SpinnyThingy.GetComponent<AudioSource>().isPlaying)
                    {
                        SpinnyThingy.GetComponent<AudioSource>().Pause();
                    }
                    if (!SpinnyThingy_end.GetComponent<AudioSource>().isPlaying)
                    {
                        SpinnyThingy_end.GetComponent<AudioSource>().Play();
                    }
                }
                else
                {
                    if (SpinnyThingy.GetComponent<AudioSource>().isPlaying)
                    {
                        SpinnyThingy.GetComponent<AudioSource>().Pause();
                    }
                    if (SpinnyThingy_end.GetComponent<AudioSource>().isPlaying)
                    {
                        SpinnyThingy_end.GetComponent<AudioSource>().Pause();
                    }
                }

            }
            /*else if (scenario == "dop")
            {
                dop.transform.Rotate(new Vector3(0, rotational_change, 0));
                if (analog > 240)
                {
                    dop.transform.parent = null;
                    scenario = "0";
                    analog_data = "0.0";
                }
            }*/
            prev_analog = analog; //Set the prev values for the next loop
        }
    }



}
