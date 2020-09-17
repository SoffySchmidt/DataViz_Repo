
//Kageformen/Skabelonen for personen
public class Person
{
    public string firstName, lastName;
    public int age, postNumber, cohabitantsCount, streamGamesCount, siblingCount, id;
    public bool hadCovid, hasPet;
    public CovidRelationLevel covidRelationLevel;
    //enum is like an int
    //the values you declare in the enum automatically obtains an index number
    //so family is [0], FamilyOrFriend [1], etc...
    public enum CovidRelationLevel
    {
        Family, FamilyOrFriend, Anyone, None
    }

    public Person(int id)
    {
        this.id = id;
    }
}
