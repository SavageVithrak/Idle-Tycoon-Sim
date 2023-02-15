using UnityEngine;

class Person
{
    public string Name { get; }
    public Person( string name ) => Name = name;
}

class Company
{
    Person[] personal;

    public Company( Person[] people ) => 
        personal = people;

    // индексатор
    public Person this[ int index ]
    {
        get => personal[ index ]; 
        set => personal[ index ] = value;
    }
    
    public Person this[ Vector2Int index ]
    {
        get => personal[ index.x ]; 
        set => personal[ index.y ] = value;
    }
}

public class UseTest
{
    public void Method()
    {
        Company company = new Company(new Person[4]);
        var a = company[ 1 ];
        var b = company[ Vector2Int.zero ];
    }
}


public enum SomeEnum
{
    A, B, C,
}
class SomeMatrix
{
    public SomeEnum[,] Sample;
}