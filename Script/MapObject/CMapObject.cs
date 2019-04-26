using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class CMapObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void UpdatePos()
    {
        
    }

    public virtual void Write(ref BinaryWriter binaryWriter)
    {
        binaryWriter.Write(Type);

        binaryWriter.Write(Mathf.Round(transform.position.x));
        binaryWriter.Write(Mathf.Round(transform.position.y));
    }

    public virtual void Read(ref BinaryReader binaryReader)
    {
        Type = binaryReader.ReadByte();
        Position.x = binaryReader.ReadSingle();
        Position.y = binaryReader.ReadSingle();

        //float x = binaryReader.ReadSingle(); 
        //float y = binaryReader.ReadSingle();
        transform.position = new Vector3(Position.x, Position.y, DefaultPosZ);
    }

    public void SetPosition(Vector2 InPosition)
    {
      //  Position = InPosition;
    }

    protected byte Type;
    protected Vector2 Position;
    protected const float DefaultPosZ = 1.0f;
}
