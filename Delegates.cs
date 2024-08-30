using UnityEngine;

/*
    This class holds all the standard Delegates used for general Unity Projects.
*/
namespace Delegates
{
    // Standard Events
    public delegate void VoidEvent();

    public delegate bool BoolReturnEvent();
    public delegate void BoolEvent(bool b);

    public delegate void FloatEvent(float f);
    public delegate float FloatReturnEvent();

    public delegate void IntEvent(int i);
    public delegate int IntReturnEvent();

    public delegate void StringEvent(string s);
    public delegate string StringReturnEvent();

    public delegate void GameObjectEvent(GameObject g);
    public delegate void TransformEvent(Transform f);

    public delegate void Vector3Event(Vector3 v);
    public delegate void Vector2Event(Vector2 v);

    public delegate void ObjectEvent(object o);

    public delegate void FlagEvent(string s, int i);
    public delegate int FlagReturnEvent(string s);    
}
