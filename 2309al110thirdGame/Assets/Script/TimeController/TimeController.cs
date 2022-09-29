using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{

    /// <summary>
    /// Gravità del gioco
    /// </summary>
    public static float _gravity = -100;

    /// <summary>
    /// Data memorizzato del mio oggetto, che verrà poi usato per permettere lo shifting nel tempo
    /// </summary>
    public struct RecordedData
    {
        public Vector2 pos;
        public Vector2 vel;
        public float animationTime;
        public bool full;
    }
    /// <summary>
    /// La struttura di di record data è data nel seguente modo, ovvero ogni riga corrrisponde ad
    /// un oggetto, per ogni oggetto poi ho il salvataggio delle informazioni descritte in Recorded data
    /// 
    /// </summary>
    RecordedData[,] recordedData;
    public int recordMax = 10000;
    int recordCount;
    int recordIndex;
    bool wasSteppinBack = false;

    /// <summary>
    /// Array di tutti gli oggetti dei quali dovrò ricordare le caratteristiche 
    /// definite all'interno del record data
    /// </summary>
    TimeControlled[] timeObjects;
    private void Awake()
    {

        timeObjects = GameObject.FindObjectsOfType<TimeControlled>();  
        recordedData = new RecordedData[timeObjects.Length,recordMax];
    }
    void Start(){}

    void Update()
    {
        bool pause = Input.GetKey(KeyCode.UpArrow);
        bool stepBack = Input.GetKey(KeyCode.LeftArrow);
        bool stepForward = Input.GetKey(KeyCode.RightArrow);

        ///Torno indietro nel tempo
        if(stepBack)
        {
            wasSteppinBack = true;
            if(recordIndex > 0)
            {
                /// <summary>
                /// Vado a cercare nella mia matrice tutti i valori precendenti al mio valore attuale
                /// sino a quando non arrivo al valore di origine, aggiornando nel mentre il valore attuale
                /// </summary>
                recordIndex--;
                
                for (int i = 0; i < timeObjects.Length; i++)
                {
                    TimeControlled timeObject = timeObjects[i];
                    RecordedData data = recordedData[i,recordIndex];
                    timeObject.transform.position = data.pos;
                    timeObject._velocity = data.vel;
                    timeObject.animationTime = data.animationTime;

                    timeObject.UpdateAnimation();

                }
            } 

        }
        else if(pause && stepForward)
        {
            wasSteppinBack = true;
            if(recordIndex < recordCount -1)
            {
                /// <summary>
                /// Stesso ragionamento di prima, però al posto di andare indetro vado avanti fino 
                /// alla posizione corrente rispetto al passato
                /// </summary>
                recordIndex++;//decremento la posizione
                for (int i = 0; i < timeObjects.Length; i++)
                {
                    TimeControlled timeObject = timeObjects[i];
                    RecordedData data = recordedData[i,recordIndex];
                    timeObject.transform.position = data.pos;
                    timeObject._velocity = data.vel;
                    timeObject.animationTime = data.animationTime;

                    timeObject.UpdateAnimation();

                }                
            }
        }
        /// <summary>
        /// Se invece non vado ne avanti ne indietro nel tempo, memorizzo le informazioni correnti 
        /// </summary>
        /// <param name="!stepBack"></param>
        /// <returns></returns>
        else if(!pause && !stepBack)
        {
            
            if(wasSteppinBack)
            {
                recordCount = recordIndex;
                wasSteppinBack = false;
            }
            for (int i = 0; i < timeObjects.Length; i++)
            {
                TimeControlled timeObject = timeObjects[i];
                RecordedData data = new RecordedData();
                data.pos = timeObject.transform.position;
                data.vel = timeObject._velocity; 
                data.animationTime = timeObject.animationTime;
                data.full = true;
                recordedData[i,recordCount] = data;
            }
            recordCount++;
            recordIndex = recordCount;
            foreach(TimeControlled timeObject in timeObjects)
            {
                timeObject.TimeUpdate();
                timeObject.UpdateAnimation();
            }
        }
        if(CheckIfRecordIsFull())
            Debug.Log("C'è spazio" + recordCount);
        else
        {   
            Debug.Log("Risetto il tempo");
            recordedData = new RecordedData[timeObjects.Length,recordMax];
            recordCount = 0;
            recordIndex = 0;
        }
    }

    private bool CheckIfRecordIsFull()
    {
        for (int i = 0; i < recordMax; i++)
        {
            if(recordedData[0,i].full != true)
                return true;
        }
        return false;
    }
}
